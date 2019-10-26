namespace Models {
	public class PlayerModel {
		#region Singleton

		private PlayerModel() {

		}

		private static PlayerModel _instance;

		public static PlayerModel Instance {
			get { return _instance ?? (_instance = new PlayerModel()); }
		}

		#endregion

		public float Weight { get; private set; }

		public void EatCookie(float eatenCookieScale) {
			Weight += eatenCookieScale;
		}

		public void ResetWeight() {
			Weight = 0.5f;
		}
	}
}