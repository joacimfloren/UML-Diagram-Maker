using RajdRed.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RajdRed.ViewModels.Base
{
    public abstract class NodViewModelBase
    {
        public static Point CenterBetweenNodes(NodModelBase Nod1, NodModelBase Nod2)
        {
            double newNodePosX;
            double newNodePosY;
            if (Nod1.PositionLeft > Nod2.PositionLeft)
            {
                newNodePosX = Nod2.PositionLeft + (Nod1.PositionLeft - Nod2.PositionLeft) / 2;

                if (Nod1.PositionTop > Nod2.PositionTop)
                    newNodePosY = Nod2.PositionTop + (Nod1.PositionTop - Nod2.PositionTop) / 2;
                else
                    newNodePosY = Nod1.PositionTop + (Nod2.PositionTop - Nod1.PositionTop) / 2;
            }
            else
            {
                newNodePosX = Nod1.PositionLeft + (Nod2.PositionLeft - Nod1.PositionLeft) / 2;
                if (Nod1.PositionTop > Nod2.PositionTop)
                    newNodePosY = Nod2.PositionTop + (Nod1.PositionTop - Nod2.PositionTop) / 2;
                else
                    newNodePosY = Nod1.PositionTop + (Nod2.PositionTop - Nod1.PositionTop) / 2;
            }

            return new Point(newNodePosX, newNodePosY);
        }
    }
}
