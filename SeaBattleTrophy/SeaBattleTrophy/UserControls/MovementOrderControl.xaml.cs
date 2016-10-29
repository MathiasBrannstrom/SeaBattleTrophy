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

        public delegate void MovementOrderAdded(float distance);

        public event MovementOrderAdded OrderAdded;

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(MovementOrderControl));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty DistanceProperty = DependencyProperty.Register("Distance", typeof(float), typeof(MovementOrderControl),
            new PropertyMetadata(100f, new PropertyChangedCallback(HandleDistanceChanged), new CoerceValueCallback(CoerceDistance)));

        public float Distance
        {
            get { return (float)GetValue(DistanceProperty); }
            set { SetValue(DistanceProperty, value); }
        }

        private static object CoerceDistance(DependencyObject d, object value)
        {
            var control = (MovementOrderControl)d;

            var distanceValue = (float)value;

            return (float)Math.Max(Math.Min(control.MaxDistance, distanceValue), control.MinDistance);
        }

        private static void HandleDistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(MaxDistanceProperty);
            d.CoerceValue(MinDistanceProperty);
        }

        public static readonly DependencyProperty MaxDistanceProperty = DependencyProperty.Register("MaxDistance", typeof(float), typeof(MovementOrderControl), new PropertyMetadata(1f, new PropertyChangedCallback(MaxDistanceChanged)));

        private static void MaxDistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(DistanceProperty, e.NewValue);
        }

        public static readonly DependencyProperty MinDistanceProperty = DependencyProperty.Register("MinDistance", typeof(float), typeof(MovementOrderControl), new PropertyMetadata(0f, new PropertyChangedCallback(MinDistanceChanged)));

        private static void MinDistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(DistanceProperty);
        }

        public float MaxDistance
        {
            get { return (float)GetValue(MaxDistanceProperty); }
            set { SetValue(MaxDistanceProperty, value); }
        }

        public float MinDistance
        {
            get { return (float)GetValue(MinDistanceProperty); }
            set { SetValue(MinDistanceProperty, value); }
        }

        private void HandleButtonClicked(object sender, RoutedEventArgs e)
        {
            if (OrderAdded != null)
                OrderAdded(Distance);
        }
    }
}
