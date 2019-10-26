using gRaFFit.Agar.Controllers.GameScene.MainControllers;
using gRaFFit.Agar.Models.ControllerSwitcherSystem;

namespace Controllers {
	public class LobbyControllerView : AControllerView {
		public override void Activate() {
		}

		public override ControllerType Type => ControllerType.Lobby;
	}
}