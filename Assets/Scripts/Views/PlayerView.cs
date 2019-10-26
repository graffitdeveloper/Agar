using gRaFFit.Agar.Controllers.InputSystem;
using gRaFFit.Agar.Utils;
using gRaFFit.Agar.Views.CameraControls;
using UnityEngine;

namespace gRaFFit.Agar.Views {
	public class PlayerView : MonoBehaviour {
		private static string ANIMATION_STATE_CAT_SIT = "Sit";
		private static string ANIMATION_STATE_CAT_WALK = "Walk";

		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private Animator _animator;

		public void PlayWalkAnimation() {
			AnimatorHelper.Instance.PlayAnimation(_animator, ANIMATION_STATE_CAT_WALK);
		}

		public void Stop() {
			_spriteRenderer.transform.rotation = Quaternion.identity;
			_spriteRenderer.flipY = false;

			AnimatorHelper.Instance.PlayAnimation(_animator, ANIMATION_STATE_CAT_SIT);
		}

		public void FaceToTouch() {
			var touchPosition = InputController.Instance.GetTouchWorldPosition();

			_spriteRenderer.transform.rotation = RotationHelper.FaceObject(transform.position, touchPosition, 180f);

			var playerPosition =
				CachedMainCamera.Instance.Camera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f,
					Screen.height * 0.5f)) + CameraView.Instance.GetCameraOffset();

			_spriteRenderer.flipY = playerPosition.x < touchPosition.x;
		}
	}
}