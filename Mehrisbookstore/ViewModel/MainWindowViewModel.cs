
using Mehrisbookstore.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Input;

namespace Mehrisbookstore.ViewModel;

internal class MainWindowViewModel : ViewModelBase
{

    public PublisherViewModel PublisherViewModel { get; }
    public StoresViewModel StoresViewModel { get; }
    public AuthorViewModel AuthorViewModel { get; }
    public TitlesViewModel TitlesViewModel { get; }
    public BooksViewModel BooksViewModel { get; }
    public GenreViewModel GenreViewModel { get; }
    public DelegateCommand ShowAuthorViewCommand { get; }
    public DelegateCommand ShowStoresViewCommand { get; }
    public DelegateCommand ShowTitlesViewCommand { get; }
    public DelegateCommand ShowBooksViewCommand { get; }
    public DelegateCommand ShowPublisherViewCommand { get; }
    public MainWindowViewModel() //konstruktor
    {
        StoresViewModel = new StoresViewModel(this);
        PublisherViewModel = new PublisherViewModel(this);
        AuthorViewModel = new AuthorViewModel(this);
        TitlesViewModel = new TitlesViewModel(this);
        BooksViewModel = new BooksViewModel(this);
        GenreViewModel = new GenreViewModel(this);
        ShowAuthorViewCommand = new DelegateCommand(ShowAuthorView);
        ShowStoresViewCommand = new DelegateCommand(ShowStoresView);
        ShowTitlesViewCommand = new DelegateCommand(ShowTitlesView);
        ShowBooksViewCommand = new DelegateCommand(ShowBooksView);
        ShowPublisherViewCommand = new DelegateCommand(ShowPublisherView);
       
    }

    private void ShowPublisherView(object obj)
    {
        AuthorViewModel.AuthorVisibility = Visibility.Hidden;
        StoresViewModel.StoresVisibility = Visibility.Hidden;
        TitlesViewModel.TitlesVisibility = Visibility.Hidden;
        BooksViewModel.BooksVisibility = Visibility.Hidden;
        PublisherViewModel.PublisherVisibility = Visibility.Visible;
    }

    private void ShowBooksView(object obj)
    {
        AuthorViewModel.AuthorVisibility = Visibility.Hidden;
        StoresViewModel.StoresVisibility = Visibility.Hidden;
        TitlesViewModel.TitlesVisibility = Visibility.Hidden;
        PublisherViewModel.PublisherVisibility = Visibility.Hidden;
        BooksViewModel.BooksVisibility = Visibility.Visible;
    }

    private void ShowTitlesView(object obj)
    {
        AuthorViewModel.AuthorVisibility = Visibility.Hidden;
        StoresViewModel.StoresVisibility = Visibility.Hidden;
        BooksViewModel.BooksVisibility = Visibility.Hidden;
        PublisherViewModel.PublisherVisibility = Visibility.Hidden;
        TitlesViewModel.TitlesVisibility = Visibility.Visible;
    }

    private void ShowStoresView(object obj)
    {
        AuthorViewModel.AuthorVisibility = Visibility.Hidden;
        TitlesViewModel.TitlesVisibility = Visibility.Hidden;
        BooksViewModel.BooksVisibility = Visibility.Hidden;
        PublisherViewModel.PublisherVisibility = Visibility.Hidden;
        StoresViewModel.StoresVisibility = Visibility.Visible;
    }

    private void ShowAuthorView(object obj)
    {
        StoresViewModel.StoresVisibility = Visibility.Hidden;
        TitlesViewModel.TitlesVisibility = Visibility.Hidden;
        BooksViewModel.BooksVisibility = Visibility.Hidden;
        PublisherViewModel.PublisherVisibility = Visibility.Hidden;
        AuthorViewModel.AuthorVisibility = Visibility.Visible;
    }
}
