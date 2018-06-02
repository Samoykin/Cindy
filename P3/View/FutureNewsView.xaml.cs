namespace P3.View
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>Interaction logic for FutureNewsView.xaml.</summary>
    public partial class FutureNewsView : UserControl
    {
        /// <summary>Initializes a new instance of the <see cref="FutureNewsView" /> class.</summary>
        public FutureNewsView()
        {
            this.InitializeComponent();            
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {            
        }

        private void UserControl_LostMouseCapture(object sender, MouseEventArgs e)
        {
            var window = Application.Current.Windows[1];
            if (window != null)
            {
                window.Close();
            }
        }
    }
}