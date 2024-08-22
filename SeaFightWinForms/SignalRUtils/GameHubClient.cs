using Microsoft.AspNetCore.SignalR.Client;
using SeaFightToolkit.SignalR.Contracts;

namespace SeaFightWinForms.Utils
{
    /// <summary>
    /// Клиент SignalR
    /// </summary>
    class GameHubClient
    {
        /// <summary>
        /// Подключение к хабу
        /// </summary>
        private HubConnection Connection { get; set; } = default!;

        /// <summary>
        /// Возвращает один экземпляр класса
        /// </summary>
        public static GameHubClient Instance => _instance.Value;

        /// <summary>
        /// Обеспечивает ленивую инициализацию экземпляра
        /// </summary>
        private static readonly Lazy<GameHubClient> _instance = new(() => new GameHubClient());

        public event Action? ConnectedEvent;
        public event Action? LoopOfferEvent;
        public event Action? ErrorEvent;
        public event Action<string, string>? ShowPlayersEvent;
        public event Action<string, string>? UpdateMessageListEvent;
        public event Action<string, string, string>? OfferToYouEvent;
        public event Action<string, string, string, string>? AnswerOfferingEvent;
        public event Action? DenyOfferYouAreBusyEvent;
        public event Action? DenyOfferOpponentIsBusyEvent;

        public event Action? NetMyVictoryEvent;
        public event Action? NetOpponentVictoryEvent;
        public event Action<string>? NetOpponentAbortGameEvent;
        public event Action<string, string>? BeginNetGameEvent;
        public event Action<string, string>? PlayerHitEvent;
        public event Action<string, string>? PlayerMissEvent;
        public event Action<string, string>? OpponentHitEvent;
        public event Action<string, string>? OpponentMissEvent;
        public event Action<string, string>? DecreasePlayerShipCountEvent;
        public event Action<string, string>? DecreaseOpponentShipCountEvent;
        public event Action<string>? MarkPlayerDestroyedShipEvent;
        public event Action<string>? MarkOpponentDestroyedShipEvent;
        public event Action? MarkMyTurnEvent;
        public event Action? MarkOpponentTurnEvent;
        public event Action? NameIsOccupied;

        /// <summary>
        /// Подключение к хабу
        /// </summary>
        public async Task ConnectAsync(string hubUrl)
        {
            try
            {
                Connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();

                RegisterHubEvents();
                await Connection.StartAsync();
                if (Connection.State == HubConnectionState.Connected) 
                {
                    OnConnectedEvent();
                }
            }
            catch
            {
                OnErrorEvent();
            }
        }

        /// <summary>
        /// Регистрация игрока
        /// </summary>
        /// <param name="playerName">Имя игрока</param>
        public async Task RegisterAsync(string playerName) 
        {
            await Connection.InvokeAsync(nameof(ISeaFightHub.Register), playerName);
        }

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="name">Имя игрока</param>
        /// <param name="message">Сообщение</param>
        public async Task SendMessageAsync(string name, string message)
        {
            await Connection.InvokeAsync(nameof(ISeaFightHub.SendMessage), name, message);
        }

        /// <summary>
        /// Ответ на предложение
        /// </summary>
        /// <param name="idFrom">Идентификатор отправителя</param>
        /// <param name="answer">Ответ</param>
        public async Task AnswerOfferAsync(string idFrom, string answer)
        {
            await Connection.InvokeAsync(nameof(ISeaFightHub.AnswerOffer), idFrom, Connection.ConnectionId, answer);
        }

        /// <summary>
        /// Отключение
        /// </summary>
        public async Task DisconnectAsync()
        {
            if (Connection != null && Connection.State == HubConnectionState.Connected)
            {
                await Connection.DisposeAsync();
            }
        }

        /// <summary>
        /// Предложение сыграть
        /// </summary>
        /// <param name="idTo">Идентификатор адресата предложения</param>
        /// <returns></returns>
        public async Task OfferGameAsync(string idTo) 
        {
            if (idTo == Connection.ConnectionId)
            {
                OnLoopOfferEvent();
                return;
            }

            await Connection.InvokeAsync(nameof(ISeaFightHub.OfferGame), Connection.ConnectionId, idTo);
        }

        /// <summary>
        /// Готовность начать игру
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <param name="ships">Корабли</param>
        public async Task ReadyToStartAsync(string sessionId, string ships) 
        {
            await Connection.InvokeAsync(nameof(ISeaFightHub.ReadyToStart), sessionId, Connection.ConnectionId, ships);
        }

