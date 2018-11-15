using System;
using System.Linq;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using Minesweeper.Properties;
namespace Minesweeper
{
    public interface IArea
    {
        MyList<CellView> GenerateGrid();
        bool CheckAllFreeCellOpen();
        void OpenAllClosedCells();
        void ExplodeAllMines();
        void CheckMarkedAndOpenAdjacentCells(Point position);
    }
    public class GameArea : IArea
    {
        MyList<CellView> Cells { get; }
        IMinesweeper game;
        Settings settings;
        internal GameArea(Settings settings, IMinesweeper game)
        {
            Cells = new MyList<CellView>();
            this.settings = settings;
            this.game = game;
        }
        public void ExplodeAllMines()
        {
            Cells.Where(c => c.IsMine && !c.IsExplode).ToList().ForEach(c => c.IsExplode = true);
        }
        public void OpenAllClosedCells()
        {
            Cells.Where(c => !c.IsOpen).ToList().ForEach(c => c.IsOpen = true);
        }
        public bool CheckAllFreeCellOpen()
        {
            return Cells.Where(c => c.IsOpen).Count() == (settings.WidthArea * settings.HeightArea) - settings.MinesArea;
        }
        public MyList<CellView> GenerateGrid()
        {
            Cells.Clear();
            List<CellView> cells = new List<CellView>();
            for (int widthIterator = 0; widthIterator < settings.WidthArea; widthIterator++) {
                for (int heightIterator = 0; heightIterator < settings.HeightArea; heightIterator++) {
                    Point currentCellPosition = new Point(widthIterator, heightIterator);
                    CellView cell = new CellView() {
                        IsMine = game.CheckMine(currentCellPosition),
                        AdjacentMines = GetBombsCount(currentCellPosition),
                        Position = currentCellPosition
                    };
                    cell.PropertyChanged += CellPropertyChanged;
                    cells.Add(cell);
                }
            }
            Cells.AddRange(cells);
            return Cells;
        }
        public void CheckMarkedAndOpenAdjacentCells(Point position)
        {
            int minesCount = GetCell(position).AdjacentMines;
            int markCellsCount = CountAdjacentMarkedCells(position);
            if (markCellsCount == minesCount) {
                OpenAdjacentCells(position, true);
                game.CheckVictory();
            }
        }
        void OpenAdjacentCells(Point position, bool checkMine = false)
        {
            EachAdjacentCells(position, positionAdjacent => {
                CellView cell = GetCell(positionAdjacent);
                if (!cell.IsMark) {
                    if (checkMine && cell.IsMine) {
                        cell.IsExplode = true;
                    }
                    cell.IsOpen = true;
                }
            });
        }

        void CellPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            CellView cell = (CellView)sender;
            switch (e.PropertyName) {
                case nameof(CellView.IsExplode):
                    game.GameOver();
                    break;
                case nameof(CellView.IsOpen):
                    if (cell.AdjacentMines == 0) {
                        OpenAdjacentCells(cell.Position);
                    }
                    game.CheckVictory();
                    break;
            }
        }

        void EachAdjacentCells(Point position, Action<Point> action)
        {
            for (int iteratorX = -1; iteratorX <= 1; iteratorX++) {
                for (int iteratorY = -1; iteratorY <= 1; iteratorY++) {
                    if (position.X + iteratorX >= 0 && position.X + iteratorX < settings.WidthArea && position.Y + iteratorY >= 0 && position.Y + iteratorY < settings.HeightArea) {
                        action(new Point(position.X + iteratorX, position.Y + iteratorY));
                    }
                }
            }
        }
        int CountAdjacentMarkedCells(Point position)
        {
            int result = 0;
            EachAdjacentCells(position, positionAdjacent => {
                CellView cell = GetCell(positionAdjacent);
                if (cell.IsMark) {
                    result++;
                }
            });
            return result;
        }
        CellView GetCell(Point position)
        {
            return Cells.Single(c => c.Position.X == position.X && c.Position.Y == position.Y);
        }
        int GetBombsCount(Point position)
        {
            int result = 0;
            EachAdjacentCells(position, positionAdjacent => { if (game.CheckMine(positionAdjacent)) { result++; } });
            return result;
        }
    }
}
