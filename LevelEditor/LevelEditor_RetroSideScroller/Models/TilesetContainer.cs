using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.Models
{
    class TilesetContainer
    {
        public static TilesetContainer TsContainer = new TilesetContainer();

        private static BitmapImage[] tilesets = new BitmapImage[0];
        private static string[] tilesetNames = new string[0];
        private int[] numberOfRow = new int[0];
        private int[] numberOfColumn = new int[0];
        public BitmapImage[] Tilesets
        {
            get { return tilesets; }
            set { tilesets = value; }
        }
        public string[] TilesetNames
        {
            get { return tilesetNames; }
            set { tilesetNames = value; }
        }
        public int[] NumberOfRow
        {
            get { return numberOfRow; }
            set { numberOfRow = value; }
        }
        public int[] NumberOfColumn
        {
            get { return numberOfColumn; }
            set { numberOfColumn = value; }
        }
        public int GetTilesetIndexFromName(string name)
        {
            for (int i = 0; i < TilesetNames.Length; i++)
                if (TilesetNames[i] == name)
                    return i;
            return 0;
        }
        public TilesetContainer()
        {

        }
    }
}
