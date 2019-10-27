using Controllers;
using UnityEngine;

namespace gRaFFit.Agar.Views {

	public class EnemyView : CharacterView {

		private CookieView _targetCookie;
		private CharacterView _targetCharacter;

		public override void MoveToTarget() {
			if (_collider2D.enabled) {
				if (_targetCharacter != null) {
					_rigidbody2D.velocity =
						((Vector2) _targetCharacter.transform.position - (Vector2) transform.position).normalized *
						_moveSpeed;

					FaceToPosition(_targetCharacter.transform.position);
				} else if (_targetCookie != null) {
					_rigidbody2D.velocity =
						((Vector2) _targetCookie.transform.position - (Vector2) transform.position).normalized *
						_moveSpeed;

					FaceToPosition(_targetCookie.transform.position);
				} else {
					_rigidbody2D.velocity = Vector3.Lerp(_rigidbody2D.velocity, Vector3.zero, 1f * Time.deltaTime);
				}
			}
		}

		private CharacterView FindNearestCharacter() {
			CharacterView nearestCharacter = null;
			float distanceToNearestCharacter = 0;

			for (int i = 0; i < CharactersContainer.Instance.CharacterViews.Count; i++) {
				var currentCharacter = CharactersContainer.Instance.CharacterViews[i];
				if (nearestCharacter == null) {
					nearestCharacter = currentCharacter;
					distanceToNearestCharacter =
						Vector2.Distance(currentCharacter.transform.position, transform.position);
				} else {
					var distanceToCurrentCharacter =
						Vector2.Distance(currentCharacter.transform.position, transform.position);
					if (distanceToCurrentCharacter < distanceToNearestCharacter) {
						distanceToNearestCharacter = distanceToCurrentCharacter;
						nearestCharacter = currentCharacter;
					}
				}
			}

			return nearestCharacter;
		}

		private CookieView FindNearestCookie() {
			CookieView foundCookie = null;
			float distanceToNearestCookie = 0;

			if (CookieSpawner.Instance.Cookies != null) {
				for (int i = 0; i < CookieSpawner.Instance.Cookies.Count; i++) {
					var currentCookie = CookieSpawner.Instance.Cookies[i];
					if (foundCookie == null) {
						foundCookie = currentCookie;
						distanceToNearestCookie =
							Vector2.Distance(currentCookie.transform.position, transform.position);
					} else {
						var distanceToCurrentCookie =
							Vector2.Distance(currentCookie.transform.position, transform.position);
						if (distanceToCurrentCookie < distanceToNearestCookie) {
							distanceToNearestCookie = distanceToCurrentCookie;
							foundCookie = currentCookie;
						}
					}
				}
			}

			if (foundCookie != null) {
				PlayWalkAnimation();
				foundCookie.SignalOnCookieEatenByCharacter.AddListener(OnMyCookieEaten);
			} else {
				Stop();
				Debug.LogError("Can't find cookies :(");
			}

			return foundCookie;
		}


		public void FindNewTarget() {
			if (_targetCookie != null) {
				if (_targetCookie.SignalOnCookieEatenByCharacter != null) {
					_targetCookie.SignalOnCookieEatenByCharacter.RemoveListener(OnMyCookieEaten);
				}
			}

			/*
			var nearestCharacterView = FindNearestCharacter();
			var nearestCharacter = CharactersContainer.Instance.GetCharacter(nearestCharacterView.ID);
			var meCharacterView = CharactersContainer.Instance.GetCharacterView(ID);
			var meCharacter = CharactersContainer.Instance.GetCharacter(ID);

			if (nearestCharacterView != null && nearestCharacter != null &&
			    meCharacterView != null && meCharacter != null) {
				var distance = Vector2.Distance(nearestCharacterView.transform.position, transform.position);
				if (distance < 5f) {

					if (nearestCharacter.Weight < meCharacter.Weight) {
						_targetCharacter = nearestCharacterView;
						PlayWalkAnimation();
					}
				}
			} else {
			*/
				_targetCookie = FindNearestCookie();
			//}
		}

		private void OnMyCookieEaten(CookieView cookie, int enemyID) {
			FindNewTarget();
		}

		protected override void EnableCollider() {
			FindNewTarget();
			
			base.EnableCollider();
		}
	}
}