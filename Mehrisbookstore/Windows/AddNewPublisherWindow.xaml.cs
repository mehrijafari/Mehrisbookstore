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
using System.Windows.Shapes;

namespace Mehrisbookstore.Windows
{
    /// <summary>
    /// Interaction logic for AddNewPublisherWindow.xaml
    /// </summary>
    public partial class AddNewPublisherWindow : Window
    {
        public AddNewPublisherWindow()
        {
            InitializeComponent();
            DataContext = (App.Current.MainWindow as MainWindow).DataContext;
        }
    }
}
