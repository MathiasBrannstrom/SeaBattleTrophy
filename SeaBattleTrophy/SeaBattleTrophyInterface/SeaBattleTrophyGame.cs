using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophy.Objects.Ship;

namespace SeaBattleTrophy
{

    public interface ISeaBattleTrophyGame
    {
        List<IShip> Ships { get; }
    }

    public class SeaBattleTrophyGame : ISeaBattleTrophyGame
    {
        public SeaBattleTrophyGame()
        {
            Ships = new List<IShip>();

            Ships.Add(new Ship { Width = 10, Length = 40 });
        }

        public List<IShip> Ships { get; private set; }
    }
}
