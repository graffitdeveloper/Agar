using System.Collections.Generic;
using UnityEngine;

namespace gRaFFit.Agar.Views {

	public class EnemyView : CharacterView {

		private CookieView _currentCookie;

		public void MoveToTarget() {
			if (_currentCookie != null) {
				_rigidbody2D.velocity =
					((Vector2) _currentCookie.transform.position - (Vector2) transform.position).normalized *
					_moveSpeed;

				FaceToPosition(_currentCookie.transform.position);
			}
		}

		public void FindNewTarget() {
			CookieView foundCookie = null;
			float distanceToNearestCookie = 0;

			for (int i = 0; i < CookieSpawner.Instance.Cookies.Count; i++) {
				var currentCookie = CookieSpawner.Instance.Cookies[i];
				if (foundCookie == null) {
					foundCookie = currentCookie;
					distanceToNearestCookie = Vector2.Distance(currentCookie.transform.position, transform.position);
				} else {
					var distanceToCurrentCookie =
						Vector2.Distance(currentCookie.transform.position, transform.position);
					if (distanceToCurrentCookie < distanceToNearestCookie) {
						distanceToNearestCookie = distanceToCurrentCookie;
						foundCookie = currentCookie;
					}
				}
			}

			if (foundCookie != null) {
				_currentCookie = foundCookie;
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
	}
}