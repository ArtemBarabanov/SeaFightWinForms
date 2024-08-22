using SeaFightToolkit.ClassicSeaFight.Constants;
using SeaFightWinForms.Media;
using SeaFightWinForms.Media.Enums;
using SeaFightWinForms.UIElements;

namespace SeaFightWinForms
{
    public partial class GameForm
    {
        SeaCellPictureBox[,]? _playerPctrMas;
        SeaCellPictureBox[,]? _opponentPctrMas;
        ShipTypeRadioBox[]? _radioPctr = new ShipTypeRadioBox[4];
        PictureBox? _orientationPctr;
        PictureBox? _orientationSign;
        PictureBox? _one;
        PictureBox? _two;
        PictureBox? _three;
        PictureBox? _four;
        PictureBox? _compOne;
        PictureBox? _compTwo;
        PictureBox? _compThree;
        PictureBox? _compFour;
        PictureBox? _oneDeck;
        PictureBox? _twoDeck;
        PictureBox? _threeDeck;
        PictureBox? _fourDeck;
        PictureBox? _winPctr;
        PictureBox? _turn;

        /// <summary>
        /// Создание начального игрового меню
        /// </summary>
        private void CreateGameMenu()
        {
            CreateShipChoice();
            CreateDirectionButton();
            CreatePlacementCounters();
        }

        /// <summary>
        /// Создание счетчиков размещения кораблей игрока
        /// </summary>
        private void CreatePlacementCounters()
        {
            _one = new PictureBox
            {
                Location = new Point(300, 500),
                Size = new Size(60, 40),
                BackColor = Color.Transparent,
                Image = Properties.Resources.X4
            };
            Controls.Add(_one);

            _two = new PictureBox
            {
                Location = new Point(300, 550),
                Size = new Size(60, 40),
                BackColor = Color.Transparent,
                Image = Properties.Resources.X3
            };
            Controls.Add(_two);

            _three = new PictureBox
            {
                Location = new Point(300, 600),
                Size = new Size(60, 40),
                BackColor = Color.Transparent,
                Image = Properties.Resources.X2
            };
            Controls.Add(_three);

            _four = new PictureBox
            {
                Location = new Point(300, 650),
                Size = new Size(60, 40),
                BackColor = Color.Transparent,
                Image = Properties.Resources.X1
            };
            Controls.Add(_four);
        }

        /// <summary>
        /// Создание кнопки выбора направления корабля
        /// </summary>
        private void CreateDirectionButton()
        {
            _orientationPctr = new PictureBox
            {
                Location = new Point(400, 500),
                Size = new Size(100, 100),
                BackColor = Color.Transparent,
                Image = Properties.Resources.RudderHorizontal
            };
            _orientationPctr.Click += OrientationPctr_Click;
            Controls.Add(_orientationPctr);

            _orientationSign = new PictureBox
            {
                Location = new Point(390, 600),
                Size = new Size(130, 30),
                BackColor = Color.Transparent,
                Image = Properties.Resources.SignDirection
            };
            Controls.Add(_orientationSign);
        }

        /// <summary>
        /// Создание меню выбора кораблей
        /// </summary>
        private void CreateShipChoice()
        {
            int X = 80;
            int Y = 500;

            for (int i = 0; i < 4; i++)
            {
                _radioPctr![i] = new ShipTypeRadioBox();
                _radioPctr[i].Location = new Point(X, Y);
                _radioPctr[i].DeckNumber = i + 1;
                _radioPctr[i].Click += RadioPctr_Click;
                if (i == 0)
                {
                    _radioPctr[i].Image = Properties.Resources._1Deck;
                    _radioPctr[i].Size = new Size(40, 40);
                }
                else if (i == 1)
                {
                    _radioPctr[i].Image = Properties.Resources._2Deck;
                    _radioPctr[i].Size = new Size(80, 40);
                }
                else if (i == 2)
                {
                    _radioPctr[i].Image = Properties.Resources._3Deck;
                    _radioPctr[i].Size = new Size(120, 40);
                }
                else if (i == 3)
                {
                    _radioPctr[i].Image = Properties.Resources._4Deck;
                    _radioPctr[i].Size = new Size(160, 40);
                }

                Controls.Add(_radioPctr[i]);

                Y += 50;
            }
        }

        /// <summary>
        /// Создание клеточного поля компьютера и игрока
        /// </summary>
        private void CreateFields()
        {
            //Поле игрока
            _playerPctrMas = new SeaCellPictureBox[10, 10];

            int X = 80;
            int Y = 80;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _playerPctrMas[i, j] = new SeaCellPictureBox
                    {
                        Location = new Point(X, Y),
                        Size = new Size(40, 40),
                        X = i,
                        Y = j
                    };
                    _playerPctrMas[i, j].Click += PlayerPctr_Click;
                    _playerPctrMas[i, j].MouseEnter += Pctr_MouseEnter;
                    _playerPctrMas[i, j].MouseLeave += Pctr_MouseLeave;
                    _playerPctrMas[i, j].BackColor = Color.Transparent;
                    Controls.Add(_playerPctrMas[i, j]);
                    Y += 40;
                }
                Y = 80;
                X += 40;
            }

