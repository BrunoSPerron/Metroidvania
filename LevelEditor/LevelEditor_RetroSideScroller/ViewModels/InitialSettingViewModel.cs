using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Commands;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    class InitialSettingViewModel : INotifyPropertyChanged
    {
        MainViewModel mvm;
        public ICommand DiskToTilesetCommand { get; private set; }
        public ICommand TilesetToDiskCommand { get; private set; }
        public ICommand GenerateLevelCommand { get; private set; }

        public bool CanSave
        {
            get { return mvm.CanSave; }
            set { mvm.CanSave = value; }
        }

        public int levelWidth { get; set; }
        public int levelHeight { get; set; }

        private ObservableCollection<ListViewItem> tilesetOnDisk;
        private ObservableCollection<ListViewItem> levelTileset;

        public ObservableCollection<ListViewItem> TilesetOnDisk
        {
            get => tilesetOnDisk;
            set
            {
                tilesetOnDisk = value;
                OnPropertyChanged("tilesetOnDisk");
            }
        }
        public ObservableCollection<ListViewItem> LevelTileset
        {
            get => levelTileset;
            set
            {
                levelTileset = value;
                OnPropertyChanged("levelTileset");
            }
        }

        public InitialSettingViewModel(MainViewModel mvm)
        {
            levelWidth = 4;
            levelHeight = 6;
            this.mvm = mvm;
            CanSave = false;
            TilesetOnDisk = new ObservableCollection<ListViewItem>();
            LevelTileset = new ObservableCollection<ListViewItem>();

            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            DirectoryInfo d = new DirectoryInfo(path + @"\tilesets\");
            foreach (var file in d.GetFiles("*"))
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Content = file.Name.Substring(0, file.Name.Length - 4);
                TilesetOnDisk.Add(lvi);
            }

            DiskToTilesetCommand = new BaseCommand(DiskToTileset, ojb => TilesetOnDisk.Count > 0);
            TilesetToDiskCommand = new BaseCommand(TilesetToDisk, ojb => LevelTileset.Count > 0);
            GenerateLevelCommand = new BaseCommand(GenerateLevel, obj => LevelTileset.Count > 0);
        }

        private void GenerateLevel(object obj)
        {
            string[] tilesetsToUse = new string[LevelTileset.Count];
            for (int i = 0; i < LevelTileset.Count; i++)
                tilesetsToUse[i] = LevelTileset[i].Content.ToString();

            mvm.FileModified = false;
            mvm.CurrentControl = new EditorViewModel(tilesetsToUse, levelWidth, levelHeight, mvm);
            CanSave = true;
        }

        public void DiskToTileset(object param)
        {
            for (int i = 0; i < TilesetOnDisk.Count; i++)
            {
                ListViewItem lvi = TilesetOnDisk[i];
                if (lvi.IsSelected)
                {
                    TilesetOnDisk.Remove(lvi);
                    LevelTileset.Add(lvi);
                    i--;
                }
            }
        }

        public void TilesetToDisk(object param)
        {
            for (int i = 0; i < LevelTileset.Count; i++)
            {
                ListViewItem lvi = LevelTileset[i];
                if (lvi.IsSelected)
                {
                    LevelTileset.Remove(lvi);
                    TilesetOnDisk.Add(lvi);
                    i--;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
