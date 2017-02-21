using RajdRed.Models;
using RajdRed.Models.Base;
using RajdRed.Repositories.Base;
using RajdRed.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace RajdRed.Repositories
{
    public class KlassRepository : BaseRepository<KlassViewModel>
    {
        public KlassRepository(MainRepository m) : base(m) { }
        public KlassViewModel AddNewKlass(Point startPosition)
        {
            KlassViewModel kvm = new KlassViewModel(startPosition, this);
            Add(kvm);
            kvm.Select();

            return kvm;
        }

        // --------------------------------- Override Base ---------------------------------------- //

        public override void Select(KlassViewModel k)
        {
            List<NodModelBase> nodList = new List<NodModelBase>();

            foreach (NodKlassViewModel n in k.NodKlassRepository)
            {
                if (n.IsSet())
                {
                    n.Select();
                    nodList.Add(n.NodKlassModel);
                }
            }

            MainRepository.SelectLinesOfNod(ref nodList);
            k.Select();
        }

        public override void Deselect(KlassViewModel k)
        {
            List<NodModelBase> nodList = new List<NodModelBase>();

            foreach (NodKlassViewModel n in k.NodKlassRepository)
            {
                if (n.IsSet())
                {
                    n.Deselect();
                    nodList.Add(n.NodKlassModel);
                }
            }

            MainRepository.DeselectLinesOfNod(ref nodList);
            k.Deselect();
        }

        public override void DeselectAll()
        {
            if (HasSelected() || HasNodeSelected())
            {
                foreach (KlassViewModel k in this)
                {
                    if (k.KlassModel.IsSelected)
                    {
                        k.Deselect();
                    }

                    if (k.NodKlassRepository.HasSelected())
                    {
                        foreach (NodKlassViewModel nvkm in k.NodKlassRepository)
                        {
                            if (k.NodKlassRepository.NumberOfSelected == 0)
                                break;

                            nvkm.Deselect();
                        }
                    }
                }
            }
        }

        public override void SelectAll()
        {
            if (NumberOfSelected != this.Count)
            {
                foreach (KlassViewModel k in this)
                    k.Select();
            }
        }

        public override void DeleteSelected()
        {
            int size = this.Count;
            List<KlassViewModel> deleteEverythingInThisList = new List<KlassViewModel>();

            for (int i = 0; i < size; i++)
                if (this[i].KlassModel.IsSelected)
                    deleteEverythingInThisList.Add(this[i]);

            foreach (KlassViewModel kvm in deleteEverythingInThisList)
            {
                kvm.Delete();
            }
        }

        // -------------//------------------ Override Base END --------------//------------------------ //

        public bool HasNodeSelected()
        {
            foreach (KlassViewModel k in this) {
                if (k.NodKlassRepository.HasSelected())
                    return true;
            }

            return false;
        }

		public void SelectIfHit(Point mouseDownPos, Point mouseUpPos, ref List<NodModelBase> nodList)
        {
            foreach (KlassViewModel kvm in this)
            {
                Point leftTopCorner = new Point(kvm.KlassModel.PositionLeft, kvm.KlassModel.PositionTop);
                Point rightTopCorner = new Point(kvm.KlassModel.PositionLeft + kvm.KlassModel.Width, kvm.KlassModel.PositionTop);
                Point leftBotCorner = new Point(kvm.KlassModel.PositionLeft, kvm.KlassModel.PositionTop + kvm.KlassModel.Height);
                Point rightBotCorner = new Point(kvm.KlassModel.PositionLeft + kvm.KlassModel.Width, kvm.KlassModel.PositionTop + kvm.KlassModel.Height);

                if (rightTopCorner.X >= mouseDownPos.X && leftTopCorner.X <= mouseUpPos.X)
                {
                    if (rightBotCorner.Y >= mouseDownPos.Y && leftTopCorner.Y <= mouseUpPos.Y)
                    {
                        kvm.Select();

						foreach (NodKlassViewModel nod in kvm.NodKlassRepository)
						{
							if (nod.NodKlassModel.IsSet)
							{
                                nod.Select();
								nodList.Add(nod.NodKlassModel);
							}
						}
                    }
                }
            }
        }

        public void ShowAllKlassNodes()
        {
            foreach (KlassViewModel k in this)
                k.ShowNodes();
        }

        public void HideAllKlassNodes()
        {
            foreach (KlassViewModel k in this)
                k.HideNodes();
        }

        public KlassViewModel GetKlassByPoint(Point p)
        {
            foreach (KlassViewModel k in this) {
                if (k.KlassModel.PositionLeft <= p.X && k.KlassModel.PositionTop <= p.Y
                    && (k.KlassModel.PositionLeft + k.KlassModel.Width) >= p.X
                    && (k.KlassModel.PositionTop + k.KlassModel.Height) >= p.Y)
                {
                    return k;
                }
            }

            return null;
        }
    }
}
