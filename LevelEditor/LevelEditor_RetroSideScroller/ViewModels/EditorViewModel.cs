using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Models;
using System.Collections.ObjectModel;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    public class EditorViewModel : INotifyPropertyChanged
    {
        public Tool SelectedTool => mvm.SelectedTool;
        public Layer SelectedLayer => mvm.SelectedLayer;
        private MainViewModel mvm;
        private TilesetViewModel tilesetControl;
        private AnimatedTilesetViewModel animatedTilesetControl;

        public TilesetViewModel TilesetControl
        {
            get => tilesetControl;
            set
            {
                tilesetControl = value;
                OnPropertyChanged("tilesetControl");
            }
        }

        public AnimatedTilesetViewModel AnimatedTilesetControl
        {
            get => animatedTilesetControl;
            set
            {
                animatedTilesetControl = value;
                OnPropertyChanged("animatedTilesetControl");
            }
        }

        private object levelControl;
        public object LevelControl
        {
            get => levelControl;
            set
            {
                levelControl = value;
                OnPropertyChanged("levelControl");
            }
        }

        private TileIdentity[,] selectedTiles;

        public TileIdentity[,] SelectedTiles
        {
            get
            {
                return selectedTiles;
            }
            set
            {
                selectedTiles = value;
            }
        }

        public EditorViewModel(string[] tilesets, int levelwidth, int levelHeight, MainViewModel mvm)
        {
            this.mvm = mvm;
            mvm.CanSave = true;
            tilesetControl = new TilesetViewModel(tilesets, this);
            AnimatedTilesetControl = new AnimatedTilesetViewModel(this);
            levelControl = new LevelViewModel(this, levelwidth, levelHeight);
            ((LevelViewModel)levelControl).PopulateEmptyGrid();

            SelectedTiles = new TileIdentity[1, 1];
            SelectedTiles[0, 0] = new TileIdentity(tilesets[0], 0, 0);
        }

        public EditorViewModel(string[] tileTilesetsNames, TileIdentity[,] underTI, TileIdentity[,] defaultTI, TileIdentity[,] overTI, MainViewModel mvm)
        {
            this.mvm = mvm;
            mvm.CanSave = true;
            tilesetControl = new TilesetViewModel(tileTilesetsNames, this);
            AnimatedTilesetControl = new AnimatedTilesetViewModel(this);
            levelControl = new LevelViewModel(this, defaultTI.GetLength(0), defaultTI.GetLength(1));
            ((LevelViewModel)levelControl).PopulateGridFromTileIdentities(underTI, defaultTI, overTI);

            SelectedTiles = new TileIdentity[1, 1];
            SelectedTiles[0, 0] = new TileIdentity(tileTilesetsNames[0], 0, 0);
        }

        public void OnLoadedTilesetChange()
        {
            ((LevelViewModel)levelControl).OnLoadedTilesetChange();
        }

        public AnimatedTile[] GetAnimatedTiles => tilesetControl.GetAnimatedTiles();
        public void UpdateAnimatedTileset(ObservableCollection<AnimatedTileBuilder> atc)
        {
            animatedTilesetControl.UpdateAnimatedTileset(atc);
        }

        public void AddAnimatedTile(AnimatedTileBuilder atb)
        {
            animatedTilesetControl.AddAnimatedTile(atb);
        }

        public void OnAnimatedTilesetSelection()
        {
            TilesetControl.UnselectTiles();
        }

        public void OnTilesetSelection()
        {
            if (AnimatedTilesetControl != null)
                AnimatedTilesetControl.UnselectTiles();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public struct TileIdentity
{
    public string tileset;
    public int[] x;
    public int[] y;
    public int[] showTimeMS;
    public TileIdentity(string tileset, int x, int y)
    {
        this.x = new int[1];
        this.y = new int[1];
        showTimeMS = new int[0];
        this.tileset = tileset;
        this.x[0] = x;
        this.y[0] = y;
    }
    public TileIdentity(string tileset, int[] x, int[] y, int[] showTimeMS)
    {
        this.tileset = tileset;
        this.x = x;
        this.y = y;
        this.showTimeMS = showTimeMS;
    }
}