using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame.Objects.Ship;
using Utilities;

namespace SeaBattleTrophyGame
{

    public interface ISeaBattleTrophyGame
    {
        List<IShip> Ships { get; }

        ISeaMap SeaMap { get; }
    }

    public class SeaBattleTrophyGameManager : ISeaBattleTrophyGame
    {
        public SeaBattleTrophyGameManager()
        {
            Ships = new List<IShip>();

            Ships.Add(new Ship { Width = 10, Length = 40, Position = new Point2D { X = 30, Y = 30 }, AngleInDegrees = 30 });
            SeaMap = new SeaMap { SizeInMeters = 200 };
        }

        public ISeaMap SeaMap { get; private set; }

        public List<IShip> Ships { get; private set; }
    }
}
