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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace P3.View
{
    /// <summary>
    /// Interaction logic for FutureNewsView.xaml
    /// </summary>
    public partial class FutureNewsView : UserControl
    {
        public FutureNewsView()
        {
            InitializeComponent();
            
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserControl_LostMouseCapture(object sender, MouseEventArgs e)
        {
            var window = Application.Current.Windows[1];
            if (window != null)
                window.Close();
        }
    }
}
