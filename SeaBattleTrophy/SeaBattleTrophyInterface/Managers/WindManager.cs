using Maths;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace SeaBattleTrophyGame
{
    public interface IWind
    {
        // In degrees. 0 is blowing towards the north, the same as the angle for a ship
        double Angle { get; }

        // in m/s
        double Velocity { get; }
    }

    public struct Wind : IWind
    {
        public Wind(double angle, double velocity)
        {
            Angle = angle.Modulo(360);
            Velocity = velocity;
        }

        // 0 is blowing towards the north, the same as the angle for a ship
        public double Angle { get; }

        // in m/s
        public double Velocity { get; }
    }
    
    public interface IWindManagerReadOnly : INotifyPropertyChanged
    {
        IWind CurrentWind { get; }
    }

    public interface IWindManager : IWindManagerReadOnly
    {
        /// <summary>
        /// Updates the wind. Change is based on how much time has passed since last update.
        /// </summary>
        /// <param name="timeStep">Time step in seconds.</param>
        void UpdateWind(TimeSpan timeStep);
    }

    public class WindManager : IWindManager
    {
        private Wind _wind = new Wind(270, 10);
        public IWind CurrentWind { get { return _wind; } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateWind(TimeSpan timeStep)
        {
            //_wind = new Wind(_wind.Angle + timeStep.TotalSeconds*4, _wind.Velocity);
            // Do nothing for now.
            PropertyChanged.Raise(() => CurrentWind);
        }
    }
}
