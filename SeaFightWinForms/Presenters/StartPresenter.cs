using Newtonsoft.Json;
using SeaFightToolkit.SignalR.Models;
using SeaFightWinForms.Utils;

namespace SeaFightWinForms.Presenter
{
    class StartPresenter
    {
        private readonly IStartView _startView;
        private readonly GameHubClient _hubClient;
        private string? _playerName;
        private string? _habUrl;

        public StartPresenter(IStartView iStartView, string habUrl)
        {
            _habUrl = habUrl;
            _startView = iStartView;
            _hubClient = GameHubClient.Instance;
            _startView.StartOfflineGameEvent += OnStartCompGameEvent;
            _startView.ConnectEvent += OnConnectEvent;
            _startView.DisconnectEvent += OnDisconnectEvent;
            _startView.SendMessageEvent += OnSendMessageEvent;
            _startView.OfferGameEvent += OnOfferGameEvent;
            _startView.AnswerOfferEvent += OnAnswerOfferEvent;
            _startView.StartOnlineGameEvent += OnStartNetGameEvent;
        }

        /// <summary>
        /// Создает экземпляр презентера при игре с компьютером
        /// </summary>
        /// <param name="gameForm"></param>
        private void OnStartCompGameEvent(IView gameForm)
        {
            var gamePresenter = new GamePresenterOffline(gameForm);
        }

        private void OnStartNetGameEvent(IView gameForm, string sessionId)
        {
            var gamePresenter = new GamePresenterOnline(gameForm, sessionId);
        }

        private async void OnAnswerOfferEvent(string idFrom, string answer)
        {
            await _hubClient.AnswerOfferAsync(idFrom, answer);
        }

        private async void OnDisconnectEvent()
        {
            await _hubClient.DisconnectAsync();
            DeregisterHubEvents();
        }

        private async void OnOfferGameEvent(string idTo)
        {
            await _hubClient.OfferGameAsync(idTo);
        }

        private async void OnSendMessageEvent(string name, string message)
        {
            await _hubClient.SendMessageAsync(name, message);
        }

        private async void OnConnectEvent(string name)
        {
            _playerName = name;
            RegisterHubEvents();
            await _hubClient.ConnectAsync(_habUrl);
        }

        private void RegisterHubEvents()
        {
            _hubClient.ConnectedEvent += OnConnected;
            _hubClient.ErrorEvent += OnError;
            _hubClient.AnswerOfferingEvent += OnAnswerOfferingEvent;
            _hubClient.DenyOfferOpponentIsBusyEvent += OnOpponentIsBusyEvent;
            _hubClient.DenyOfferYouAreBusyEvent += OnYouAreBusyEvent;
            _hubClient.OfferToYouEvent += OnOfferToYouEvent;
            _hubClient.ShowPlayersEvent += OnShowPlayersEvent;
            _hubClient.UpdateMessageListEvent += OnUpdateMessageListEvent;
            _hubClient.NameIsOccupied += OnNameIsOccupied;
            _hubClient.LoopOfferEvent += LoopOfferEvent;
        }

        private void DeregisterHubEvents()
        {
            _hubClient.ConnectedEvent -= OnConnected;
            _hubClient.ErrorEvent -= OnError;
            _hubClient.AnswerOfferingEvent -= OnAnswerOfferingEvent;
            _hubClient.DenyOfferOpponentIsBusyEvent -= OnOpponentIsBusyEvent;
            _hubClient.DenyOfferYouAreBusyEvent -= OnYouAreBusyEvent;
            _hubClient.OfferToYouEvent -= OnOfferToYouEvent;
            _hubClient.ShowPlayersEvent -= OnShowPlayersEvent;
            _hubClient.UpdateMessageListEvent -= OnUpdateMessageListEvent;
            _hubClient.NameIsOccupied -= OnNameIsOccupied;
        }

        private void LoopOfferEvent() 
        {
            _startView.NameMatched();
        }

        private void OnNameIsOccupied() 
        {
            _startView.NameIsOccupied();
        }

        private void OnError()
        {
            _startView.ErrorConnection();
        }

        private async void OnConnected()
        {
            _startView.SuccessfullConnection();
            await _hubClient.RegisterAsync(_playerName!);
        }

        private void OnUpdateMessageListEvent(string name, string message)
        {
            _startView.UpdateMessageList(name, message);
        }

        private void OnShowPlayersEvent(string players, string myName)
        {
            var currentPlayers = JsonConvert.DeserializeObject<List<Player>>(players) 
                ?? Enumerable.Empty<Player>().ToList();
            _startView.ShowPlayers(currentPlayers, myName);
        }

        private void OnOfferToYouEvent(string nameFrom, string idFrom, string nameTo)
        {
            _startView.OfferToYou(nameFrom, idFrom, nameTo);
        }

        private void OnYouAreBusyEvent()
        {
            _startView.YouAreBusy();
        }

        private void OnOpponentIsBusyEvent()
        {
            _startView.OpponentIsBusy();
        }

        private void OnAnswerOfferingEvent(string nameFrom, string nameTo, string answer, string sessionID)
        {
            _startView.AnswerOffering(nameFrom, nameTo, answer, sessionID);
        }
    }
}
