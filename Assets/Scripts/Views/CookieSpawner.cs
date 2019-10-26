using System.Collections.Generic;
using gRaFFit.Agar.Utils.Signals;
using gRaFFit.Agar.Views.Pool;
using UnityEngine;

namespace gRaFFit.Agar.Views {
	public class CookieSpawner : MonoBehaviour {
		[SerializeField] private Transform _spawnMinPoint;
		[SerializeField] private Transform _spawnMaxPoint;
		[SerializeField] private CookieView _cookiePrefab;
		[SerializeField] private int _targetCookiesCount;

		[SerializeField] private float _minCookieScale;
		[SerializeField] private float _maxCookieScale;
		
		
		
		private List<CookieView> _currentCookiesInstances;
		public Signal<float> SignalOnCookieEatenByPlayer = new Signal<float>();
		public Signal<float, int> SignalOnCookieEatenByEnemy = new Signal<float, int>();

		public List<CookieView> Cookies => _currentCookiesInstances;
		
		public const string COOKIE_POOL_ID = "COOKIE_POOL_ID";

		public void Init() {
			PoolService.Instance.InitPoolWithNewObject(COOKIE_POOL_ID, _cookiePrefab);
		}

		public void InstantiateAllCookies() {
			Clear();
			_currentCookiesInstances = new List<CookieView>();
			
			for (int i = 0; i < _targetCookiesCount; i++) {
				SpawnNewCookie();
			}
		}

		public Vector3 GetRandomPositionInLevel() {
			return new Vector3(
				Random.Range(_spawnMinPoint.position.x, _spawnMaxPoint.position.x),
				Random.Range(_spawnMinPoint.position.y, _spawnMaxPoint.position.y));
		}

		public void SpawnNewCookie() {
			var newCookie = PoolService.Instance.PopObject(COOKIE_POOL_ID, transform) as CookieView;
			if (newCookie != null) {
				newCookie.transform.position = GetRandomPositionInLevel();
				newCookie.SetScale(Random.Range(_minCookieScale, _maxCookieScale));
				newCookie.SignalOnCookieEatenByPlayer.AddListener(OnCookieEatenByPlayer);
				newCookie.SignalOnCookieEatenByEnemy.AddListener(OnCookieEatenByEnemy);
				_currentCookiesInstances.Add(newCookie);
			} else {
				Debug.LogError("ERROR SPAWNING COOKIE");
			}
		}

		private void OnCookieEatenByEnemy(CookieView cookieView, int enemyID) {
			_currentCookiesInstances.Remove(cookieView);

			var scale = cookieView.CookieScale;
			cookieView.Dispose();
			SignalOnCookieEatenByEnemy.Dispatch(scale, enemyID);
		}

		private void OnCookieEatenByPlayer(CookieView cookieView) {
			_currentCookiesInstances.Remove(cookieView);

			var scale = cookieView.CookieScale;
			cookieView.Dispose();
			SignalOnCookieEatenByPlayer.Dispatch(scale);
		}

		public void Clear() {
			if (_currentCookiesInstances != null) {
				for (int i = 0; i < _currentCookiesInstances.Count; i++) {
					_currentCookiesInstances[i].Dispose();
				}

				_currentCookiesInstances.Clear();
				_currentCookiesInstances = null;
			}
		}
	}
}