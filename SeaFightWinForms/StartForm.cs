using SeaFightWinForms.Media.Interfaces;
using SeaFightWinForms.Presenter;

namespace SeaFightWinForms
{
    /// <summary>
    /// Стартовое окно
    /// </summary>
    public partial class StartForm : Form, IStartView
    {
        private CancellationTokenSource? _ctsConnected;
        private CancellationTokenSource? _ctsError;
        private CancellationTokenSource? _ctsCancel;
        private readonly SynchronizationContext _uiContext;
        private readonly IAnimation _animation;
        private readonly ISound _sound;
        private GameForm? _gameForm;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sound">Интерфейс работы со звуком</param>
        /// <param name="animation">Интерфейс работы с анимацией</param>
        public StartForm(ISound sound, IAnimation animation)
        {
            InitializeComponent();
            _uiContext = SynchronizationContext.Current!;
            CreateStartScreen();
            _sound = sound;
            _animation = animation;
        }

        /// <summary>
        /// Обработчик клика по кнопке Игра с компьютером
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private void ComputerGameButtonClick(object? sender, EventArgs e)
        {
            _gameForm = new GameForm(_sound, _animation)
            {
                Owner = this
            };
            _gameForm.Show();
            Hide();
            OnStartOfflineGameEvent(_gameForm);
        }

        /// <summary>
        /// Обработчик клика по кнопке Игра по сети
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private void NetGameButtonClick(object? sender, EventArgs e)
        {
            CreateChatScreen();
        }

        /// <summary>
        /// Обработчик закрытия формы
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private void StartFormClosed(object? sender, FormClosedEventArgs e)
        {
            _gameForm?.Close();
            OnDisconnectEvent();
        }
    }
}
