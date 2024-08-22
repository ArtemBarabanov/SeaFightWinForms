using SeaFightWinForms.Presenter;
using System.Resources;
using SeaFightWinForms.Properties;
using SeaFightWinForms.UIElements;
using SeaFightToolkit.Common.Enums;
using SeaFightWinForms.Media.Interfaces;
using SeaFightToolkit.ClassicSeaFight.Constants;

namespace SeaFightWinForms
{
    /// <summary>
    /// Игровое окно [Graphics]
    /// </summary>
    public partial class GameForm : Form, IView
    {
        private readonly ISound _sound;
        private readonly IAnimation _animation;
        private readonly ResourceManager _imageResources;
        private readonly SynchronizationContext _uiContext;

        public GameForm(ISound sound, IAnimation animation)
        {
            InitializeComponent();
            _imageResources = new ResourceManager(typeof(Resources));
            _uiContext = SynchronizationContext.Current!;
            _sound = sound;
            _animation = animation;
            _sound.SeaAndGulls();
        }

        public event Action<int, int>? PlayerFieldClickEvent;
        public event Action<int, int>? MouseInEvent;
        public event Action? MouseOutEvent;
        public event Action? ShipDirectionEvent;
        public event Action<int>? ChangeShipTypeEvent;
        public event Action? StartGameEvent;
        public event Action<int, int>? OpponentFieldClickEvent;
        public event Action? AutoGenerateShipsEvent;
        public event Action? AbortGameEvent;
        public event Action<int, int>? MoveFinished;

        //Выстрел игрока
        public void PlayerTargetHit((int, int) destinationPoint)
        {
            (int x, int y) = destinationPoint;
            Task.Run(() =>
            {
                Invoke(new Action(() => { 
                _opponentPctrMas![x, y].Image = Resources.Deck; }
                ));
                _sound.Explosion();
                _animation.Explosion(_opponentPctrMas![x, y]);
                _animation.StartSmoke(_opponentPctrMas[x, y]);
                OnMoveFinished(x, y);
            });
        }

        //Промах игрока
        public void PlayerTargetMiss((int, int) destinationPoint)
        {
            (int x, int y) = destinationPoint;
            Task.Run(() =>
            {
                _sound.Pluk();
                _animation.WaterSplash(_opponentPctrMas![x, y]);
                Invoke(new Action(() =>
                {
                    _opponentPctrMas[x, y].Image = Resources.VisitedFlag;
                }));
            });
        }

        //Выстрел оппонента
        public void OpponentTargetHit((int, int) destinationPoint)
        {
            (int x, int y) = destinationPoint;
            Task.Run(() =>
            {
                _sound.Explosion();
                _animation.Explosion(_playerPctrMas![x, y]);
                _animation.StartSmoke(_playerPctrMas[x, y]);
                OnMoveFinished(x, y);
            });
        }

        //Промах оппонента
        public void OpponentTargetMiss((int, int) destinationPoint)
        {
            (int x, int y) = destinationPoint;
            Task.Run(() =>
            {
                _sound.Pluk();
                _animation.WaterSplash(_playerPctrMas![x, y]);
            });
        }

        private void CompPctr_Click(object? sender, EventArgs e)
        {
            var pictureBox = sender as SeaCellPictureBox;
            OnOpponentFieldClickEvent(pictureBox!.X, pictureBox!.Y);
        }

        #region Перемещение курсора по полю игрока/клик по полю игрока

        private void Pctr_MouseLeave(object? sender, EventArgs e)
        {
            OnMouseOutEvent();
        }

        private void Pctr_MouseEnter(object? sender, EventArgs e)
        {
            var pictureBox = sender as SeaCellPictureBox;
            OnMouseInEvent(pictureBox!.X, pictureBox!.Y);
        }

        private void PlayerPctr_Click(object? sender, EventArgs e)
        {
            var pctr = sender as SeaCellPictureBox;
            OnPlayerFieldClickEvent(pctr!.X, pctr!.Y);
        }

        public void BackTransperent(List<(int, int)> coordinates)
        {
            foreach((int x, int y) in coordinates)
            {
                _playerPctrMas![x, y].Image = null;
            }
        }

        public void ChangeColorBad(List<(int, int)> coordinates)
        {
            foreach ((int x, int y) in coordinates)
            {
                _playerPctrMas![x, y].Image = Resources.BadPosition;
            }
        }

        public void ChangeColorGood(List<(int, int)> coordinates)
        {
            foreach ((int x, int y) in coordinates)
            {
                _playerPctrMas![x, y].Image = Resources.GoodPosition;
            }
        }

        #endregion

        public void PlacePlayerShip(List<(int, int)> coordinates)
        {
            foreach ((int x, int y) in coordinates)
            {
                _playerPctrMas![x, y].Image = Resources.Deck;
            }
        }

