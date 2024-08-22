using SeaFightToolkit.Common.Enums;

namespace SeaFightWinForms.Presenter
{
    class GamePresenterOffline : GamePresenter
    {
        public GamePresenterOffline(IView view) : base(view)
        {
            RegisterGameEvents();
        }

        private void RegisterGameEvents()
        {
            _game.OpponentShipDestroyed += OpponentShipDestroyed;
            _game.PlayerShipDestroyed += PlayerShipDestroyed;
            _game.OpponentShipHit += OpponentShipHit;
            _game.OpponentShipMiss += OpponentShipMiss;
            _game.PlayerShipHit += PlayerShipHit;
            _game.PlayerShipMiss += PlayerShipMiss;
            _game.VictoryHappened += VictoryHappened;
            _game.DecreaseOpponentShipCount += DecreaseOpponentShipCount;
            _game.DecreasePlayerShipCount += DecreasePlayerShipCount;
            _game.FirstChosen += FirstChosen;
            _game.MarkMyTurnEvent += MarkMyTurnEvent;
            _game.MarkOpponentTurnEvent += MarkOpponentTurnEvent;
        }

        private void MarkOpponentTurnEvent()
        {
            _view.MarkOpponentTurn();
        }

        private void MarkMyTurnEvent()
        {
            _view.MarkMyTurn();
        }

        private void FirstChosen(Participants first)
        {
            _view.BeginGame(first);
        }

        private void DecreasePlayerShipCount(int deck, int value)
        {
            _view.DecreasePlayerShipCount(deck, value);
        }

        private void DecreaseOpponentShipCount(int deck, int value)
        {
            _view.DecreaseOpponentShipCount(deck, value);
        }

        private void VictoryHappened(Participants victorious)
        {
            _view.Victory(victorious);
        }

        private void PlayerShipMiss((int, int) destinationPoint)
        {
            _view.PlayerTargetMiss(destinationPoint);
        }

        private void PlayerShipHit((int, int) destinationPoint)
        {
            _view.PlayerTargetHit(destinationPoint);
        }

        private void OpponentShipMiss((int, int) destinationPoint)
        {
            _view.OpponentTargetMiss(destinationPoint);
        }

        private void OpponentShipHit((int, int) destinationPoint)
        {
            _view.OpponentTargetHit(destinationPoint);
        }

        private void PlayerShipDestroyed(List<(int, int)> decks)
        {
            _view.MarkPlayerDestroyedShip(decks);
        }

        private void OpponentShipDestroyed(List<(int, int)> decks)
        {
            _view.MarkOpponentDestroyedShip(decks);
        }

        protected override void StartGameEvent()
        {
            _sessionHelper.AutoOpponentShips();
            _game.StartGame();
        }

        protected override void OpponentFieldClickEvent(int x, int y)
        {
            _game.Turn(x, y);
        }

        protected override void MoveFinished(int x, int y)
        {
            _game.CompletingTurn(x, y);
        }
    }
}
