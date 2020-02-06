using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class DragAndDropController:BaseController
    {
        /// <summary>
        /// тэг объектов, которые можно перемещать
        /// </summary>
        private string tag;
        /// <summary>
        /// здесь храним камеру
        /// </summary>
        private Camera camera;
        /// <summary>
        /// здесь храним текущий объект, который можно перетаскивать мышью
        /// </summary>
        private Transform curObj;
        /// <summary>
        /// оффсет и координаты объекта
        /// </summary>
        private Vector3 offset, objPos;
        /// <summary>
        /// флаг для отслеживания нажатой кнопки
        /// </summary>
        private bool mouseDown = false;
        /// <summary>
        /// флаг для отслеживания двойного нажатия
        /// </summary>
        private bool doubleClick = false;
        /// <summary>
        /// флаг для отслеживания нажатия правой кнопки
        /// </summary>
        private bool rightBtnClick = false;
        /// <summary>
        /// флаг для отслеживания ищется ли пара для прямоугольника
        /// </summary>
        private bool pairing = false;
        /// <summary>
        /// флаг для отслеживания есть ли у прямоугольника пара
        /// </summary>
        private bool alreadyPair = false;
        /// <summary>
        /// флаг для отслеживания ищется ли пара для прямоугольника
        /// </summary>
        private bool alreadyPairing = false;
        /// <summary>
        /// здесь храним объект для которого ищется пара
        /// </summary>
        private Rectangle pairingObject;

        /// <summary>
        /// Передаем сюда главную камеру и тэг для объектов, которые можем перемещать
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="tags"></param>
        public DragAndDropController(Camera camera, string tag)
        {
            this.camera = camera;
            this.tag = tag;
        }

        /// <summary>
        /// проверяем тэг
        /// </summary>
        /// <param name="curTag"></param>
        /// <returns></returns>
        bool GetTag(string curTag)
        {
            bool result = false;
            if (tag == curTag) result = true;
            return result;
        }

        public override void OnUpdate()
        {
            if (rightBtnClick)
            {
                if (pairing)
                {
                    RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit.transform != null)
                    {
                        if (GetTag(hit.transform.tag) && hit.rigidbody)
                        {
                            hit.transform.gameObject.GetComponent<Rectangle>().RightClick(out alreadyPair, out alreadyPairing);
                            if (!alreadyPair && !alreadyPairing)//при нажатие на объект без пары делаем делаем его парой закэшированному объекту
                            {
                                pairing = false;
                                pairingObject.Pair(hit.transform.gameObject.GetComponent<Rectangle>());
                                pairingObject = null;
                            }
                            if (!alreadyPair && alreadyPairing)//в противном случае очищаем флаги
                            {
                                pairing = false;
                                pairingObject.Clear();
                                pairingObject = null;
                            }
                        }
                    }
                    else
                    {
                        pairing = false;
                        pairingObject.Clear();
                        pairingObject = null;
                    }
                    alreadyPair = false;
                    alreadyPairing = false;
                }
                if (!pairing)//начинаем искать пару для прямоугольника
                {
                    RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit.transform != null)
                    {
                        if (GetTag(hit.transform.tag) && hit.rigidbody)
                        {
                            hit.transform.gameObject.GetComponent<Rectangle>().RightClick(out alreadyPair,out alreadyPairing);
                            if (!alreadyPair)//при нажатие на объект без пары делаем его объектом для поиска пары
                            {
                                pairing = true;
                                pairingObject = hit.transform.gameObject.GetComponent<Rectangle>();
                            }
                            alreadyPair = false;
                            alreadyPairing = false;
                        }
                    }
                }
                rightBtnClick = false;
            }

            if (mouseDown)
            {
                ///ищем перетягиваемые предметы по тэгам
                RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.transform != null)
                {
                    if (GetTag(hit.transform.tag) && hit.rigidbody && !curObj)
                    {
                        curObj = hit.transform;
                        curObj.GetComponent<Rigidbody2D>().freezeRotation = true;//убираем вращение для dynamic rigidbody
                        offset = hit.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));//ищем оффсет, чтобы можно было перетягивать объект за край
                    }
                }
                if (curObj)
                {
                    if (doubleClick == false)
                    {
                        curObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;//делаем rigidbody dinamic для взаимодействия
                        Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
                        curObj.GetComponent<Rigidbody2D>().MovePosition(new Vector3(mousePosition.x, mousePosition.y) + offset);//объект следует за мышкой
                    }
                    else
                    {
                        curObj.GetComponent<Rectangle>().Destr();
                        Main.Instance.ObjectsController.DeleteRectangle(curObj.GetComponent<Rectangle>());
                        Object.Destroy(curObj.gameObject);//уничтожаем объект при даблклике и удаляем его из контроллера
                    }
                    
                }

                doubleClick = false;
                mouseDown = false;
            }
            else if (curObj) 
            {
                curObj.GetComponent<Rigidbody2D>().freezeRotation = false;
                curObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                curObj = null;
            }
        }

        /// <summary>
        /// передаем нажатие из инпут контроллера
        /// </summary>
        /// <param name="btn"></param>
        public void MouseBtn(bool btn)
        {
            mouseDown = btn;
        }

        /// <summary>
        /// передаем нажатие из инпут контроллера
        /// </summary>
        /// <param name="btn"></param>
        public void DoubleClick(bool btn)
        {
            doubleClick = btn;
        }

        public void RightBtnClick(bool btn)
        {
            rightBtnClick = btn;
        }
        
    }
}