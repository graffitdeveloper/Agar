using UnityEngine;

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

#pragma warning restore 649

        private Transform _playerTransform;

        private float _cachedCameraZPosition;

        public void Update() {
            if (_playerTransform == null) return;

            var cameraTargetPosition = new Vector3(_playerTransform.position.x, _playerTransform.position.y,
                _cachedCameraZPosition);

            transform.position = Vector3.Lerp(transform.position, cameraTargetPosition, _cameraSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Возвращает сдвиг камеры относительно кота; векторное расстояние, которое должна пройти камера что бы
        /// достичь игрока
        /// </summary>
        public Vector3 GetCameraOffset() {
            if (_playerTransform == null)
                return Vector3.zero;

            return _playerTransform.position - transform.position;
        }
    }
}