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
    public class AdvancedTile : Tile
    {

        private TileManager TM;

        private Coord pos;
        public SimpleCommand MouseEnterCommand { get; set; }
        public SimpleCommand MouseLeaveCommand { get; set; }
        public SimpleCommand MouseDownCommand { get; set; }
        public SimpleCommand MouseUpCommand { get; set; }

        public AdvancedTile(string tilesetName, Coord pos, int _indexX, int _indexY, object parent) : base(tilesetName, _indexX, _indexY)
        {
            TM = (TileManager)parent;
            this.pos = pos;
            MouseEnterCommand = new SimpleCommand(OnMouseEnter);
            MouseLeaveCommand = new SimpleCommand(OnMouseLeave);
            MouseDownCommand = new SimpleCommand(OnMouseDown);
            MouseUpCommand = new SimpleCommand(OnMouseUp);
        }

        public void OnMouseEnter(object param)
        {
            CurrentAlpha = 0.7f;
            TM.MouseEnteringTile(TilesetIndexX, TilesetIndexY, pos);
        }

        public void OnMouseLeave(object param)
        {
            TM.MouseLeavingTile(TilesetIndexX, TilesetIndexY, pos);
        }

        public void OnMouseDown(object param)
        {
            TM.MouseDownOnTile(TilesetIndexX, TilesetIndexY, pos);
        }

        public void OnMouseUp(object param)
        {
            TM.MouseReleaseOnTile(TilesetIndexX, TilesetIndexY, pos);
        }
    }
}
