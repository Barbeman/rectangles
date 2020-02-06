using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class InputController:BaseController
    {
        /// <summary>
        /// таймер двойного клика
        /// </summary>
        private float timerForDoubleClick = 0.0f;
        /// <summary>
        /// задержка между двойными кликами
        /// </summary>
        private float delay = 0.15f;
        /// <summary>
        /// флаг для отслеживания двойного клика
        /// </summary>
        private bool isDoubleClick = false;

        public override void OnUpdate()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Main.Instance.DragAndDropController.RightBtnClick(true);//при нажатии правой кнопки передаем значение в нужный контроллер
            }
            if (Input.GetMouseButtonDown(0))
            {
                Main.Instance.ObjectsController.MouseClick(true);//при нажатии левой кнопки передаем значение в нужный контроллер
            }
            if (Input.GetMouseButtonUp(0) && isDoubleClick == false)
            {
                isDoubleClick = true;
            }
            if (isDoubleClick == true)
            {
                timerForDoubleClick += Time.deltaTime;
            }
            if (timerForDoubleClick >= delay)
            {
                timerForDoubleClick = 0.0f;
                isDoubleClick = false;
            }
            if (Input.GetMouseButtonDown(0) && timerForDoubleClick < delay && isDoubleClick == true)
            {
                Main.Instance.DragAndDropController.DoubleClick(isDoubleClick);//передаем двойной клик при необходимом значении таймера и флага
            }
            if (Input.GetMouseButton(0))
            {
                Main.Instance.DragAndDropController.MouseBtn(true);
            }

        }
    }
}