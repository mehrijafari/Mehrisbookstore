using Mehrisbookstore.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Mehrisbookstore.Windows
{
    /// <summary>
    /// Interaction logic for AddNewBookWindow.xaml
    /// </summary>
    public partial class AddNewBookWindow : Window
    {
        public AddNewBookWindow()
        {
            InitializeComponent();
            DataContext = (App.Current.MainWindow as MainWindow).DataContext;
        }

        private void PriceInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void PagesInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void ISBN13Input(object sender, TextCompositionEventArgs e)
        {
           e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
