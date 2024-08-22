namespace SeaFightWinForms.UIElements
{
    /// <summary>
    /// Радио-кнопка типа корабля
    /// </summary>
    internal class ShipTypeRadioBox : PictureBox
    {
        /// <summary>
        /// Выбрана/невыбрана
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Количество палуб
        /// </summary>
        public int DeckNumber { get; set; }
    }
}
