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
		[SerializeField] private Rigidbody2D _rigidbody2D;
		[SerializeField] private float _moveSpeed;
		
		public void PlayWalkAnimation() {
			AnimatorHelper.Instance.PlayAnimation(_animator, ANIMATION_STATE_CAT_WALK);
		}

		private Vector3 _startPosition;
		
		public void Awake() {
			_startPosition = transform.position;
		}

		public void Stop() {
			_spriteRenderer.transform.rotation = Quaternion.identity;
			_spriteRenderer.flipY = false;
			_rigidbody2D.velocity = Vector2.zero;

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

		public Vector3 GetOffset() {
			return _startPosition - transform.position;
		}

		public Vector3 GetTouchOffset() {
			return ((Vector3) InputController.Instance.GetTouchWorldPosition() - transform.position).normalized;
		}

		public void MoveByControls() {
			_rigidbody2D.velocity = GetTouchOffset() * _moveSpeed;
		}
	}
}