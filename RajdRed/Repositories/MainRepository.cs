using RajdRed.Models;
using RajdRed.Models.Base;
using RajdRed.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RajdRed.Repositories
{
    public class MainRepository
    {
        //Använd till vad du vill...
        public Point TempPosition { get; set; }

        private MainWindow _mainWindow;
        public MainWindow MainWindow
        {
            get { return _mainWindow; }
        }
        
        private KlassRepository _klassRepository;
        public KlassRepository KlassRepository
        {
            get { return _klassRepository; }
        }

        private NodCanvasRepository _nodCanvasRepository;
        public NodCanvasRepository NodCanvasRepository
        {
            get { return _nodCanvasRepository; }
        }

        private LinjeRepository _linjeRepository;
        public LinjeRepository LinjeRepository
        {
            get { return _linjeRepository; }
        }

        private TextBoxRepository _textBoxRepository;
        public TextBoxRepository TextBoxRepository
        {
            get { return _textBoxRepository; }
        }
        
        public MainRepository(MainWindow mw)
        {
            _mainWindow = mw;
            _klassRepository = new KlassRepository(this);
            _linjeRepository = new LinjeRepository(this);
            _nodCanvasRepository = new NodCanvasRepository(this);
            _textBoxRepository = new TextBoxRepository(this);
        }

        public void SelectIfHit(Point mouseDownPos, Point mouseUpPos)
        {
			List<NodModelBase> nmbList = new List<NodModelBase>();

            /* kontroll för klasser innanför selection */
            KlassRepository.SelectIfHit(mouseDownPos, mouseUpPos, ref nmbList);

            /*Kontroll för canvasnoder inanför selection*/
            NodCanvasRepository.SelectIfHit(mouseDownPos, mouseUpPos, ref nmbList);

            /*Kontroll för canvasnoder inanför selection*/
            TextBoxRepository.SelectIfHit(mouseDownPos, mouseUpPos);

			SelectLinesOfNod(ref nmbList);
        }

        public void Select(RajdElement re)
        {
			if (re is KlassModel) {
                KlassModel k = re as KlassModel;
                _klassRepository.Select(k.KlassViewModel);
            }
			else if (re is LinjeModel) {
                LinjeModel l = re as LinjeModel;
                _linjeRepository.Select(l.LinjeViewModel);
            }
			else if (re is NodCanvasModel) {
                NodCanvasModel n = re as NodCanvasModel;
                _nodCanvasRepository.Select(n.NodCanvasViewModel);
            }
            else if (re is TextBoxModel)
            {
                TextBoxModel t = re as TextBoxModel;
                _textBoxRepository.Select(t.TextBoxViewModel);
            }
        }

        public void Deselect(RajdElement re)
        {
            if (re is KlassModel)
            {
                KlassModel k = re as KlassModel;
                _klassRepository.Deselect(k.KlassViewModel);
            }
            else if (re is LinjeModel)
            {
                LinjeModel l = re as LinjeModel;
                _linjeRepository.Deselect(l.LinjeViewModel);
            }
            else if (re is NodCanvasModel)
            {
                NodCanvasModel n = re as NodCanvasModel;
                _nodCanvasRepository.Deselect(n.NodCanvasViewModel);
            }
            else if (re is TextBoxModel)
            {
                TextBoxModel t = re as TextBoxModel;
                _textBoxRepository.Deselect(t.TextBoxViewModel);
            }
        }

        public bool HasSelected()
        {
            return (_klassRepository.HasSelected()
                || _linjeRepository.HasSelected()
                || _nodCanvasRepository.HasSelected()
                || _textBoxRepository.HasSelected() ? true : false);
        }

        public void DeselectAll()
        {
            if (HasSelected())
            {
                _klassRepository.DeselectAll();
                _linjeRepository.DeselectAll();
                _nodCanvasRepository.DeselectAll();
                _textBoxRepository.DeselectAll();
            }
        }

        public void DeleteSelected()
        {
            if (HasSelected())
            {
                _klassRepository.DeleteSelected();
                _linjeRepository.DeleteSelected();
                _nodCanvasRepository.DeleteSelected();
                _textBoxRepository.DeleteSelected();
            }
        }

		public void SelectLinesOfNod(ref List<NodModelBase> nmbList) 
		{
			List<LinjeModel> selectedLinjerList = new List<LinjeModel>();

			/* selektera alla linjer som hör till noden */
			foreach (NodModelBase n in nmbList)
			{
				foreach (LinjeModel l in n.LinjeListModel)
				{
					Select(l);
					selectedLinjerList.Add(l);
				}
			}

			foreach (LinjeModel li in selectedLinjerList)
			{
				if (li.Nod1.IsSelected && li.Nod2.IsSelected)
				{
					continue;
				}

				else if (!li.Nod1.IsSelected)
				{
					bool shouldNodBeSelected = true;

					foreach (LinjeModel lm in li.Nod1.LinjeListModel)
					{
						if (!lm.IsSelected)
						{
							shouldNodBeSelected = false;
							break;
						}
					}

					if (shouldNodBeSelected)
					{
						if (li.Nod1 is NodCanvasModel)
							Select(li.Nod1 as NodCanvasModel);
                        else
                        {
                            NodKlassModel n = li.Nod1 as NodKlassModel;
                            n.NodKlassViewModel.Select();
                        }
					}
				}

				else
				{
					bool shouldNodBeSelected = true;

					foreach (LinjeModel lm in li.Nod2.LinjeListModel)
					{
						if (!lm.IsSelected)
						{
							shouldNodBeSelected = false;
							break;
						}
					}

					if (shouldNodBeSelected)
						if (li.Nod2 is NodCanvasModel)
							Select(li.Nod2 as NodCanvasModel);
						else
                        {
                            NodKlassModel n = li.Nod2 as NodKlassModel;
                            n.NodKlassViewModel.Select();
                        }
				}
			}
		}

        public void DeselectLinesOfNod(ref List<NodModelBase> nmbList)
        {
            List<LinjeModel> selectedLinjerList = new List<LinjeModel>();

            /* Deselekterar alla linjer som hör till noden */
            foreach (NodModelBase n in nmbList)
            {
                foreach (LinjeModel l in n.LinjeListModel)
                {
                    Deselect(l);
                    selectedLinjerList.Add(l);
                }
            }

            foreach (LinjeModel li in selectedLinjerList)
            {
                if (!li.Nod1.IsSelected && !li.Nod2.IsSelected)
                {
                    continue;
                }

                else if (li.Nod1.IsSelected)
                {
                    bool shouldNodBeSelected = false;

                    foreach (LinjeModel lm in li.Nod1.LinjeListModel)
                    {
                        if (lm.IsSelected)
                        {
                            shouldNodBeSelected = true;
                            break;
                        }
                    }

                    if (!shouldNodBeSelected)
                    {
                        if (li.Nod1 is NodCanvasModel)
                            Deselect(li.Nod1 as NodCanvasModel);
                        else
                        {
                            NodKlassModel n = li.Nod1 as NodKlassModel;
                            n.NodKlassViewModel.Deselect();
                        }
                    }
                }

                else
                {
                    bool shouldNodBeSelected = false;

                    foreach (LinjeModel lm in li.Nod2.LinjeListModel)
                    {
                        if (lm.IsSelected)
                        {
                            shouldNodBeSelected = true;
                            break;
                        }
                    }

                    if (!shouldNodBeSelected)
                        if (li.Nod2 is NodCanvasModel)
                            Deselect(li.Nod2 as NodCanvasModel);
                        else
                        {
                            NodKlassModel n = li.Nod2 as NodKlassModel;
                            n.NodKlassViewModel.Deselect();
                        }
                }
            }
        }
    }
}
