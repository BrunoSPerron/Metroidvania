using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Commands;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Models;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    public class AnimationManagerWindowViewModel
    {
        /*private EditorViewModel evm;

        public AnimatedTileBuilder currentATB { get; set; }
        public ObservableCollection<AnimatedTileBuilder> AnimatedTilesCollection { get; set; }
        
        public int NumberOfFrames
        {
            get => currentATB.IndexesX.Length;
            set
            {
                if (value != currentATB.IndexesX.Length)
                {
                    currentATB.SetNbOfFrames(value);
                    if (value < CurrentFrameModified)
                        CurrentFrameModified = value;
                    OnPropertyChanged("NumberOfFrames");
                }
            }
        }

        private int _currentFrameModified;
        public int CurrentFrameModified
        {
            get => _currentFrameModified + 1;
            set
            {
                _currentFrameModified = value - 1;
                OnPropertyChanged("CurrentFrameModified");
                OnPropertyChanged("CurrentX");
                OnPropertyChanged("CurrentY");
                OnPropertyChanged("CurrentShowTime");
            }
        }
        public int CurrentX
        {
            get => currentATB.IndexesX[CurrentFrameModified - 1] + 1;
            set
            {
                currentATB.IndexesX[CurrentFrameModified - 1] = value - 1;
                OnPropertyChanged("CurrentX");
            }
        }
        public int CurrentY
        {
            get => currentATB.IndexesY[CurrentFrameModified - 1] + 1;
            set
            {
                currentATB.IndexesY[CurrentFrameModified - 1] = value - 1;
                OnPropertyChanged("CurrentY");
            }
        }
        public int CurrentShowTime
        {
            get => currentATB.ShowTimes[CurrentFrameModified - 1];
            set
            {
                currentATB.ShowTimes[CurrentFrameModified - 1] = value;
                OnPropertyChanged("CurrentShowTime");
            }
        }

        public string[] TilesetDroplistOptions => TilesetContainer.TsContainer.TilesetNames;
        public ICommand TilesetComboboxChangedCommand { get; set; }
        public ICommand AddCommand { get; set; }

        public AnimationManagerWindowViewModel(EditorViewModel evm)
        {
            this.evm = evm;

            AnimatedTilesCollection = new ObservableCollection<AnimatedTileBuilder>();
            PopulateLocalFromAnimatedTile(evm.GetAnimatedTiles);
            for (int i = 0; i < 4; i++)
                AnimatedTilesCollection.Add(new AnimatedTileBuilder(TilesetContainer.TsContainer.TilesetNames[0], new int[] { 4, 4, 4, 4 }, new int[] { i, i + 1, i + 2, i + 3 }, new int[] { 250, 100, 2000, 50 }, this));
            currentATB = AnimatedTilesCollection[0];

            NumberOfFrames = 1;
            CurrentFrameModified = 1;
            CurrentX = 1;
            CurrentY = 1;
            CurrentShowTime = 250;
            AddCommand = new SimpleCommand(AddCurrentTile);
            TilesetComboboxChangedCommand = new SimpleCommand(TilesetComboboxChanged);
        }

        private void TilesetComboboxChanged(object param)
        {
            currentATB.TilesetName = (string)((ComboBox)(((SelectionChangedEventArgs)param).Source)).SelectedItem;
        }

        private void PopulateLocalFromAnimatedTile(AnimatedTile[] currentsTiles)
        {
            foreach (AnimatedTile at in currentsTiles)
                AnimatedTilesCollection.Add(new AnimatedTileBuilder(at.TilesetName, at.IndexesX, at.IndexesY, at.ShowTimes, this));
        }

        private void AddCurrentTile(object param) 
        {
            AnimatedTilesCollection.Add(currentATB);
            currentATB = new AnimatedTileBuilder(TilesetContainer.TsContainer.TilesetNames[0], new int[] { 0 }, new int[] { 0 }, new int[] { 250 }, this);
            OnPropertyChanged("AnimatedTilesCollection");
            evm.UpdateAnimatedTileset(AnimatedTilesCollection);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));*/
    }
}
