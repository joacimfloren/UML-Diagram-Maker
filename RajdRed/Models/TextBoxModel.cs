using RajdRed.Models.Base;
using RajdRed.ViewModels;
using System.Windows;
using System.Windows.Media;
namespace RajdRed.Models
{
    public class TextBoxModel : RajdElement
    {
        public TextBoxViewModel TextBoxViewModel { get; set; }
        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged("Text"); }
        }

        private double _positionLeft;
        public double PositionLeft
        {
            get { return _positionLeft; }
            set { _positionLeft = value; OnPropertyChanged("PositionLeft"); }
        }

        private double _positionTop;
        public double PositionTop
        {
            get { return _positionTop; }
            set { _positionTop = value; OnPropertyChanged("PositionTop"); }
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
                OnPropertyChanged("Visible");
            }
        }

        private bool _editable = false;
        public bool Editable
        {
            get { return _editable; }
            set { _editable = value; OnPropertyChanged("Editable"); }
        }
        
        public Visibility Visible
        {
            get
            {
                if (IsSelected)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
        }

        public TextBoxModel(Point p, TextBoxViewModel tbvm)
        {
            PositionLeft = p.X;
            PositionTop = p.Y;
            TextBoxViewModel = tbvm;
        }
        
    }
}
