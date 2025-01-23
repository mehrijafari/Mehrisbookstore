using Accessibility;
using Mehrisbookstore.Command;
using Mehrisbookstore.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mehrisbookstore.ViewModel;

internal class PublisherViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;
    public ObservableCollection<string> Publishers { get; set; }
    public AddNewPublisherWindow AddNewPublisherWindow { get; set; }
    public DelegateCommand OpenAddNewPulisherCommand { get; }
    public DelegateCommand CloseAddNewPublisherCommand { get; }
    public DelegateCommand AddNewPublisherCommand { get; }

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
    private Visibility _publisherVisibility;

    public Visibility PublisherVisibility
    {
        get => _publisherVisibility;
        set 
        {
            _publisherVisibility = value;
            RaisePropertyChanged();
        }
    }
    private Publisher _newPublisher;

    public Publisher NewPublisher
    {
        get => _newPublisher;
        set 
        {
            _newPublisher = value;
            RaisePropertyChanged();
        }
    }

    public PublisherViewModel(MainWindowViewModel? mainWindowViewModel) //konstruktor
    {
        this.mainWindowViewModel = mainWindowViewModel;
        LoadPublishers();
        PublisherVisibility = Visibility.Hidden;
        OpenAddNewPulisherCommand = new DelegateCommand(OpenAddNewPublisher);
        CloseAddNewPublisherCommand = new DelegateCommand(CloseAddNewPublisher);
        AddNewPublisherCommand = new DelegateCommand(AddNewPublisher);
    }

    private void AddNewPublisher(object obj)
    {
        using var db = new MehrisbookstoreContext();

        if (NewPublisher.NameOfPublisher == null)
        {
            MessageBox.Show("Name of publisher can not be empty");
            return;
        }

        db.Publishers.Add(NewPublisher);
        db.SaveChanges();

        LoadPublishers();
        RaisePropertyChanged("Publishers");
        mainWindowViewModel.BooksViewModel.LoadPublishers();
        mainWindowViewModel.BooksViewModel.LoadPublishersInEdit();
        AddNewPublisherWindow.Close();
    }

    private void CloseAddNewPublisher(object obj)
    {
        AddNewPublisherWindow.Close();
    }

    private void OpenAddNewPublisher(object obj)
    {
        NewPublisher = new Publisher();
        AddNewPublisherWindow = new AddNewPublisherWindow();
        AddNewPublisherWindow.Show();
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
