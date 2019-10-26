using gRaFFit.Agar.Utils;
using gRaFFit.Agar.Views.CameraControls;
using gRaFFit.Agar.Views.Pool;
using UnityEngine;

namespace gRaFFit.Agar.Views {
	public abstract class CharacterView : GameObjectPoolable {
		private static string ANIMATION_STATE_SIT = "Sit";
		private static string ANIMATION_STATE_WALK = "Walk";

		[SerializeField] protected SpriteRenderer _spriteRenderer;
		[SerializeField] protected Animator _animator;
		[SerializeField] protected Rigidbody2D _rigidbody2D;
		[SerializeField] protected float _moveSpeed;
		[SerializeField] protected float _weightScaleCost;
		[SerializeField] protected Vector3 _minimumSize;

		public void Init(int id) {
			ID = id;
		}

		public int ID { get; set; }

		public void PlayWalkAnimation() {
			AnimatorHelper.Instance.PlayAnimation(_animator, ANIMATION_STATE_WALK);
		}

		public void Stop() {
			_spriteRenderer.transform.rotation = Quaternion.identity;
			_spriteRenderer.flipY = false;
			_rigidbody2D.velocity = Vector2.zero;
			_rigidbody2D.angularVelocity = 0;

			AnimatorHelper.Instance.PlayAnimation(_animator, ANIMATION_STATE_SIT);
		}


		public void SetWeight(float newWeight) {
			transform.localScale = _minimumSize + (_weightScaleCost * newWeight * Vector3.one);
		}

		protected void FaceToPosition(Vector3 targetPosition) {
			_spriteRenderer.transform.rotation = RotationHelper.FaceObject(transform.position, targetPosition, 180f);

			var playerPosition =
				CachedMainCamera.Instance.Camera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f,
					Screen.height * 0.5f)) + CameraView.Instance.GetCameraOffset();

			_spriteRenderer.flipY = playerPosition.x < targetPosition.x;
		}

		public override void Dispose() {
			Stop();

			base.Dispose();
		}
	}
}