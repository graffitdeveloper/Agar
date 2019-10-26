using gRaFFit.Agar.Utils.Signals;
using gRaFFit.Agar.Views.Pool;
using UnityEngine;

namespace gRaFFit.Agar.Views {
	public class CookieView : GameObjectPoolable {
		public Signal<float> SignalOnCookieEaten = new Signal<float>();

		private float _cookieScale;
		
		private void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag("Player")) {
				SignalOnCookieEaten.Dispatch(_cookieScale);
				Dispose();
			}
		}

		public override void Instantiate() {
			base.Instantiate();
			SetScale(1f);
			
			SignalOnCookieEaten = new Signal<float>();
		}

		public override void Dispose() {
			base.Dispose();

			if (SignalOnCookieEaten != null) {
				SignalOnCookieEaten.RemoveAllListeners();
				SignalOnCookieEaten = null;
			}
		}

		public void SetScale(float scale) {
			_cookieScale = scale;
			transform.localScale = new Vector3(_cookieScale, _cookieScale, _cookieScale);
		}
	}
}