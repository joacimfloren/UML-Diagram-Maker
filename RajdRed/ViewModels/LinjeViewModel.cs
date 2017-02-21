using RajdRed.Models;
using RajdRed.Models.Base;
using RajdRed.Repositories;
using System.Windows;

namespace RajdRed.ViewModels
{
    public class LinjeViewModel
    {
        public LinjeModel LinjeModel { get; set; }
        public LinjeRepository LinjeRepository { get; set; }
        public LinjeViewModel(LinjeRepository lr, NodModelBase n1, NodModelBase n2)
        {
            LinjeRepository = lr;
            LinjeModel = new LinjeModel(this, n1, n2);
        }

        public LinjeViewModel(){}

        public void Split(Point p)
        {
            NodCanvasViewModel ncvm = LinjeRepository.MainRepository.NodCanvasRepository.AddNewCanvasNod(p);
            LinjeRepository.AddNewLinje(LinjeModel.Nod1, ncvm.NodCanvasModel);
            LinjeRepository.AddNewLinje(LinjeModel.Nod2, ncvm.NodCanvasModel);

            ncvm.Select();

            JustDelete();
        }

        public void Delete()
        {
            if (LinjeModel.Nod1 is NodKlassModel && LinjeModel.Nod1.IsSelected)
            {
                NodKlassModel n = LinjeModel.Nod1 as NodKlassModel;
                n.NodKlassViewModel.UnSet();
            }

            if (LinjeModel.Nod2 is NodKlassModel && LinjeModel.Nod2.IsSelected) 
            {
                NodKlassModel n = LinjeModel.Nod2 as NodKlassModel;
                n.NodKlassViewModel.UnSet();
            }

            LinjeModel.Nod1.LinjeListModel.Remove(LinjeModel);
            LinjeModel.Nod2.LinjeListModel.Remove(LinjeModel);

            Deselect();
            LinjeRepository.Remove(this);
        }

        public void JustDelete()
        {
            LinjeModel.Nod1.LinjeListModel.Remove(LinjeModel);
            LinjeModel.Nod2.LinjeListModel.Remove(LinjeModel);

            Deselect();
            LinjeRepository.Remove(this);
        }

        public void Select()
        {
            if (!IsSelected())
            {
                LinjeModel.IsSelected = true;
                LinjeRepository.IncreaseSelected();

                if (!LinjeModel.Nod1.IsSelected)
                {
                    if (LinjeModel.Nod1.LinjeListModel.Count == 1 || LinjeModel.Nod1.LinjeListModel.AllIsSelected())
                    {
                        if (LinjeModel.Nod1 is NodCanvasModel)
                        {
                            NodCanvasModel n = LinjeModel.Nod1 as NodCanvasModel;
                            n.NodCanvasViewModel.Select();
                        }
                        else
                        {
                            NodKlassModel n = LinjeModel.Nod1 as NodKlassModel;
                            n.NodKlassViewModel.Select();
                        }
                    }
                }

                if (!LinjeModel.Nod2.IsSelected)
                {
                    if (LinjeModel.Nod2.LinjeListModel.Count == 1 || LinjeModel.Nod2.LinjeListModel.AllIsSelected())
                    {
                        if (LinjeModel.Nod2 is NodCanvasModel)
                        {
                            NodCanvasModel n = LinjeModel.Nod2 as NodCanvasModel;
                            n.NodCanvasViewModel.Select();
                        }
                        else
                        {
                            NodKlassModel n = LinjeModel.Nod2 as NodKlassModel;
                            n.NodKlassViewModel.Select();
                        }
                    }
                }
            }
        }

        public void Deselect()
        {
            if (IsSelected())
            {
                LinjeModel.IsSelected = false;
                LinjeRepository.DecreaseSelected();
            }
        }

        public bool IsSelected()
        {
            return (LinjeModel.IsSelected ? true : false);
        }
    }
}
