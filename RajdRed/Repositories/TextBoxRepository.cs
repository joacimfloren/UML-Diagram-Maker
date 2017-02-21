using RajdRed.Models;
using RajdRed.Repositories.Base;
using RajdRed.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace RajdRed.Repositories
{
    public class TextBoxRepository : BaseRepository<TextBoxViewModel>
    {
        public TextBoxRepository(MainRepository m) : base(m) { }
        public TextBoxViewModel AddNewTextBox()
        {
            TextBoxViewModel tbvm = new TextBoxViewModel(TempPosition, this);
            Add(tbvm);
            tbvm.Select();

            return tbvm;
        }

        // --------------------------------- Override Base ---------------------------------------- //

        public override void Select(TextBoxViewModel t)
        {
            t.Select();
        }

        public override void Deselect(TextBoxViewModel t)
        {
            t.Deselect();
        }

        public override void DeselectAll()
        {
            if (HasSelected())
            {
                foreach (TextBoxViewModel t in this)
                    t.Deselect();
            }
        }

        public override void DeleteSelected()
        {
            int size = this.Count;
            List<TextBoxViewModel> deleteEverythingInThisList = new List<TextBoxViewModel>();

            for (int i = 0; i < size; i++)
                if (this[i].TextBoxModel.IsSelected)
                    deleteEverythingInThisList.Add(this[i]);

            foreach (TextBoxViewModel tbvm in deleteEverythingInThisList)
            {
                tbvm.Delete();
            }
        }

        // -------------//---------------- Override Base END --------------//------------------------ //

        public void SelectIfHit(Point mouseDownPos, Point mouseUpPos)
        {
            foreach (TextBoxViewModel tbvm in this)
            {
                Point leftTopCorner = new Point(tbvm.TextBoxModel.PositionLeft, tbvm.TextBoxModel.PositionTop);
                Point rightTopCorner = new Point(tbvm.TextBoxModel.PositionLeft + tbvm.TextBoxView.ActualWidth, tbvm.TextBoxModel.PositionTop);
                Point leftBotCorner = new Point(tbvm.TextBoxModel.PositionLeft, tbvm.TextBoxModel.PositionTop + tbvm.TextBoxView.ActualHeight);
                Point rightBotCorner = new Point(tbvm.TextBoxModel.PositionLeft + tbvm.TextBoxView.ActualWidth, tbvm.TextBoxModel.PositionTop + tbvm.TextBoxView.ActualHeight);
                if (rightTopCorner.X >= mouseDownPos.X && leftTopCorner.X <= mouseUpPos.X)
                {
                    if (rightBotCorner.Y >= mouseDownPos.Y && leftTopCorner.Y <= mouseUpPos.Y)
                    {
                        tbvm.Select();
                    }
                }
            }
        }
    }
}
