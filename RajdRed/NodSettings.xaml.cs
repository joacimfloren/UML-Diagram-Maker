using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RajdRed.Models.Adds;
using RajdRed.ViewModels;

namespace RajdRed
{
    /// <summary>
    /// Interaction logic for NodSettings.xaml
    /// </summary>
    public partial class NodSettings : UserControl
    {
        NodTypesModel ntm = new NodTypesModel();
        NodKlassViewModel NodKlassViewModel;
        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        Grid _backGroundGrid;
        Path tempPath;
        bool isClosed;
        public NodSettings(NodKlassViewModel nkvm, Grid g)
        {
            InitializeComponent();
            NodKlassViewModel = nkvm;
            _backGroundGrid = g;
            DataContext = ntm; 
        }

        private void Association_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NodKlassViewModel.TurnToAssosiation();
            mw.theCanvas.Children.Remove(_backGroundGrid);
            mw.theCanvas.Children.Remove(this);
            isClosed = true;
            
        }

        private void Aggregation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NodKlassViewModel.TurnToAggregation();
            mw.theCanvas.Children.Remove(_backGroundGrid);
            mw.theCanvas.Children.Remove(this);
            isClosed = true;
        }

        private void Composition_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NodKlassViewModel.TurnToComposition();
            mw.theCanvas.Children.Remove(_backGroundGrid);
            mw.theCanvas.Children.Remove(this);
            isClosed = true;
        }

        private void Generalization_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NodKlassViewModel.TurnToGeneralization();
            mw.theCanvas.Children.Remove(_backGroundGrid);
            mw.theCanvas.Children.Remove(this);
            isClosed = true;
        }

        private void Association_MouseEnter(object sender, MouseEventArgs e)
        {
            tempPath = NodKlassViewModel.NodKlassModel.Path;
            NodKlassViewModel.TurnToAssosiation();
            
        }

        private void Association_MouseLeave(object sender, MouseEventArgs e)
        {
            if(!isClosed)
                NodKlassViewModel.NodKlassModel.Path = tempPath;
        }

        private void Aggregation_MouseEnter(object sender, MouseEventArgs e)
        {
            tempPath = NodKlassViewModel.NodKlassModel.Path;
            NodKlassViewModel.TurnToAggregation();
        }

        private void Aggregation_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!isClosed)
                NodKlassViewModel.NodKlassModel.Path = tempPath;
        }

        private void Composition_MouseEnter(object sender, MouseEventArgs e)
        {
            tempPath = NodKlassViewModel.NodKlassModel.Path;
            NodKlassViewModel.TurnToComposition();
        }

        private void Composition_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!isClosed)
                NodKlassViewModel.NodKlassModel.Path = tempPath;
        }

        private void Generalization_MouseEnter(object sender, MouseEventArgs e)
        {
            tempPath = NodKlassViewModel.NodKlassModel.Path;
            NodKlassViewModel.TurnToGeneralization();
        }

        private void Generalization_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!isClosed)
                NodKlassViewModel.NodKlassModel.Path = tempPath;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ScaleTransform sct = new ScaleTransform(0, 0);
            this.RenderTransformOrigin = new Point(0.5, 0.5);
            this.RenderTransform = sct;
            DoubleAnimation da = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(0.17)));
            sct.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            sct.BeginAnimation(ScaleTransform.ScaleYProperty, da);
        }

    }
}
