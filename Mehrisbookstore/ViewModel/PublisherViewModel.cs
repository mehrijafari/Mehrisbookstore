using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrisbookstore.ViewModel;

internal class PublisherViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;
    public ObservableCollection<string> Publishers { get; set; }

    public PublisherViewModel(MainWindowViewModel? mainWindowViewModel) //konstruktor
    {
        this.mainWindowViewModel = mainWindowViewModel;
        LoadPublishers();
    }
    private string? _selectedPublisher;

    public string? SelectedPublisher
    {
        get => _selectedPublisher;
        set 
        { 
            _selectedPublisher = value;
            RaisePropertyChanged();
        }
    }

    private void LoadPublishers()
    {
        using var db = new MehrisbookstoreContext();

        Publishers = new ObservableCollection<string>(
            db.Publishers.Select(p => p.NameOfPublisher).ToList()
        );

    }

    private void LoadPublisherInfo()
    {

    }
}
