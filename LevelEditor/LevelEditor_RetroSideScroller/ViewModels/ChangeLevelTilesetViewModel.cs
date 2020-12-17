using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Input;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Commands;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    class ChangeLevelTilesetViewModel: INotifyPropertyChanged
    {
        TilesetViewModel tvm;
        private ObservableCollection<ListViewItem> tilesetOnDisk;
        private ObservableCollection<ListViewItem> levelTileset;
        public ICommand DiskToTilesetCommand { get; private set; }
        public ICommand TilesetToDiskCommand { get; private set; }
        public ICommand UpdateTilesetsCommand { get; private set; }
        public ObservableCollection<ListViewItem> TilesetOnDisk
        {
            get
            {
                return tilesetOnDisk;
            }
            set
            {
                tilesetOnDisk = value;
                OnPropertyChanged("TilesetOnDisk");
            }
        }
        public ObservableCollection<ListViewItem> LevelTileset
        {
            get
            {
                return levelTileset;
            }
            set
            {
                levelTileset = value;
                OnPropertyChanged("LevelTileset");
            }
        }
        public ChangeLevelTilesetViewModel(TilesetViewModel tvm)
        {
            this.tvm = tvm;

            TilesetOnDisk = new ObservableCollection<ListViewItem>();
            LevelTileset = new ObservableCollection<ListViewItem>();

            for (int i = 0; i < tvm.TilesetNames.Length; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Content = tvm.TilesetNames[i];
                LevelTileset.Add(lvi);
            }

            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            DirectoryInfo d = new DirectoryInfo(path + @"\tilesets\");
            foreach (var file in d.GetFiles("*"))
            {
                string fileName = file.Name.Substring(0, file.Name.Length - 4);
                ListViewItem lvi = new ListViewItem();
                lvi.Content = fileName;

                bool addFile = true;
                for (int i = 0; i < LevelTileset.Count; i++)
                    if ((string)LevelTileset[i].Content == fileName)
                        addFile = false;

                if (addFile)
                    TilesetOnDisk.Add(lvi);
            }


            DiskToTilesetCommand = new BaseCommand(DiskToTileset, ojb => TilesetOnDisk.Count > 0);
            TilesetToDiskCommand = new BaseCommand(TilesetToDisk, ojb => LevelTileset.Count > 0);
            UpdateTilesetsCommand = new BaseCommand(UpdateTilesets, obj => LevelTileset.Count > 0);
        }

        public void UpdateTilesets(object param)
        {
            string[] names = new string[levelTileset.Count];
            for (int i = 0; i < levelTileset.Count; i++)
                names[i] = (string)levelTileset[i].Content;
            tvm.UpdateTileset(names);
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
