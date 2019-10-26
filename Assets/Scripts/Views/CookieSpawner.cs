using System.Collections.Generic;
using gRaFFit.Agar.Views.Pool;
using UnityEngine;

namespace gRaFFit.Agar.Views {
	public class CookieSpawner : MonoBehaviour {
		[SerializeField] private Transform _spawnMinPoint;
		[SerializeField] private Transform _spawnMaxPoint;
		[SerializeField] private CookieView _cookiePrefab;
		[SerializeField] private int _targetCookiesCount;

		private List<CookieView> _currentCookiesInstances;

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

		public void SpawnNewCookie() {
			var newCookie = PoolService.Instance.PopObject(COOKIE_POOL_ID, transform) as CookieView;
			if (newCookie != null) {
				_currentCookiesInstances.Add(newCookie);
				newCookie.transform.position = new Vector3(
					Random.Range(_spawnMinPoint.position.x, _spawnMaxPoint.position.x),
					Random.Range(_spawnMinPoint.position.y, _spawnMaxPoint.position.y));
			} else {
				Debug.LogError("ERROR SPAWNING COOKIE");
			}
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