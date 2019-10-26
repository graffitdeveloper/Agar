using gRaFFit.Agar.Utils.Signals;
using gRaFFit.Agar.Views.Pool;
using UnityEngine;

namespace gRaFFit.Agar.Views {
	public class CookieView : GameObjectPoolable {
		public Signal<float> SignalOnCookieEatenByPlayer = new Signal<float>();
		public Signal<float, EnemyView> SignalOnCookieEatenByEnemy = new Signal<float, EnemyView>();

		private float _cookieScale;

		private void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag("Player")) {
				SignalOnCookieEatenByPlayer.Dispatch(_cookieScale);
				Dispose();
			} else if (other.CompareTag("Enemy")) {
				SignalOnCookieEatenByEnemy.Dispatch(_cookieScale, other.GetComponent<EnemyView>());
				Dispose();
			}
		}

		public override void Instantiate() {
			base.Instantiate();
			SetScale(1f);

			SignalOnCookieEatenByPlayer = new Signal<float>();
		}

		public override void Dispose() {
			base.Dispose();

			if (SignalOnCookieEatenByPlayer != null) {
				SignalOnCookieEatenByPlayer.RemoveAllListeners();
				SignalOnCookieEatenByPlayer = null;
			}
		}

		public void SetScale(float scale) {
			_cookieScale = scale;
			transform.localScale = new Vector3(_cookieScale, _cookieScale, _cookieScale);
		}
	}
}