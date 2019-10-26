﻿using gRaFFit.Agar.Controllers.InputSystem;
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
        [SerializeField] private float _cameraOffsetMultiplier;
        
#pragma warning restore 649

        private PlayerView _player;

        private float _cachedCameraZPosition;

        public void Update() {
            if (_player == null) return;

            var cameraTargetPosition =
                new Vector3(_player.transform.position.x, _player.transform.position.y, _cachedCameraZPosition);

            if (InputController.Instance.IsTouch()) {
                cameraTargetPosition += (Vector3) _player.GetTouchOffset() * _cameraOffsetMultiplier;
            }

            transform.position = Vector3.Lerp(transform.position, cameraTargetPosition, _cameraSpeed * Time.deltaTime);
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
            transform.position = _player.transform.position;
        }
    }
}