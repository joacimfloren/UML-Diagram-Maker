using RajdRed.Models.Base;
using RajdRed.ViewModels;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RajdRed.Models
{
    public class LinjeModel : RajdElement
    {
        public LinjeViewModel LinjeViewModel { get; set; }

        private double _x1;
        public double X1
        {
            get { return _x1; }
            set {
                _x1 = value + NodModelBase.MinSize / 2;
                OnPropertyChanged("X1");  
            }
        }

        private double _y1;
        public double Y1
        {
            get { return _y1; }
            set {
                _y1 = value + NodModelBase.MinSize / 2; 
                OnPropertyChanged("Y1"); 
            }
        }

        private double _x2;
        public double X2
        {
            get { return _x2; }
            set {
                _x2 = value + NodModelBase.MinSize / 2; 
                OnPropertyChanged("X2"); 
            }
        }

        private double _y2;
        public double Y2
        {
            get { return _y2; }
            set {
                _y2 = value + NodModelBase.MinSize / 2; 
                OnPropertyChanged("Y2"); 
            }
        }

        private NodModelBase _nod1;
        public NodModelBase Nod1
        {
            get
            {
                return _nod1;
            }
            set
            {
                _nod1 = value;
                X1 = value.PositionLeft;
                Y1 = value.PositionTop;
                OnPropertyChanged("X1");
                OnPropertyChanged("Y1");
            }
        }

        private NodModelBase _nod2;
        public NodModelBase Nod2
        {
            get
            {
                return _nod2;
            }
            set
            {
                _nod2 = value;
                X2 = value.PositionLeft;
                Y2 = value.PositionTop;
                OnPropertyChanged("X2");
                OnPropertyChanged("Y2");
            }
        }

        public override bool IsSelected
        {
            get
            {
                return base.IsSelected;
            }
            set
            {
                base.IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public LinjeModel(LinjeViewModel lvm, NodModelBase n1, NodModelBase n2)
        {
            LinjeViewModel = lvm;
            Nod1 = n1;
            Nod2 = n2;

            SetOnPropertyChanged();
        }

        public void SetOnPropertyChanged()
        {
            _nod1.PropertyChanged += (sender, eArgs) =>
            {
                if (eArgs.PropertyName == "PositionLeft" || eArgs.PropertyName == "PositionTop")
                {
                    X1 = _nod1.PositionLeft;
                    Y1 = _nod1.PositionTop;
                }
            };
            _nod2.PropertyChanged += (sender, eArgs) =>
            {
                if (eArgs.PropertyName == "PositionLeft" || eArgs.PropertyName == "PositionTop")
                {
                    X2 = _nod2.PositionLeft;
                    Y2 = _nod2.PositionTop;
                }
            };
        }

        public void ReplaceNod(NodModelBase oldNod, NodModelBase newNod)
        {
            if (Nod1 == oldNod)
                Nod1 = newNod;
            else
                Nod2 = newNod;

            SetOnPropertyChanged();
        }

        public static LinjeModel GetSharingLinje(NodModelBase n1, NodModelBase n2)
        {
            foreach (LinjeModel l in n1.LinjeListModel)
            {
                if (l.Nod1 == n2 || l.Nod2 == n2)
                    return l;
            }

            return null;
        }
    }
}
