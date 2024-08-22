using SeaFightToolkit.ClassicSeaFight.Game;
using SeaFightToolkit.ClassicSeaFight.AI;

namespace SeaFightWinForms.Presenter
{
    abstract class GamePresenter
    {
        protected ComputerSession _game;
        protected SessionPrepareHelper _sessionHelper;
        protected IView _view;

        public GamePresenter(IView view)
        {
            _view = view;
            _game = new ComputerSession(new Computer(new StandartStrategy()));
            _sessionHelper = new SessionPrepareHelper(_game);
            RegisterGameHelperEvents();
        }

        private void RegisterGameHelperEvents()
        {
            _sessionHelper.ArrowHorizontal += ArrowHorizontal;
            _sessionHelper.ArrowVertical += ArrowVertical;
            _sessionHelper.IndifferentPositionEvent += BackTransperent;
            _sessionHelper.BadPositionEvent += ChangeColorBad;
            _sessionHelper.GoodPositionEvent += ChangeColorGood;
            _sessionHelper.ChangeFourDeckCount += ChangeFourDeckCount;
            _sessionHelper.ChangeOneDeckCount += ChangeOneDeckCount;
            _sessionHelper.ChangeTwoDeckCount += ChangeTwoDeckCount;
            _sessionHelper.ChangeThreeDeckCount += ChangeThreeDeckCount;
            _sessionHelper.PlacePlayerShip += PlacePlayerShip;
            _sessionHelper.HideStartButton += HideStartButton;
            _sessionHelper.ShipTypeChanged += ShipTypeChanged;
            _sessionHelper.ShowStartButton += ShowStartButton;

            _view.OpponentFieldClickEvent += OpponentFieldClickEvent;
            _view.AutoGenerateShipsEvent += _iView_AutoGenerateShipsEvent;
            _view.MouseInEvent += _iView_MouseInEvent;
            _view.MouseOutEvent += _iView_MouseOutEvent;
            _view.PlayerFieldClickEvent += _iView_PlayerFieldClickEvent;
            _view.ChangeShipTypeEvent += _iView_ChangeShipTypeEvent;
            _view.ShipDirectionEvent += _iView_ShipDirectionEvent;
            _view.StartGameEvent += StartGameEvent;
            _view.AutoGenerateShipsEvent += _iView_AutoGenerateShipsEvent;
            _view.MoveFinished += MoveFinished;
        }

        protected void ShowStartButton()
        {
            _view.ShowStartButton();
        }

        protected void ShipTypeChanged(int type)
        {
            _view.ChangeShipType(type);
        }

        protected void HideStartButton()
        {
            _view.HideStartButton();
        }

        protected void PlacePlayerShip(List<(int, int)> ship)
        {
            _view.PlacePlayerShip(ship);
        }

        protected void ChangeThreeDeckCount(int count)
        {
            _view.ChangeThreeDeckCount(count);
        }

        protected void ChangeTwoDeckCount(int count)
        {
            _view.ChangeTwoDeckCount(count);
        }

        protected void ChangeOneDeckCount(int count)
        {
            _view.ChangeOneDeckCount(count);
        }

        protected void ChangeFourDeckCount(int count)
        {
            _view.ChangeFourDeckCount(count);
        }

        protected void ChangeColorGood(List<(int, int)> ship)
        {
            _view.ChangeColorGood(ship);
        }

        protected void ChangeColorBad(List<(int, int)> ship)
        {
            _view.ChangeColorBad(ship);
        }

        protected void BackTransperent(List<(int, int)> ship)
        {
            _view.BackTransperent(ship);
        }

        protected void ArrowVertical()
        {
            _view.ArrowVertical();
        }

        protected void ArrowHorizontal()
        {
            _view.ArrowHorizontal();
        }

        #region Подготовка игры
        //Обработка нажатия на кнопку Лень
        protected void _iView_AutoGenerateShipsEvent()
        {
            _sessionHelper.AutoPlayerShips();
        }

        //Обработка нажатия на кнопку Направление
        protected void _iView_ShipDirectionEvent()
        {
            _sessionHelper.ChangeShipOrientation();
        }

        //Обработка нажатия на тип корабля
        protected void _iView_ChangeShipTypeEvent(int type)
        {
            _sessionHelper.ChangeShipType(type);
        }

        //Обработка клика по полю игрока
        protected void _iView_PlayerFieldClickEvent(int x, int y)
        {
            _sessionHelper.PlayerFieldClick(x, y);
        }

        //Обработка выхода курсора мыши
        protected void _iView_MouseOutEvent()
        {
            _sessionHelper.MouseOut();
        }

        //Обработка входа курсора мыши
        protected void _iView_MouseInEvent(int x, int y)
        {
            _sessionHelper.MouseIn(x, y);
        }

        abstract protected void MoveFinished(int x, int y);

        abstract protected void StartGameEvent();

        abstract protected void OpponentFieldClickEvent(int x, int y);
        #endregion
    }
}
