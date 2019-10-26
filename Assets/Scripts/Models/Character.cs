namespace Models {
	public class Character {
		private static int _currentFreeID;

		public Character() {
			ID = _currentFreeID;
			_currentFreeID++;
			Weight = 0.5f;
		}

		public int ID { get; private set; }
		public float Weight { get; private set; }

		public void EatCookie(float eatenCookieScale) {
			Weight += eatenCookieScale;
		}

		public void ResetWeight() {
			Weight = 0.5f;
		}
	}
}