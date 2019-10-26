using System;
using gRaFFit.Agar.Utils;
using gRaFFit.Agar.Views.InputControls;
using UnityEngine;

namespace gRaFFit.Agar.Views.CameraControls {
    /// <summary>
    /// Скрипт игрока
    /// </summary>
    public class CameraView : MonoBehaviour {

#pragma warning disable 649

        [SerializeField] private float _cameraSpeed;

        [SerializeField] private float _cameraDirectionOffset;

        [SerializeField] private float _cameraOrthoChangeSpeed;

        [SerializeField] private float _cameraWalkOrthoValue;

#pragma warning restore 649

        private Transform _playerTransform;

        private float _cachedCameraZPosition;

        private float _cachedCameraOrtho;

        private Camera _thisCamera;
        public bool ReactToControls { get; set; }

        public void Awake() {
            _cachedCameraZPosition = transform.position.z;
            _cachedCameraOrtho = CachedMainCamera.Instance.Camera.orthographicSize;
            _thisCamera = GetComponent<Camera>();
            InputController.Instance.SetCamera(this);
        }

        public void SetToPlayer(Transform playerTransform) {
            _playerTransform = playerTransform;
            transform.position = playerTransform.position;
        }

        public void Update() {
            if (_playerTransform == null) return;

            var cameraTargetPosition = new Vector3(_playerTransform.position.x, _playerTransform.position.y,
                _cachedCameraZPosition);

            if (ReactToControls) {
                switch (InputController.Instance.CurrentMovingDirection) {
                    case InputDirection.Up:
                        cameraTargetPosition += new Vector3(0, _cameraDirectionOffset, 0);
                        break;

                    case InputDirection.Right:
                        cameraTargetPosition += new Vector3(_cameraDirectionOffset, 0, 0);
                        break;

                    case InputDirection.Down:
                        cameraTargetPosition += new Vector3(0, -_cameraDirectionOffset, 0);
                        break;

                    case InputDirection.Left:
                        cameraTargetPosition += new Vector3(-_cameraDirectionOffset, 0, 0);
                        break;

                    case InputDirection.None:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            transform.position = Vector3.Lerp(transform.position, cameraTargetPosition, _cameraSpeed * Time.deltaTime);
            SetOrtho();
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

        private void SetOrtho() {
            if (ReactToControls) {
                _thisCamera.orthographicSize = Mathf.Lerp(_thisCamera.orthographicSize,
                    InputController.Instance.CurrentMovingDirection == InputDirection.None
                        ? _cachedCameraOrtho
                        : _cameraWalkOrthoValue, _cameraOrthoChangeSpeed * Time.deltaTime);
            } else {
                _thisCamera.orthographicSize = Mathf.Lerp(_thisCamera.orthographicSize,
                    _cachedCameraOrtho, _cameraOrthoChangeSpeed * Time.deltaTime);
            }
        }
    }
}