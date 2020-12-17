using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Models;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    public class LevelViewModel : TileManager
    {
        EditorViewModel evm;
        protected Coord LastTileOver { get; set; }

        public ObservableCollection<Tile> LayerTilesCollection
        {
            get
            {
                if (evm.SelectedLayer == Layer.DEFAULT)
                    return TilesCollection;
                else if (evm.SelectedLayer == Layer.BACK)
                    return UnderTilesCollection;
                else
                    return OverTilesCollection;
            }
        }

        public Tile[,] CurrentLayer
        {
            get
            {
                if (evm.SelectedLayer == Layer.DEFAULT)
                    return tiles;
                else if (evm.SelectedLayer == Layer.BACK)
                    return underTiles;
                else
                    return overTiles;
            }
        }

        private ObservableCollection<Tile> overTilesCollection;
        public ObservableCollection<Tile> OverTilesCollection
        {
            get => overTilesCollection;
            set
            {
                overTilesCollection = value;
            }
        }

        private ObservableCollection<Tile> underTilesCollection;
        public ObservableCollection<Tile> UnderTilesCollection
        {
            get => underTilesCollection;
            set
            {
                underTilesCollection = value;
            }
        }
        private int levelNumberOfRow;
        private int levelNumberOfColumn;
        public int LevelNumberOfRow
        {
            get => levelNumberOfRow;
            set
            {
                levelNumberOfRow = value;
                OnPropertyChanged("LevelNumberOfRow");
            }
        }
        public int LevelNumberOfColumn
        {
            get => levelNumberOfColumn;
            set
            {
                levelNumberOfColumn = value;
                OnPropertyChanged("LevelNumberOfColumn");
            }
        }
        public Tile[,] tiles { get; set; }
        public Tile[,] overTiles { get; set; }
        public Tile[,] underTiles { get; set; }
        TileIdentity[,] backupTiles;
        public LevelViewModel(EditorViewModel evm, int levelWidth, int levelHeight)
        {
            LevelNumberOfRow = levelHeight;
            LevelNumberOfColumn = levelWidth;
            OverTilesCollection = new ObservableCollection<Tile>();
            UnderTilesCollection = new ObservableCollection<Tile>();
            tiles = new Tile[LevelNumberOfRow, LevelNumberOfColumn];
            overTiles = new Tile[LevelNumberOfRow, LevelNumberOfColumn];
            underTiles = new Tile[LevelNumberOfRow, LevelNumberOfColumn];
            this.evm = evm;
        }

        public void PopulateEmptyGrid()
        {
            for (short i = 0; i < LevelNumberOfRow; i++)
            {
                for (short j = 0; j < LevelNumberOfColumn; j++)
                {
                    Tile newTile = new Tile(TilesetNames[0], 0, 0);
                    Tile newUnderTile = new Tile(TilesetNames[0], 0, 0);
                    Tile newOverTile = new AdvancedTile(TilesetNames[0], new Coord(i, j), 0, 0, this);
                    tiles[i, j] = newTile;
                    overTiles[i, j] = newOverTile;
                    underTiles[i, j] = newUnderTile;
                    TilesCollection.Add(tiles[i, j]);
                    OverTilesCollection.Add(overTiles[i, j]);
                    UnderTilesCollection.Add(underTiles[i, j]);
                }
            }
            OnPropertyChanged("TilesCollection");
            OnPropertyChanged("OverTilesCollection");
            OnPropertyChanged("UnderTilesCollection");
        }

        public void PopulateGridFromTileIdentities(TileIdentity[,] underTI, TileIdentity[,] defaultTI, TileIdentity[,] overTI)
        {
            for (short i = 0; i < LevelNumberOfRow; i++)
            {
                for (short j = 0; j < LevelNumberOfColumn; j++)
                {
                    if (defaultTI[j, i].showTimeMS.Length > 0)
                        tiles[i, j] = new AnimatedTile(defaultTI[j, i].tileset, new Coord(i, j), defaultTI[j, i].x, defaultTI[j, i].y, defaultTI[j, i].showTimeMS, this);
                    else
                        tiles[i, j] = new Tile(defaultTI[j, i].tileset, defaultTI[j, i].x[0], defaultTI[j, i].y[0]);
                    TilesCollection.Add(tiles[i, j]);

                    if (underTI[j, i].showTimeMS.Length > 0)
                        underTiles[i, j] = new AnimatedTile(underTI[j, i].tileset, new Coord(i, j), underTI[j, i].x, underTI[j, i].y, underTI[j, i].showTimeMS, this);
                    else
                        underTiles[i, j] = new Tile(underTI[j, i].tileset, underTI[j, i].x[0], underTI[j, i].y[0]);
                    UnderTilesCollection.Add(underTiles[i, j]);

                    if (overTI[j, i].showTimeMS.Length > 0)
                        overTiles[i, j] = new AnimatedTile(overTI[j, i].tileset, new Coord(i, j), overTI[j, i].x, overTI[j, i].y, overTI[j, i].showTimeMS, this);
                    else
                        overTiles[i, j] = new AdvancedTile(overTI[j, i].tileset, new Coord(i, j), overTI[j, i].x[0], overTI[j, i].y[0], this);
                    OverTilesCollection.Add(overTiles[i, j]);
                }
            }
            OnPropertyChanged("TilesCollection");
            OnPropertyChanged("OverTilesCollection");
            OnPropertyChanged("UnderTilesCollection");
        }

        #region mouse events manager
        public override void MouseDownOnTile(int x, int y, Coord pos)
        {
            if (Mouse.RightButton == MouseButtonState.Pressed)
                RightMouseDownOnTile(pos);
            else if (evm.SelectedTool == Tool.DRAW)
                MouseDownOnTileDraw(pos);
            else if (evm.SelectedTool == Tool.RECT)
                MouseDownOnTileRect(pos);
            else if (evm.SelectedTool == Tool.FILL)
                MouseDownOnTileFill(pos);
        }

        public override void MouseEnteringTile(int x, int y, Coord pos)
        {
            if (RightKeyDownOnTile.x != -1)
                RightMouseEnterTile(pos);
            else if (evm.SelectedTool == Tool.DRAW)
                MouseEnterTileDraw(pos);
            else if (evm.SelectedTool == Tool.RECT)
                MouseEnterTileRect(pos);
            else if (evm.SelectedTool == Tool.FILL)
                MouseEnterTileFill(pos);
        }

        public override void MouseLeavingTile(int x, int y, Coord pos)
        {
            if (RightKeyDownOnTile.x != -1)
                RightMouseLeaveTile(pos);
            else if (evm.SelectedTool == Tool.DRAW)
                MouseLeaveTileDraw(pos);
            else if (evm.SelectedTool == Tool.RECT)
                MouseLeaveTileRect(pos);
            else if (evm.SelectedTool == Tool.FILL)
                MouseLeaveTileFill(pos);
        }

        public override void MouseReleaseOnTile(int x, int y, Coord pos)
        {
            if (RightKeyDownOnTile.x != -1)
                RightMouseReleaseOnTile(pos);
            /*else if (evm.SelectedTool == Tool.DRAW)
                MouseReleaseOnTileDraw(pos);*/
            else if (evm.SelectedTool == Tool.RECT)
                MouseReleaseOnTileRect(pos);
            else if (evm.SelectedTool == Tool.FILL)
                MouseReleaseOnTileFill(pos);
        }
        #endregion

        #region Right mouse button events
        private void RightMouseDownOnTile(Coord pos)
        {
            if (RightKeyDownOnTile.x == -1)
            {
                MouseLeavingTile(0, 0, pos);
                RightKeyDownOnTile = pos;
                CurrentLayer[pos.x, pos.y].CurrentAlpha = 0.8f;
            }
        }
        private void RightMouseReleaseOnTile(Coord pos)
        {
            int minX = Math.Min(pos.x, RightKeyDownOnTile.x);
            int minY = Math.Min(pos.y, RightKeyDownOnTile.y);
            int maxX = Math.Max(pos.x, RightKeyDownOnTile.x);
            int maxY = Math.Max(pos.y, RightKeyDownOnTile.y);
            TileIdentity[,] newSelectedTiles = new TileIdentity[maxY - minY + 1, maxX - minX + 1];

            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    CurrentLayer[i, j].CurrentAlpha = 1;
                    newSelectedTiles[j - minY, i - minX] = new TileIdentity(CurrentLayer[i, j].TilesetName, CurrentLayer[i, j].TilesetIndexX, CurrentLayer[i, j].TilesetIndexY);
                }
            }
            RightKeyDownOnTile = new Coord(-1, -1);
            evm.SelectedTiles = newSelectedTiles;
            MouseEnteringTile(0, 0, pos);
        }
        private void RightMouseEnterTile(Coord pos)
        {
            int minX = Math.Min(pos.x, RightKeyDownOnTile.x);
            int minY = Math.Min(pos.y, RightKeyDownOnTile.y);
            int maxX = Math.Max(pos.x, RightKeyDownOnTile.x);
            int maxY = Math.Max(pos.y, RightKeyDownOnTile.y);
            for (int i = minX; i <= maxX; i++)
                for (int j = minY; j <= maxY; j++)
                    CurrentLayer[i, j].CurrentAlpha = 0.8f;
        }
        private void RightMouseLeaveTile(Coord pos)
        {
            int minX = Math.Min(pos.x, RightKeyDownOnTile.x);
            int minY = Math.Min(pos.y, RightKeyDownOnTile.y);
            int maxX = Math.Max(pos.x, RightKeyDownOnTile.x);
            int maxY = Math.Max(pos.y, RightKeyDownOnTile.y);
            for (int i = minX; i <= maxX; i++)
                for (int j = minY; j <= maxY; j++)
                    CurrentLayer[i, j].CurrentAlpha = 1;
        }
        #endregion

        #region Draw tool events
        private void MouseDownOnTileDraw(Coord pos)
        {
            TileIdentity[,] ti = evm.SelectedTiles;
            KeyDownOnTile = pos;
            backupTiles = new TileIdentity[ti.GetLength(1), ti.GetLength(0)];
            for (int i = 0; i < ti.GetLength(1); i++)
                for (int j = 0; j < ti.GetLength(0); j++)
                    backupTiles[i, j] = ti[j, i];
        }
        private void MouseReleaseOnTileDraw(Coord pos)
        {

        }
        private void MouseEnterTileDraw(Coord pos)
        {
            TileIdentity[,] ti = evm.SelectedTiles;
            backupTiles = new TileIdentity[ti.GetLength(1), ti.GetLength(0)];

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                for (int i = 0; i < ti.GetLength(1) && pos.x + i < LevelNumberOfRow; i++)
                {
                    for (int j = 0; j < ti.GetLength(0) && pos.y + j < LevelNumberOfColumn; j++)
                    {
                        int x = (pos.x - KeyDownOnTile.x + i) % ti.GetLength(1);
                        int y = (pos.y - KeyDownOnTile.y + j) % ti.GetLength(0);
                        while (x < 0)
                            x += ti.GetLength(1);
                        while (y < 0)
                            y += ti.GetLength(0);
                        TileIdentity currentTI = ti[y, x];
                        CurrentLayer[pos.x + i, pos.y + j].ApplyTileIdentity(currentTI);
                        backupTiles[i, j] = currentTI;
                    }
                }
            }
            else
            {
                for (int i = 0; i < ti.GetLength(0) && pos.y + i < LevelNumberOfColumn; i++)
                {
                    for (int j = 0; j < ti.GetLength(1) && pos.x + j < LevelNumberOfRow; j++)
                    {
                        TileIdentity currentTI;
                        if (CurrentLayer[pos.x + j, pos.y + i] is AnimatedTile)
                            currentTI = new TileIdentity(CurrentLayer[pos.x + j, pos.y + i].TilesetName, ((AnimatedTile)CurrentLayer[pos.x + j, pos.y + i]).IndexesX, ((AnimatedTile)CurrentLayer[pos.x + j, pos.y + i]).IndexesY, ((AnimatedTile)CurrentLayer[pos.x + j, pos.y + i]).ShowTimes);
                        else
                            currentTI = new TileIdentity(CurrentLayer[pos.x + j, pos.y + i].TilesetName, CurrentLayer[pos.x + j, pos.y + i].TilesetIndexX, CurrentLayer[pos.x + j, pos.y + i].TilesetIndexY);
                        backupTiles[j, i] = currentTI;
                        CurrentLayer[pos.x + j, pos.y + i].ApplyTileIdentity(ti[i, j]);
                        CurrentLayer[pos.x + j, pos.y + i].CurrentAlpha = 0.8f;
                    }
                }
            }

        }
        private void MouseLeaveTileDraw(Coord pos)
        {
            for (int i = 0; i < backupTiles.GetLength(1) && pos.y + i < LevelNumberOfColumn; i++)
            {
                for (int j = 0; j < backupTiles.GetLength(0) && pos.x + j < LevelNumberOfRow; j++)
                {
                    CurrentLayer[pos.x + j, pos.y + i].ApplyTileIdentity(backupTiles[j, i]);
                    CurrentLayer[pos.x + j, pos.y + i].CurrentAlpha = 1;
                }
            }
        }
        #endregion

        #region Rect tool events
        private void MouseDownOnTileRect(Coord pos)
        {
            KeyDownOnTile = pos;
            backupTiles = new TileIdentity[1 + LevelNumberOfRow - pos.y, 1 + LevelNumberOfColumn - pos.x];
            //backupTiles[0, 0] = evm.SelectedTiles[0, 0];

            for (int i = 0; i < LevelNumberOfColumn - pos.x; i++)
            {
                for (int j = 0; j < LevelNumberOfRow - pos.y; j++)
                {
                    backupTiles[j, i] = CurrentLayer[j + pos.y, i + pos.x].GetIdentity();
                }
            }
        }
        private void MouseReleaseOnTileRect(Coord pos)
        {
            backupTiles = new TileIdentity[1, 1];
            backupTiles[0, 0] = CurrentLayer[pos.x, pos.y].GetIdentity();

            int minX = Math.Min(pos.x, KeyDownOnTile.x);
            int minY = Math.Min(pos.y, KeyDownOnTile.y);
            int maxX = Math.Max(pos.x, KeyDownOnTile.x);
            int maxY = Math.Max(pos.y, KeyDownOnTile.y);

            for (int i = minX; i <= maxX; i++)
                for (int j = minY; j <= maxY; j++)
                    CurrentLayer[i, j].CurrentAlpha = 1;
        }
        private void MouseEnterTileRect(Coord pos)
        {
            TileIdentity[,] ti = evm.SelectedTiles;
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {

                if (LastTileOver.x >= KeyDownOnTile.x && LastTileOver.y >= KeyDownOnTile.y)
                {
                    if (pos.x >= LastTileOver.x)
                    {
                        for (int i = LastTileOver.x; i <= pos.x; i++)
                        {
                            for (int j = KeyDownOnTile.y; j <= pos.y; j++)
                            {
                                //backupTiles[i - KeyDownOnTile.x, j - KeyDownOnTile.y] = CurrentLayer[i, j].GetIdentity();
                                CurrentLayer[i, j].ApplyTileIdentity(ti[(j - KeyDownOnTile.y) % ti.GetLength(0), (i - KeyDownOnTile.x) % ti.GetLength(1)]);
                            }
                        }
                    }
                    else
                    {
                        for (int i = LastTileOver.x; i > pos.x; i--)
                        {
                            for (int j = KeyDownOnTile.y; j <= pos.y; j++)
                            {
                                CurrentLayer[i, j].ApplyTileIdentity(backupTiles[i - KeyDownOnTile.x, j - KeyDownOnTile.y]);
                            }
                        }
                    }

                    if (pos.y >= LastTileOver.y)
                    {
                        for (int i = LastTileOver.y; i <= pos.y; i++)
                        {
                            for (int j = KeyDownOnTile.x; j <= pos.x; j++)
                            {
                                //backupTiles[j - KeyDownOnTile.x, i - KeyDownOnTile.y] = CurrentLayer[j, i].GetIdentity();
                                CurrentLayer[j, i].ApplyTileIdentity(ti[(i - KeyDownOnTile.y) % ti.GetLength(0), (j - KeyDownOnTile.x) % ti.GetLength(1)]);
                            }
                        }
                    }
                    else
                    {
                        for (int i = LastTileOver.y; i > pos.y; i--)
                        {
                            for (int j = KeyDownOnTile.x; j <= pos.x; j++)
                            {
                                CurrentLayer[j, i].ApplyTileIdentity(backupTiles[j - KeyDownOnTile.x, i - KeyDownOnTile.y]);
                            }
                        }
                    }
                }
            }
            else
            {
                backupTiles = new TileIdentity[1, 1];
                backupTiles[0, 0] = CurrentLayer[pos.x, pos.y].GetIdentity();
                CurrentLayer[pos.x, pos.y].ApplyTileIdentity(ti[0, 0]);
            }





            /*if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                int minX = Math.Min(pos.x, KeyDownOnTile.x);
                int minY = Math.Min(pos.y, KeyDownOnTile.y);
                int maxX = Math.Max(pos.x, KeyDownOnTile.x);
                int maxY = Math.Max(pos.y, KeyDownOnTile.y);
                backupTiles = new TileIdentity[maxX - minX + 1, maxY - minY + 1];
                for (int i = minX; i <= maxX; i++)
                {
                    for (int j = minY; j <= maxY; j++)
                    {
                        backupTiles[i - minX, j - minY] = new TileIdentity(LayerTiles[i, j].TilesetName, LayerTiles[i, j].TilesetIndexX, LayerTiles[i, j].TilesetIndexY);
                        TileIdentity currentTI = ti[(j - minY) % ti.GetLength(0), (i - minX) % ti.GetLength(1)];
                        LayerTiles[i, j].ApplyTileIdentity(currentTI);
                        LayerTiles[i, j].CurrentAlpha = 0.8f;
                    }
                }
            }*/

        }
        private void MouseLeaveTileRect(Coord pos)
        {
            LastTileOver = pos;
            if (Mouse.LeftButton == MouseButtonState.Released)
            {
                CurrentLayer[pos.x, pos.y].ApplyTileIdentity(backupTiles[0, 0]);
            }
        }
        #endregion

        #region Fill tool events
        private void MouseDownOnTileFill(Coord pos)
        {

        }
        private void MouseReleaseOnTileFill(Coord pos)
        {
            TileIdentity[,] ti = evm.SelectedTiles;
            Stack<Coord> tilesToTransform = new Stack<Coord>();
            List<Coord> checkedPos = new List<Coord>();
            tilesToTransform.Push(pos);
            CurrentLayer[pos.x, pos.y].ApplyTileIdentity(backupTiles[0, 0]);
            while (tilesToTransform.Count != 0)
            {
                Coord current = tilesToTransform.Pop();
                if (!checkedPos.Contains(current))
                {
                    TileIdentity currentIdentity = new TileIdentity(CurrentLayer[current.x, current.y].TilesetName, CurrentLayer[current.x, current.y].TilesetIndexX, CurrentLayer[current.x, current.y].TilesetIndexY);
                    if (backupTiles[0, 0].tileset == currentIdentity.tileset && currentIdentity.x.OrderBy(a => a).SequenceEqual(backupTiles[0, 0].x.OrderBy(a => a)) && currentIdentity.y.OrderBy(a => a).SequenceEqual(backupTiles[0, 0].y.OrderBy(a => a)))
                    {
                        TileIdentity newTI = ti[(current.y + pos.y) % ti.GetLength(0), (current.x + pos.x) % ti.GetLength(1)];
                        CurrentLayer[current.x, current.y].ApplyTileIdentity(newTI);
                        checkedPos.Add(current);
                        if (current.x + 1 < LevelNumberOfRow && !checkedPos.Contains(new Coord(current.x + 1, current.y)))
                            tilesToTransform.Push(new Coord(current.x + 1, current.y));
                        if (current.x - 1 >= 0 && !checkedPos.Contains(new Coord(current.x - 1, current.y)))
                            tilesToTransform.Push(new Coord(current.x - 1, current.y));
                        if (current.y + 1 < LevelNumberOfColumn && !checkedPos.Contains(new Coord(current.x, current.y + 1)))
                            tilesToTransform.Push(new Coord(current.x, current.y + 1));
                        if (current.y - 1 >= 0 && !checkedPos.Contains(new Coord(current.x, current.y - 1)))
                            tilesToTransform.Push(new Coord(current.x, current.y - 1));
                    }
                }
            }
            backupTiles[0, 0] = new TileIdentity(CurrentLayer[pos.x, pos.y].TilesetName, CurrentLayer[pos.x, pos.y].TilesetIndexX, CurrentLayer[pos.x, pos.y].TilesetIndexY);
        }

        private void MouseEnterTileFill(Coord pos)
        {
            TileIdentity[,] ti = evm.SelectedTiles;
            backupTiles = new TileIdentity[1, 1];
            backupTiles[0, 0] = new TileIdentity(CurrentLayer[pos.x, pos.y].TilesetName, CurrentLayer[pos.x, pos.y].TilesetIndexX, CurrentLayer[pos.x, pos.y].TilesetIndexY);
            CurrentLayer[pos.x, pos.y].ApplyTileIdentity(ti[0, 0]);
        }
        private void MouseLeaveTileFill(Coord pos)
        {
            CurrentLayer[pos.x, pos.y].ApplyTileIdentity(backupTiles[0, 0]);
        }
        #endregion

        public void UpdateLevelSize(int x, int y)
        {
            ObservableCollection<Tile> newTileCollection = new ObservableCollection<Tile>();
            ObservableCollection<Tile> newOverTileCollection = new ObservableCollection<Tile>();
            ObservableCollection<Tile> newUnderTileCollection = new ObservableCollection<Tile>();
            Tile[,] newTiles = new Tile[y, x];
            Tile[,] newUnderTiles = new Tile[y, x];
            Tile[,] newOverTiles = new Tile[y, x];
            LevelNumberOfRow = y;
            LevelNumberOfColumn = x;
            short s1 = 0;
            for (short i = 0; i < LevelNumberOfRow; i++)
            {
                for (short j = 0; j < LevelNumberOfColumn; j++)
                {
                    Tile newTile;
                    Tile newUnderTile;
                    Tile newOverTile;
                    if (j < tiles.GetLength(1) && i < tiles.GetLength(0))
                    {
                        newTile = tiles[i, j];
                        newUnderTile = underTiles[i, j];
                        newOverTile = overTiles[i, j];
                    }
                    else
                    {
                        newTile = new Tile(TilesetNames[0], 0, 0);
                        newUnderTile = new Tile(TilesetNames[0], 0, 0);
                        newOverTile = new AdvancedTile(TilesetNames[0], new Coord(i, j), 0, 0, this);
                    }
                    newTiles[i, j] = newTile;
                    newUnderTiles[i, j] = newUnderTile;
                    newOverTiles[i, j] = newOverTile;
                    newTileCollection.Add(newTiles[i, j]);
                    newOverTileCollection.Add(newOverTiles[i, j]);
                    newUnderTileCollection.Add(newUnderTiles[i, j]);
                    s1++;
                }
            }
            tiles = newTiles;
            underTiles = newUnderTiles;
            overTiles = newOverTiles;
            TilesCollection = newTileCollection;
            OverTilesCollection = newOverTileCollection;
            UnderTilesCollection = newUnderTileCollection;

            OnPropertyChanged("TilesCollection");
            OnPropertyChanged("OverTilesCollection");
            OnPropertyChanged("UnderTilesCollection");
        }
        public void OnLoadedTilesetChange()
        {
            foreach (Tile t in tiles)
                t.OnLoadedTilesetChange();
            foreach (Tile t in underTiles)
                t.OnLoadedTilesetChange();
            foreach (Tile t in overTiles)
                t.OnLoadedTilesetChange();
        }
    }
}