        private void RadioPctr_Click(object? sender, EventArgs e)
        {
            var shipType = sender as ShipTypeRadioBox;
            shipType!.Checked = true;
            shipType.Image = (Image)_imageResources.GetObject($"{shipType.DeckNumber}Deckchosen")!;
            OnChangeShipTypeEvent(shipType.DeckNumber);

            foreach(var radioShipType in _radioPctr!.Where(r => r.DeckNumber != shipType.DeckNumber))
            {
                radioShipType.Checked = false;
                radioShipType.Image = (Image)_imageResources.GetObject($"{radioShipType.DeckNumber}Deck")!;
            }
        }

        #region Смена направления корабля

        private void OrientationPctr_Click(object? sender, EventArgs e)
        {
            OnShipDirectionEvent();
        }

        public void ArrowHorizontal()
        {
            _orientationPctr!.Image = Resources.RudderHorizontal;
        }

        public void ArrowVertical()
        {
            _orientationPctr!.Image = Resources.RudderVertical;
        }

        #endregion

        //Изменение счетчика однопалубных кораблей
        public void ChangeOneDeckCount(int x)
        {
            _one!.Image = (Image)_imageResources.GetObject($"X{x}")!;
        }

        //Изменение счетчика двупалубных кораблей
        public void ChangeTwoDeckCount(int x)
        {
            _two!.Image = (Image)_imageResources.GetObject($"X{x}")!;
        }

        //Изменение счетчика трехпалубных кораблей
        public void ChangeThreeDeckCount(int x)
        {
            _three!.Image = (Image)_imageResources.GetObject($"X{x}")!;
        }

        //Изменение счетчика четырехпалубных кораблей
        public void ChangeFourDeckCount(int x)
        {
            _four!.Image = (Image)_imageResources.GetObject($"X{x}")!;
        }

        //Начало игры
        private void StartButton_Click(object? sender, EventArgs e)
        {
            _sound.StopSeaAndGulls();
            _sound.Rynda();
            OnStartGameEvent();
        }

        //Отображение информации, кто ходит первым
        public void BeginGame(Participants who)
        {
            if (who == Participants.Opponent)
            {
                MessageBox.Show(Resources.ComputerFirstTurnText);
            }
            else
            {
                MessageBox.Show(Resources.YourFirstTurnText);
            }
            CreateGameSessionScreen();
        }

