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

		public void Start() {
			InputController.Instance.SignalOnTouchStart.AddListener(OnTouchStart);
			InputController.Instance.SignalOnTouchEnd.AddListener(OnTouchEnd);
			InputController.Instance.SignalOnTouch.AddListener(OnTouch);
		}

		private void OnTouch(Vector2 obj) {
			var touchPosition =
				CachedMainCamera.Instance.Camera.ScreenToWorldPoint(InputController.Instance.GetTouchPosition());

			_spriteRenderer.transform.rotation = RotationHelper.FaceObject(transform.position, touchPosition, 180f);

			var playerPosition = CachedMainCamera.Instance.Camera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f,
				                     Screen.height * 0.5f)) + CameraView.Instance.GetCameraOffset();

			_spriteRenderer.flipY = playerPosition.x < touchPosition.x;
		}

		private void OnTouchEnd(Vector2 obj) {
			_spriteRenderer.transform.rotation = Quaternion.identity;
			_spriteRenderer.flipY = false;


			AnimatorHelper.Instance.PlayAnimation(_animator, ANIMATION_STATE_CAT_SIT);
		}

		private void OnTouchStart(Vector2 obj) {
			AnimatorHelper.Instance.PlayAnimation(_animator, ANIMATION_STATE_CAT_WALK);
		}
	}
}