using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Commands;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    class ChangeLevelSizeWindowViewModel 
    {
        LevelViewModel lvm;
        private int levelWidth;
        private int levelHeight;
        public int LevelWidth
        {
            get
            {
                return levelWidth;
            }
            set
            {
                levelWidth = value;
                OnPropertyChanged("LevelWidth");
            }
        }

        public int LevelHeight
        {
            get
            {
                return levelHeight;
            }
            set
            {
                levelHeight = value;
                OnPropertyChanged("LevelHeight");
            }
        }
        public ICommand UpdateSizeCommand { get; set; }

        public ChangeLevelSizeWindowViewModel(LevelViewModel lvm)
        {
            this.lvm = lvm;
            LevelHeight = lvm.LevelNumberOfRow;
            LevelWidth = lvm.LevelNumberOfColumn;
            UpdateSizeCommand = new BaseCommand(UpdateLevelSize, obj => true);
        }

        private void UpdateLevelSize(object param)
        {
            lvm.UpdateLevelSize(levelWidth, LevelHeight);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
