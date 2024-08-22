using SeaFightToolkit.SignalR.Models;

namespace SeaFightWinForms.Presenter
{
    public interface IStartView
    {
        event Action<IView> StartOfflineGameEvent;
        event Action<IView, string> StartOnlineGameEvent;
        event Action<string> ConnectEvent;
        event Action DisconnectEvent;
        event Action<string, string> SendMessageEvent;
        event Action<string> OfferGameEvent;
        event Action<string, string> AnswerOfferEvent;

        void SuccessfullConnection();
        void ErrorConnection();
        void ShowPlayers(List<Player> players, string myName);
        void UpdateMessageList(string name, string message);
        void OfferToYou(string nameFrom, string idFrom, string nameTo);
        void AnswerOffering(string nameFrom, string nameTo, string answer, string sessionID);
        void YouAreBusy();
        void OpponentIsBusy();
        void NameMatched();
        void NameIsOccupied();
    }
}
