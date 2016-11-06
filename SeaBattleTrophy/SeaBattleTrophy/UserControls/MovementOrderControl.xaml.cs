using System;
using System.Windows;
using System.Windows.Controls;

namespace SeaBattleTrophy.WPF
{
    /// <summary>
    /// Interaction logic for MovementOrderControl.xaml
    /// </summary>
    public partial class MovementOrderControl : UserControl
    {
        public MovementOrderControl()
        {
            InitializeComponent();
        }

        public delegate void MovementOrderAdded(double distance);

        public event MovementOrderAdded OrderAdded;

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(MovementOrderControl));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(double), typeof(MovementOrderControl),
            new PropertyMetadata(100.0, new PropertyChangedCallback(HandleTimeChanged), new CoerceValueCallback(CoerceTime)));

        public double Time
        {
            get { return (double)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        private static object CoerceTime(DependencyObject d, object value)
        {
            var control = (MovementOrderControl)d;

            var distanceValue = (double)value;

            return Math.Max(Math.Min(control.MaxTime, distanceValue), control.MinTime);
        }

        private static void HandleTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(MaxTimeProperty);
            d.CoerceValue(MinTimeProperty);
        }

        public static readonly DependencyProperty MaxTimeProperty = DependencyProperty.Register("MaxTime", typeof(double), 
            typeof(MovementOrderControl), new PropertyMetadata(1.0, new PropertyChangedCallback(MaxTimeChanged)));

        private static void MaxTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(TimeProperty, e.NewValue);
        }

        public static readonly DependencyProperty MinTimeProperty = DependencyProperty.Register("MinTime", typeof(double), 
            typeof(MovementOrderControl), new PropertyMetadata(0.0, new PropertyChangedCallback(MinTimeChanged)));

        private static void MinTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(TimeProperty);
        }

        public double MaxTime
        {
            get { return (double)GetValue(MaxTimeProperty); }
            set { SetValue(MaxTimeProperty, value); }
        }

        public double MinTime
        {
            get { return (double)GetValue(MinTimeProperty); }
            set { SetValue(MinTimeProperty, value); }
        }

        private void HandleButtonClicked(object sender, RoutedEventArgs e)
        {
            if (OrderAdded != null)
                OrderAdded(Time);
        }

        private void HandleMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Time += e.Delta/240.0;
        }
    }
}
