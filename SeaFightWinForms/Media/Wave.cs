using SeaFightWinForms.Media.Enums;

namespace SeaFightWinForms.Media
{
    /// <summary>
    /// Модель отображения волны
    /// </summary>
    class Wave
    {
        /// <summary>
        /// Начало (координата X)
        /// </summary>
        public int StartsWithX { get; set; }

        /// <summary>
        /// Конец (координата X)
        /// </summary>
        public int EndsWithX { get; set; }

        /// <summary>
        /// Координата Y
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Направление изменения
        /// </summary>
        public WavesDirection GrowDirection { get; set; }
    }
}
