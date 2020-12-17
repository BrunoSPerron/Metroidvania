using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Commands;
using ProjetJeuxVideo_LevelEditor_Metroidvania.HelperClasses;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Models;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    public class AnimationMakerViewModel : TileManager
    {
        EditorViewModel evm;
        public AnimatedTileBuilder currentATB { get; set; }
        int currentTilesetIndex;

        private ObservableCollection<AdvancedTileForBuilder> currentTilesCollection;
        public BitmapImage CurrentTileset => LoadedTilesets[currentTilesetIndex];
        public int CurrentNumberOfRow => NumberOfRow[currentTilesetIndex];

        internal void UpdateShowTime(int value, int index)
        {
            currentATB.ShowTimes[index] = value;
        }

        public int CurrentNumberOfColumn => NumberOfColumn[currentTilesetIndex];
        public string[] TilesetDroplistOptions
        {
            get
            {
                return tilesetNames;
            }
        }

        public ObservableCollection<AdvancedTileForBuilder> CurrentTilesCollection
        {
            get => currentTilesCollection;
            set
            {
                currentTilesCollection = value;
                OnPropertyChanged("CurrentTilesCollection");
            }
        }

        public SimpleCommand TilesetComboboxChangedCommand { get; set; }
        public SimpleCommand AddCommand { get; set; }

        public AnimationMakerViewModel(EditorViewModel evm)
        {
            this.evm = evm;
            currentTilesetIndex = 0;
            currentATB = new AnimatedTileBuilder(TilesetNames[currentTilesetIndex], this);
            PopulateGridFromTileset(0);
            TilesetComboboxChangedCommand = new SimpleCommand(TilesetComboboxChanged);
            CurrentTilesCollection = new ObservableCollection<AdvancedTileForBuilder>();
            AddCommand = new SimpleCommand(SendCurrentTile);
        }

        private void SendCurrentTile(object obj)
        {
            evm.AddAnimatedTile(currentATB);
        }

        private void PopulateGridFromTileset(int tilesetIndex)
        {
            TilesCollection = new ObservableCollection<Tile>();
            for (short i = 0; i < NumberOfRow[tilesetIndex]; i++)
                for (short j = 0; j < NumberOfColumn[tilesetIndex]; j++)
                    TilesCollection.Add(new AdvancedTile(TilesetNames[tilesetIndex], new Coord(j, i), j, i, this));

            OnPropertyChanged("CurrentNumberOfRow");
            OnPropertyChanged("CurrentNumberOfColumn");
            OnPropertyChanged("TilesCollection");
        }
        private void PopulateGridFromTileset(string s)
        {
            for (int i = 0; i < TilesetNames.Length; i++)
                if (TilesetNames[i] == s)
                    PopulateGridFromTileset(i);
        }

        public void TilesetComboboxChanged(object param)
        {
            string text = ((ComboBox)(((SelectionChangedEventArgs)param).Source)).SelectedItem as String;

            for (int i = 0; i < TilesetNames.Length; i++)
                if (TilesetNames[i] == text)
                    currentTilesetIndex = i;

            PopulateGridFromTileset(text);
        }

        public override void MouseLeavingTile(int x, int y, Coord pos)
        {
            foreach (Tile t in TilesCollection)
                t.CurrentAlpha = 1;
        }

        public override void MouseReleaseOnTile(int x, int y, Coord pos)
        {
            AdvancedTileForBuilder CurrentTile = new AdvancedTileForBuilder(TilesetContainer.TsContainer.TilesetNames[currentTilesetIndex], CurrentTilesCollection.Count, x, y, this);
            CurrentTilesCollection.Add(CurrentTile);
            currentATB.AddFrame(pos.x, pos.y);
        }

        public void MouseReleaseOnFrame(int index)
        {
            currentATB.RemoveFrame(index);
            CurrentTilesCollection.RemoveAt(index);
            int i = 0;
            while (i < CurrentTilesCollection.Count)
            {
                CurrentTilesCollection[i].index = i;
                i++;
            }
        }
    }
}
