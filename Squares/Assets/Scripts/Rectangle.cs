using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Rectangle:MonoBehaviour
    {
        /// <summary>
        /// для хранения связанного прямоугольника
        /// </summary>
        private Rectangle pair;
        /// <summary>
        /// для хранения спрайта объекта
        /// </summary>
        private SpriteRenderer sprite;
        /// <summary>
        /// здесь храним лайнрендерер для отрисовки связей прямоугольников
        /// </summary>
        private LineRenderer lRenderer;
        /// <summary>
        /// флаг поиска пары для лайнрендерера
        /// </summary>
        private bool readyToPair = false;
        /// <summary>
        /// флаг наличия у объекта пары
        /// </summary>
        private bool isaPair = false;
        /// <summary>
        /// флаг принадлежности объекта к главному в паре, лайнрендерер будет рисоваться им
        /// </summary>
        private bool firstOfPair = false;
        
        private void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            //Меняем цвет прямоугольника
            sprite.color = Random.ColorHSV(0, 1f);
            lRenderer = GetComponent<LineRenderer>();
            StartCoroutine(Starting());
        }

        /// <summary>
        /// для обновления объекта контроллером
        /// </summary>
        public void tick()
        {
            if (readyToPair)
            {
                lRenderer.enabled = true;
                lRenderer.SetPosition(0,gameObject.transform.position);
                lRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));//прорисовка линии до курсора
            }
            if (firstOfPair && isaPair)
            {
                lRenderer.enabled = true;
                lRenderer.SetPosition(0, gameObject.transform.position);
                lRenderer.SetPosition(1, pair.gameObject.transform.position);//прорисовка линии до парного объекта
            }
        }

        /// <summary>
        /// корутина, чтобы объект становился доступен для перетягивания не сразу
        /// </summary>
        /// <returns></returns>
        private IEnumerator Starting()
        {
            yield return new WaitForSeconds(0.4f);
            gameObject.tag = "Player";
            yield break;
        }

        /// <summary>
        /// правый клик по объекту
        /// </summary>
        /// <param name="pairing">1 для начала поиска пары, 2 для пары</param>
        /// <param name="already">out переменная для флага присутствия пары у прямоугольника</param>
        /// <param name="isPairing">out переменная для флага поиска пары</param>
        public void RightClick(out bool already,out bool isPairing)
        {
            already = isaPair;
            isPairing = readyToPair;
            if (isaPair == false && readyToPair == false)
            {
                readyToPair = true;
            }
        }

        /// <summary>
        /// метод для добавления пары объекту
        /// </summary>
        /// <param name="pairObj"></param>
        public void Pair(Rectangle pairObj)
        {
            if (readyToPair)
            {
                pair = pairObj;
                readyToPair = false;
                isaPair = true;
                firstOfPair = true;
                pairObj.Pair(this);
            }
            else
            {
                pair = pairObj;
                isaPair = true;
            }
        }

        /// <summary>
        /// очистка флагов
        /// </summary>
        public void Clear()
        {
            readyToPair = false;
            isaPair = false;
            firstOfPair = false;
            pair = null;
            lRenderer.enabled = false;
        }

        /// <summary>
        /// удаление объекта, очистка флагов у пары
        /// </summary>
        public void Destr()
        {
            if(pair!=null)
                pair.Clear();
            Clear();
        }
    }
}