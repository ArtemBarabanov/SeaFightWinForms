namespace SeaFightWinForms.Media.Interfaces
{
    /// <summary>
    /// Интерфейс работы со звуками
    /// </summary>
    public interface ISound
    {
        /// <summary>
        /// Проигрывание звука взрыва
        /// </summary>
        void Explosion();

        /// <summary>
        /// Проигрывание крика чаек и плеска волн
        /// </summary>
        void SeaAndGulls();

        /// <summary>
        /// Остановка проигрывания крика чаек и плеска волн
        /// </summary>
        void StopSeaAndGulls();

        /// <summary>
        /// Проигрывание звука выстрела
        /// </summary>
        void Shot();

        /// <summary>
        /// Проигрывание звука всплеска
        /// </summary>
        void Pluk();

        /// <summary>
        /// Пригрывание удара в рынду
        /// </summary>
        void Rynda();
    }
}
