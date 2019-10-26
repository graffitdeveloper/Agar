using gRaFFit.Agar.Models;
using gRaFFit.Agar.Views.CameraControls;
using UnityEngine;
using UnityEngine.EventSystems;

namespace gRaFFit.Agar.Views.InputControls {
    public class InputController : MonoBehaviour {
        #region Singleton

        private InputController() {
        }

        private static InputController _instance;

        public static InputController Instance {
            get {
                if (_instance == null) {
                    var gameObject = new GameObject("InputController");
                    _instance = gameObject.AddComponent<InputController>();
                }

                return _instance;
            }
        }

        #endregion

        private InputDirection _currentMovingDirection = InputDirection.None;

        private CameraView _cameraView;

        public InputDirection CurrentMovingDirection {
            get { return _currentMovingDirection; }
            private set { _currentMovingDirection = value; }
        }

        public void Update() {
            if (GameManager.Instance.IsGamePaused) {
                CurrentMovingDirection = InputDirection.None;
                return;
            }

            CheckDirectionsInput();

            if (!EventSystem.current.IsPointerOverGameObject())
                CheckMouseInput();
        }

        private void CheckMouseInput() {
            if (Input.GetMouseButton(0)) {
                var cameraOffset = _cameraView.GetCameraOffset();

                var playerPosition =
                    Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f)) +
                    cameraOffset;
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                var distance = new Vector3(Mathf.Abs(mousePosition.x - playerPosition.x),
                    Mathf.Abs(mousePosition.y - playerPosition.y), 0);

                if (distance.x < distance.y)
                    CurrentMovingDirection =
                        mousePosition.y > playerPosition.y ? InputDirection.Up : InputDirection.Down;
                else if (distance.x > distance.y)
                    CurrentMovingDirection =
                        mousePosition.x > playerPosition.x ? InputDirection.Right : InputDirection.Left;
            }
        }

        private void CheckDirectionsInput() {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                CurrentMovingDirection = InputDirection.Up;

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                CurrentMovingDirection = InputDirection.Right;

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                CurrentMovingDirection = InputDirection.Down;

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                CurrentMovingDirection = InputDirection.Left;

            if (!Input.anyKey)
                CurrentMovingDirection = InputDirection.None;
        }

        public void SetCamera(CameraView cameraView) {
            _cameraView = cameraView;
        }
    }
}