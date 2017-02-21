using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RajdRed.Models.Adds
{
    public class LinjeListModel : List<LinjeModel>
    {
        public bool AllIsSelected()
        {
            bool selected = true;
            foreach (LinjeModel l in this)
                selected = selected && l.IsSelected;

            return selected;
        }
    }
}
