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
        }

        private void SetupSeaMap()
        {
            SeaMap.DataContext = _gameViewModel;
        }

        private void SetupShipOrderControl()
        {
            var shipOrderViewModel = new ShipOrderViewModel(_gameViewModel.SelectedShip, _gameViewModel.ShipOrderManager);
            ShipOrderControl.DataContext = shipOrderViewModel;
        }

    }
}
