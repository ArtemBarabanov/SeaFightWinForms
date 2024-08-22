using System.Resources;
using SeaFightWinForms.Media.Interfaces;
using SeaFightWinForms.UIElements;

namespace SeaFightWinForms.Media
{
    /// <summary>
    /// Класс анимации
    /// </summary>
    class Animation : IAnimation
    {
        private System.Windows.Forms.Timer _frameTimer = new System.Windows.Forms.Timer();
        private ResourceManager _imageResources = new ResourceManager(typeof(Properties.Resources));
        private IEnumerator<(double, double)> _flyPath = default!;
        private PictureBox _shellPictureBox = default!;

        public Animation()
        {
            _frameTimer.Tick += FrameTimer_Tick;
            _frameTimer.Interval = 100;
        }

        private void FrameTimer_Tick(object? sender, EventArgs e)
        {
            if (_flyPath.MoveNext())
            {
                _shellPictureBox.Location = new Point((int)_flyPath.Current.Item1, (int)_flyPath.Current.Item2);
            }
        }

        /// <summary>
        /// Анимация взрыва
        /// </summary>
        /// <param name="seaCell">Клетка для запуска анимации взрыва</param>
        public void Explosion(SeaCellPictureBox seaCell)
        {
            for (int i = 1; i < 15; i++)
            {
                seaCell!.Image = _imageResources.GetObject($"Explosion{i}") as Image;
                Thread.Sleep(50);
            }
            for (int i = 14; i > 0; i--)
            {
                seaCell!.Image = _imageResources.GetObject($"Explosion{i}") as Image;
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Анимация повреждения
        /// </summary>
        /// <param name="seaCell">Клетка для запуска анимации повреждения</param>
        public void StartSmoke(SeaCellPictureBox seaCell)
        {
            for (int i = 1; i < 7; i++)
            {
                seaCell!.Image = _imageResources.GetObject($"Damage{i}") as Image;
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Анимация всплеска воды
        /// </summary>
        /// <param name="seaCell">Клетка для запуска анимации всплеска воды</param>
        public void WaterSplash(SeaCellPictureBox seaCell)
        {
            for (int i = 1; i < 14; i++)
            {
                seaCell!.Image = _imageResources.GetObject($"Miss{i}") as Image;
                Thread.Sleep(50);
            }
        }
    }
}
