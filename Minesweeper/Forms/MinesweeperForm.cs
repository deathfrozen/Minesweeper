using Minesweeper.Properties;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class MinesweeperForm : Form
    {
        IMinesweeper game;
        IArea gameArea;
        Settings settings;
        public MinesweeperForm()
        {
            settings = new Settings();
            game = new MinesweeperGame(settings);
            gameArea = new GameArea(settings, game);
            cells = game.StartNewGame(gameArea);
            game.GameOverAction = (start) => new GameOverForm(start).ShowDialog();
            game.VictoryAction = (start) => new VictoryForm(start).ShowDialog();
            InitializeComponent();
        }
    }
}
