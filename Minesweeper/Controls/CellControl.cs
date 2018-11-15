using System;
using System.Drawing;
using System.Windows.Forms;
using Minesweeper.Properties;

namespace Minesweeper
{
    public partial class CellControl : UserControl
    {
        public Point Position { get; set; }
        bool isExplode;
        public bool IsExplode {
            get {
                return isExplode;
            }
            set {
                if (isExplode != value) {
                    isExplode = value;
                    Invalidate();
                    DataBindings[nameof(CellView.IsExplode)].WriteValue();
                }
            }
        }
        public bool IsMark { get; set; }
        bool isOpen;
        public bool IsOpen {
            get {
                return isOpen;
            }
            set {
                if (isOpen != value) {
                    isOpen = value;
                    Invalidate();
                    DataBindings[nameof(CellView.IsOpen)].WriteValue();
                }
            }
        }
        public bool IsMine { get; set; }
        public int AdjacentMines { get; set; }
        Action<Point> checkMarkedAndOpenAdjacentCells;
        Settings settings;
        internal CellControl(Settings settings, Action<Point> checkMarkedAndOpenAdjacentCells)
        {
            this.settings = settings;
            this.checkMarkedAndOpenAdjacentCells = checkMarkedAndOpenAdjacentCells;
            InitializeComponent();
        }
        protected override void OnClick(EventArgs e)
        {
            MouseEventArgs args = (MouseEventArgs)e;
            base.OnClick(e);
            if (args.Button == MouseButtons.Left) {
                if (!IsMark) {
                    if (IsMine) {
                        IsExplode = true;
                    } else {
                        IsOpen = true;
                    }
                }
            }
            if (args.Button == MouseButtons.Right) {
                if (!IsOpen) {
                    IsMark = !IsMark;
                    DataBindings["isMark"].WriteValue();
                }
            }
            if (args.Button == MouseButtons.Middle && IsOpen && AdjacentMines > 0) {
                checkMarkedAndOpenAdjacentCells(Position);
            }
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Location = new Point(Position.X * settings.CellSize, Position.Y * settings.CellSize);
            Size = new Size(settings.CellSize, settings.CellSize);
            if (IsExplode) {
                e.Graphics.DrawString("*", new Font("Arial", 12, FontStyle.Regular), new SolidBrush(Color.Black), new PointF(4, 3));
            }
            if (IsMark && !IsOpen) {
                e.Graphics.DrawString("¶", new Font("Arial", 12, FontStyle.Regular), new SolidBrush(Color.Black), new PointF(4, 3));
            }
            if (IsOpen && AdjacentMines > 0 && !IsMine) {
                e.Graphics.DrawString(AdjacentMines.ToString(), new Font("Arial", 12, FontStyle.Regular), new SolidBrush(Color.Black), new PointF(4, 3));
            }
            if (IsOpen && IsMine) {
                e.Graphics.DrawString("*", new Font("Arial", 12, FontStyle.Regular), new SolidBrush(Color.Black), new PointF(4, 3));
            }
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
            BackColor = IsExplode ? Color.Red : IsOpen ? (IsMine ? Color.Aqua : Color.Green) : Color.Gray;
            base.OnPaint(e);
        }
    }
}
