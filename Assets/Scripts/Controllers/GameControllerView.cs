using gRaFFit.Agar.Controllers.GameScene.MainControllers;
using gRaFFit.Agar.Models.ControllerSwitcherSystem;
using gRaFFit.Agar.Views;
using UnityEngine;

namespace Controllers {
	public class GameControllerView : AControllerView {
		[SerializeField] private PlayerView _player;
		
		public override void Activate() {
			_player.gameObject.SetActive(true);
		}

		public override ControllerType Type => ControllerType.Game;
	}
}