using gRaFFit.Agar.Utils;
using gRaFFit.Agar.Views.CameraControls;
using UnityEngine;

namespace gRaFFit.Agar.Views {
	public class PlayerView : MonoBehaviour {
		[SerializeField] private SpriteRenderer _spriteRenderer;
		public void Update() {
			var mousePosition = CachedMainCamera.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);

			transform.rotation = RotationHelper.FaceObject(transform.position, mousePosition, 180f);

			var playerPosition = CachedMainCamera.Instance.Camera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f,
				                     Screen.height * 0.5f)) + CameraView.Instance.GetCameraOffset();

			_spriteRenderer.flipY = playerPosition.x < mousePosition.x;
		}
	}
}