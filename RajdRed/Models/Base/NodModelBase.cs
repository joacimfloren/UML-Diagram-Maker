using RajdRed.Models.Adds;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using System;

namespace RajdRed.Models.Base
{
    public class NodModelBase : RajdElement
    {
        public LinjeListModel LinjeListModel = new LinjeListModel();

        public int Number { get; set; }
        public static int MinSize = 15;

        private bool _isSet = false;
        public bool IsSet
        {
            get { return _isSet; }
            set { _isSet = value; OnPropertyChanged("IsSet"); }
        }
        
        private double _width = MinSize;
        public virtual double Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged("Width"); }
        }

        private double _height = MinSize;
        public virtual double Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged("Height"); }
        }

        public NodTypesModel NodTypesModel;
        
        
        private Path _path;
        public Path Path
        {
            get { return _path; }
            set
            {
                if (_path != value)
                    _path = value;

                OnPropertyChanged("Path");
            }
        }
        
        private double _positionLeft;
        public double PositionLeft
        {
            get { return _positionLeft; }
            set
            {
                if (_positionLeft != value)
                    _positionLeft = value;

                OnPropertyChanged("PositionLeft");
            }
        }

        private double _positionTop;
        public double PositionTop
        {
            get { return _positionTop; }
            set 
            { 
                _positionTop = value; 
                OnPropertyChanged("PositionTop");
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

        public NodModelBase()
        {
            NodTypesModel = new NodTypesModel();
        }

    }
}
