
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Mehrisbookstore.ViewModel;

internal class MainWindowViewModel : ViewModelBase
{

    public PublisherViewModel PublisherViewModel { get; }
    public StoresViewModel StoresViewModel { get; }


    public MainWindowViewModel() //konstruktor
    {
        StoresViewModel = new StoresViewModel(this);
        PublisherViewModel = new PublisherViewModel(this);
    }

   
}
