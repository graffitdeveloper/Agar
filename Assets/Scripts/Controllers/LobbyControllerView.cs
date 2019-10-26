using gRaFFit.Agar.Controllers.GameScene.MainControllers;
using gRaFFit.Agar.Models.ControllerSwitcherSystem;
using gRaFFit.Agar.Views.UIPanelSystem;

namespace Controllers {
	public class LobbyControllerView : AControllerView {

		public override ControllerType Type => ControllerType.Lobby;


		public override void Activate() {
			var mainPanel = UIManager.Instance.ShowPanel<MainPanelView>();
			mainPanel.SignalOnPlayButtonClicked.AddListener(OnPlayButtonClicked);
		}

		public override void Deactivate() {
			var mainPanel = UIManager.Instance.GetPanel<MainPanelView>();
			mainPanel.SignalOnPlayButtonClicked.RemoveListener(OnPlayButtonClicked);
			mainPanel.Hide();

			base.Deactivate();
		}

		private void OnPlayButtonClicked() {
			ControllerSwitcher.Instance.SwitchController(ControllerType.Game);
		}
	}
}