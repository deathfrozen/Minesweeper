using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper
{
    public interface IMinesweeper
    {
        Action<Func<MyList<CellView>>> GameOverAction { get; set; }
        Action<Func<MyList<CellView>>> VictoryAction { get; set; }
        MyList<CellView> StartNewGame(IArea area);
        bool CheckMine(Point position);
        void GameOver();
        void CheckVictory();
    }
    public class MinesweeperGame : IMinesweeper
    {
        List<int> Mines { get; }
        public Action<Func<MyList<CellView>>> GameOverAction { get; set; }
        public Action<Func<MyList<CellView>>> VictoryAction { get; set; }
        bool gameOverProcess = false;
        IArea gameArea;
        public MinesweeperGame()
        {
            Mines = new List<int>();
        }
        public MyList<CellView> StartNewGame(IArea area)
        {
            gameArea = area;
            GenerateMines();
            return gameArea.GenerateGrid();
        }
        public void CheckVictory()
        {
            if (!gameOverProcess && gameArea.CheckAllFreeCellOpen()) {
                gameArea.OpenAllClosedCells();
                if (VictoryAction == null) {
                    StartNewGame(gameArea);
                } else {
                    VictoryAction(() => {
                        return StartNewGame(gameArea);
                    });
                }
            }
        }
        public void GameOver()
        {
            if (!gameOverProcess) {
                gameOverProcess = true;
                gameArea.OpenAllClosedCells();
                gameArea.ExplodeAllMines();
                if (GameOverAction == null) {
                    StartNewGame(gameArea);
                } else {
                    GameOverAction(() => {
                        return StartNewGame(gameArea);
                    });
                }
                gameOverProcess = false;
            }
        }
        void GenerateMines()
        {
            Mines.Clear();
            Random random = new Random();
            while (Mines.Count < Settings.MinesArea) {
                int minePosition = random.Next(1, Settings.WidthArea * Settings.HeightArea);
                if (!Mines.Any(m => m == minePosition)) {
                    Mines.Add(minePosition);
                }
            }
        }
        public bool CheckMine(Point position)
        {
            return Mines.Any(m => m == position.X + (position.Y * Settings.WidthArea));
        }
    }
}
