using RajdRed.ViewModels;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RajdRed.Views
{
    /// <summary>
    /// Interaction logic for TextBoxView.xaml
    /// </summary>
    public partial class TextBoxView : UserControl
    {
        TextBoxViewModel TextBoxViewModel { get { return DataContext as TextBoxViewModel; } }
        private Point _posOnUserControlOnHit;
        public TextBoxView()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                TextBoxViewModel.SetView(this);
                args.Handled = true;
            };
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseCaptured)
            {
                Point p = e.GetPosition(Application.Current.MainWindow);

				if (!((p.Y - _posOnUserControlOnHit.Y) <= 100.5))
					SetValue(Canvas.TopProperty, p.Y - _posOnUserControlOnHit.Y);

				if (!((p.X - _posOnUserControlOnHit.X) <= 0.5))
					SetValue(Canvas.LeftProperty, p.X - _posOnUserControlOnHit.X);
            }

            e.Handled = true;
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

            e.Handled = true;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                TextBoxViewModel.Edit();
            }
            else
            {
                if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    TextBoxViewModel.TextBoxRepository.MainRepository.DeselectAll();
                }

                TextBoxViewModel.Select();
                _posOnUserControlOnHit = Mouse.GetPosition(this);
                CaptureMouse();
            }
                
            e.Handled = true;
        }

    }
}
