using RajdRed.Models;
using RajdRed.Repositories.Base;
using RajdRed.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace RajdRed.Repositories
{
    public class NodKlassRepository : BaseRepository<NodKlassViewModel>
    {
        public NodKlassRepository(MainRepository m, KlassViewModel kvm) : base(m)
        {
            //left
            for (int i = 1; i <= 3; i++)
                Add(new NodKlassViewModel(new NodKlassModel() {
                    Row = i,
                    Column = 0,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                }, kvm, this));

            //right
            for (int i = 1; i <= 3; i++)
                Add(new NodKlassViewModel(new NodKlassModel()
                {
                    Row = i,
                    Column = 5,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center
                }, kvm, this));

            //top
            for (int i = 1; i <= 3; i++)
                Add(new NodKlassViewModel(new NodKlassModel()
                {
                    Row = 0,
                    Column = i,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top
                }, kvm, this));

            //bottom
            for (int i = 1; i <= 3; i++)
                Add(new NodKlassViewModel(new NodKlassModel()
                {
                    Row = 5,
                    Column = i,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom
                }, kvm, this));
        }

        // --------------------------------- Override Base ---------------------------------------- //

        public override void Select(NodKlassViewModel t)
        {
            t.Select();
        }

        public override void Deselect(NodKlassViewModel t)
        {
            t.Deselect();
        }

        public override void SelectAll()
        {
            if (HasSelected())
            {
                foreach (NodKlassViewModel n in this)
                    n.Select();
            }
        }

        public override void DeselectAll()
        {
            if (HasSelected())
            {
                foreach (NodKlassViewModel n in this)
                    n.Deselect();
            }
        }

        // -------------//------------------ Override Base END --------------//------------------------ //
    }
}
