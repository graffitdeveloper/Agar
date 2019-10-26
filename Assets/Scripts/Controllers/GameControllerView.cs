using gRaFFit.Agar.Controllers.GameScene.MainControllers;
using gRaFFit.Agar.Controllers.InputSystem;
using gRaFFit.Agar.Models.ControllerSwitcherSystem;
using gRaFFit.Agar.Views;
using gRaFFit.Agar.Views.CameraControls;
using UnityEngine;

namespace Controllers {
	public class GameControllerView : AControllerView {
		[SerializeField] private PlayerView _player;
		[SerializeField] private MeshRenderer _bgMeshRenderer;

		public override void Activate() {
			_player.gameObject.SetActive(true);
			_bgMeshRenderer.gameObject.SetActive(true);

			CameraView.Instance.SetToPlayer(_player.transform);

			AddListeners();
		}

		public override void Deactivate() {
			_player.gameObject.SetActive(false);
			_bgMeshRenderer.gameObject.SetActive(false);

			base.Deactivate();
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
			_player.MoveByControls();

			_bgMeshRenderer.material.mainTextureOffset = _player.GetOffset();
			_bgMeshRenderer.transform.position = new Vector3(
				_player.transform.position.x,
				_player.transform.position.y,
				_bgMeshRenderer.transform.position.z);
		}

		public override ControllerType Type => ControllerType.Game;
	}
}