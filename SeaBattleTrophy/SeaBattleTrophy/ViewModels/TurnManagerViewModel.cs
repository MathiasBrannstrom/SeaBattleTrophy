using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattleTrophyGame;
using System.ComponentModel;
using Utilities;
using System.Windows.Input;

namespace SeaBattleTrophy.WPF.ViewModels
{
    public class TurnManagerViewModel : INotifyPropertyChanged
    {
        private ITurnManager _turnManager;

        public TurnManagerViewModel(ITurnManager turnManager)
        {
            _turnManager = turnManager;
            _turnManager.PropertyChanged += HandleTurnManagerPropertyChanged;
            FinishCurrentPhaseCommand = new Command(() => _turnManager.FinishCurrentPhase());
        }

        private void HandleTurnManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ITurnManager.ReadyToFinishCurrentPhase))
                PropertyChanged.Raise(() => ReadyToFinishCurrentPhase);
        }

        public bool ReadyToFinishCurrentPhase { get { return _turnManager.ReadyToFinishCurrentPhase; } }
        
        public ICommand FinishCurrentPhaseCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
