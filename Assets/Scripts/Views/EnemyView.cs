using System.Collections.Generic;
using UnityEngine;

namespace gRaFFit.Agar.Views {

	public class EnemyView : CharacterView {

		private Transform _currentTarget;

		public void MoveToTarget() {
			if (_currentTarget != null) {
				_rigidbody2D.velocity =
					((Vector2) _currentTarget.transform.position - (Vector2) transform.position).normalized *
					_moveSpeed;

				FaceToPosition(_currentTarget.position);
			}
		}

		public void FindNewTarget(List<CookieView> cookies) {
			Transform currentCookieTransform = null;
			float distanceToNearestCookie = 0;

			for (int i = 0; i < cookies.Count; i++) {
				var currentCookie = cookies[i];
				if (currentCookieTransform == null) {
					currentCookieTransform = currentCookie.transform;
					distanceToNearestCookie = Vector2.Distance(currentCookie.transform.position, transform.position);
				} else {
					var distanceToCurrentCookie =
						Vector2.Distance(currentCookie.transform.position, transform.position);
					if (distanceToCurrentCookie < distanceToNearestCookie) {
						distanceToNearestCookie = distanceToCurrentCookie;
						currentCookieTransform = currentCookie.transform;
					}
				}
			}
			
			if (currentCookieTransform != null) {
				_currentTarget = currentCookieTransform;
				PlayWalkAnimation();
			} else {
				Stop();
				Debug.LogError("Can't find cookies :(");
			}
		}
	}
}