using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ObjectsController:BaseController
    {
        /// <summary>
        /// здесь храним камеру
        /// </summary>
        private Camera camera;
        /// <summary>
        /// флаг для отслеживания того, можно ли создавать объекты
        /// </summary>
        private bool iCanPlace = false;
        /// <summary>
        /// флаг для отслеживания нажатия кнопки
        /// </summary>
        private bool mouseClick = false;
        /// <summary>
        /// все прямоугольники на сцене
        /// </summary>
        private List<Rectangle> rectangles=new List<Rectangle>();

        public ObjectsController(Camera camera)
        {
            this.camera = camera;
        }

        public override void OnUpdate()
        {
            //ставим прямоугольник если область не занята
            if (iCanPlace)
            {
                rectangles.Add(Rectangle.Instantiate(Main.Instance.Rectangle, camera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10), Quaternion.identity));
            }
            //апдейтим все прямоугольники
            foreach (Rectangle rec in rectangles)
            {
                if(rec != null)
                rec.tick();
            }
            iCanPlace = false;
            mouseClick = false;

        }

        private void CanIPlaceHere(Vector3 start)
        {
            //кидаем лучи для проверки, прямоугольников в соседних областях
            RaycastHit2D hitUp = Physics2D.Raycast(start, Vector2.up, 0.5f);
            RaycastHit2D hitDown = Physics2D.Raycast(start, Vector2.down, 0.5f);
            RaycastHit2D hitLeft = Physics2D.Raycast(start, Vector2.left, 1f);
            RaycastHit2D hitRight = Physics2D.Raycast(start, Vector2.right, 1f);
            RaycastHit2D hitUpL = Physics2D.Raycast(start, new Vector2(-1,0.5f), 1.118f);
            RaycastHit2D hitUpR = Physics2D.Raycast(start, new Vector2(1, 0.5f), 1.118f);
            RaycastHit2D hitDownL = Physics2D.Raycast(start, new Vector2(-1, -0.5f), 1.118f);
            RaycastHit2D hitDownR = Physics2D.Raycast(start, new Vector2(1, -0.5f), 1.118f);

            if (!hitRight && !hitLeft && !hitUp && !hitDown && !hitUpL && !hitUpR && !hitDownL && !hitDownR)
            {
                iCanPlace = true;
            }
            
        }

        /// <summary>
        /// передаем нажатие из инпут контроллера
        /// </summary>
        /// <param name="btn"></param>
        public void MouseClick(bool btn)
        {
            CanIPlaceHere(camera.ScreenToWorldPoint(Input.mousePosition));
            if (iCanPlace)
            {
                mouseClick = true;
            }
        }

        /// <summary>
        /// для удаления объектов при уничтожение
        /// </summary>
        /// <param name="rect"></param>
        public void DeleteRectangle(Rectangle rect)
        {
            rectangles.Remove(rect);
        }
    }
}