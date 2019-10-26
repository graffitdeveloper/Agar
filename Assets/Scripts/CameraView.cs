using gRaFFit.Agar.Controllers.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace gRaFFit.Agar.Views.CameraControls {
    /// <summary>
    /// Скрипт игрока
    /// </summary>
    public class CameraView : MonoBehaviour {

        #region MonoSingleton

        private CameraView() {

        }

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(Instance);
            }

            _cachedCameraZPosition = transform.position.z;

            Instance = this;
        }

        public static CameraView Instance { get; private set; }

        #endregion

#pragma warning disable 649

        [SerializeField] private float _cameraSpeed;
        [SerializeField] private float _cameraOffsetMultiplier;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _basicOrtho;
        [SerializeField] private float _weightOrthoCost;
        
#pragma warning restore 649

        private PlayerView _player;

        private float _cachedCameraZPosition;

        private float _targetOrtho;

        public void Update() {
            if (_player == null) return;

            var cameraTargetPosition =
                new Vector3(_player.transform.position.x, _player.transform.position.y, _cachedCameraZPosition);

            if (InputController.Instance.IsTouch() && !EventSystem.current.IsPointerOverGameObject()) {
                cameraTargetPosition += (Vector3) _player.GetTouchOffset() * _cameraOffsetMultiplier;
            }

            SetCameraPosition(Vector3.Lerp(transform.position, cameraTargetPosition, _cameraSpeed * Time.deltaTime));
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetOrtho, Time.deltaTime);
        }

        /// <summary>
        /// Возвращает сдвиг камеры относительно кота; векторное расстояние, которое должна пройти камера что бы
        /// достичь игрока
        /// </summary>
        public Vector3 GetCameraOffset() {
            if (_player == null)
                return Vector3.zero;

            return _player.transform.position - transform.position;
        }


        public void SetToPlayer(PlayerView playerTransform) {
            _player = playerTransform;
            SetCameraPosition(_player.transform.position);
        }

        public void SetCameraPosition(Vector3 position) {
            transform.position = new Vector3(position.x, position.y, _cachedCameraZPosition);
        }

        public void SetTargetOrthoAccordingWithWeight(float weight) {
            _targetOrtho = _basicOrtho + weight * _weightOrthoCost;
        }
    }
}