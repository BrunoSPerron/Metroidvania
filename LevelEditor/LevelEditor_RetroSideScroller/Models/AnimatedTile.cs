using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.Models
{
    public class AnimatedTile : AdvancedTile
    {
        private DispatcherTimer TimedChanges;
        public int[] IndexesX;
        public int[] IndexesY;
        public int[] ShowTimes;
        private int currentFrame;

        public AnimatedTile(string tilesetName, Coord pos, int[] _indexX, int[] _indexY, int[] _showTimes, object parent) : base(tilesetName, pos, _indexX[0], _indexY[0], parent)
        {
            IndexesX = _indexX;
            IndexesY = _indexY;
            ShowTimes = _showTimes;
            currentFrame = 0;

            TimedChanges = new DispatcherTimer();
            TimedChanges.Tick += new EventHandler(TimedChangesTick);
            TimedChanges.Interval = new TimeSpan(0, 0, 0, 0, ShowTimes[currentFrame]);
            TimedChanges.Start();
        }

        public void TimedChangesTick(object sender, EventArgs e)
        {
            if (ShowTimes.Length > currentFrame + 1)
                currentFrame++;
            else
                currentFrame = 0;

            TilesetIndexX = IndexesX[currentFrame];
            TilesetIndexY = IndexesY[currentFrame];
            OnPropertyChanged("CropRect");

            TimedChanges.Interval = new TimeSpan(0, 0, 0, 0, ShowTimes[currentFrame]);
        }
    }
}
