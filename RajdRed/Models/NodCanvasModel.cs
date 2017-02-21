using RajdRed.Models.Base;
using RajdRed.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RajdRed.Models
{
    public class NodCanvasModel : NodModelBase
    {
        public NodCanvasViewModel NodCanvasViewModel { get; set; }
        public NodCanvasModel(){}

        public NodCanvasModel(Point p, NodCanvasViewModel n)
        {
            PositionLeft = p.X - Width / 2;
            PositionTop = p.Y - Height / 2;
            Path = NodTypesModel.Association;
            NodCanvasViewModel = n;
        }

        void SetLinje(LinjeModel lm)
        {
            LinjeListModel.Add(lm);
        }

        public static NodCanvasModel CopyNod(NodKlassModel n)
        {
            NodCanvasModel ncm = new NodCanvasModel()
            {
                Height = n.Height,
                Width = n.Width,
                IsSelected = n.IsSelected,
                LinjeListModel = n.LinjeListModel,
                OnField = n.OnField,
                Path = n.Path,
                NodTypesModel = n.NodTypesModel,
                PositionLeft = n.PositionLeft,
                PositionTop = n.PositionTop
            };

            foreach (LinjeModel l in n.LinjeListModel)
            {
                if (l.Nod1 == n)
                    l.Nod1 = ncm;
                else
                    l.Nod2 = ncm;

                l.SetOnPropertyChanged();
            }

            return ncm;
        }
    }
}
