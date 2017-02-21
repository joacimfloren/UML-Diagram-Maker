using RajdRed.Models;
using RajdRed.Models.Base;
using RajdRed.Repositories.Base;
using RajdRed.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace RajdRed.Repositories
{
    public class NodCanvasRepository : BaseRepository<NodCanvasViewModel>
    {
        public NodCanvasRepository(MainRepository m) : base(m) { }
        public NodCanvasViewModel AddNewCanvasNod(Point p)
        {
            NodCanvasViewModel nkvm = new NodCanvasViewModel(p, this);
            Add(nkvm);
            nkvm.Select();

            return nkvm;
        }

        public NodCanvasViewModel CreateFromNodModelBase(NodKlassModel n)
        {
            NodCanvasViewModel nkvm = NodCanvasViewModel.CopyNodKlassToNew(n, this);
            Add(nkvm);

            return nkvm;
        }

        // --------------------------------- Override Base ---------------------------------------- //
        public override void Select(NodCanvasViewModel n)
        {
            n.Select();
        }

        public override void Deselect(NodCanvasViewModel n)
        {
            n.Deselect();
        }

        public override void SelectAll()
        {
            if (NumberOfSelected != this.Count)
            {
                foreach (NodCanvasViewModel n in this)
                    n.Select();
            }
        }

        public override void DeselectAll()
        {
            if (HasSelected())
            {
                foreach (NodCanvasViewModel n in this)
                {
                    if (n.IsSelected())
                    {
                        n.Deselect();
                    }
                }
            }
        }

        public override void DeleteSelected()
        {
            int size = this.Count;
            List<NodCanvasViewModel> deleteEverythingInThisList = new List<NodCanvasViewModel>();

            for (int i = 0; i < size; i++)
                if (this[i].NodCanvasModel.IsSelected)
                    deleteEverythingInThisList.Add(this[i]);

            foreach (NodCanvasViewModel ncvm in deleteEverythingInThisList)
                ncvm.Delete();
        }

        // -------------//------------------ Override Base END --------------//------------------------ //


		public void SelectIfHit(Point mouseDownPos, Point mouseUpPos, ref List<NodModelBase> nodList)
        {
			//Nummer 1 - Markera alla noder
            foreach (NodCanvasViewModel ncm in this)
            {
                Point leftTopCorner = new Point(ncm.NodCanvasModel.PositionLeft, ncm.NodCanvasModel.PositionTop);
                Point rightTopCorner = new Point(ncm.NodCanvasModel.PositionLeft + ncm.NodCanvasModel.Width, ncm.NodCanvasModel.PositionTop);
                Point leftBotCorner = new Point(ncm.NodCanvasModel.PositionLeft, ncm.NodCanvasModel.PositionTop + ncm.NodCanvasModel.Height);
                Point rightBotCorner = new Point(ncm.NodCanvasModel.PositionLeft + ncm.NodCanvasModel.Width, ncm.NodCanvasModel.PositionTop + ncm.NodCanvasModel.Height);

                if (rightTopCorner.X >= mouseDownPos.X && leftTopCorner.X <= mouseUpPos.X)
                {
                    if (rightBotCorner.Y >= mouseDownPos.Y && leftTopCorner.Y <= mouseUpPos.Y)
                    {
                        ncm.Select();
						nodList.Add(ncm.NodCanvasModel);
                    }
                }
            }
        }
    }
}
