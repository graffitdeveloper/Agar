using gRaFFit.Agar.Controllers.GameScene.MainControllers;
using gRaFFit.Agar.Controllers.InputSystem;
using gRaFFit.Agar.Models.ControllerSwitcherSystem;
using gRaFFit.Agar.Views;
using UnityEngine;

namespace Controllers {
	public class GameControllerView : AControllerView {
		[SerializeField] private PlayerView _player;

		public override void Activate() {
			_player.gameObject.SetActive(true);

			AddListeners();
		}

		protected override void AddListeners() {
			base.AddListeners();

			InputController.Instance.SignalOnTouchStart.AddListener(OnTouchStart);
			InputController.Instance.SignalOnTouchEnd.AddListener(OnTouchEnd);
			InputController.Instance.SignalOnTouch.AddListener(OnTouch);
		}

		protected override void RemoveListeners() {
			InputController.Instance.SignalOnTouchStart.RemoveListener(OnTouchStart);
			InputController.Instance.SignalOnTouchEnd.RemoveListener(OnTouchEnd);
			InputController.Instance.SignalOnTouch.RemoveListener(OnTouch);

			base.RemoveListeners();
		}

		private void OnTouchStart(Vector2 obj) {
			_player.PlayWalkAnimation();
		}

		private void OnTouchEnd(Vector2 obj) {
			_player.Stop();
		}

		private void OnTouch(Vector2 obj) {
			_player.FaceToTouch();
		}

		public override ControllerType Type => ControllerType.Game;
	}
}