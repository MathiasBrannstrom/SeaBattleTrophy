using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SeaBattleTrophyGame;
using SeaBattleTrophy.WPF.ViewModels;
using System.Threading;
using System.Globalization;

namespace SeaBattleTrophy.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SeaBattleTrophyGameViewModel _gameViewModel;

        public MainWindow()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("sv-SV");
            InitializeComponent();
            _gameViewModel = new SeaBattleTrophyGameViewModel();
            SetupSeaMap();
            SetupShipOrderControl();
            SetupTurnControl();
            SetupShipStatusControl();
            SetupWindDisplay();
        }

        private void SetupWindDisplay()
        {
            var windDisplayViewModel = new WindDisplayViewModel(_gameViewModel.WindManager);
            WindDisplay.DataContext = windDisplayViewModel;
        }

        private void SetupTurnControl()
        {
            var turnManagerViewModel = new TurnManagerViewModel(_gameViewModel.TurnManager);
            TurnControl.DataContext = turnManagerViewModel;
        }

        private void SetupShipStatusControl()
        {
            var shipStatusViewModel = new ShipStatusViewModel(_gameViewModel.SelectedShip);
            ShipStatusControl.DataContext = shipStatusViewModel;
        }

        private void SetupSeaMap()
        {
            SeaMap.DataContext = _gameViewModel;
        }

        private void SetupShipOrderControl()
        {
            var shipOrderViewModel = new ShipOrderViewModel(_gameViewModel.SelectedShip, _gameViewModel.ShipOrderEditor);
            ShipOrderControl.DataContext = shipOrderViewModel;
        }

        private void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            _gameViewModel.SeaMapSizeInPixels.Value = Math.Max(10, _gameViewModel.SeaMapSizeInPixels.Value + e.Delta);
        }
    }
}
