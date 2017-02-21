using RajdRed.Models;
using RajdRed.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace RajdRed.Views
{
    /// <summary>
    /// Interaction logic for KlassView.xaml
    /// </summary>
    public partial class KlassView : UserControl
    {
        public KlassViewModel KlassViewModel { get { return DataContext as KlassViewModel; } }
        private Point _posOnUserControlOnHit;
        private Point _startPoint;
        private bool _isDown = false;
        private KlassViewModel _selectedElement = null;

        public KlassView()
        {
            InitializeComponent();
            Loaded += (sender, eArgs) =>
            {
                if (!KlassViewModel.KlassModel.OnField)
                {
                    CaptureMouse();
                    KlassViewModel.SetKlassView(this);
                    _posOnUserControlOnHit = new Point(ActualWidth / 2, ActualHeight / 2);
                    KlassViewModel.KlassModel.PositionLeft = KlassViewModel.KlassModel.PositionLeft - (ActualWidth / 2);
                    KlassViewModel.KlassModel.PositionTop = KlassViewModel.KlassModel.PositionTop - (ActualHeight / 2);

                    Cursor = Cursors.SizeAll;
                }

                eArgs.Handled = true;
            };
        }

        public void KlassGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            Keyboard.Focus(this);

            if (e.ClickCount == 2)
            {
                Grid g = new Grid() { Width = mw.theCanvas.ActualWidth, Height = mw.theCanvas.ActualHeight, Background = Brushes.Black, Opacity = 0.2 };
                Canvas.SetLeft(g, 0);
                Canvas.SetTop(g, 0);

                ClassSettings cs = new ClassSettings(KlassViewModel, g);
                g.MouseDown += (sendr, eventArgs) => { cs.Save(); };	//Skapar ett mouseDown-event för Grid g som anropar CloseSettings

                double x = (KlassViewModel.KlassModel.PositionLeft + _posOnUserControlOnHit.X - cs.Width / 2.33);
                double y = (KlassViewModel.KlassModel.PositionTop + _posOnUserControlOnHit.Y - cs.Height / 2);

                if (cs.Width + x > mw.ActualWidth)
                    Canvas.SetLeft(cs, x - (x + cs.Width - mw.ActualWidth));
                else if (x < 0)
                    Canvas.SetLeft(cs, x - x);
                else
                    Canvas.SetLeft(cs, x);

                if (cs.Height + y > mw.ActualHeight)
                    Canvas.SetTop(cs, y - (y + cs.Height - mw.ActualHeight));
                else if (y < 0)
                    Canvas.SetTop(cs, y - y);
                else
                    Canvas.SetTop(cs, y);

                mw.theCanvas.Children.Add(g);
                mw.theCanvas.Children.Add(cs);
            }
            else if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                mw.DeselectAll();
            }

            CaptureMouse();
            _posOnUserControlOnHit = Mouse.GetPosition(this);

            KlassViewModel.Select();

			e.Handled = true;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {

            if (IsMouseCaptured && KlassViewModel.IsSelected() && (KlassViewModel.KlassModel.Resize=="arrow"))
            {
                Point p = e.GetPosition(Application.Current.MainWindow);

                if (!(KlassViewModel.KlassModel.OnField && ((p.Y - _posOnUserControlOnHit.Y) <= 100.5)))
                    SetValue(Canvas.TopProperty, p.Y - _posOnUserControlOnHit.Y);

                if (!((p.X - _posOnUserControlOnHit.X) <= 0.5))
                    SetValue(Canvas.LeftProperty, p.X - _posOnUserControlOnHit.X);
            }
            if (KlassViewModel.KlassModel.Resize == "NWSE" && _isDown)
            {
                double _widthChange, _heightChange;
                Point pos = e.GetPosition(Application.Current.MainWindow);
                _widthChange = Math.Min((_startPoint.X - pos.X), (KlassViewModel.KlassModel.Width - KlassModel.MinSize));
                _heightChange = Math.Min((_startPoint.Y - pos.Y), (KlassViewModel.KlassModel.Height - KlassModel.MinSize));
                //_newWidth = _startPoint.X - pos.X;
                KlassViewModel.KlassModel.Width -= _widthChange;
                KlassViewModel.KlassModel.Height -= _heightChange;
                _startPoint = pos;

            }
            else if (KlassViewModel.KlassModel.Resize == "NS" && _isDown)
            {
                double _widthChange, _heightChange;
                Point pos = e.GetPosition(Application.Current.MainWindow);
                //_widthChange = Math.Min((_startPoint.X - pos.X), (KlassViewModel.KlassModel.Width - KlassModel.MinSize));
                _heightChange = Math.Min((_startPoint.Y - pos.Y), (KlassViewModel.KlassModel.Height - KlassModel.MinSize));
                //_newWidth = _startPoint.X - pos.X;
                //KlassViewModel.KlassModel.Width -= _widthChange;
                KlassViewModel.KlassModel.Height -= _heightChange;
                _startPoint = pos;

            }
            else if (KlassViewModel.KlassModel.Resize == "WE" && _isDown)
            {
                double _widthChange, _heightChange;
                Point pos = e.GetPosition(Application.Current.MainWindow);
                _widthChange = Math.Min((_startPoint.X - pos.X), (KlassViewModel.KlassModel.Width - KlassModel.MinSize));
                //_heightChange = Math.Min((_startPoint.Y - pos.Y), (KlassViewModel.KlassModel.Height - KlassModel.MinSize));
                //_newWidth = _startPoint.X - pos.X;
                KlassViewModel.KlassModel.Width -= _widthChange;
                //KlassViewModel.KlassModel.Height -= _heightChange;
                _startPoint = pos;

            }

            e.Handled = true;
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

            Point p = e.GetPosition(Application.Current.MainWindow);
            Point posOfUserControlOnCanvas = new Point();
            posOfUserControlOnCanvas.X = p.X - _posOnUserControlOnHit.X;
            posOfUserControlOnCanvas.Y = p.Y - _posOnUserControlOnHit.Y;

            if (posOfUserControlOnCanvas.Y <= 100 && !KlassViewModel.KlassModel.OnField)
                KlassViewModel.Delete();
            else
                KlassViewModel.KlassModel.OnField = true;

            _isDown = false;
            KlassViewModel.KlassModel.Resize = "arrow";
            e.Handled = true;
        }

		public void CloseSettings(ClassSettings cs, Grid g)
        {
			MainWindow mw = (MainWindow)Application.Current.MainWindow;
            mw.theCanvas.Children.Remove(cs);
			mw.theCanvas.Children.Remove(g);
        }

        private void InnerBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeAll;
            KlassViewModel.KlassModel.Resize = "arrow";
        }

        private void InnerBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                KlassViewModel.ShowNodes();
            }));
            _selectedElement = KlassViewModel;
            int boarder = KlassViewModel.OnSide(e.GetPosition(this));
            KlassViewModel.KlassModel.Resize = "arrow";
            switch(boarder)
            {
                case 1:
                    KlassViewModel.KlassModel.Resize = "arrow";
                    break;
                case 2:
                    KlassViewModel.KlassModel.Resize = "arrow";
                    break;
                case 3:
                    Cursor = Cursors.SizeNWSE;
                    KlassViewModel.KlassModel.Resize = "NWSE";
                    break;
                case 4:
                    Cursor = Cursors.SizeNS;
                    KlassViewModel.KlassModel.Resize = "NS";
                    break;
                case 5:
                    Cursor = Cursors.SizeWE;
                    KlassViewModel.KlassModel.Resize = "WE";
                    break;
                default:
                    break;
            }
            
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            _selectedElement = null;
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(KlassViewModel.KlassModel.Resize == "arrow"))
            {
                CaptureMouse();
                _startPoint = e.GetPosition(Application.Current.MainWindow);
                _isDown = true;
                Cursor = Cursors.Hand;
            }
            e.Handled = true;
        }

        private void InnerGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => {
                KlassViewModel.ShowNodes();
            }));
        }

        private void OuterGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                KlassViewModel.HideNodes();
            }));
        }
    }
}