using SeaFightToolkit.SignalR.Models;
using SeaFightWinForms.Presenter;
using SeaFightWinForms.Properties;
using SeaFightWinForms.UIElements.Enums;

namespace SeaFightWinForms
{
    /// <summary>
    /// Стартовое окно - Окно управления подключением, чатом, сетевой игрой [Online]
    /// </summary>
    partial class StartForm
    {
        public event Action<string>? ConnectEvent;
        public event Action? DisconnectEvent;
        public event Action<string, string>? SendMessageEvent;
        public event Action<string>? OfferGameEvent;
        public event Action<string, string>? AnswerOfferEvent;
        public event Action<IView, string>? StartOnlineGameEvent;
        public event Action<IView>? StartOfflineGameEvent;

        /// <summary>
        /// Обработчик клика по кнопке назад
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private void BackButtonClick(object? sender, EventArgs e)
        {
            _ctsCancel?.Cancel();
            DeactivateChatPanel();
            DestroyChatScreen();
            OnDisconnectEvent();
            WindowSizeAnimationTimer.Enabled = true;
            CreateStartScreen();
        }

        /// <summary>
        /// Обработчик двойного клика по строке
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private void PlayersGridViewCellMouseDoubleClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1) 
            {
                var id = _playersGridView?.Rows[e.RowIndex].Cells[Resources.IdColumnName].Value.ToString();
                OnOfferGameEvent(id!); 
            }
        }

        /// <summary>
        /// Обработчик клика по кнопке Отправить сообщение
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private void SendMessageButtonClick(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_messageTextBox?.Text))
            {
                _messageTextBox!.BackColor = Color.LightCoral;
                MessageBox.Show(Resources.MessageTextBoxErrorWarning);
                return;
            }

            OnSendMessageEvent(_nameTextBox!.Text, _messageTextBox.Text);
        }

        /// <summary>
        /// Обработчик клика по кнопке Регистрация/Выход
        /// </summary>
        /// <param name="sender">Объект инициатор события</param>
        /// <param name="e">Аргументы события</param>
        private async void RegisterButtonClick(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_nameTextBox?.Text))
            {
                _nameTextBox!.BackColor = Color.LightCoral;
                MessageBox.Show(Resources.NameTextBoxErrorWarning);
                return;
            }

            if (_registerButton!.State == RegisterButtonState.Exit)
            {
                _ctsCancel?.Cancel();
                _statusTextBox!.Text = Resources.StatusTextBoxText;
                _statusTextBox.BackColor = Color.White;
                _playersGridView?.Rows.Clear();
                DeactivateChatPanel();
                OnDisconnectEvent();
                return;
            }

            _registerButton.State = RegisterButtonState.Exit;
            _ctsConnected = new CancellationTokenSource();
            _ctsError = new CancellationTokenSource();
            _ctsCancel = new CancellationTokenSource();
            OnConnectEvent(_nameTextBox.Text);
            await ChangeTextAsync();
        }

        //Изменение статуса подключения
        public async Task ChangeTextAsync()
        {
            for (int i = 1; i < 4; i++)
            {
                if (_ctsError!.IsCancellationRequested)
                {
                    _ctsError.Dispose();
                    _statusTextBox!.Text = Resources.StatusTextBoxErrorText;
                    _statusTextBox.BackColor = Color.LightCoral;
                    MessageBox.Show(Resources.StatusTextBoxErrorText);
                    break;
                }
                else if (_ctsConnected!.IsCancellationRequested)
                {
                    _ctsConnected.Dispose();
                    _statusTextBox!.Text = Resources.StatusTextBoxConnectedText;
                    _statusTextBox.BackColor = Color.LightGreen;
                    break;
                }
                else if (_ctsCancel!.IsCancellationRequested)
                {
                    _ctsCancel.Dispose();
                    break;
                }
                else
                {
                    _statusTextBox!.BackColor = Color.White;
                    _statusTextBox.Text = $"{Resources.StatusTextBoxInProcess}{new string('.', i)}";
                    await Task.Delay(500);
                    i = i == 3 ? 0 : i;
                }
            }
        }

        #region Реализация интерфейса IStartView

        public void ShowPlayers(List<Player> players, string id)
        {
            _playersGridView?.Invoke(new Action(() => FilterDataGrid(players, id)));
        }

        public void SuccessfullConnection()
        {
            _ctsConnected?.Cancel();
            _uiContext.Post(new SendOrPostCallback(o => { _registerButton!.Enabled = true; ActivateChatPanel(); }), null);
        }

        public void NameIsOccupied() 
        {
            MessageBox.Show("Данное имя уже зарегистрировано! Выберите другое");
        }

        public void ErrorConnection()
        {
            _ctsError?.Cancel();
            _uiContext.Post(new SendOrPostCallback(o => { _registerButton!.Enabled = true; }), null);
        }

        private void FilterDataGrid(List<Player> players, string id)
        {
            _playersGridView?.Rows.Clear();

            foreach (var player in players)
            {
                var playerStatus = player.IsBusy ? Resources.PlayerStatusBusy : Resources.PlayerStatusFree;
                var rowIndex = _playersGridView!.Rows.Add(player.Id, player.Name, playerStatus);

                if (player.Id == id)
                {
                    _playersGridView!.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                    _playersGridView!.Rows[rowIndex].Cells[1].Value += Resources.ThisIsYouMarkText;
                }

                if (player.IsBusy)
                {
                    _playersGridView.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }

            _playersGridView?.ClearSelection();
        }

        public void UpdateMessageList(string name, string message)
        {
            _uiContext.Post(new SendOrPostCallback(o => { chatTextBox!.Text += $"{name} - {message}" + Environment.NewLine; _messageTextBox!.Text = ""; }), null);
        }

        public void OfferToYou(string nameFrom, string idFrom, string nameTo)
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                if (MessageBox.Show($"Вам предлагает сыграть {nameFrom}!", "Предложение!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    OnAnswerOfferEvent(idFrom, "yes");
                }
                else
                {
                    OnAnswerOfferEvent(idFrom, "no");
                }
            }), null);
        }

        public void AnswerOffering(string nameFrom, string nameTo, string answer, string sessionId)
        {
            _uiContext.Post(new SendOrPostCallback(o =>
            {
                if (answer == "yes")
                {
                    MessageBox.Show($"Игра начинается!"); 
                    var gameForm = new GameForm(_sound, _animation); 
                    gameForm.Show(); 
                    OnStartOnlineGameEvent(gameForm, sessionId);
                }
                else
                {
                    MessageBox.Show($"{nameFrom} отказался(-лась) играть!");
                }
            }), null);
        }

        public void OpponentIsBusy()
        {
            MessageBox.Show("Данный игрок уже играет!");
        }

        public void YouAreBusy()
        {
            MessageBox.Show("Вы уже играете!");
        }

        public void NameMatched()
        {
            MessageBox.Show("Нельзя выбрать себя!");
        }
        #endregion

        protected virtual void OnConnectEvent(string playerName)
        {
            var raiseEvent = ConnectEvent;
            if (raiseEvent != null)
            {
                raiseEvent(playerName);
            }
        }

        protected virtual void OnDisconnectEvent()
        {
            var raiseEvent = DisconnectEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnSendMessageEvent(string name, string message)
        {
            var raiseEvent = SendMessageEvent;
            if (raiseEvent != null)
            {
                raiseEvent(name, message);
            }
        }

        protected virtual void OnOfferGameEvent(string playerId)
        {
            var raiseEvent = OfferGameEvent;
            if (raiseEvent != null)
            {
                raiseEvent(playerId);
            }
        }

        protected virtual void OnAnswerOfferEvent(string playerId, string answer)
        {
            var raiseEvent = AnswerOfferEvent;
            if (raiseEvent != null)
            {
                raiseEvent(playerId, answer);
            }
        }

        protected virtual void OnStartOnlineGameEvent(IView gameForm, string sessionId)
        {
            var raiseEvent = StartOnlineGameEvent;
            if (raiseEvent != null)
            {
                raiseEvent(gameForm, sessionId);
            }
        }

        protected virtual void OnStartOfflineGameEvent(IView gameForm)
        {
            var raiseEvent = StartOfflineGameEvent;
            if (raiseEvent != null)
            {
                raiseEvent(gameForm);
            }
        }
    }
}
