using SeaFightWinForms.UIElements;

namespace SeaFightWinForms.Media.Interfaces
{
    /// <summary>
    /// Интерфейс работы со анимацией
    /// </summary>
    public interface IAnimation
    {
        /// <summary>
        /// Анимация взрыва
        /// </summary>
        /// <param name="seaCell">Клетка для запуска анимации взрыва</param>
        void Explosion(SeaCellPictureBox seaCell);

        /// <summary>
        /// Анимация всплеска воды
        /// </summary>
        /// <param name="seaCell">Клетка для запуска анимации всплеска воды</param>
        void WaterSplash(SeaCellPictureBox seaCell);

        /// <summary>
        /// Анимация повреждения
        /// </summary>
        /// <param name="seaCell">Клетка для запуска анимации повреждения</param>
        void StartSmoke(SeaCellPictureBox seaCell);
    }
}
