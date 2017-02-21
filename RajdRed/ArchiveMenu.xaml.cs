using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.IO.Packaging;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;

namespace RajdRed
{
    /// <summary>
    /// Interaction logic for ArchiveMenu.xaml
    /// </summary>
    public partial class ArchiveMenu : UserControl
    {
        public ArchiveMenu()
        {
            InitializeComponent();
        }

		private void exitButton_click(object sender, RoutedEventArgs e)
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
					bool didISave = openSaveBox(mw);

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

		private void newButton_click(object sender, RoutedEventArgs e)
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
					bool didISave = openSaveBox(mw);

					if (didISave)
					{
						mw.TitleTextBox.Text = "Untitled";
						for (int i = 0; i < classlength; i++)
							mw._mainRepository.KlassRepository.RemoveAt(0);

						for (int i = 0; i < linjelength; i++)
							mw._mainRepository.LinjeRepository.RemoveAt(0);

						for (int i = 0; i < nodlength; i++)
							mw._mainRepository.NodCanvasRepository.RemoveAt(0);

						for (int i = 0; i < textlength; i++)
							mw._mainRepository.TextBoxRepository.RemoveAt(0);
					}
				}

				else if (messageBoxResult != MessageBoxResult.Cancel)
				{
					mw.TitleTextBox.Text = "Untitled";
					for (int i = 0; i < classlength; i++)
						mw._mainRepository.KlassRepository.RemoveAt(0);

					for (int i = 0; i < linjelength; i++)
						mw._mainRepository.LinjeRepository.RemoveAt(0);

					for (int i = 0; i < nodlength; i++)
						mw._mainRepository.NodCanvasRepository.RemoveAt(0);

					for (int i = 0; i < textlength; i++)
						mw._mainRepository.TextBoxRepository.RemoveAt(0);
				}
			}

			else
				if (mw.TitleTextBox.Text != "Untitled")
					mw.TitleTextBox.Text = "Untitled";

			mw.theCanvas.Children.Remove(this);
			mw.isArchiveMenuActive = false;
			mw.archiveMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);
		}

		private void saveButton_click(object sender, RoutedEventArgs e)
		{
			openSaveBox((MainWindow)Application.Current.MainWindow);
		}

		public bool openSaveBox(MainWindow mw)
		{
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.FileName = mw.TitleTextBox.Text; // Default file name
			dlg.DefaultExt = ".pdf"; // Default file extension
			dlg.Filter = "PDF documents (.pdf)|*.pdf"; // Filter files by extension

			// Show save file dialog box
			Nullable<bool> result = dlg.ShowDialog();

			// Process save file dialog box results
			if (result == true)
			{
				// Save document
				string filename = dlg.FileName;
				writeFile(filename);
				return true;
			}

			else return false;
		}

		private void writeFile(string filename)
		{
			MainWindow mw = (MainWindow)Application.Current.MainWindow;

			mw.theCanvas.Children.Remove(this);
			mw.isArchiveMenuActive = false;
			mw.archiveMenuBtn.SetCurrentValue(Control.BackgroundProperty, Brushes.Transparent);

			MemoryStream lMemoryStream = new MemoryStream();
			Package package = Package.Open(lMemoryStream, FileMode.Create);
			XpsDocument doc = new XpsDocument(package);
			XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
			writer.Write(mw.theCanvas);
			doc.Close();
			package.Close();

			var pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(lMemoryStream);
			PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, filename, 0);
		}
    }
}