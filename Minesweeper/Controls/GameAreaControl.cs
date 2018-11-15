using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class GameAreaControl : UserControl
    {
        MyList<CellView> Cells { get; set; }
        Action<Point> checkMarkedAndOpenAdjacentCells;
        public GameAreaControl(MyList<CellView> cells, Action<Point> checkMarkedAndOpenAdjacentCells)
        {
            DoubleBuffered = true;
            this.checkMarkedAndOpenAdjacentCells = checkMarkedAndOpenAdjacentCells;
            Cells = cells;
            Cells_ListChanged(Cells, null);
            Cells.ListChanged += Cells_ListChanged;
            InitializeComponent();
        }

        private void Cells_ListChanged(Object sender, EventArgs e)
        {
            Controls.Clear();
            foreach(CellView item in Cells) {
                CellControl cell = new CellControl(checkMarkedAndOpenAdjacentCells);
                cell.DataBindings.Add(nameof(CellControl.Position), item, nameof(CellView.Position), false, DataSourceUpdateMode.OnPropertyChanged);
                cell.DataBindings.Add(nameof(CellControl.AdjacentMines), item, nameof(CellView.AdjacentMines), false, DataSourceUpdateMode.OnPropertyChanged);
                cell.DataBindings.Add(nameof(CellControl.IsExplode), item, nameof(CellView.IsExplode), false, DataSourceUpdateMode.OnPropertyChanged);
                cell.DataBindings.Add(nameof(CellControl.IsMark), item, nameof(CellView.IsMark), false, DataSourceUpdateMode.OnPropertyChanged);
                cell.DataBindings.Add(nameof(CellControl.IsMine), item, nameof(CellView.IsMine), false, DataSourceUpdateMode.OnPropertyChanged);
                cell.DataBindings.Add(nameof(CellControl.IsOpen), item, nameof(CellView.IsOpen), false, DataSourceUpdateMode.OnPropertyChanged);
                Controls.Add(cell);
            }
        }
    }
}
