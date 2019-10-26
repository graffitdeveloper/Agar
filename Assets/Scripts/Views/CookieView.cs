using System;
using gRaFFit.Agar.Utils.Signals;
using gRaFFit.Agar.Views.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

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

			_isPunch = false;
		}

		public void SetScale(float scale) {
			CookieScale = scale;
			transform.localScale = new Vector3(CookieScale, CookieScale, CookieScale);
		}

		private bool _isPunch;
		private Vector2 _punchPosition;

		private const float CookiePunchIntensity = 2f;

		public void Punch() {
			_isPunch = true;
			_punchPosition = transform.position + new Vector3(
				                 Random.Range(-CookiePunchIntensity, CookiePunchIntensity),
				                 Random.Range(-CookiePunchIntensity, CookiePunchIntensity), 0);
		}

		public void Update() {
			if (_isPunch) {
				transform.position = Vector3.Lerp(transform.position, _punchPosition, Time.deltaTime * 10);
				if (Vector3.Distance(transform.position, _punchPosition) < 0.1f) {
					_isPunch = false;
				}
			}
		}
	}
}