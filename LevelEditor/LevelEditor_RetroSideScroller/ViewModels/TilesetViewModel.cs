using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Models;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Commands;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Views;
using System.Windows.Input;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    public class TilesetViewModel : TileManager
    {
        EditorViewModel evm;
        int currentTilesetIndex;
        public BitmapImage CurrentTileset => LoadedTilesets[currentTilesetIndex];
        public int CurrentNumberOfRow => NumberOfRow[currentTilesetIndex];
        public int CurrentNumberOfColumn => NumberOfColumn[currentTilesetIndex];
        public string[] TilesetDroplistOptions
        {
            get
            {
                return tilesetNames;
            }
        }

        private bool[,] SelectedTiles;
        public SimpleCommand TilesetComboboxChangedCommand { get; set; }

        public TilesetViewModel(string[] tilesets, EditorViewModel evm)
        {
            tilesetNames = new string[0];
            currentTilesetIndex = 0;
            this.evm = evm;
            LoadedTilesets = new BitmapImage[0];
            foreach (string s in tilesets)
                LoadTileset(s);
            PopulateGridFromTileset(0);
            TilesetComboboxChangedCommand = new SimpleCommand(TilesetComboboxChanged);
            SelectedTiles = new bool[CurrentNumberOfColumn, CurrentNumberOfRow];
        }

        private void PopulateGridFromTileset(int tilesetIndex)
        {
            TilesCollection = new ObservableCollection<Tile>();
            for (short i = 0; i < NumberOfRow[tilesetIndex]; i++)
            {
                for (short j = 0; j < NumberOfColumn[tilesetIndex]; j++)
                {
                    TilesCollection.Add(new AdvancedTile(TilesetNames[tilesetIndex], new Coord(j, i), j, i, this));
                }
            }

            SelectedTiles = new bool[CurrentNumberOfColumn, CurrentNumberOfRow];
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

        public override void MouseEnteringTile(int x, int y, Coord na)
        {
            if (KeyDownOnTile.x != -1)
            {
                foreach (Tile t in TilesCollection)
                {
                    if (t.TilesetIndexX >= Math.Min(KeyDownOnTile.x, x)
                        && t.TilesetIndexX <= Math.Max(KeyDownOnTile.x, x)
                        && t.TilesetIndexY >= Math.Min(KeyDownOnTile.y, y)
                        && t.TilesetIndexY <= Math.Max(KeyDownOnTile.y, y))
                    {
                        t.CurrentAlpha = 0.7f;
                    }
                    else
                    {
                        t.CurrentAlpha = SelectedTiles[t.TilesetIndexX, t.TilesetIndexY] ? 0.8f : 1;
                    }
                }
            }
        }


        public override void MouseLeavingTile(int x, int y, Coord pos)
        {
            foreach (Tile t in TilesCollection)
                if (t.TilesetIndexX == x && t.TilesetIndexY == y)
                    t.CurrentAlpha = SelectedTiles[t.TilesetIndexX, t.TilesetIndexY] ? 0.8f : 1;
        }

        public override void MouseReleaseOnTile(int x, int y, Coord na)
        {
            int minX = Math.Min(KeyDownOnTile.x, x);
            int maxX = Math.Max(KeyDownOnTile.x, x);
            int minY = Math.Min(KeyDownOnTile.y, y);
            int maxY = Math.Max(KeyDownOnTile.y, y);
            TileIdentity[,] newSelection = new TileIdentity[1 + maxX - minX, 1 + maxY - minY];

            foreach (Tile t in TilesCollection)
            {
                if (t.TilesetIndexX >= minX
                        && t.TilesetIndexX <= maxX
                        && t.TilesetIndexY >= minY
                        && t.TilesetIndexY <= maxY)
                {
                    SelectedTiles[t.TilesetIndexX, t.TilesetIndexY] = true;
                    newSelection[t.TilesetIndexX - minX, t.TilesetIndexY - minY] = new TileIdentity(TilesetNames[currentTilesetIndex], t.TilesetIndexX, t.TilesetIndexY);
                }
                else
                {
                    SelectedTiles[t.TilesetIndexX, t.TilesetIndexY] = false;
                }

                t.CurrentAlpha = SelectedTiles[t.TilesetIndexX, t.TilesetIndexY] ? 0.8f : 1;
            }
            evm.SelectedTiles = newSelection;
            base.MouseReleaseOnTile(x, y, na);
            evm.OnTilesetSelection();
        }


        public void TilesetComboboxChanged(object param)
        {
            string text = ((ComboBox)(((SelectionChangedEventArgs)param).Source)).SelectedItem as String;

            for (int i = 0; i < TilesetNames.Length; i++)
            {
                if (TilesetNames[i] == text)
                    currentTilesetIndex = i;
            }
            PopulateGridFromTileset(text);
        }

        public void UpdateTileset(string[] newTilesetsNames)
        {
            UnloadTilesets();
            foreach (string name in newTilesetsNames)
                LoadTileset(name);
            evm.OnLoadedTilesetChange();
            currentTilesetIndex = 0;
            PopulateGridFromTileset(0);
        }

        public AnimatedTile[] GetAnimatedTiles()
        {
            return new AnimatedTile[0];
        }

        public void UnselectTiles()
        {
            foreach (Tile t in TilesCollection)
                t.CurrentAlpha = 1;
        }
    }
}