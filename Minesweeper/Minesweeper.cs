using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using Minesweeper.Properties;

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
        Settings settings;
        internal MinesweeperGame(Settings settings)
        {
            Mines = new List<int>();
            this.settings = settings;
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
            while (Mines.Count < settings.MinesArea) {
                int minePosition = random.Next(1, settings.WidthArea * settings.HeightArea);
                if (!Mines.Any(m => m == minePosition)) {
                    Mines.Add(minePosition);
                }
            }
        }
        public bool CheckMine(Point position)
        {
            return Mines.Any(m => m == position.X + (position.Y * settings.WidthArea));
        }
    }
}
