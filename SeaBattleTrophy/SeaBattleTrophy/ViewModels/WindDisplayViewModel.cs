using SeaBattleTrophyGame;
using System.ComponentModel;
using Utilities;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class WindDisplayViewModel : INotifyPropertyChanged
    {
        private IWindManagerReadOnly _windManager;

        public event PropertyChangedEventHandler PropertyChanged;

        public WindDisplayViewModel(IWindManagerReadOnly windManager)
        {
            _windManager = windManager;
            _windManager.PropertyChanged += HandleWindManagerPropertyChanged;
        }

        private void HandleWindManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IWindManagerReadOnly.CurrentWind))
                PropertyChanged.Raise(() => CompassRotationAngle);
        }

        public double CompassRotationAngle { get { return -_windManager.CurrentWind.Angle; } }
    }
}
