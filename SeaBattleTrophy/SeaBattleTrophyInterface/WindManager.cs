using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleTrophyGame
{
    public interface IWind
    {
        // In degrees. 0 is blowing towards the north, the same as the angle for a ship
        float Angle { get; }

        // in m/s
        float Velocity { get; }
    }

    public struct Wind : IWind
    {
        public Wind(float angle, float velocity)
        {
            Angle = angle;
            Velocity = velocity;
        }

        // 0 is blowing towards the north, the same as the angle for a ship
        public float Angle { get; }

        // in m/s
        public float Velocity { get; }
    }

    public interface IWindManager
    {
        IWind CurrentWind { get; }

        /// <summary>
        /// Updates the wind. Change is based on how much time has passed since last update.
        /// </summary>
        /// <param name="timeStep">Time step in seconds.</param>
        void UpdateWind(float timeStep);
    }

    public class WindManager : IWindManager
    {
        private Wind _wind = new Wind(0, 10);
        public IWind CurrentWind { get { return _wind; } }

        public void UpdateWind(float timeStep)
        {
            // Do nothing for now.
        }
    }
}
