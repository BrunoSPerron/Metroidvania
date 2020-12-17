using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Models;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    public class AnimatedTilesetViewModel : TileManager
    {
        private EditorViewModel evm;
        private int selectedTile;
        private ObservableCollection<AnimatedTile> animatedTilesCollection;
        public ObservableCollection<AnimatedTile> AnimationTilesCollection
        {
            get => animatedTilesCollection;
            set
            {
                animatedTilesCollection = value;
                OnPropertyChanged("AnimationTilesCollection");
            }
        }
        public AnimatedTilesetViewModel(EditorViewModel evm)
        {
            this.evm = evm;
            selectedTile = -1;
            animatedTilesCollection = new ObservableCollection<AnimatedTile>();
        }

        internal void UpdateAnimatedTileset(ObservableCollection<AnimatedTileBuilder> atc)
        {
            AnimationTilesCollection = new ObservableCollection<AnimatedTile>();
            for (int i = 0; i < atc.Count; i++)
            {
                AnimatedTileBuilder atb = atc[i];
                AnimationTilesCollection.Add(new AnimatedTile(atb.TilesetName, new Coord(i,0), atb.IndexesX, atb.IndexesY, atb.ShowTimes, this));
            }

            OnPropertyChanged("AnimationTilesCollection");
        }

        public override void MouseLeavingTile(int x, int y, Coord pos)
        {
            AnimationTilesCollection[pos.x].CurrentAlpha = selectedTile == pos.x ? .8f: 1;
        }

        public override void MouseReleaseOnTile(int x, int y, Coord pos)
        {
            selectedTile = pos.x;
            evm.SelectedTiles = new TileIdentity[1, 1];
            evm.SelectedTiles[0, 0] = new TileIdentity(AnimationTilesCollection[pos.x].TilesetName, AnimationTilesCollection[pos.x].IndexesX, AnimationTilesCollection[pos.x].IndexesY, AnimationTilesCollection[pos.x].ShowTimes);
            evm.OnAnimatedTilesetSelection();
            for (int i = 0; i < animatedTilesCollection.Count; i++)
            {
                AnimatedTile at = animatedTilesCollection[i];
                at.CurrentAlpha = i == pos.x ? 0.8f : 1;
            }
        }

        internal void UnselectTiles()
        {
            foreach (AnimatedTile at in animatedTilesCollection)
                at.CurrentAlpha = 1;
        }

        internal void AddAnimatedTile(AnimatedTileBuilder atb)
        {
            AnimationTilesCollection.Add(new AnimatedTile(atb.TilesetName, new Coord(AnimationTilesCollection.Count, 0), atb.IndexesX, atb.IndexesY, atb.ShowTimes, this));
        }
    }
}
