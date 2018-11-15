using System.ComponentModel;
using System.Drawing;

namespace Minesweeper
{
    public class CellView : INotifyPropertyChanged
    {
        public CellView()
        {
        }
        Point position;
        public Point Position {
            get {
                return position;
            }
            set {
                if (position != value) {
                    position = value;
                    RaisePropertyChanged(nameof(Position));
                }
            }

        }
        bool isMark;
        public bool IsMark {
            get {
                return isMark;
            }
            set {
                if (isMark != value) {
                    isMark = value;
                    RaisePropertyChanged(nameof(IsMark));
                }
            }

        }
        bool isExplode;
        public bool IsExplode {
            get {
                return isExplode;
            }
            set {
                if (isExplode != value) {
                    isExplode = value;
                    RaisePropertyChanged(nameof(IsExplode));
                }
            }
        }
        bool isOpen;
        public bool IsOpen {
            get {
                return isOpen;
            }
            set {
                if (isOpen != value) {
                    isOpen = value;
                    RaisePropertyChanged(nameof(IsOpen));
                }
            }
        }
        bool isMine;
        public bool IsMine {
            get {
                return isMine;
            }
            set {
                if (isMine != value) {
                    isMine = value;
                    RaisePropertyChanged(nameof(IsMine));
                }
            }
        }
        int adjacentMines;
        public int AdjacentMines {
            get {
                return adjacentMines;
            }
            set {
                if (adjacentMines != value) {
                    adjacentMines = value;
                    RaisePropertyChanged(nameof(AdjacentMines));
                }
            }
        }
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
