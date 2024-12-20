using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
namespace Mehrisbookstore.Windows
{
    /// <summary>
    /// Interaction logic for EditQuantity.xaml
    /// </summary>
    public partial class EditQuantity : Window
    {
        public EditQuantity()
        {
            InitializeComponent();
            DataContext = (App.Current.MainWindow as MainWindow).DataContext;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9-]+").IsMatch(e.Text);
        }
    }
}
