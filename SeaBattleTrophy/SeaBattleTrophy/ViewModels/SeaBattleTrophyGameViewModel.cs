using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame;
using Utilities;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class SeaBattleTrophyGameViewModel
    {

        private ISeaBattleTrophyGame _game;

        public SeaBattleTrophyGameViewModel()
        {
            SetupGame();
        }

        public void SetupGame()
        {
            _game = new SeaBattleTrophyGameManager();
            SelectedShip = new ValueHolder<IShipReadOnly>(_game.Ships.First());
        }

        public IValueHolder<IShipReadOnly> SelectedShip { get; private set; }

        public IEnumerable<IShipReadOnly> Ships { get { return _game.Ships; } }

        public IShipOrderManager ShipOrderManager { get { return _game.ShipOrderManager; } }

        public ISeaMap SeaMap { get { return _game.SeaMap; } }
    }
}