        /// <summary>
        /// Ход
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public async Task MoveAsync(string sessionId, string x, string y) 
        {
            await Connection.InvokeAsync(nameof(ISeaFightHub.Move), sessionId, Connection.ConnectionId, x, y);
        }

        /// <summary>
        /// Завершение хода
        /// </summary>
        /// <param name="sessionId">Идентификатор сессии</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public async Task CompletingTurnAsync(string sessionId, string x, string y)
        {
            await Connection.InvokeAsync(nameof(ISeaFightHub.CompletingTurn), sessionId, Connection.ConnectionId, x, y);
        }

        /// <summary>
        /// Прерывание игры
        /// </summary>
        public async Task NetAbortGameAsync() 
        {
            await Connection.InvokeAsync(nameof(ISeaFightHub.NetAbortGame));
        }

        /// <summary>
        /// Определение, чей ход первый
        /// </summary>
        /// <param name="connectionId">Идентификатор сессии</param>
        public bool IsMyTurnFirstAsync(string connectionId)
        {
            return connectionId == Connection.ConnectionId;
        }

        /// <summary>
        /// Подписка на события хаба
        /// </summary>
        private void RegisterHubEvents()
        {
            Connection!.On<string, string>(
                nameof(ISeaFightHub.BroadcastMessage), 
                OnUpdateMessageListEvent);

            Connection.On<string>(
                nameof(ISeaFightHub.GetPlayers), 
                (players) => { OnShowPlayersEvent(players, Connection.ConnectionId!); });

            Connection.On<string, string, string>(
                nameof(ISeaFightHub.OfferGame), 
                OnOfferToYouEvent);

            Connection.On<string, string, string, string>(
                nameof(ISeaFightHub.AnswerOffer), 
                OnAnswerOfferingEvent);

            Connection.On(
                nameof(ISeaFightHub.DenyOfferOpponentIsBusy), 
                OnDenyOfferOpponentIsBusyEvent);

            Connection.On(
                nameof(ISeaFightHub.DenyOfferYouAreBusy),
                OnDenyOfferYouAreBusyEvent);

            Connection.On<string, string>(
                nameof(ISeaFightHub.StartGame), 
                OnBeginNetGameEvent);

            Connection.On<string, string>(
                nameof(ISeaFightHub.OpponentHit), 
                (x, y) => { OnOpponentHitEvent(x, y); OnMarkOpponentTurnEvent(); });

            Connection.On<string, string>(
                nameof(ISeaFightHub.OpponentMiss), 
                (x, y) => { OnOpponentMissEvent(x, y); OnMarkMyTurnEvent(); });

            Connection.On<string, string>(
                nameof(ISeaFightHub.MyHit), 
                (x, y) => { OnPlayerHitEvent(x, y); OnMarkMyTurnEvent(); });

            Connection.On<string, string>(
                nameof(ISeaFightHub.MyMiss), 
                (x, y) => { OnPlayerMissEvent(x, y); OnMarkOpponentTurnEvent(); });

            Connection.On<string, string, string>(
                nameof(ISeaFightHub.MyShipDestroyed), 
                (ship, deckCount, liveShipsCount) => { OnDecreasePlayerShipCountEvent(deckCount, liveShipsCount); OnMarkPlayerDestroyedShipEvent(ship); });

            Connection.On<string, string, string>(
                nameof(ISeaFightHub.OpponentShipDestroyed), 
                (ship, deckCount, liveShipsCount) => { OnDecreaseOpponentShipCountEvent(deckCount, liveShipsCount); OnMarkOpponentDestroyedShipEvent(ship); });

            Connection.On<string>(
                nameof(ISeaFightHub.OpponentAbortGame), 
                OnNetOpponentAbortGameEvent);

            Connection.On<string>(
                nameof(ISeaFightHub.Victory), 
                (victoryId) => { if (victoryId == Connection.ConnectionId) { OnNetMyVictoryEvent(); } else { OnNetOpponentVictoryEvent(); } });

            Connection.On(
                nameof(ISeaFightHub.NameIsOccupied), 
                OnNameIsOccupiedEvent);
        }