        //Изменение счетчиков кораблей игрока во время боя
        public void DecreasePlayerShipCount(int deckNumber, int liveShips)
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                if (deckNumber == SessionConstants.OneDeckShipDecksCount)
                {
                    _one!.Image = (Image)_imageResources.GetObject($"X{liveShips}")!;
                }
                else if (deckNumber == SessionConstants.TwoDeckShipDecksCount)
                {
                    _two!.Image = (Image)_imageResources.GetObject($"X{liveShips}")!;
                }
                else if (deckNumber == SessionConstants.ThreeDeckShipDecksCount)
                {
                    _three!.Image = (Image)_imageResources.GetObject($"X{liveShips}")!;
                }
                else if (deckNumber == SessionConstants.FourDeckShipDecksCount)
                {
                    _four!.Image = (Image)_imageResources.GetObject($"X{liveShips}")!;
                }
            }), null);
        }

        //Изменение счетчиков кораблей противника во время боя
        public void DecreaseOpponentShipCount(int deckNumber, int liveShips)
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                if (deckNumber == SessionConstants.OneDeckShipDecksCount)
                {
                    _compOne!.Image = (Image)_imageResources.GetObject($"X{liveShips}")!;
                }
                else if (deckNumber == SessionConstants.TwoDeckShipDecksCount)
                {
                    _compTwo!.Image = (Image)_imageResources.GetObject($"X{liveShips}")!;
                }
                else if (deckNumber == SessionConstants.ThreeDeckShipDecksCount)
                {
                    _compThree!.Image = (Image)_imageResources.GetObject($"X{liveShips}")!;
                }
                else if (deckNumber == SessionConstants.FourDeckShipDecksCount)
                {
                    _compFour!.Image = (Image)_imageResources.GetObject($"X{liveShips}")!;
                }
            }), null);
        }

        //Формирование экрана победы
        public void Victory(Participants who)
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                CreateVictoryScreen();

                if (who == Participants.Opponent)
                {
                    _winPctr!.Image = Resources.Lost;
                }
                else
                {
                    _winPctr!.Image = Resources.Victory;
                }
            }), null);
        }

        //Смена типа корабля при расстановке
        public void ChangeShipType(int decks)
        {
            for (int i = 0; i < 4; i++)
            {
                if (_radioPctr![i].DeckNumber != decks)
                {
                    _radioPctr[i].Checked = false;
                    _radioPctr[i].Image = (Image)_imageResources.GetObject($"{_radioPctr[i].DeckNumber}Deck")!;
                }
                else
                {
                    _radioPctr[i].Checked = true;
                    _radioPctr[i].Image = (Image)_imageResources.GetObject($"{_radioPctr[i].DeckNumber}Deckchosen")!;
                }
            }
        }
        //Обработка нажатия на кнопку Лень
        private void LazyPctr_Click(object? sender, EventArgs e)
        {
            OnAutoGenerateShipsEvent();
        }

        /// <summary>
        /// Обработка перемещения курсора на кнопку Лень
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private void LazyPctr_MouseEnter(object? sender, EventArgs e)
        {
            lazyPctr.Image = Resources.LazyChosen;
        }

        /// <summary>
        /// Обработка перемещения курсора с кнопки Лень
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private void LazyPctr_MouseLeave(object? sender, EventArgs e)
        {
            lazyPctr.Image = Resources.Lazy;
        }

        /// <summary>
        /// Скрытие кнопки В бой
        /// </summary>
        public void HideStartButton()
        {
            startButton.Visible = false;
        }

        /// <summary>
        /// Отображение кнопки В бой
        /// </summary>
        public void ShowStartButton()
        {
            startButton.Visible = true;
        }

        /// <summary>
        /// Обработка клика по кнопке Сворачивания окна
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private void MinimizePctr_Click(object? sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Обработка клика по кнопке закрытия формы
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private void ClosePctr_Click(object? sender, EventArgs e)
        {
            _sound.StopSeaAndGulls();
            var main = Owner as StartForm;
            main?.Show();
            OnAbortGameEvent();
            Close();
        }

        //Маркировка уничтоженного корабля игрока
        public void MarkPlayerDestroyedShip(List<(int, int)> decks)
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                foreach (var (x, y) in decks)
                {
                    _playerPctrMas![x, y].Image = Resources.Destroyed;
                }
            }), null);
        }

        //Маркировка уничтоженного корабля противника
        public void MarkOpponentDestroyedShip(List<(int, int)> decks)
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                foreach (var (x, y) in decks)
                {
                    _opponentPctrMas![x, y].Image = Resources.Destroyed;
                }
            }), null);
        }

        //Начало сетевой игры
        public void BeginNetGame(string whoIsFirst)
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                CreateGameSessionScreen(); MessageBox.Show($"{Resources.PlayersAreReadyText} {whoIsFirst}");
            }), null);
        }

        //Отображение хода игрока
        public void MarkMyTurn()
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                _turn!.Image = Resources.MyTurn;
            }), null);
        }

        //Отображение хода противника
        public void MarkOpponentTurn()
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                _turn!.Image = Resources.OpponentTurn;
            }), null);
        }

        //Победа игрока в сетевой игре
        public void NetMyVictory()
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                CreateVictoryScreen();
                _winPctr!.Image = Resources.Victory;
            }), null);
        }

        //Победа противника в сетевой игре
        public void NetOpponentVictory()
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                CreateVictoryScreen();
                _winPctr!.Image = Resources.Lost;
            }), null);
        }

        //Прерывание сетевой игры одним из игроков
        public void NetOpponentAbortGame(string name)
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                _sound.StopSeaAndGulls();
                Close();
                MessageBox.Show($"{name} {Resources.PlayerLeftGameText}");
            }), null);
        }

        private void GameForm_Load(object? sender, EventArgs e)
        {
            CreateGameMenu();
            CreateFields();
        }

        private void ClosePctr_MouseEnter(object? sender, EventArgs e)
        {
            closePctr.Image = Resources.CloseChosen;
        }

        private void ClosePctr_MouseLeave(object? sender, EventArgs e)
        {
            closePctr.Image = Resources.Close;
        }

        private void MinimizePctr_MouseEnter(object? sender, EventArgs e)
        {
            minimizePctr.Image = Resources.MinimizeChosen;
        }

        private void MinimizePctr_MouseLeave(object? sender, EventArgs e)
        {
            minimizePctr.Image = Resources.Minimize;
        }

        private void StartButton_MouseEnter(object? sender, EventArgs e)
        {
            startButton.Image = Resources.FightChosen;
        }

        private void StartButton_MouseLeave(object? sender, EventArgs e)
        {
            startButton.Image = Resources.Fight;
        }

        protected virtual void OnPlayerFieldClickEvent(int x, int y) 
        {
            var raiseEvent = PlayerFieldClickEvent;
            if (raiseEvent != null)
            {
                raiseEvent(x, y);
            }
        }

        protected virtual void OnOpponentFieldClickEvent(int x, int y)
        {
            var raiseEvent = OpponentFieldClickEvent;
            if (raiseEvent != null)
            {
                raiseEvent(x, y);
            }
        }

        protected virtual void OnMouseInEvent(int x, int y)
        {
            var raiseEvent = MouseInEvent;
            if (raiseEvent != null)
            {
                raiseEvent(x, y);
            }
        }

        protected virtual void OnMouseOutEvent()
        {
            var raiseEvent = MouseOutEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnShipDirectionEvent()
        {
            var raiseEvent = ShipDirectionEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnStartGameEvent()
        {
            var raiseEvent = StartGameEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnAutoGenerateShipsEvent()
        {
            var raiseEvent = AutoGenerateShipsEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnAbortGameEvent()
        {
            var raiseEvent = AbortGameEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnChangeShipTypeEvent(int deckNumber)
        {
            var raiseEvent = ChangeShipTypeEvent;
            if (raiseEvent != null)
            {
                raiseEvent(deckNumber);
            }
        }

        protected virtual void OnMoveFinished(int x, int y)
        {
            var raiseEvent = MoveFinished;
            if (raiseEvent != null)
            {
                raiseEvent(x, y);
            }
        }
    }
}