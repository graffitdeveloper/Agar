using gRaFFit.Agar.Models.Timers;
using gRaFFit.Agar.Utils;
using gRaFFit.Agar.Utils.Signals;
using gRaFFit.Agar.Views.Pool;
using UnityEngine;

namespace gRaFFit.Agar.Views {
	public abstract class CharacterView : GameObjectPoolable {
		private static string ANIMATION_STATE_SIT = "Sit";
		private static string ANIMATION_STATE_WALK = "Walk";

		[SerializeField] protected SpriteRenderer _spriteRenderer;
		[SerializeField] protected Animator _animator;
		[SerializeField] protected Rigidbody2D _rigidbody2D;
		[SerializeField] protected Collider2D _collider2D;
		[SerializeField] protected float _moveSpeed;
		[SerializeField] protected float _weightScaleCost;
		[SerializeField] protected Vector3 _minimumSize;

		public Signal<CharacterView, CharacterView> SignalOnCharactersCollided;
		private Timer _timer;

		private void OnCollisionEnter2D(Collision2D other) {
			if (other.collider.CompareTag("Player") || other.collider.CompareTag("Enemy")) {
				SignalOnCharactersCollided.Dispatch(other.gameObject.GetComponent<CharacterView>(), this);
			}
		}

		public void Init(int id) {
			ID = id;
			SignalOnCharactersCollided = new Signal<CharacterView, CharacterView>();
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
			_spriteRenderer.transform.rotation = Quaternion.Lerp(
				_spriteRenderer.transform.rotation,
				RotationHelper.FaceObject(transform.position, targetPosition, 180f),
				10f * Time.deltaTime);
			_spriteRenderer.flipY = transform.position.x < targetPosition.x;
		}

		public override void Dispose() {
			Stop();
			
			if (SignalOnCharactersCollided != null) {
				SignalOnCharactersCollided.RemoveAllListeners();
				SignalOnCharactersCollided = null;
			}

			if (_timer != null) {
				_timer.Stop();
			}
			
			base.Dispose();
		}

		public void CookiesDropped() {
			_collider2D.enabled = false;
			_timer = TimerManager.Instance.SetTimeout(2f, EnableCollider);
		}

		private void EnableCollider() {
			_collider2D.enabled = true;
		}
	}
}