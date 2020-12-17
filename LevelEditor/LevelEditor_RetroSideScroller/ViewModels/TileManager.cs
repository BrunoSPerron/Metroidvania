using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Models;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    public class TileManager : INotifyPropertyChanged
    {
        TilesetContainer tilesetsContainer = new TilesetContainer();

        protected Coord KeyDownOnTile { get; set; }
        protected Coord RightKeyDownOnTile { get; set; }
        protected string[] tilesetNames
        {
            get => tilesetsContainer.TilesetNames;
            set
            {
                tilesetsContainer.TilesetNames = value;
                OnPropertyChanged("TilesetDroplistOptions");
            }
        }
        
        protected BitmapImage[] loadedTilesets
        {
            get => tilesetsContainer.Tilesets;
            set
            {
                tilesetsContainer.Tilesets = value;
            }
        }

        private ObservableCollection<Tile> tilesCollection;
        public ObservableCollection<Tile> TilesCollection
        {
            get => tilesCollection;
            set
            {
                tilesCollection = value;
            }
        }

        public string[] TilesetNames
        {
            get
            {
                return tilesetNames;
            }
            set
            {
                tilesetNames = value;
                OnPropertyChanged("TilesetNames");
            }
        }

        public int[] NumberOfRow
        {
            get => TilesetContainer.TsContainer.NumberOfRow;
            set
            {
                TilesetContainer.TsContainer.NumberOfRow = value;
                OnPropertyChanged("numberOfRow");
            }
        }
        public int[] NumberOfColumn
        {
            get => TilesetContainer.TsContainer.NumberOfColumn;
            set
            {
                TilesetContainer.TsContainer.NumberOfColumn = value;
                OnPropertyChanged("numberOfColumn");
            }
        }

        public BitmapImage[] LoadedTilesets
        {
            get => loadedTilesets;
            set
            {
                loadedTilesets = value;
                OnPropertyChanged("loadedTilesets");
            }
        }

        public TileManager()
        {
            tilesCollection = new ObservableCollection<Tile>();
            KeyDownOnTile = new Coord(-1, -1);
            RightKeyDownOnTile = new Coord(-1, -1);
        }
        protected void LoadTileset(string s)
        {
            BitmapImage[] newLoadedTilesets = new BitmapImage[LoadedTilesets.Length + 1];
            int[] newNumberOfRow = new int[LoadedTilesets.Length + 1];
            int[] newNumberOfColumn = new int[LoadedTilesets.Length + 1];
            string[] newTilesetNames = new string[LoadedTilesets.Length + 1];
            for (int i = 0; i < LoadedTilesets.Length; i++)
            {
                newLoadedTilesets[i] = LoadedTilesets[i];
                newNumberOfRow[i] = NumberOfRow[i];
                newNumberOfColumn[i] = NumberOfColumn[i];
                newTilesetNames[i] = TilesetNames[i];
            }
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            BitmapImage ts = new BitmapImage();
            ts.BeginInit();
            ts.UriSource = new Uri(path + @"\tilesets\" + s + ".png");
            ts.EndInit();

            newNumberOfRow[newNumberOfRow.Length - 1] = ts.PixelHeight / 16;
            newNumberOfColumn[newNumberOfColumn.Length - 1] = ts.PixelWidth / 16;
            newLoadedTilesets[newLoadedTilesets.Length - 1] = ts;
            newTilesetNames[newLoadedTilesets.Length - 1] = s;

            LoadedTilesets = newLoadedTilesets;
            NumberOfRow = newNumberOfRow;
            NumberOfColumn = newNumberOfColumn;
            tilesetNames = newTilesetNames;
        }

        protected void UnloadTilesets()
        {
            LoadedTilesets = new BitmapImage[0];
            NumberOfRow = new int[0];
            NumberOfColumn = new int[0];
            tilesetNames = new string[0];
        }

        public virtual void MouseDownOnTile(int x, int y, Coord pos)
        {
            KeyDownOnTile = new Coord(x, y);
        }

        public virtual void MouseEnteringTile(int x, int y, Coord pos)
        {

        }

        public virtual void MouseLeavingTile(int x, int y, Coord pos)
        {

        }

        public virtual void MouseReleaseOnTile(int x, int y, Coord pos)
        {
            KeyDownOnTile = new Coord(-1, -1);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public struct Coord
{
    public int x;
    public int y;
    public Coord(int x = -1, int y = -1)
    {
        this.x = x;
        this.y = y;
    }
}