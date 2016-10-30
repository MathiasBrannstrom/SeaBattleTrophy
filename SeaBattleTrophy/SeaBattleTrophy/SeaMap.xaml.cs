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
using System.Windows.Shapes;
using SeaBattleTrophyGame;
using SeaBattleTrophy.WPF.UserControls.Ships;
using SeaBattleTrophy.WPF.UserControls;
using SeaBattleTrophy.WPF.ViewModels;

namespace SeaBattleTrophy.WPF
{
    /// <summary>
    /// Interaction logic for SeaMap.xaml
    /// </summary>
    public partial class SeaMap : UserControl
    {
        SeaBattleTrophyGameViewModel _game;

        public const int SeaMapSizeInPixels = 1000;

        float _metersPerPixel;

        public SeaMap()
        {
            InitializeComponent();
            Width = SeaMapSizeInPixels;
            Height = SeaMapSizeInPixels;
        }

        private void Reset()
        {
            ShipGrid.Children.Clear();
            LandMassGrid.Children.Clear();
            _game = null;
        }

        public void AddShips()
        {
            foreach(var ship in _game.Ships)
            {
                var shipVM = new ShipViewModel(ship, _metersPerPixel, _game.SelectedShip);

                var shipControl = new Ship();
                shipControl.DataContext = shipVM;
                ShipGrid.Children.Add(shipControl);
            }
        }

        public void AddLandMasses()
        {
            foreach(var landMass in _game.SeaMap.LandMasses)
            {
                var landMassVM = new LandMassViewModel(landMass, _metersPerPixel);

                var landMassControl = new LandMass();
                landMassControl.DataContext = landMassVM;
                LandMassGrid.Children.Add(landMassControl);
            }
        }

        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var game = DataContext as SeaBattleTrophyGameViewModel;
            if (game == null)
                throw new InvalidCastException("The data context should be of correct type");

            Reset();

            _game = game;
            _metersPerPixel = _game.SeaMap.SizeInMeters / SeaMapSizeInPixels;

            AddShips();
            AddLandMasses();
        }
    }
}
