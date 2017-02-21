using RajdRed.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RajdRed
{
    /// <summary>
    /// Interaction logic for ClassSettings.xaml
    /// </summary>
    public partial class ClassSettings : UserControl
    {
		private KlassViewModel _kvm;
		private Grid _backgroundGrid;

        public ClassSettings(KlassViewModel kvm, Grid g)
        {
            InitializeComponent();
			_kvm = kvm;
			_backgroundGrid = g;
			DataContext = _kvm.KlassModel;
		}

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Är du säker på att du vill ta bort \"" + ClassName.Text + "\"", "Konfirmera borttagning", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _kvm.KlassRepository.MainRepository.DeleteSelected();
				_kvm.KlassView.CloseSettings(this, _backgroundGrid);
            }
        }

        public void Btn_Abort_Click(object sender, RoutedEventArgs e)
        {
			_kvm.KlassView.CloseSettings(this, _backgroundGrid);
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void ClassSettings_Loaded(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            ScaleTransform sct = new ScaleTransform(0, 0);
            gridAnimate.RenderTransformOrigin = new Point(0.5, 0.5);
            gridAnimate.RenderTransform = sct;
            DoubleAnimation da = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(0.17)));
            sct.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            sct.BeginAnimation(ScaleTransform.ScaleYProperty, da);
        }

        public void Save()
        {
            BindingExpression classNameBE = BindingOperations.GetBindingExpression(ClassName, TextBox.TextProperty);
            BindingExpression attributesBE = BindingOperations.GetBindingExpression(Attributes, TextBox.TextProperty);
            BindingExpression methodsBE = BindingOperations.GetBindingExpression(Methods, TextBox.TextProperty);

            classNameBE.UpdateSource();
            attributesBE.UpdateSource();
            methodsBE.UpdateSource();

            _kvm.KlassView.CloseSettings(this, _backgroundGrid);
        }
    }
}
