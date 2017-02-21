using RajdRed.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RajdRed.Views
{
    /// <summary>
    /// Interaction logic for KlassNodView.xaml
    /// </summary>
    public partial class NodKlassView : UserControl
    {
        NodKlassViewModel NodKlassViewModel { get { return DataContext as NodKlassViewModel; } }
        public NodKlassView()
        {
            InitializeComponent();
            Loaded += (sender, eArgs) =>
            {
                if (NodKlassViewModel != null)
                {
                    NodKlassViewModel.SetView(this);
                }
            };
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                NodKlassViewModel.NodKlassRepository.MainRepository.DeselectAll();
            }

            NodKlassViewModel.NodKlassModel.IsPressed = true;
            
            if (NodKlassViewModel.IsSet())
                NodKlassViewModel.Select();
            
            e.Handled = true;
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (NodKlassViewModel.NodKlassModel.IsSet)
            {
                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                Grid g = new Grid() { Width = mw.theCanvas.ActualWidth, Height = mw.theCanvas.ActualHeight, Background = Brushes.Black, Opacity = 0 };
                Canvas.SetLeft(g, 0);
                Canvas.SetTop(g, 0);
                
                NodSettings ns = new NodSettings(NodKlassViewModel, g);
                Point pt = this.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
                
                double x = (pt.X + Width/2 - ns.Width / 2);
                double y = (pt.Y + Height/2 - ns.Height / 2);

                if (ns.Width + x > mw.ActualWidth)
                    Canvas.SetLeft(ns, x - (x + ns.Width - mw.ActualWidth));
                else if (x < 0)
                    Canvas.SetLeft(ns, x - x);
                else
                    Canvas.SetLeft(ns, x);

                if (ns.Height + y > mw.ActualWidth)
                    Canvas.SetTop(ns, y - (y + ns.Height - mw.ActualHeight));
                else if (y < 0)
                    Canvas.SetTop(ns, y - y);
                else
                    Canvas.SetTop(ns, y);
                
                

                g.MouseDown += (sendr, eventArgs) =>
                {
                    mw.theCanvas.Children.Remove(g);
                    mw.theCanvas.Children.Remove(ns);
                };

                mw.theCanvas.Children.Add(g);
                mw.theCanvas.Children.Add(ns);
                
            }
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NodKlassViewModel.NodKlassModel.IsPressed = false;
        }

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{
            if (!NodKlassViewModel.NodKlassModel.IsSet)
            {
                NodKlassViewModel.TurnToAssosiation();
            }
		}

		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
            if (NodKlassViewModel.NodKlassModel.IsPressed)
            {
                if (!NodKlassViewModel.NodKlassModel.IsSet)
                {
                    //Skapa ny linje
                    NodKlassViewModel.CreateLinje(e.GetPosition(Application.Current.MainWindow));
                }
                else
                {
                    //Lossa linje
                    NodKlassViewModel.LooseLinje(e.GetPosition(Application.Current.MainWindow));
                }

                NodKlassViewModel.NodKlassModel.IsPressed = false;
            }
            else if (!NodKlassViewModel.NodKlassModel.IsSet)
            {
                NodKlassViewModel.TurnToNode();
            }
		}              
    }
}