            //Поле компьютера
            _opponentPctrMas = new SeaCellPictureBox[FieldConstants.FieldWidth, FieldConstants.FieldHeight];

            X = 760;
            Y = 80;

            for (int i = 0; i < FieldConstants.FieldWidth; i++)
            {
                for (int j = 0; j < FieldConstants.FieldHeight; j++)
                {
                    _opponentPctrMas[i, j] = new SeaCellPictureBox
                    {
                        Location = new Point(X, Y),
                        Size = new Size(40, 40),
                        X = i,
                        Y = j,
                        Enabled = false
                    };
                    _opponentPctrMas[i, j].Click += CompPctr_Click;
                    _opponentPctrMas[i, j].BackColor = Color.Transparent;
                    Controls.Add(_opponentPctrMas[i, j]);
                    Y += 40;
                }
                Y = 80;
                X += 40;
            }
        }

        /// <summary>
        /// Создание Экрана победы (окончание игры)
        /// </summary>
        private void CreateVictoryScreen()
        {
            _one?.Dispose();
            _two?.Dispose();
            _three?.Dispose();
            _four?.Dispose();
            _compOne?.Dispose();
            _compTwo?.Dispose();
            _compThree?.Dispose();
            _compFour?.Dispose();
            _oneDeck?.Dispose();
            _twoDeck?.Dispose();
            _threeDeck?.Dispose();
            _fourDeck?.Dispose();
            startButton?.Dispose();
            lazyPctr?.Dispose();
            _orientationPctr?.Dispose();
            _orientationSign?.Dispose();

            for (int i = 0; i < _radioPctr!.Length; i++)
            {
                _radioPctr[i]?.Dispose();
            }

            _one = null;
            _two = null;
            _three = null;
            _four = null;
            _compOne = null;
            _compTwo = null;
            _compThree = null;
            _compFour = null;
            _oneDeck = null;
            _twoDeck = null;
            _threeDeck = null;
            _fourDeck = null;
            startButton = null;
            lazyPctr = null;
            _orientationPctr = null;
            _orientationSign = null;
            BlockFields();

            _winPctr = new PictureBox()
            {
                Location = new Point(350, 500),
                Size = new Size(600, 200),
                BackColor = Color.Transparent,
            };
            Controls.Add(_winPctr);
        }

        private void BlockFields()
        {
            for (int i = 0; i < FieldConstants.FieldWidth; i++)
            {
                for (int j = 0; j < FieldConstants.FieldHeight; j++)
                {
                    _playerPctrMas![i, j].Enabled = false;
                    _opponentPctrMas![i, j].Enabled = false;
                }
            }
        }

        /// <summary>
        /// Создание Экрана игровой сессии (во время уже самой игры)
        /// </summary>
        private void CreateGameSessionScreen()
        {
            _orientationPctr?.Dispose();
            _orientationSign?.Dispose();
            lazyPctr.Dispose();
            startButton.Visible = false;
            lazyPctr.Click -= LazyPctr_Click;
            lazyPctr.MouseLeave -= LazyPctr_MouseLeave;
            lazyPctr.MouseEnter -= LazyPctr_MouseEnter;
            _orientationSign!.Click -= OrientationPctr_Click;

            _orientationPctr = null;
            lazyPctr = null;
            _orientationSign = null;

            //Типы кораблей игрока
            for (int i = 0; i < _radioPctr!.Length; i++)
            {
                if (_radioPctr[i] != null)
                {
                    _radioPctr[i].Image = (Image)_imageResources.GetObject($@"{i + 1}Deck")!;
                    _radioPctr[i].Enabled = false;
                }
            }

            //Счетчики кораблей игрока
            _one!.Image = Properties.Resources.X4;
            _two!.Image = Properties.Resources.X3;
            _three!.Image = Properties.Resources.X2;
            _four!.Image = Properties.Resources.X1;

            for (int i = 0; i < FieldConstants.FieldWidth; i++)
            {
                for (int j = 0; j < FieldConstants.FieldHeight; j++)
                {
                    _opponentPctrMas![i, j].Enabled = true;
                    _playerPctrMas![i, j].Enabled = false;
                }
            }

            //Счетчики типов кораблей компьютера
            _compOne = new PictureBox
            {
                Location = new Point(980, 500),
                Size = new Size(60, 40),
                BackColor = Color.Transparent,
                Image = Properties.Resources.X4
            };
            Controls.Add(_compOne);

            _compTwo = new PictureBox
            {
                Location = new Point(980, 550),
                Size = new Size(60, 40),
                BackColor = Color.Transparent,
                Image = Properties.Resources.X3
            };
            Controls.Add(_compTwo);

            _compThree = new PictureBox
            {
                Location = new Point(980, 600),
                Size = new Size(60, 40),
                BackColor = Color.Transparent,
                Image = Properties.Resources.X2
            };
            Controls.Add(_compThree);

            _compFour = new PictureBox
            {
                Location = new Point(980, 650),
                Size = new Size(60, 40),
                BackColor = Color.Transparent,
                Image = Properties.Resources.X1
            };
            Controls.Add(_compFour);

            //Типы кораблей компьютера
            _oneDeck = new PictureBox
            {
                Image = Properties.Resources._1Deck,
                BackColor = Color.Transparent,
                Location = new Point(760, 500),
                Size = new Size(40, 40)
            };
            Controls.Add(_oneDeck);

            _twoDeck = new PictureBox
            {
                Image = Properties.Resources._2Deck,
                BackColor = Color.Transparent,
                Location = new Point(760, 550),
                Size = new Size(80, 40)
            };
            Controls.Add(_twoDeck);

            _threeDeck = new PictureBox
            {
                Image = Properties.Resources._3Deck,
                BackColor = Color.Transparent,
                Location = new Point(760, 600),
                Size = new Size(120, 40)
            };
            Controls.Add(_threeDeck);

            _fourDeck = new PictureBox
            {
                Image = Properties.Resources._4Deck,
                BackColor = Color.Transparent,
                Location = new Point(760, 650)
            };
            Controls.Add(_fourDeck);
            _fourDeck.Size = new Size(160, 40);

            CreateTurnIcon();
        }

        private void CreateTurnIcon()
        {
            _turn = new PictureBox()
            {
                Size = new Size(100, 100),
                Location = new Point(565, 210),
                BackColor = Color.Transparent
            };
            Controls.Add(_turn);
        }

        private readonly Wave[] _waves = {
            new() { StartsWithX = 140, Y = 140, EndsWithX = 140 },
            new() { StartsWithX = 371, Y = 140, EndsWithX = 389 },
            new() { StartsWithX = 211, Y = 220, EndsWithX = 229 },
            new() { StartsWithX = 451, Y = 220, EndsWithX = 469 },
            new() { StartsWithX = 340, Y = 260, EndsWithX = 340 },
            new() { StartsWithX = 180, Y = 340, EndsWithX = 180 },
            new() { StartsWithX = 380, Y = 340, EndsWithX = 380 },
            new() { StartsWithX = 291, Y = 420, EndsWithX = 309 },
            new() { StartsWithX = 131, Y = 460, EndsWithX = 149 },
            new() { StartsWithX = 451, Y = 460, EndsWithX = 469 },

            new() { StartsWithX = 940, Y = 100, EndsWithX = 940 },
            new() { StartsWithX = 1060, Y = 140, EndsWithX = 1060 },
            new() { StartsWithX = 780, Y = 180, EndsWithX = 780 },
            new() { StartsWithX = 891, Y = 180, EndsWithX = 909 },
            new() { StartsWithX = 971, Y = 260, EndsWithX = 989 },
            new() { StartsWithX = 820, Y = 300, EndsWithX = 820 },
            new() { StartsWithX = 1100, Y = 340, EndsWithX = 1100 },
            new() { StartsWithX = 891, Y = 380, EndsWithX = 909 },
            new() { StartsWithX = 1051, Y = 420, EndsWithX = 1069 },
            new() { StartsWithX = 820, Y = 460, EndsWithX = 820 } };

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            Pen WavePen = new(Color.Blue, 3);
            Pen LinesPen = new(Color.MediumPurple, 1);

            //Волны
            for (int i = 0; i < _waves.Length; i++)
            {
                gr.DrawLine(WavePen, _waves[i].StartsWithX, _waves[i].Y, _waves[i].EndsWithX, _waves[i].Y);
            }

            //Горизонтальные линии
            for (int i = 0; i < Width / 40; i++)
            {
                gr.DrawLine(LinesPen, i * 40, 0, i * 40, Height);
            }

            //Вертикальные линии
            for (int i = 0; i < Height / 40; i++)
            {
                gr.DrawLine(LinesPen, 0, i * 40, Width, i * 40);
            }
        }

        private void Waves_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < _waves.Length; i++)
            {
                if (_waves[i].GrowDirection == WavesDirection.Grow)
                {
                    _waves[i].EndsWithX += 1;
                    _waves[i].StartsWithX -= 1;
                    if (_waves[i].EndsWithX - _waves[i].StartsWithX == 20)
                    {
                        _waves[i].GrowDirection = WavesDirection.Shrink;
                    }
                }
                else
                {
                    _waves[i].EndsWithX -= 1;
                    _waves[i].StartsWithX += 1;
                    if (_waves[i].EndsWithX - _waves[i].StartsWithX == 0)
                    {
                        _waves[i].GrowDirection = WavesDirection.Grow;
                    }
                }

            }
            Invalidate();
        }
    }
}
