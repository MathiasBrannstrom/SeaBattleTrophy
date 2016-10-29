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
using SeaBattleTrophy.WPF.ViewModels;

namespace SeaBattleTrophy.WPF.UserControls.Ships
{
    /// <summary>
    /// Interaction logic for Ship.xaml
    /// </summary>
    public partial class Ship : UserControl
    {
        private ShipViewModel _shipViewModel;

        public Ship()
        {
            InitializeComponent();
        }

        private void HandleShipMouseDown(object sender, MouseButtonEventArgs e)
        {
            _shipViewModel.SelectThisShip();
        }

        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var shipViewModel = DataContext as ShipViewModel;
            if (shipViewModel == null)
                throw new InvalidCastException("DataContext for Ship should be correct type");

            _shipViewModel = shipViewModel;
        }
    }
}
