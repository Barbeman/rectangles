using Assets.Scripts.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Основной управляющий скрипт. Должен присутствовать на игровой сцене
    /// </summary>
    public class Main : MonoBehaviour
    {

        public static Main Instance;

        /// <summary>
        /// тут храним префаб прямоугольников
        /// </summary>
        private Rectangle rectanglePrefab;

        /// <summary>
        /// тэг для объектов, которые можно перетаскивать
        /// </summary>
        private string tag = "Player";

        /// <summary>
        /// тут храним камеру
        /// </summary>
        private Camera mainCamera;

        private DragAndDropController dragAndDropController;
        private InputController inputController;
        private ObjectsController objectsController;
        /// <summary>
        /// массив для хранения контроллеров
        /// </summary>
        private BaseController[] controllers;

        internal Rectangle Rectangle
        {
            get => rectanglePrefab;
            private set => rectanglePrefab = value;
        }

        /// <summary>
        /// контроллер перемещаемых объектов
        /// </summary>
        internal DragAndDropController DragAndDropController
        {
            get => dragAndDropController;
            private set => dragAndDropController = value;
        }

        /// <summary>
        /// контроллер ввода
        /// </summary>
        internal InputController InputController
        {
            get => inputController;
            private set => inputController = value;
        }

        /// <summary>
        /// контроллер объектов
        /// </summary>
        internal ObjectsController ObjectsController
        {
            get => objectsController;
            private set => objectsController = value;
        }

        private void Awake()
        {
            Instance = this;

            Rectangle = Resources.Load<Rectangle>("Rectangle"); ///подгружаем префаб из папки resources
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            InputController = new InputController();
            DragAndDropController = new DragAndDropController(mainCamera, tag);
            ObjectsController = new ObjectsController(mainCamera);
            controllers = new BaseController[3];
            controllers[0] = DragAndDropController;
            controllers[1] = InputController;
            controllers[2] = ObjectsController;
            foreach (BaseController contr in controllers)//включаем все контроллеры
            {
                contr.On();
            }

        }

        /// <summary>
        /// апдейтим контроллеры
        /// </summary>
        private void Update()
        {
            foreach (BaseController c in controllers)
            {
                if (c.IsActive) c.OnUpdate();
            }
        }

    }
}

