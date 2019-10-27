using UnityEngine;

namespace gRaFFit.Agar.Views {

	public class EnemyView : CharacterView {

		private CookieView _targetCookie;
		private CharacterView _targetCharacter;

		public void MoveToTarget() {
			if (_targetCookie != null && _collider2D.enabled) {
				_rigidbody2D.velocity =
					((Vector2) _targetCookie.transform.position - (Vector2) transform.position).normalized *
					_moveSpeed;

				FaceToPosition(_targetCookie.transform.position);
			} else {
				_rigidbody2D.velocity = Vector3.Lerp(_rigidbody2D.velocity, Vector3.zero, 1f * Time.deltaTime);
			}
		}

		public void FindNewTarget() {
			if (_targetCookie != null) {
				if (_targetCookie.SignalOnCookieEatenByEnemy != null) {
					_targetCookie.SignalOnCookieEatenByEnemy.RemoveListener(OnMyCookieEaten);
				}

				if (_targetCookie.SignalOnCookieEatenByPlayer != null) {
					_targetCookie.SignalOnCookieEatenByPlayer.RemoveListener(OnMyCookieEaten);
				}
			}

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
				_targetCookie = foundCookie;
				PlayWalkAnimation();
				foundCookie.SignalOnCookieEatenByEnemy.AddListener(OnMyCookieEaten);
				foundCookie.SignalOnCookieEatenByPlayer.AddListener(OnMyCookieEaten);
			} else {
				Stop();
				Debug.LogError("Can't find cookies :(");
			}
		}

		private void OnMyCookieEaten(CookieView cookie) {
			FindNewTarget();
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