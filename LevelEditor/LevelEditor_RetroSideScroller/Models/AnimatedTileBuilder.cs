using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections;
using ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.Models
{
    public class AnimatedTileBuilder : Tile
    {
        private DispatcherTimer TimedChanges;

        public AnimationMakerViewModel amvm;
        public int[] IndexesX { get; set; }
        public int[] IndexesY { get; set; }
        public int[] ShowTimes { get; set; }
        private int currentFrame;

        public AnimatedTileBuilder(string tilesetName, AnimationMakerViewModel amvm) : base(tilesetName, 1, 1)
        {
            this.amvm = amvm;
            IndexesX = new int[0];
            IndexesY = new int[0];
            ShowTimes = new int[0];
            currentFrame = 0;
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

        public void SetNbOfFrames(int newSize)
        {
            int[] newIndexesX = new int[newSize];
            int[] newIndexesY = new int[newSize];
            int[] newShowTimes = new int[newSize];
            int smallest = Math.Min(IndexesX.Length, newSize);
            for (int i = 0; i < newSize; i++)
            {
                if (i < IndexesX.Length)
                {
                    newIndexesX[i] = IndexesX[i];
                    newIndexesY[i] = IndexesY[i];
                    newShowTimes[i] = ShowTimes[i];
                }
                else
                {
                    newIndexesX[i] = 1;
                    newIndexesY[i] = 1;
                    newShowTimes[i] = 250;
                }
            }

            IndexesX = newIndexesX;
            IndexesY = newIndexesY;
            ShowTimes = newShowTimes;
        }

        public void AddFrame(int x, int y, int showTime = 250)
        {
            SetNbOfFrames(ShowTimes.Length + 1);
            IndexesX[IndexesX.Length - 1] = x;
            IndexesY[IndexesY.Length - 1] = y;
            ShowTimes[ShowTimes.Length - 1] = showTime;

            if (ShowTimes.Length > 0)
            {
                if (TimedChanges != null)
                    TimedChanges.Stop();
                currentFrame = 0;
                TimedChanges = new DispatcherTimer();
                TimedChanges.Tick += new EventHandler(TimedChangesTick);
                TimedChanges.Interval = new TimeSpan(0, 0, 0, 0, ShowTimes[currentFrame]);
                TimedChanges.Start();
            }
        }

        public void RemoveFrame(int index)
        {
            TimedChanges.Stop();
            int[] oldIndexesX = IndexesX;
            IndexesX = new int[IndexesX.Length - 1];
            int[] oldIndexesY = IndexesY;
            IndexesY = new int[IndexesY.Length - 1];
            int[] oldShowTimes = ShowTimes;
            ShowTimes = new int[ShowTimes.Length - 1];

            int i = 0;
            while (i < ShowTimes.Length)
            {
                if (i < index)
                {
                    IndexesX[i] = oldIndexesX[i];
                    IndexesY[i] = oldIndexesY[i];
                    ShowTimes[i] = oldShowTimes[i];
                }
                else if (i >= index)
                {
                    IndexesX[i] = oldIndexesX[i + 1];
                    IndexesY[i] = oldIndexesY[i + 1];
                    ShowTimes[i] = oldShowTimes[i + 1];
                }
                i++;
            }

            if (ShowTimes.Length > 0)
            {
                currentFrame = 0;
                TimedChanges = new DispatcherTimer();
                TimedChanges.Tick += new EventHandler(TimedChangesTick);
                TimedChanges.Interval = new TimeSpan(0, 0, 0, 0, ShowTimes[currentFrame]);
                TimedChanges.Start();
            }
        }
    }
}