using RajdRed.Models.Base;
using RajdRed.Repositories.Base;
using RajdRed.ViewModels;
using System.Collections.Generic;

namespace RajdRed.Repositories
{
    public class LinjeRepository : BaseRepository<LinjeViewModel>
    {
        public LinjeRepository(MainRepository m) : base(m) { }
        public LinjeViewModel AddNewLinje(NodModelBase n1, NodModelBase n2)
        {
            LinjeViewModel lvm = new LinjeViewModel(this, n1, n2);

            n1.LinjeListModel.Add(lvm.LinjeModel);
            n2.LinjeListModel.Add(lvm.LinjeModel);

            Add(lvm);

            lvm.Select();

            return lvm;
        }

        // --------------------------------- Override Base ---------------------------------------- //

        public override void SelectAll()
        {
            if (NumberOfSelected != this.Count)
            {
                foreach (LinjeViewModel l in this)
                    l.Select();
            }
        }

        public override void DeselectAll()
        {
            if (HasSelected())
            {
                foreach (LinjeViewModel l in this)
                {
                    if (NumberOfSelected == 0)
                        break;
                        
                    l.Deselect();
                }
            }
        }

        public override void DeleteSelected()
        {
            int size = this.Count;
            List<LinjeViewModel> deleteEverythingInThisList = new List<LinjeViewModel>();

            for (int i = 0; i < size; i++)
                if (this[i].IsSelected())
                    deleteEverythingInThisList.Add(this[i]);

            foreach (LinjeViewModel lvm in deleteEverythingInThisList)
                lvm.Delete();
        }

        public override void Select(LinjeViewModel l)
        {
            l.Select();
        }

        public override void Deselect(LinjeViewModel l)
        {
            l.Deselect();
        }

        // -------------//------------------ Override Base END --------------//------------------------ //
    }
}
