using gRaFFit.Agar.Controllers.GameScene.MainControllers;
using gRaFFit.Agar.Controllers.InputSystem;
using gRaFFit.Agar.Models.ControllerSwitcherSystem;
using gRaFFit.Agar.Views;
using gRaFFit.Agar.Views.CameraControls;
using gRaFFit.Agar.Views.UIPanelSystem;
using Models;
using UnityEngine;

namespace Controllers {
	public class GameControllerView : AControllerView {
		[SerializeField] private PlayerView _player;
		[SerializeField] private MeshRenderer _bgMeshRenderer;
		[SerializeField] private CookieSpawner _cookieSpawner;
		
		public override void Activate() {
			_player.gameObject.SetActive(true);
			UIManager.Instance.ShowPanel<HudPanelView>();
			
			PlayerModel.Instance.ResetWeight();
			RefreshWeight(true);
			
			_bgMeshRenderer.gameObject.SetActive(true);
			CameraView.Instance.SetToPlayer(_player);
			_cookieSpawner.Init();
			_cookieSpawner.InstantiateAllCookies();
			
			AddListeners();
		}

		public override void Deactivate() {
			_player.gameObject.SetActive(false);
			_bgMeshRenderer.gameObject.SetActive(false);
			_cookieSpawner.Clear();

			UIManager.Instance.HidePanel<HudPanelView>();
			
			base.Deactivate();
		}

		private void RefreshWeight(bool immediately) {
			_player.SetWeight(PlayerModel.Instance.Weight);
			UIManager.Instance.GetPanel<HudPanelView>().RefreshWeight(PlayerModel.Instance.Weight, immediately);
		}

		protected override void AddListeners() {
			base.AddListeners();

			InputController.Instance.SignalOnTouchStart.AddListener(OnTouchStart);
			InputController.Instance.SignalOnTouchEnd.AddListener(OnTouchEnd);
			InputController.Instance.SignalOnTouch.AddListener(OnTouch);

			var hud = UIManager.Instance.GetPanel<HudPanelView>();
			if (hud != null) {
				hud.SignalOnRestartButtonClicked.AddListener(OnRestartButtonClicked);
				hud.SignalOnGoToLobbyButtonClicked.AddListener(OnGoToLobbyButtonClicked);
			}

			_cookieSpawner.SignalOnCookieEaten.AddListener(OnCookieEaten);
		}

		protected override void RemoveListeners() {
			InputController.Instance.SignalOnTouchStart.RemoveListener(OnTouchStart);
			InputController.Instance.SignalOnTouchEnd.RemoveListener(OnTouchEnd);
			InputController.Instance.SignalOnTouch.RemoveListener(OnTouch);

			var hud = UIManager.Instance.GetPanel<HudPanelView>();
			if (hud != null && hud.IsSignalsInited) {
				hud.SignalOnRestartButtonClicked.RemoveListener(OnRestartButtonClicked);
				hud.SignalOnGoToLobbyButtonClicked.RemoveListener(OnGoToLobbyButtonClicked);
			}

			_cookieSpawner.SignalOnCookieEaten.RemoveListener(OnCookieEaten);

			base.RemoveListeners();
		}

		private void OnGoToLobbyButtonClicked() {
			ControllerSwitcher.Instance.SwitchController(ControllerType.Lobby);
		}

		private void OnRestartButtonClicked() {
			ControllerSwitcher.Instance.SwitchController(ControllerType.Game);
		}

		private void OnCookieEaten(float cookieScale) {
			PlayerModel.Instance.EatCookie(cookieScale);
			RefreshWeight(false);
			
			_cookieSpawner.SpawnNewCookie();
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