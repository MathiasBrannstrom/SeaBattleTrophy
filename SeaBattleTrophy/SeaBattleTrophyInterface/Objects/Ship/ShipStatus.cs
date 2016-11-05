using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace SeaBattleTrophyGame
{
    public interface IShipStatusReadOnly : INotifyPropertyChanged
    {
        double DistanceFromLand { get; }
    }

    public interface IShipStatus : IShipStatusReadOnly
    {
        new double DistanceFromLand { get; set; }
    }

    public class ShipStatus : IShipStatus
    {
        private double _distanceFromLand;
        public double DistanceFromLand
        {
            get
            {
                return _distanceFromLand;
            }

            set
            {
                if(_distanceFromLand != value)
                {
                    _distanceFromLand = value;
                    PropertyChanged.Raise(() => DistanceFromLand);
                }
            }
        }

        double IShipStatusReadOnly.DistanceFromLand
        {
            get
            {
                return DistanceFromLand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
