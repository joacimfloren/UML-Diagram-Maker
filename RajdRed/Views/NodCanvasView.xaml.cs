using RajdRed.Models;
using RajdRed.ViewModels;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace RajdRed.Views
{
    /// <summary>
    /// Interaction logic for NodCanvasView.xaml
    /// </summary>
    public partial class NodCanvasView : UserControl
    {
        NodCanvasViewModel NodCanvasViewModel { get { return DataContext as NodCanvasViewModel; } }
        Point _posOnUserControlOnHit;
        Point _startDragPosition;
        public NodCanvasView()
        {
            InitializeComponent();
            Loaded += (sender, eArgs) => {
                NodCanvasViewModel.SetNodCanvasView(this);
                if (!NodCanvasViewModel.NodCanvasModel.IsSet)
                {
                    CaptureMouse(); //Avkommenteras om/när man kan dra nod från klass   
                    Point p = Mouse.GetPosition(Application.Current.MainWindow);
                    _startDragPosition = new Point(p.X-NodCanvasViewModel.NodCanvasModel.Width, p.Y-NodCanvasViewModel.NodCanvasModel.Height);
					_posOnUserControlOnHit = Mouse.GetPosition(this);

                    Dispatcher.Invoke(new Action(() =>
                    {
                        NodCanvasViewModel.NodCanvasRepository.MainRepository.KlassRepository.ShowAllKlassNodes();
                    }));
                }

                eArgs.Handled = true;
            };
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _startDragPosition = _posOnUserControlOnHit = Mouse.GetPosition(this);

            Dispatcher.Invoke(new Action(() => {
                NodCanvasViewModel.NodCanvasRepository.MainRepository.KlassRepository.ShowAllKlassNodes();
            }));

            if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                NodCanvasViewModel.NodCanvasRepository.MainRepository.DeselectAll();
            }

            NodCanvasViewModel.Select();
            CaptureMouse();
            e.Handled = true;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseCaptured && IsLoaded)
            {
                Point p = e.GetPosition(Application.Current.MainWindow);

               //if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
               // {
               //     double dx = (p.X - _startDragPosition.X) * (p.X - _startDragPosition.X);
               //     double dy = (p.Y - _startDragPosition.Y) * (p.Y - _startDragPosition.Y);

               //     if (dx >= dy) {
               //         SetValue(Canvas.LeftProperty, p.X - _posOnUserControlOnHit.X);
               //         SetValue(Canvas.TopProperty, _startDragPosition.Y - _posOnUserControlOnHit.Y);
               //     }
               //     else
               //     {
               //         SetValue(Canvas.LeftProperty, _startDragPosition.X - _posOnUserControlOnHit.X);
               //         SetValue(Canvas.TopProperty, p.Y - _posOnUserControlOnHit.Y);
               //     }
               // }

               // else
                {
					if (!((p.Y - _posOnUserControlOnHit.Y) <= 100.5))
						SetValue(Canvas.TopProperty, p.Y - _posOnUserControlOnHit.Y);

					if (!((p.X - _posOnUserControlOnHit.X) <= 0.5))
						SetValue(Canvas.LeftProperty, p.X - _posOnUserControlOnHit.X);
                }
            }
        }


        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            NodCanvasViewModel.LookAndAttachCanvasNod(e.GetPosition(Application.Current.MainWindow));

            Dispatcher.Invoke(new Action(() =>
            {
                NodCanvasViewModel.NodCanvasRepository.MainRepository.KlassRepository.HideAllKlassNodes();
            }));

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                foreach(LinjeModel l in this.NodCanvasViewModel.NodCanvasModel.LinjeListModel)
                {
                    double x, y;
                    if(l.Nod1 == this.NodCanvasViewModel.NodCanvasModel)
                    {
                        
                        if((l.Nod1.PositionTop-l.Nod2.PositionTop) < 0)
                            y = l.Nod2.PositionTop - l.Nod1.PositionTop;
                        else
                            y = l.Nod1.PositionTop - l.Nod2.PositionTop;
                        if ((l.Nod1.PositionLeft - l.Nod2.PositionLeft) < 0)
                            x = l.Nod2.PositionLeft - l.Nod1.PositionLeft;
                        else
                            x = l.Nod1.PositionLeft - l.Nod2.PositionLeft;
                        if (x < y)
                        {
                            if ((l.Nod1.PositionLeft - l.Nod2.PositionLeft) < 0)
                                l.Nod1.PositionLeft = l.Nod1.PositionLeft + x;// l.Nod1.PositionLeft;
                            else
                                l.Nod1.PositionLeft = l.Nod1.PositionLeft - x;
                        }
                        else
                        {
                            if ((l.Nod1.PositionTop - l.Nod2.PositionTop) < 0)
                                l.Nod1.PositionTop = l.Nod1.PositionTop + y;// l.Nod1.PositionTop;
                            else
                                l.Nod1.PositionTop = l.Nod1.PositionTop - y;// l.Nod1.PositionTop;
                        }

                    }
                    else
                    { 
                        
                        if((l.Nod2.PositionTop-l.Nod1.PositionTop) < 0)
                            y = l.Nod1.PositionTop - l.Nod2.PositionTop;
                        else
                            y = l.Nod2.PositionTop - l.Nod1.PositionTop;
                        if ((l.Nod2.PositionLeft - l.Nod1.PositionLeft) < 0)
                            x = l.Nod1.PositionLeft - l.Nod2.PositionLeft;
                        else
                            x = l.Nod2.PositionLeft - l.Nod1.PositionLeft;
                        if (x < y)
                        {
                            if((l.Nod2.PositionLeft - l.Nod1.PositionLeft) < 0)
                                l.Nod2.PositionLeft = l.Nod2.PositionLeft + x;// l.Nod1.PositionLeft;
                            else
                                l.Nod2.PositionLeft = l.Nod2.PositionLeft - x;
                        }
                           
                        else
                        {
                            if((l.Nod2.PositionTop - l.Nod1.PositionTop) < 0)
                                l.Nod2.PositionTop = l.Nod2.PositionTop + y;// l.Nod1.PositionTop;
                            else
                                l.Nod2.PositionTop = l.Nod2.PositionTop - y;// l.Nod1.PositionTop;
                        }
                            
                    

                    }
                }
            }

            e.Handled = true;
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {

            //Content.Opacity = 1;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            //Content.Opacity = 0.1;
        }
    }
}
