using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ProjetJeuxVideo_LevelEditor_Metroidvania.HelperClasses;
using ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Commands;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.Models
{
    public class AdvancedTileForBuilder : Tile
    {

        private AnimationMakerViewModel AMVM;

        public int index;
        private int showTime;
        public int ShowTime
        {
            get => showTime;
            set
            {
                showTime = value;
                AMVM.UpdateShowTime(value, index);
            }
        }
        public SimpleCommand MouseEnterCommand { get; set; }
        public SimpleCommand MouseLeaveCommand { get; set; }
        public SimpleCommand MouseDownCommand { get; set; }
        public SimpleCommand MouseUpCommand { get; set; }

        public AdvancedTileForBuilder(string tilesetName, int index, int _indexX, int _indexY, AnimationMakerViewModel parent) : base(tilesetName, _indexX, _indexY)
        {
            AMVM = parent;
            this.index = index;
            MouseEnterCommand = new SimpleCommand(OnMouseEnter);
            MouseLeaveCommand = new SimpleCommand(OnMouseLeave);
            MouseDownCommand = new SimpleCommand(OnMouseDown);
            MouseUpCommand = new SimpleCommand(OnMouseUp);
            showTime = 250;
        }

        public void OnMouseEnter(object param)
        {
            CurrentAlpha = 0.7f;
        }

        public void OnMouseLeave(object param)
        {
            CurrentAlpha = 1;
        }

        public void OnMouseDown(object param)
        {

        }

        public void OnMouseUp(object param)
        {
            AMVM.MouseReleaseOnFrame(index);
        }
    }
}
