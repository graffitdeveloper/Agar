using UnityEngine;

namespace gRaFFit.Agar.Controllers.InputSystem {
    /// <summary>
    /// Контроллер ввода для мыши
    /// </summary>
    public class MouseInputController : InputController {
        #region Overrides

        /// <summary>
        /// Проверка ввода
        /// </summary>
        protected override void CheckInput() {
            if (Input.GetMouseButtonDown(0)) {
                HandleTouchStart(Input.mousePosition);
            } else if (Input.GetMouseButton(0)) {
                HandleTouch(Input.mousePosition);
            } else if (Input.GetMouseButtonUp(0)) {
                HandleTouchEnd(Input.mousePosition);
            }
        }

        public override Vector2 GetTouchPosition() {
            return Input.mousePosition;
        }

        #endregion
    }
}