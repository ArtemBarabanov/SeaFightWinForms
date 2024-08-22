using SeaFightWinForms.Properties;
using SeaFightWinForms.UIElements;
using SeaFightWinForms.UIElements.Enums;

namespace SeaFightWinForms
{
    /// <summary>
    /// Стартовое окно - Создание элементов управления [Controls]
    /// </summary>
    partial class StartForm
    {
        private Label? _nameLabel;
        private Button? _netGameButton;
        private Button? _computerGameButton;
        private RegisterButton? _registerButton;
        private Button? _sendMessageButton;
        private Button? _backButton;
        private TextBox? _messageTextBox;
        private TextBox? _nameTextBox;
        private TextBox? chatTextBox;
        private TextBox? _statusTextBox;
        private DataGridView? _playersGridView;

        private bool _isShrinking;

        /// <summary>
        /// Создание стартового окна
        /// </summary>
        private void CreateStartScreen()
        {
            _netGameButton = new Button()
            {
                Text = Resources.NetGameButtonText,
                Location = new Point(40, 20),
                Size = new Size(300, 50)
            };
            _netGameButton.Click += NetGameButtonClick;
            Controls.Add(_netGameButton);

            _computerGameButton = new Button()
            {
                Text = Resources.ComputerGameButtonText,
                Location = new Point(40, 90),
                Size = new Size(300, 50)
            };
            _computerGameButton.Click += ComputerGameButtonClick;
            Controls.Add(_computerGameButton);
        }

        /// <summary>
        /// Трансформация стартового окна для сетевой игры
        /// </summary>
        private void CreateChatControls()
        {
            _statusTextBox = new TextBox()
            {
                Text = Resources.StatusTextBoxText,
                Size = new Size(90, 20),
                Location = new Point(290, 320),
                Enabled = false,
                BackColor = Color.White
            };

            _nameLabel = new Label()
            {
                Text = Resources.NameLabelText,
                Location = new Point(300, 235)
            };

            _registerButton = new RegisterButton()
            {
                Text = Resources.RegisterButtonText,
                Size = new Size(90, 25),
                Location = new Point(290, 290)
            };
            _registerButton.Click += RegisterButtonClick;

            _backButton = new Button()
            {
                Text = Resources.BackButtonText,
                Size = new Size(90, 25),
                Location = new Point(290, 350)
            };
            _backButton.Click += BackButtonClick;

            _sendMessageButton = new Button()
            {
                Text = Resources.SendMessageButtonText,
                Size = new Size(385, 25),
                Location = new Point(0, 190),
                Enabled = false
            };
            _sendMessageButton.Click += SendMessageButtonClick;

            _nameTextBox = new TextBox()
            {
                Size = new Size(90, 20),
                Location = new Point(290, 260),
            };
            _nameTextBox.Click += NameTextBoxClick;

            chatTextBox = new TextBox()
            {
                Size = new Size(385, 150),
                Multiline = true,
                Enabled = false,
                ScrollBars = ScrollBars.Vertical
            };

            _messageTextBox = new TextBox()
            {
                Size = new Size(390, 20),
                Location = new Point(0, 170),
                Enabled = false,
                PlaceholderText = Resources.MessageTextBoxPlaceholderText
            };
            _messageTextBox.Click += MessageTextBoxClick;

            _playersGridView = new DataGridView()
            {
                Size = new Size(230, 145),
                Location = new Point(5, 230),
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };
            _playersGridView.Columns.Add(Resources.IdColumnName, Resources.IdColumnStatusHeader);
            _playersGridView.Columns[Resources.IdColumnName].Visible = false;
            _playersGridView.Columns.Add(Resources.PlayerColumnName, Resources.PlayerColumnNameHeader);
            _playersGridView.Columns.Add(Resources.PlayerColumnStatus, Resources.PlayerColumnStatusHeader);
            _playersGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _playersGridView.CellMouseDoubleClick += PlayersGridViewCellMouseDoubleClick;

            Controls.Add(_statusTextBox);
            Controls.Add(_nameLabel);
            Controls.Add(_registerButton);
            Controls.Add(_backButton);
            Controls.Add(_nameTextBox);
            Controls.Add(chatTextBox);
            Controls.Add(_messageTextBox);
            Controls.Add(_sendMessageButton);
            Controls.Add(_playersGridView);
        }

        /// <summary>
        /// Обработчик нажатия на кнопку Отправить сообщение
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private void MessageTextBoxClick(object? sender, EventArgs e)
        {
            _messageTextBox!.BackColor = Color.White;
        }

        /// <summary>
        /// Обработчик клика по полю Имя
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>>
        private void NameTextBoxClick(object? sender, EventArgs e)
        {
            _nameTextBox!.BackColor = Color.White;
        }

        /// <summary>
        /// СОздает анимацию сжатия/расширения окна с помощью таймера
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private void WindowSizeTimerTick(object? sender, EventArgs e)
        {
            if (!_isShrinking)
            {
                if (Height < 420)
                {
                    Height += 5;
                    return;
                }

                WindowSizeAnimationTimer.Stop();
                _isShrinking = true;
                return;
            }

            if (Height > 205)
            {
                Height -= 5;
            }
            else
            {
                WindowSizeAnimationTimer.Stop();
                _isShrinking = false;
            }
        }

        private void DestroyStartScreen()
        {
            _netGameButton?.Dispose();
            _computerGameButton?.Dispose();
        }

        private void DestroyChatScreen()
        {
            _statusTextBox?.Dispose();
            _registerButton?.Dispose();
            _backButton?.Dispose();
            _nameTextBox?.Dispose();
            chatTextBox?.Dispose();
            _messageTextBox?.Dispose();
            _sendMessageButton?.Dispose();
            _playersGridView?.Dispose();
            _nameLabel?.Dispose();
        }

        private void CreateChatScreen()
        {
            DestroyStartScreen();
            WindowSizeAnimationTimer.Enabled = true;
            CreateChatControls();
        }

        private void ActivateChatPanel()
        {
            _sendMessageButton!.Enabled = true;
            _messageTextBox!.Enabled = true;
            _nameTextBox!.Enabled = false;
            _registerButton!.Text = Resources.RegisterButtonExitText;
            _registerButton.State = RegisterButtonState.Exit;
        }

        private void DeactivateChatPanel()
        {
            _sendMessageButton!.Enabled = false;
            _messageTextBox!.Enabled = false;
            _nameTextBox!.Enabled = true;
            _registerButton!.Text = Resources.RegisterButtonText;
            _registerButton.State = RegisterButtonState.Register;
        }
    }
}
