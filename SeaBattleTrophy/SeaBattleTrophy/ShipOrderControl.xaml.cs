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

namespace SeaBattleTrophy.WPF
{
    /// <summary>
    /// Interaction logic for ShipOrderControl.xaml
    /// </summary>
    public partial class ShipOrderControl : UserControl
    {
        private ShipOrderViewModel _shipOrderViewModel;

        public ShipOrderControl()
        {
            InitializeComponent();
        }

        private void ForwardButtonClicked(object sender, RoutedEventArgs e)
        {
            _shipOrderViewModel.ForwardButton();
        }

        private void CWButtonClicked(object sender, RoutedEventArgs e)
        {
            _shipOrderViewModel.TurnCW();
        }

        private void CCWButtonClicked(object sender, RoutedEventArgs e)
        {
            _shipOrderViewModel.TurnCCW();
        }

        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var shipOrderViewModel = DataContext as ShipOrderViewModel;

            if (shipOrderViewModel == null)
                throw new InvalidCastException("The data context should be correct type");

            _shipOrderViewModel = shipOrderViewModel;
        }
    }
}
