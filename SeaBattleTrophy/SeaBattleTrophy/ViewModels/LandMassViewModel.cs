using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame;
using System.Windows.Media;
using System.Windows;
using Utilities;
using Maths.Geometry;
using System.ComponentModel;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class LandMassViewModel : INotifyPropertyChanged
    {
        private IValueHolderReadOnly<float> _metersPerPixel;
        private IValueHolderReadOnly<float> _seaMapSizeInPixels;
        private LandMass _landMass;

        public event PropertyChangedEventHandler PropertyChanged;

        public LandMassViewModel(LandMass landMass, IValueHolderReadOnly<float> metersPerPixel, IValueHolderReadOnly<float> seaMapSizeInPixels)
        {
            _landMass = landMass;
            _metersPerPixel = metersPerPixel;
            _seaMapSizeInPixels = seaMapSizeInPixels;

            _metersPerPixel.PropertyChanged += HandleMetersPerPixelPropertyChanged;
            HandleMetersPerPixelPropertyChanged(this, new PropertyChangedEventArgs(null));
        }

        private void HandleMetersPerPixelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePixelCoordinates();
        }

        private void UpdatePixelCoordinates()
        {
            PixelCoordinates = new PointCollection(_landMass.CornerCoordinates.Select(p => 
            new Point(p.X / _metersPerPixel.Value, _seaMapSizeInPixels.Value - p.Y / _metersPerPixel.Value)));
            PropertyChanged.Raise(() => PixelCoordinates);
        }

        public PointCollection PixelCoordinates { get; private set; }

        public Color LandColor { get { return Colors.DarkGreen; } }
    }
}
