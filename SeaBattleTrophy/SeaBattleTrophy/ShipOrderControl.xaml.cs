using System;
using System.Windows;
using System.Windows.Controls;
using SeaBattleTrophy.WPF.ViewModels;
using SeaBattleTrophyGame;

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

        private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var shipOrderViewModel = DataContext as ShipOrderViewModel;

            if (shipOrderViewModel == null)
                throw new InvalidCastException("The data context should be correct type");

            _shipOrderViewModel = shipOrderViewModel;
        }

        private void HandleRemoveMovementOrderButtonClicked(object sender, RoutedEventArgs e)
        {
            var dataContext = ((FrameworkElement)sender).DataContext;

            var movementOrder = (MovementOrder)dataContext;

            _shipOrderViewModel.RemoveMovementOrder(movementOrder);
            
        }

        private void HandleForwardMovementOrderAdded(float distance)
        {
            _shipOrderViewModel.AddMovementOrder(Direction.Forward, distance);
        }

        private void HandlePortMovementOrderAdded(float distance)
        {
            _shipOrderViewModel.AddMovementOrder(Direction.Port, distance);
        }

        private void HandleStarboardMovementOrderAdded(float distance)
        {
            _shipOrderViewModel.AddMovementOrder(Direction.Starboard, distance);
        }
    }

    public class MovementOrderTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ForwardMovementOrderTemplate { get; set; }
        public DataTemplate YawMovementOrderTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (item is ForwardMovementOrder)
                return ForwardMovementOrderTemplate;

            if (item is YawMovementOrder)
                return YawMovementOrderTemplate;

            throw new InvalidOperationException("There is no template for this movement order");
        }
    }
}
