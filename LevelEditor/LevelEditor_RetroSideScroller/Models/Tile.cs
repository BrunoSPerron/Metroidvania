using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using ProjetJeuxVideo_LevelEditor_Metroidvania.HelperClasses;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.Models
{
    public class Tile : INotifyPropertyChanged
    {
        private string tilesetname;
        public Int32Rect CropRect => new Int32Rect(TilesetIndexX * 16, TilesetIndexY * 16, 16, 16);
        public BitmapImage Tileset => TilesetContainer.TsContainer.Tilesets[TilesetContainer.TsContainer.GetTilesetIndexFromName(TilesetName)];
        public int SizeInPixelAfterZoom => Settings.ZoomLevel * 16;
        public string TilesetName
        {
            get => tilesetname;
            set
            {
                tilesetname = value;
                OnPropertyChanged("Tileset");
            }
            
        }
        public int TilesetIndexX { get; set; }
        public int TilesetIndexY { get; set; }
        
        private float currentAlpha;
        public float CurrentAlpha
        {
            get => currentAlpha;
            set
            {
                currentAlpha = value;
                OnPropertyChanged("CurrentAlpha");
            }
        }

        public Tile(string tilesetName, int _indexX, int _indexY)
        {
            TilesetName = tilesetName;
            TilesetIndexX = _indexX;
            TilesetIndexY = _indexY;
            CurrentAlpha = 1;
        }

        public virtual void ApplyTileIdentity(TileIdentity ti)
        {
            TilesetIndexX = ti.x[0];
            TilesetIndexY = ti.y[0];
            TilesetName = ti.tileset;
            OnPropertyChanged("CropRect");
        }
        
        public void OnLoadedTilesetChange()
        {
            if (!TilesetContainer.TsContainer.TilesetNames.Contains(TilesetName))
            {
                TilesetIndexX = 0;
                TilesetIndexY = 0;
                TilesetName = TilesetContainer.TsContainer.TilesetNames[0];
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        internal TileIdentity GetIdentity()
        {
            return new TileIdentity(tilesetname, TilesetIndexX, TilesetIndexY);
        }
    }
}
