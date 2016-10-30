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
        float DistanceFromLand { get; }
    }

    public interface IShipStatus : IShipStatusReadOnly
    {
        new float DistanceFromLand { get; set; }
    }

    public class ShipStatus : IShipStatus
    {
        private float _distanceFromLand;
        public float DistanceFromLand
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

        float IShipStatusReadOnly.DistanceFromLand
        {
            get
            {
                return DistanceFromLand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
