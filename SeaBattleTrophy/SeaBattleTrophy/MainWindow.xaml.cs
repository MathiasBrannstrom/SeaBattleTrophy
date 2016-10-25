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
using SeaBattleTrophyGame;

namespace SeaBattleTrophy.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ISeaBattleTrophyGame _game;

        public MainWindow()
        {
            InitializeComponent();
            SetupGame();
            SeaMap.DataContext = _game;
        }

        public void SetupGame()
        {
            _game = new SeaBattleTrophyGameManager();

        }
    }
}
