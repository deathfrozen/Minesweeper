using System;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class VictoryForm : Form
    {
        Func<MyList<CellView>> startNewGame;
        public VictoryForm(Func<MyList<CellView>> startNewGame)
        {
            this.startNewGame = startNewGame;
            InitializeComponent();
        }

        private void button1_Click(Object sender, EventArgs e)
        {
            Close();
            startNewGame();
        }
    }
}
