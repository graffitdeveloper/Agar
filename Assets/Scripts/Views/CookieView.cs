using gRaFFit.Agar.Utils.Signals;
using gRaFFit.Agar.Views.Pool;
using UnityEngine;

namespace gRaFFit.Agar.Views {
	public class CookieView : GameObjectPoolable {
		public Signal<CookieView> SignalOnCookieEatenByPlayer;
		public Signal<CookieView, int> SignalOnCookieEatenByEnemy;

		public float CookieScale { get; private set; }

		private void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag("Player")) {
				if (SignalOnCookieEatenByPlayer != null) {
					SignalOnCookieEatenByPlayer.Dispatch(this);
				}
			} else if (other.CompareTag("Enemy")) {
				if (SignalOnCookieEatenByEnemy != null) {
					SignalOnCookieEatenByEnemy.Dispatch(this, other.GetComponent<EnemyView>().ID);
				}
			}
		}

		public override void Instantiate() {
			base.Instantiate();
			SetScale(1f);

			SignalOnCookieEatenByPlayer = new Signal<CookieView>();
			SignalOnCookieEatenByEnemy = new Signal<CookieView, int>();
		}

		public override void Dispose() {
			base.Dispose();

			if (SignalOnCookieEatenByPlayer != null) {
				SignalOnCookieEatenByPlayer.RemoveAllListeners();
				SignalOnCookieEatenByPlayer = null;
			}

			if (SignalOnCookieEatenByEnemy != null) {
				SignalOnCookieEatenByEnemy.RemoveAllListeners();
				SignalOnCookieEatenByEnemy = null;
			}
		}

		public void SetScale(float scale) {
			CookieScale = scale;
			transform.localScale = new Vector3(CookieScale, CookieScale, CookieScale);
		}
	}
}