        protected virtual void OnConnectedEvent()
        {
            var raiseEvent = ConnectedEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnErrorEvent()
        {
            var raiseEvent = ErrorEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnNameIsOccupiedEvent() 
        {
            var raiseEvent = NameIsOccupied;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnLoopOfferEvent() 
        {
            var raiseEvent = LoopOfferEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnNetMyVictoryEvent()
        {
            var raiseEvent = NetMyVictoryEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnNetOpponentVictoryEvent()
        {
            var raiseEvent = NetOpponentVictoryEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnNetOpponentAbortGameEvent(string name)
        {
            var raiseEvent = NetOpponentAbortGameEvent;
            if (raiseEvent != null)
            {
                raiseEvent(name);
            }
        }

        protected virtual void OnShowPlayersEvent(string players, string connectionId)
        {
            var raiseEvent = ShowPlayersEvent;
            if (raiseEvent != null)
            {
                raiseEvent(players, connectionId);
            }
        }

        protected virtual void OnUpdateMessageListEvent(string name, string message) 
        {
            var raiseEvent = UpdateMessageListEvent;
            if (raiseEvent != null)
            {
                raiseEvent(name, message);
            }
        }

        protected virtual void OnOfferToYouEvent(string nameFrom, string idFrom, string nameTo) 
        {
            var raiseEvent = OfferToYouEvent;
            if (raiseEvent != null)
            {
                raiseEvent(nameFrom, idFrom, nameTo);
            }
        }

        protected virtual void OnAnswerOfferingEvent(string nameFrom, string nameTo, string answer, string sessionId) 
        {
            var raiseEvent = AnswerOfferingEvent;
            if (raiseEvent != null)
            {
                raiseEvent(nameFrom, nameTo, answer, sessionId);
            }
        }

        protected virtual void OnBeginNetGameEvent(string whoIsFirstId, string whoIsFirstName) 
        {
            var raiseEvent = BeginNetGameEvent;
            if (raiseEvent != null)
            {
                raiseEvent(whoIsFirstId, whoIsFirstName);
            }
        }

        protected virtual void OnOpponentHitEvent(string x, string y) 
        {
            var raiseEvent = OpponentHitEvent;
            if (raiseEvent != null)
            {
                raiseEvent(x, y);
            }
        }

        protected virtual void OnOpponentMissEvent(string x, string y)
        {
            var raiseEvent = OpponentMissEvent;
            if (raiseEvent != null)
            {
                raiseEvent(x, y);
            }
        }

        protected virtual void OnPlayerHitEvent(string x, string y)
        {
            var raiseEvent = PlayerHitEvent;
            if (raiseEvent != null)
            {
                raiseEvent(x, y);
            }
        }

        protected virtual void OnPlayerMissEvent(string x, string y)
        {
            var raiseEvent = PlayerMissEvent;
            if (raiseEvent != null)
            {
                raiseEvent(x, y);
            }
        }

        protected virtual void OnMarkOpponentTurnEvent() 
        {
            var raiseEvent = MarkOpponentTurnEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnMarkMyTurnEvent() 
        {
            var raiseEvent = MarkMyTurnEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnDenyOfferOpponentIsBusyEvent() 
        {
            var raiseEvent = DenyOfferOpponentIsBusyEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnDenyOfferYouAreBusyEvent()
        {
            var raiseEvent = DenyOfferYouAreBusyEvent;
            if (raiseEvent != null)
            {
                raiseEvent();
            }
        }

        protected virtual void OnDecreasePlayerShipCountEvent(string deckCount, string liveShipsCount) 
        {
            var raiseEvent = DecreasePlayerShipCountEvent;
            if (raiseEvent != null)
            {
                raiseEvent(deckCount, liveShipsCount);
            }
        }

        protected virtual void OnDecreaseOpponentShipCountEvent(string deckCount, string liveShipsCount)
        {
            var raiseEvent = DecreaseOpponentShipCountEvent;
            if (raiseEvent != null)
            {
                raiseEvent(deckCount, liveShipsCount);
            }
        }

        protected virtual void OnMarkPlayerDestroyedShipEvent(string ship)
        {
            var raiseEvent = MarkPlayerDestroyedShipEvent;
            if (raiseEvent != null)
            {
                raiseEvent(ship);
            }
        }

        protected virtual void OnMarkOpponentDestroyedShipEvent(string ship)
        {
            var raiseEvent = MarkOpponentDestroyedShipEvent;
            if (raiseEvent != null)
            {
                raiseEvent(ship);
            }
        }
    }
}
