using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace gRaFFit.Agar.Views {


	public class EnemyView : CharacterView {

		private Transform _currentTarget;
		public void MoveToTarget() {
			if (_currentTarget != null) {
				_rigidbody2D.velocity =
					((Vector2) _currentTarget.transform.position - (Vector2) transform.position).normalized *
					_moveSpeed;
			}
		}

		public void FindNewTarget(List<CookieView> cookies) {
			
			
			for (int i = 0; i < cookies.Count; i++) {
				// TODO: FIND NEARES COOKIE
			}
			
			var nearestCookie = cookies.Min(cookie => Vector3.Distance(transform.position, cookie.transform.position));
			if (nearestCookie != null) {
				_currentTarget = nearestCookie.
			}
		}
	}
}