namespace Assets.Scripts.Controllers
{
    public abstract class BaseController
    {
        /// <summary>
        /// Признак активности контроллера
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Метод вызываемый при обновлении сцены каждый фрейм
        /// </summary>
        public abstract void OnUpdate();


        /// <summary>
        /// Включить контроллер
        /// </summary>
        public void On()
        {
            IsActive = true;
        }

        /// <summary>
        /// Отключить контроллер
        /// </summary>
        public void Off()
        {
            IsActive = false;
        }

    }
}