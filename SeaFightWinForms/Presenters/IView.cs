using SeaFightToolkit.Common.Enums;

namespace SeaFightWinForms.Presenter
{
    public interface IView
    {
        void PlayerTargetHit((int, int) destinationPoint);
        void PlayerTargetMiss((int, int) destinationPoint);
        void OpponentTargetHit((int, int) destinationPoint);
        void OpponentTargetMiss((int, int) destinationPoint);
        void DecreasePlayerShipCount(int deckNumber, int liveShips);
        void DecreaseOpponentShipCount(int deckNumber, int liveShips);
        void MarkPlayerDestroyedShip(List<(int, int)> decks);
        void MarkOpponentDestroyedShip(List<(int, int)> decks);
        void MarkMyTurn();
        void MarkOpponentTurn();

        void BeginGame(Participants who);
        void Victory(Participants who);
        void NetMyVictory();
        void NetOpponentVictory();
        void NetOpponentAbortGame(string name);
        void BeginNetGame(string whoIsFirst);

        event Action<int, int> MouseInEvent;
        event Action MouseOutEvent;
        event Action<int, int> PlayerFieldClickEvent;
        event Action ShipDirectionEvent;
        event Action<int> ChangeShipTypeEvent;
        event Action AutoGenerateShipsEvent;
        event Action AbortGameEvent;
        event Action<int, int> OpponentFieldClickEvent;
        event Action<int, int> MoveFinished;
        event Action StartGameEvent;

        void PlacePlayerShip(List<(int, int)> coordinates);

        void ChangeColorGood(List<(int, int)> coordinates);
        void ChangeColorBad(List<(int, int)> coordinates);
        void BackTransperent(List<(int, int)> coordinates);

        void ArrowHorizontal();
        void ArrowVertical();

        void HideStartButton();
        void ShowStartButton();

        void ChangeOneDeckCount(int x);
        void ChangeTwoDeckCount(int x);
        void ChangeThreeDeckCount(int x);
        void ChangeFourDeckCount(int x);
        void ChangeShipType(int decks);
    }
}
