using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using RajdRed.Models.Adds;
using RajdRed.Repositories;
using RajdRed.ViewModels;
using System.Collections.Generic;
using System.Windows.Shapes;
using RajdRed.Models;
using System.Reflection;

namespace RajdRed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
		private ArchiveMenu archiveMenu = new ArchiveMenu();
		private SettingsMenu settingsMenu = new SettingsMenu();
        public bool isArchiveMenuActive = false;
		public bool isSettingsMenuActive = false;
		public RajdColors Colors = new RajdColors(RajdColorScheme.Light);
		private Point mouseDownPos;

        public MainRepository _mainRepository;
		
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _mainRepository = new MainRepository(this);

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _mainRepository.DeselectAll();

			if (isArchiveMenuActive)
			{
				theCanvas.Children.Remove(archiveMenu);
				isArchiveMenuActive = false;
				archiveMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
			}

			if (isSettingsMenuActive)
			{
				theCanvas.Children.Remove(settingsMenu);
				isSettingsMenuActive = false;
				settingsMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
			}

            _mainRepository.KlassRepository.AddNewKlass(e.GetPosition(Application.Current.MainWindow));

            e.Handled = true;
        }

        private void Ellipse_MinimizeWindow(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
			minmaxImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/"
			 + Assembly.GetExecutingAssembly().GetName().Name
			 + ";component/"
			 + "Images/menu-max.jpg", UriKind.Absolute));
        }

        private void Ellipse_MaximizeWindow(object sender, MouseButtonEventArgs e)
        {
			if (WindowState == WindowState.Maximized)
			{
				WindowState = WindowState.Normal;
				minmaxImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/"
			 + Assembly.GetExecutingAssembly().GetName().Name
			 + ";component/"
			 + "Images/menu-max.jpg", UriKind.Absolute));
			}

			else
			{
				WindowState = WindowState.Maximized;
				minmaxImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/"
			 + Assembly.GetExecutingAssembly().GetName().Name
			 + ";component/"
			 + "Images/menu-max2.jpg", UriKind.Absolute));
			}
        }

        private void Ellipse_CloseWindow(object sender, MouseButtonEventArgs e)
        {
			MainWindow mw = (MainWindow)Application.Current.MainWindow;

			int classlength = mw._mainRepository.KlassRepository.Count;
			int linjelength = mw._mainRepository.LinjeRepository.Count;
			int nodlength = mw._mainRepository.NodCanvasRepository.Count;
			int textlength = mw._mainRepository.TextBoxRepository.Count;

			if (classlength > 0 || linjelength > 0 || nodlength > 0 || textlength > 0)
			{
				MessageBoxResult messageBoxResult = MessageBox.Show("Vill du spara dokumentet \"" + mw.TitleTextBox.Text + "\" till PDF?", "Save current?", System.Windows.MessageBoxButton.YesNoCancel);

				if (messageBoxResult == MessageBoxResult.Yes)
				{
					bool didISave = archiveMenu.openSaveBox(mw);

					if (didISave)
						Application.Current.Shutdown();
				}

				else if (messageBoxResult != MessageBoxResult.Cancel)
					Application.Current.Shutdown();
			}

			else
				Application.Current.Shutdown();

			mw.theCanvas.Children.Remove(this);
			mw.isArchiveMenuActive = false;
			mw.archiveMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
        }

        private void WindowDragAndMove(object sender, MouseButtonEventArgs e)
        {
			if (e.ClickCount == 2 && WindowState == WindowState.Maximized)
			{
				WindowState = WindowState.Normal;
				minmaxImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/"
			 + Assembly.GetExecutingAssembly().GetName().Name
			 + ";component/"
			 + "Images/menu-max.jpg", UriKind.Absolute));
			}

			else if (e.ClickCount == 2 && WindowState == WindowState.Normal)
			{
				WindowState = WindowState.Maximized;
				minmaxImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/"
			 + Assembly.GetExecutingAssembly().GetName().Name
			 + ";component/"
			 + "Images/menu-max2.jpg", UriKind.Absolute));
			}
            else DragMove();

            e.Handled = true;
        }

        private void Button_ArchiveMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
			if (!isArchiveMenuActive) 
			{
				if (isSettingsMenuActive)
				{
					theCanvas.Children.Remove(settingsMenu);
					isSettingsMenuActive = false;
					settingsMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
				}

				Canvas.SetLeft(archiveMenu, 0);
				Canvas.SetTop(archiveMenu, 100);
				theCanvas.Children.Add(archiveMenu);
				isArchiveMenuActive = true;

				archiveMenuBtn.SetCurrentValue(Control.BackgroundProperty, Colors.MenuButtonBg);
			}

			else 
			{
				theCanvas.Children.Remove(archiveMenu);
				isArchiveMenuActive = false;
				archiveMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
			}

            e.Handled = true;
        }

		private void theCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
            Keyboard.ClearFocus();
            this.Focus();
            Keyboard.Focus(this);

            /************  För selectionverktyget  ***************/
			if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
				_mainRepository.DeselectAll();

            mouseDownPos = e.GetPosition(theCanvas);

            if (mouseDownPos.Y > 100 && Mouse.Captured == null)
            {
                theCanvas.CaptureMouse();

                Canvas.SetLeft(selectionBox, mouseDownPos.X);
                Canvas.SetTop(selectionBox, mouseDownPos.Y);
                selectionBox.Width = 0;
                selectionBox.Height = 0;

                selectionBox.Visibility = Visibility.Visible;

                theCanvas.MouseMove += (sendr, eventArgs) =>
                {
                    Point mousePos = e.GetPosition(theCanvas);

                    if (mouseDownPos.X < mousePos.X)
                    {
                        Canvas.SetLeft(selectionBox, mouseDownPos.X);
                        selectionBox.Width = mousePos.X - mouseDownPos.X;
                    }
                    else
                    {
                        Canvas.SetLeft(selectionBox, mousePos.X);
                        selectionBox.Width = mouseDownPos.X - mousePos.X;
                    }

                    if (mouseDownPos.Y < mousePos.Y)
                    {
                        Canvas.SetTop(selectionBox, mouseDownPos.Y);
                        selectionBox.Height = mousePos.Y - mouseDownPos.Y;
                    }
                    else
                    {
                        Canvas.SetTop(selectionBox, mousePos.Y);
                        selectionBox.Height = mouseDownPos.Y - mousePos.Y;
                    }

                    eventArgs.Handled = true;
                };
            }

			if (isArchiveMenuActive)
			{
				theCanvas.Children.Remove(archiveMenu);
				isArchiveMenuActive = false;
				archiveMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
			}

			if (isSettingsMenuActive) 
			{
				theCanvas.Children.Remove(settingsMenu);
				isSettingsMenuActive = false;
				settingsMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
			}

            e.Handled = true;
		}

        private void theCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			theCanvas.ReleaseMouseCapture();
			selectionBox.Visibility = Visibility.Collapsed;

			Point mouseUpPos = e.GetPosition(theCanvas);

			/*Musen har släppts - Kolla om det är finns några element innanför mouseUpPos och mouseDownPos*/
			if (mouseDownPos.X > mouseUpPos.X)
			{
				double temp = mouseDownPos.X;
				mouseDownPos.X = mouseUpPos.X;
				mouseUpPos.X = temp;
			}

			if (mouseDownPos.Y > mouseUpPos.Y)
			{
				double temp = mouseDownPos.Y;
				mouseDownPos.Y = mouseUpPos.Y;
				mouseUpPos.Y = temp;
			}

            //Checks if intersect with RajdElements on Canvas
            _mainRepository.SelectIfHit(mouseDownPos, mouseUpPos);

            e.Handled = true;
		}

		private void Button_ArchiveMenu_MouseUp(object sender, MouseButtonEventArgs e)
		{
			Point pt = e.GetPosition(theCanvas);

			if ( (pt.X > 150 || pt.Y > 190) || (pt.X > 78 && pt.Y < 96)) 
			{
				theCanvas.Children.Remove(archiveMenu);
				isArchiveMenuActive = false;
				archiveMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
			}
		}

		private void Button_SettingsMenu_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (!isSettingsMenuActive) 
			{
				if (isArchiveMenuActive)
				{
					theCanvas.Children.Remove(archiveMenu);
					isArchiveMenuActive = false;
					archiveMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
				}

				Canvas.SetLeft(settingsMenu, theCanvas.ActualWidth - 150);
				Canvas.SetTop(settingsMenu, 100);
				theCanvas.Children.Add(settingsMenu);
				isSettingsMenuActive = true;

				settingsMenuBtn.SetCurrentValue(Control.BackgroundProperty, Colors.MenuButtonBg);
			}

			else 
			{
				theCanvas.Children.Remove(settingsMenu);
				isSettingsMenuActive = false;
				settingsMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
			}
		}

		private void Button_SettingsMenu_MouseUp(object sender, MouseButtonEventArgs e)
		{
			Point pt = e.GetPosition(theCanvas);

			if ((pt.X < theCanvas.ActualWidth - 150 || pt.Y > 190) || (pt.X < theCanvas.ActualWidth - 78 && pt.Y < theCanvas.ActualWidth - 96)) 
			{
				theCanvas.Children.Remove(settingsMenu);
				isSettingsMenuActive = false;
				settingsMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
			}
		}

		private void addClassButton_MouseEnter(object sender, MouseEventArgs e)
		{
			ThicknessAnimation animate = new ThicknessAnimation(new Thickness(0), TimeSpan.FromSeconds(0.1));
			addClassButton.BeginAnimation(Canvas.MarginProperty, animate);
		}

		private void addClassButton_MouseLeave(object sender, MouseEventArgs e)
		{
			ThicknessAnimation animate = new ThicknessAnimation(new Thickness(2), TimeSpan.FromSeconds(0.1));
			addClassButton.BeginAnimation(Canvas.MarginProperty, animate);
		}

		private void RajdRedMainWindow_KeyDown(object sender, KeyEventArgs k)
		{
			if (k.Key == Key.Delete || k.Key == Key.Back )
			{
				if (_mainRepository.HasSelected())
				{
                    _mainRepository.DeleteSelected();
				}
			}
		}

        public void DeselectAll()
        {
            _mainRepository.DeselectAll();
        }

        private void Line_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Line line = sender as Line;
            LinjeViewModel l = line.DataContext as LinjeViewModel;
            l.Split(e.GetPosition(this));

            e.Handled = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
           _mainRepository.TextBoxRepository.DeselectAll();
           _mainRepository.TextBoxRepository.AddNewTextBox();

           e.Handled = true;
        }

        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;

            e.Handled = true;
        }

        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            e.Handled = true;
        }

		private void theCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			Point pt = _mainRepository.TextBoxRepository.TempPosition = e.GetPosition(theCanvas);

			if (pt.Y > 101.0) {
				ContextMenu cm = new ContextMenu();

				cm.Items.Clear();
				MenuItem mi;
				MenuItem mi2;

				mi = new MenuItem();
				mi.Header = "Create Textbox / Multiplicity";
				mi.Click += new RoutedEventHandler(MenuItem_Click);
				mi2 = new MenuItem();
				mi2.Header = "Help";
				mi2.Click += new RoutedEventHandler(settingsMenu.HelpButton_Click);
				cm.Items.Add(mi);
				cm.Items.Add(mi2);
				cm.IsOpen = true;
			}
			
			if (isArchiveMenuActive)
			{
				theCanvas.Children.Remove(archiveMenu);
				isArchiveMenuActive = false;
				archiveMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
			}

			if (isSettingsMenuActive)
			{
				theCanvas.Children.Remove(settingsMenu);
				isSettingsMenuActive = false;
				settingsMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
			}

            e.Handled = true;
		}
    }
}