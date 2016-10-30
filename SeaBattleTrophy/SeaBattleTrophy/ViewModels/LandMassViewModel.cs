using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame;
using System.Windows.Media;
using System.Windows;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class LandMassViewModel
    {
        public LandMassViewModel(Land land, float metersPerPixel)
        {
            PixelCoordinates = new PointCollection(land.CornerCoordinates.Select(p => new Point(p.X / metersPerPixel, SeaMap.SeaMapSizeInPixels - p.Y/metersPerPixel)));
        }

        public PointCollection PixelCoordinates { get; private set; }

        public Color LandColor { get { return Colors.DarkGreen; } }
    }
}
