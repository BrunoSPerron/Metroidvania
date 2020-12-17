using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Win32;
using System.Windows;
using System.IO;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Models;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    public enum Tool { DRAW = 0, FILL = 1, RECT = 2 }
    public enum Layer { DEFAULT = 0, BACK = 1, FRONT = 2 }
    public class MainViewModel : INotifyPropertyChanged
    {
        public Tool SelectedTool { get; set; }
        public Layer SelectedLayer { get; set; }
        public bool FileModified { get; set; }

        public string CurrentFilePath { get; set; }
        public bool CanSave
        {
            get
            {
                return ((MenuViewModel)menuControl).CanSave;
            }
            set
            {
                ((MenuViewModel)menuControl).CanSave = value;
            }
        }
        object currentControl;
        public object CurrentControl
        {
            get => currentControl;
            set
            {
                currentControl = value;
                OnPropertyChanged("currentControl");
                if (CurrentControl.GetType() == typeof(EditorViewModel))
                    ((MenuViewModel)menuControl).ALevelIsLoaded = true;
                else
                    ((MenuViewModel)menuControl).ALevelIsLoaded = false;
            }
        }
        public object menuControl { get; set; }
        public object toolbarControl { get; set; }

        public MainViewModel()
        {
            FileModified = false;
            CurrentFilePath = null;
            menuControl = new MenuViewModel(this);
            toolbarControl = new ToolbarViewModel(this);
            CurrentControl = new OpeningViewModel();
        }

        public void Save(bool overriteCurrent)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string path = null;
            if (!overriteCurrent || CurrentFilePath == null)
            {
                saveFileDialog.Filter = "Level file (*.lvl)|*.lvl";
                if (saveFileDialog.ShowDialog() == true)
                    path = saveFileDialog.FileName;
            }
            else
            {
                path = CurrentFilePath;
            }
            if (path != null)
            {
                saveFileDialog.FileName = path;
                string levelAsString = "";
                string[] tsNames = ((TilesetViewModel)((EditorViewModel)CurrentControl).TilesetControl).TilesetNames;

                Dictionary<string, int> tilesetIndexes = new Dictionary<string, int>();

                for (int i = 0; i < tsNames.Length; i++)
                {
                    string s = tsNames[i];
                    levelAsString += ">" + i + "-" + s + "\n";
                    tilesetIndexes.Add(tsNames[i], i);
                }

                Tile[,] tiles = ((LevelViewModel)((EditorViewModel)CurrentControl).LevelControl).tiles;
                Tile[,] underTiles = ((LevelViewModel)((EditorViewModel)CurrentControl).LevelControl).underTiles;
                Tile[,] overTiles = ((LevelViewModel)((EditorViewModel)CurrentControl).LevelControl).overTiles;

                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    string addToLevelAsString = "";
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        addToLevelAsString += "|";

                        if (underTiles[i, j].TilesetIndexX != 0 || underTiles[i, j].TilesetIndexY != 0 || overTiles[i, j].TilesetIndexX != 0 || overTiles[i, j].TilesetIndexY != 0)
                        {
                            if (underTiles[i, j] is AnimatedTile)
                            {
                                addToLevelAsString += tilesetIndexes[underTiles[i, j].TilesetName] + ":";
                                for (int k = 0; k < ((AnimatedTile)underTiles[i, j]).ShowTimes.Length; k++)
                                    addToLevelAsString += ((AnimatedTile)underTiles[i, j]).IndexesX[k] + ":" + ((AnimatedTile)underTiles[i, j]).IndexesY[k] + ">" + ((AnimatedTile)underTiles[i, j]).ShowTimes[k] + ">";
                                addToLevelAsString = addToLevelAsString.Remove(addToLevelAsString.Length - 1);
                            }
                            else
                                addToLevelAsString += tilesetIndexes[underTiles[i, j].TilesetName] + ":" + underTiles[i, j].TilesetIndexX + ":" + underTiles[i, j].TilesetIndexY + "+";
                        }
                        if (tiles[i, j] is AnimatedTile)
                        {
                            addToLevelAsString += tilesetIndexes[tiles[i, j].TilesetName] + ":";
                            for (int k = 0; k < ((AnimatedTile)tiles[i, j]).ShowTimes.Length; k++)
                                addToLevelAsString += ((AnimatedTile)tiles[i, j]).IndexesX[k] + ":" + ((AnimatedTile)tiles[i, j]).IndexesY[k] + ">" + ((AnimatedTile)tiles[i, j]).ShowTimes[k] + ">";
                            addToLevelAsString = addToLevelAsString.Remove(addToLevelAsString.Length - 1);
                        }
                        else
                            addToLevelAsString += tilesetIndexes[tiles[i, j].TilesetName] + ":" + tiles[i, j].TilesetIndexX + ":" + tiles[i, j].TilesetIndexY;

                        if (overTiles[i, j].TilesetIndexX != 0 || overTiles[i, j].TilesetIndexY != 0)
                        {
                            if (overTiles[i, j] is AnimatedTile)
                            {
                                addToLevelAsString += "+" + tilesetIndexes[overTiles[i, j].TilesetName] + ":";
                                for (int k = 0; k < ((AnimatedTile)overTiles[i, j]).ShowTimes.Length; k++)
                                    addToLevelAsString += ((AnimatedTile)overTiles[i, j]).IndexesX[k] + ":" + ((AnimatedTile)overTiles[i, j]).IndexesY[k] + ">" + ((AnimatedTile)overTiles[i, j]).ShowTimes[k] + ">";
                                addToLevelAsString = addToLevelAsString.Remove(addToLevelAsString.Length - 1);
                            }
                            else
                                addToLevelAsString += "+" + tilesetIndexes[overTiles[i, j].TilesetName] + ":" + overTiles[i, j].TilesetIndexX + ":" + overTiles[i, j].TilesetIndexY;
                        }
                    }
                    levelAsString += addToLevelAsString + "\n";
                }
                File.WriteAllText(saveFileDialog.FileName, levelAsString);
                CurrentFilePath = saveFileDialog.FileName;
            }
        }

        public void Load()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Level file(*.lvl)| *.lvl";
            int startIndex = -1;
            if (ofd.ShowDialog() == true)
            {
                string[] content = File.ReadAllLines(ofd.FileName);

                //Get Dimension
                int width = 0;
                int height = 0;
                for (int i = 0; i < content.Length; i++)
                {
                    string line = (string)content[i];
                    int lineWidth = line.Split('|').Length - 1;
                    if (lineWidth > width)
                        width = lineWidth;
                    if (line.StartsWith("|"))
                    {
                        height++;
                        if (startIndex == -1)
                            startIndex = i;
                    }
                }

                TileIdentity[,] backTI = new TileIdentity[width, height];
                TileIdentity[,] defaultTI = new TileIdentity[width, height];
                TileIdentity[,] frontTI = new TileIdentity[width, height];
                Dictionary<int, string> tileTilesetsNames = new Dictionary<int, string>();

                for (int i = 0; i < content.Length; i++)
                {
                    string line = content[i];
                    if (line[0] == '>')
                    {
                        string[] splittedLine = line.Split('-');
                        int key = Int32.Parse(splittedLine[0].Split('>')[1]);
                        string value = splittedLine[1];
                        tileTilesetsNames.Add(key, value);
                    }
                    else if (line[0] == '|')
                    {
                        string[] splittedLine = line.Substring(1).Split('|');
                        for (int j = 0; j < splittedLine.Length; j++)
                        {
                            TileIdentity nextIdentity;

                            string[] splittedTilesInfo = splittedLine[j].Split('+');
                            string[] animationSplit = splittedTilesInfo[0].Split('>');

                            if (animationSplit.Length > 1)
                            {
                                nextIdentity = GetAnimatedTileIdentity(animationSplit, tileTilesetsNames);
                            }
                            else
                            {
                                string[] splittedTileInfo = splittedTilesInfo[0].Split(':');
                                string tilesetName = tileTilesetsNames[Int32.Parse(splittedTileInfo[0])];
                                nextIdentity = new TileIdentity(tilesetName, Int32.Parse(splittedTileInfo[1]), Int32.Parse(splittedTileInfo[2]));
                            }

                            if (splittedTilesInfo.Length > 1)
                            {
                                backTI[j, i - startIndex] = nextIdentity;
                                animationSplit = splittedTilesInfo[1].Split('>');

                                if (animationSplit.Length > 1)
                                {
                                    nextIdentity = GetAnimatedTileIdentity(animationSplit, tileTilesetsNames);
                                }
                                else
                                {
                                    string[] splittedTileInfo = splittedTilesInfo[1].Split(':');
                                    string tilesetName = tileTilesetsNames[Int32.Parse(splittedTileInfo[0])];
                                    nextIdentity = new TileIdentity(tilesetName, Int32.Parse(splittedTileInfo[1]), Int32.Parse(splittedTileInfo[2]));
                                }
                            }
                            else
                            {
                                backTI[j, i - startIndex] = new TileIdentity(tileTilesetsNames[0], 0, 0);
                            }

                            defaultTI[j, i - startIndex] = nextIdentity;

                            if (splittedTilesInfo.Length == 3)
                            {
                                animationSplit = splittedTilesInfo[2].Split('>');

                                if (animationSplit.Length > 1)
                                {
                                    nextIdentity = GetAnimatedTileIdentity(animationSplit, tileTilesetsNames);
                                }
                                else
                                {
                                    string[] splittedTileInfo = splittedTilesInfo[0].Split(':');
                                    string tilesetName = tileTilesetsNames[Int32.Parse(splittedTileInfo[0])];
                                    nextIdentity = new TileIdentity(tilesetName, Int32.Parse(splittedTileInfo[1]), Int32.Parse(splittedTileInfo[2]));
                                }
                                frontTI[j, i - startIndex] = nextIdentity;
                            }
                            else
                            {
                                frontTI[j, i - startIndex] = new TileIdentity(tileTilesetsNames[0], 0, 0);
                            }
                        }
                    }
                }

                string[] tileTilesesNamesArray = new string[tileTilesetsNames.Count];
                tileTilesetsNames.Values.CopyTo(tileTilesesNamesArray, 0);

                CurrentControl = new EditorViewModel(tileTilesesNamesArray, backTI, defaultTI, frontTI, this);
                CurrentFilePath = ofd.FileName;
            }
        }

        private TileIdentity GetAnimatedTileIdentity(string[] animationSplit, Dictionary<int, string> tileTilesetsNames)
        {
            string tilesetName = "";
            int[] x = new int[animationSplit.Length / 2];
            int[] y = new int[animationSplit.Length / 2];
            int[] times = new int[animationSplit.Length / 2];
            for (int k = 0; k < animationSplit.Length / 2; k++)
            {
                string[] splittedTileInfo = animationSplit[k * 2].Split(':');
                if (splittedTileInfo.Length == 3)
                {
                    tilesetName = tileTilesetsNames[Int32.Parse(splittedTileInfo[0])];
                    x[k] = Int32.Parse(splittedTileInfo[1]);
                    y[k] = Int32.Parse(splittedTileInfo[2]);
                }
                else
                {
                    x[k] = Int32.Parse(splittedTileInfo[0]);
                    y[k] = Int32.Parse(splittedTileInfo[1]);
                }
                times[k] = Int32.Parse(animationSplit[k * 2 + 1]);
            }
            return new TileIdentity(tilesetName, x, y, times);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
