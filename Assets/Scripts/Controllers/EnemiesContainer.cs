using System.Collections.Generic;
using gRaFFit.Agar.Views;
using Models;

namespace Controllers {
	public class EnemiesContainer {

		#region Singleton

		private EnemiesContainer() {

		}

		private static EnemiesContainer _instance;

		public static EnemiesContainer Instance {
			get { return _instance ?? (_instance = new EnemiesContainer()); }
		}

		#endregion

		private List<Character> _enemies = new List<Character>();
		private List<EnemyView> _enemyViews = new List<EnemyView>();

		public List<Character> Enemies => _enemies;
		public List<EnemyView> EnemyViews => _enemyViews;

		public void PutEnemy(Character character, EnemyView enemyView) {
			_enemies.Add(character);
			_enemyViews.Add(enemyView);
		}

		public Character GetEnemy(int ID) {
			return _enemies.Find(enemy => enemy.ID == ID);
		}

		public EnemyView GetEnemyView(int ID) {
			return _enemyViews.Find(enemy => enemy.ID == ID);
		}

		public void Clear() {
			_enemies.Clear();
			for (int i = 0; i < _enemyViews.Count; i++) {
				_enemyViews[i].Dispose();
			}

			_enemyViews.Clear();
		}
	}
}