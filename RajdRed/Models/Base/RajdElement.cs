using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RajdRed.Models.Base
{
    public abstract class RajdElement : BaseModel
    {
        private bool _isSelected = false;
        public virtual bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("IsSelected"); } }
        
        public bool OnField { get; set; }

        private int _zIndex = 0;
        public int ZIndex
        {
            get { return _zIndex; }
            set { 
                _zIndex = value;
                OnPropertyChanged("ZIndex");
            }
        }
        
    }
}
