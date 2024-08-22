using Newtonsoft.Json;
using SeaFightToolkit.SignalR.Dtos;
using SeaFightWinForms.Utils;

namespace SeaFightWinForms.Presenter
{
    class GamePresenterOnline : GamePresenter
    {
        private readonly string _gameSessionId;
        private readonly GameHubClient _hubClient;

        public GamePresenterOnline(IView view, string gameSessionId) : base(view)
        {
            _view = view;
            _gameSessionId = gameSessionId;
            _hubClient = GameHubClient.Instance;
            RegisterHubEvents();
            RegisterViewEvents();
        }

        private void RegisterViewEvents() 
        {
            _view.AbortGameEvent += OnAbortGameEvent;
        }

        private void RegisterHubEvents()
        {
            _hubClient.BeginNetGameEvent += BeginNetGameEvent;
            _hubClient.DecreaseOpponentShipCountEvent += DecreaseOpponentShipCountEvent;
            _hubClient.DecreasePlayerShipCountEvent += DecreasePlayerShipCountEvent;
            _hubClient.MarkMyTurnEvent += MarkMyTurnEvent;
            _hubClient.MarkOpponentDestroyedShipEvent += MarkOpponentDestroyedShipEvent;
            _hubClient.MarkOpponentTurnEvent += MarkOpponentTurnEvent;
            _hubClient.MarkPlayerDestroyedShipEvent += MarkPlayerDestroyedShipEvent;
            _hubClient.NetMyVictoryEvent += NetMyVictoryEvent;
            _hubClient.NetOpponentAbortGameEvent += NetOpponentAbortGameEvent;
            _hubClient.NetOpponentVictoryEvent += NetOpponentVictoryEvent;
            _hubClient.OpponentHitEvent += OpponentTargetHitEvent;
            _hubClient.OpponentMissEvent += OpponentTargetMissEvent;
            _hubClient.PlayerHitEvent += PlayerTargetHitEvent;
            _hubClient.PlayerMissEvent += PlayerTargetMissEvent;
        }

        private void DeregisterHubEvents()
        {
            _hubClient.BeginNetGameEvent -= BeginNetGameEvent;
            _hubClient.DecreaseOpponentShipCountEvent -= DecreaseOpponentShipCountEvent;
            _hubClient.DecreasePlayerShipCountEvent -= DecreasePlayerShipCountEvent;
            _hubClient.MarkMyTurnEvent -= MarkMyTurnEvent;
            _hubClient.MarkOpponentDestroyedShipEvent -= MarkOpponentDestroyedShipEvent;
            _hubClient.MarkOpponentTurnEvent -= MarkOpponentTurnEvent;
            _hubClient.MarkPlayerDestroyedShipEvent -= MarkPlayerDestroyedShipEvent;
            _hubClient.NetMyVictoryEvent -= NetMyVictoryEvent;
            _hubClient.NetOpponentAbortGameEvent -= NetOpponentAbortGameEvent;
            _hubClient.NetOpponentVictoryEvent -= NetOpponentVictoryEvent;
            _hubClient.OpponentHitEvent -= OpponentTargetHitEvent;
            _hubClient.OpponentMissEvent -= OpponentTargetMissEvent;
            _hubClient.PlayerHitEvent -= PlayerTargetHitEvent;
            _hubClient.PlayerMissEvent -= PlayerTargetMissEvent;
        }

        private async void OnAbortGameEvent()
        {
            await _hubClient.NetAbortGameAsync();
            DeregisterHubEvents();
        }

        private void PlayerTargetMissEvent(string x, string y)
        {
            _view.PlayerTargetMiss((int.Parse(x), int.Parse(y)));
        }

        private void PlayerTargetHitEvent(string x, string y)
        {
            _view.PlayerTargetHit((int.Parse(x), int.Parse(y)));
        }

        private void OpponentTargetMissEvent(string x, string y)
        {
            _view.OpponentTargetMiss((int.Parse(x), int.Parse(y)));
        }

        private void OpponentTargetHitEvent(string x, string y)
        {
            _view.OpponentTargetHit((int.Parse(x), int.Parse(y)));
        }

        private void NetMyVictoryEvent()
        {
            _view.NetMyVictory();
        }

        private void NetOpponentVictoryEvent()
        {
            _view.NetOpponentVictory();
        }

        private void NetOpponentAbortGameEvent(string name)
        {
            _view.NetOpponentAbortGame(name);
            DeregisterHubEvents();
        }

        private void MarkPlayerDestroyedShipEvent(string ship)
        {
            var shipDto = JsonConvert.DeserializeObject<ShipDto>(ship);
            var coordinates = new List<(int, int)>();
            foreach (var deck in shipDto!.Decks) 
            {
                coordinates.Add((deck.X, deck.Y));
            }

            _view.MarkPlayerDestroyedShip(coordinates);
        }

        private void MarkOpponentTurnEvent()
        {
            _view.MarkOpponentTurn();
        }

        private void MarkOpponentDestroyedShipEvent(string ship)
        {
            var shipDto = JsonConvert.DeserializeObject<ShipDto>(ship);
            var coordinates = new List<(int, int)>();
            foreach (var deck in shipDto!.Decks)
            {
                coordinates.Add((deck.X, deck.Y));
            }

            _view.MarkOpponentDestroyedShip(coordinates);
        }

        private void MarkMyTurnEvent()
        {
            _view.MarkMyTurn();
        }

        private void DecreasePlayerShipCountEvent(string deckCount, string liveShipsCount)
        {
            _view.DecreasePlayerShipCount(int.Parse(deckCount), int.Parse(liveShipsCount));
        }

        private void DecreaseOpponentShipCountEvent(string deckCount, string liveShipsCount)
        {
            _view.DecreaseOpponentShipCount(int.Parse(deckCount), int.Parse(liveShipsCount));
        }

        private void BeginNetGameEvent(string whoIsFirstId, string whoIsFirstName)
        {
            _view.BeginNetGame(whoIsFirstName);
            if (_hubClient.IsMyTurnFirstAsync(whoIsFirstId))
            {
                _view.MarkMyTurn();
                return;
            }

            _view.MarkOpponentTurn();
        }

        protected async void AbortGameEvent()
        {
            DeregisterHubEvents();
            await _hubClient.NetAbortGameAsync();
        }

        protected async override void StartGameEvent()
        {
            List<ShipDto> shipsDto = [];
            var playerShips = _game.GetPlayerShips();
            foreach (var ship in playerShips) 
            {
                shipsDto.Add(ShipDto.MapShipToShipDto(ship));
            }

            var ships = JsonConvert.SerializeObject(shipsDto);
            await _hubClient.ReadyToStartAsync(_gameSessionId, ships);
        }

        protected async override void MoveFinished(int x, int y)
        {
            await _hubClient.CompletingTurnAsync(_gameSessionId, x.ToString(), y.ToString());
        }

        protected async override void OpponentFieldClickEvent(int x, int y)
        {
            await _hubClient.MoveAsync(_gameSessionId, x.ToString(), y.ToString());
        }
    }
}
