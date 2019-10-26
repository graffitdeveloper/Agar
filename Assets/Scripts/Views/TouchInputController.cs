using UnityEngine;

namespace gRaFFit.Agar.Controllers.InputSystem {
    /// <summary>
    /// Контроллер ввода для тачскрина
    /// </summary>
    public class TouchInputController : InputController {
        #region Overrides

        /// <summary>
        /// Проверка ввода
        /// </summary>
        protected override void CheckInput() {
            if (Input.touchCount > 0) {
                var exitLoop = false;
                var touches = Input.touches;
                var touchesCount = touches.Length;

                for (int i = 0; i < touchesCount; i++) {
                    var currTouch = touches[i];

                    switch (currTouch.phase) {
                        case TouchPhase.Began:
                            HandleTouchStart(currTouch.position);
                            exitLoop = true;
                            break;
                        case TouchPhase.Moved:
                        case TouchPhase.Stationary:
                            HandleTouch(currTouch.position);
                            exitLoop = true;
                            break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            HandleTouchEnd(currTouch.position);
                            exitLoop = true;
                            break;
                    }

                    if (exitLoop)
                        return;
                }
            }
        }

        public override Vector2 GetTouchPosition() {
            return Input.touches[0].position;
        }

        #endregion
    }
}