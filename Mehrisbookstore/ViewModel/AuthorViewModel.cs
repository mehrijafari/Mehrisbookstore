using Mehrisbookstore.Command;
using Mehrisbookstore.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace Mehrisbookstore.ViewModel;

internal class AuthorViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public ObservableCollection<Author> Authors { get; private set; }
    public ObservableCollection<OriginalBook> AuthorsBooks { get; set; }
    public AddAuthorWindow  AddAuthorWindow { get; set; }
    public EditAuthorWindow EditAuthorWindow { get; set; }
    public DelegateCommand OpenAddAuthorCommand { get; }
    public DelegateCommand CloseAddAuthorCommand { get; }
    public DelegateCommand AddNewAuthorCommand { get; }
    public DelegateCommand DeleteAuthorCommand { get; }
    public DelegateCommand OpenEditAuthorCommand { get; }
    public DelegateCommand CloseEditAuthorCommand { get; }
    public DelegateCommand SaveEditAuthorCommand { get; }

  


    private Author _newAuthor;

    public Author NewAuthor
    {
        get => _newAuthor;
        set 
        {
            _newAuthor = value;
            RaisePropertyChanged();
        }
    }
    public DateTime? BirthDateDateTime
    {
        get
        {
            if (NewAuthor?.BirthDate != null)
            {
                return new DateTime(
                    NewAuthor.BirthDate.Value.Year,
                    NewAuthor.BirthDate.Value.Month,
                    NewAuthor.BirthDate.Value.Day
                );
            }
            return null;
        }
        set
        {
            if (value != null)
            {
                NewAuthor.BirthDate = DateOnly.FromDateTime(value.Value);
            }
            else
            {
                NewAuthor.BirthDate = null;
            }

            RaisePropertyChanged();
        }
    }

    public DateTime? AuthorDateTime
    {
        get
        {
            if (AuthorBeingEdited?.BirthDate != null)
            {
                return new DateTime(
                    AuthorBeingEdited.BirthDate.Value.Year,
                    AuthorBeingEdited.BirthDate.Value.Month,
                    AuthorBeingEdited.BirthDate.Value.Day
                );
            }
            return null;
        }
        set
        {
            if (value != null)
            {
                AuthorBeingEdited.BirthDate = DateOnly.FromDateTime(value.Value);
            }
            else
            {
                AuthorBeingEdited.BirthDate = null;
            }

            RaisePropertyChanged();
        }
    }
    private Author _authorBeingEdited;

    public Author AuthorBeingEdited
    {
        get => _authorBeingEdited;
        set 
        {
            _authorBeingEdited = value;
            RaisePropertyChanged();
            RaisePropertyChanged("AuthorDateTime");
        }
    }


    private Author _selectedAuthor;

    public Author SelectedAuthor
    {
        get => _selectedAuthor;
        set 
        {
            _selectedAuthor = value;
            RaisePropertyChanged();
            LoadAuthorsBooks();
            RaisePropertyChanged("AuthorsBooks");
            DeleteAuthorCommand.RaiseCanExecuteChanged();
            OpenEditAuthorCommand.RaiseCanExecuteChanged();
        }
    }
    private Visibility _authorVisibility;

    public Visibility AuthorVisibility
    {
        get => _authorVisibility;
        set 
        {
            _authorVisibility = value;
            RaisePropertyChanged();
        }
    }


    public AuthorViewModel(MainWindowViewModel? mainWindowViewModel) //konstruktor
    {
        this._mainWindowViewModel = mainWindowViewModel;
        AuthorVisibility = Visibility.Hidden;
        OpenAddAuthorCommand = new DelegateCommand(OpenAddAuthor);
        CloseAddAuthorCommand = new DelegateCommand(CloseAddAuthor);
        AddNewAuthorCommand = new DelegateCommand(AddNewAuthor);
        DeleteAuthorCommand = new DelegateCommand(DeleteAuthor, CanDeleteAuthor);
        OpenEditAuthorCommand = new DelegateCommand(OpenEditAuthor, CanOpenEditAuthor);
        CloseEditAuthorCommand = new DelegateCommand(CloseEditAuthor);
        SaveEditAuthorCommand = new DelegateCommand(SaveEditAuthor);
        LoadAuthors();
    }
    
    
    private void SaveEditAuthor(object obj)
    {
        using var db = new MehrisbookstoreContext();

        var author = db.Authors.Find(SelectedAuthor.Id);

        if (author != null)
        {
            author.FirstName = AuthorBeingEdited.FirstName;
            author.LastName = AuthorBeingEdited.LastName;
            author.BirthDate = AuthorBeingEdited.BirthDate;

            db.SaveChanges();

            SelectedAuthor.FirstName = AuthorBeingEdited.FirstName;
            SelectedAuthor.LastName = AuthorBeingEdited.LastName;
            SelectedAuthor.BirthDate = AuthorBeingEdited.BirthDate;
        }

        LoadAuthors();
        RaisePropertyChanged("Authors");
        _mainWindowViewModel.TitlesViewModel.LoadAuthors();

        EditAuthorWindow.Close();
    }

    private void CloseEditAuthor(object obj)
    {
        EditAuthorWindow.Close();
    }

    private void OpenEditAuthor(object obj)
    {
        AuthorBeingEdited = new Author
        {
            Id = SelectedAuthor.Id,
            FirstName = SelectedAuthor.FirstName,
            LastName = SelectedAuthor.LastName,
            BirthDate = SelectedAuthor.BirthDate
        };
        EditAuthorWindow = new EditAuthorWindow();
        EditAuthorWindow.Show();
    }

    private bool CanOpenEditAuthor(object? arg)
    {
        if (SelectedAuthor != null) return true;
        else return false;
    }

    private bool CanDeleteAuthor(object? arg)
    {
        if (SelectedAuthor != null) return true;
        else return false;
       
    }

    private void DeleteAuthor(object obj)
    {
        using var db = new MehrisbookstoreContext();

        var result = MessageBox.Show($"Are you sure you want to delete author {SelectedAuthor.FirstName} {SelectedAuthor.LastName}?",
            "Confirm Deletion",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            db.Database.ExecuteSqlRaw("DELETE FROM AuthorBook WHERE [Author ID] = {0}", SelectedAuthor.Id);

            db.Authors.Remove(SelectedAuthor);
            db.SaveChanges();
            LoadAuthors();
            RaisePropertyChanged("Authors");
            
        }
    }

    private void AddNewAuthor(object obj)
    {
        using var db = new MehrisbookstoreContext();

        if (string.IsNullOrEmpty(NewAuthor.FirstName) || string.IsNullOrEmpty(NewAuthor.LastName))
        {
            MessageBox.Show("First name and last name can not be empty.");
            return;
        }

        db.Authors.Add(NewAuthor);
        db.SaveChanges();

        LoadAuthors();
        RaisePropertyChanged("Authors");

        _mainWindowViewModel.TitlesViewModel.LoadAuthors();

        AddAuthorWindow.Close();
    }

    private void CloseAddAuthor(object obj)
    {
        AddAuthorWindow.Close();
    }

    private void OpenAddAuthor(object obj)
    {
        NewAuthor = new Author();
        AddAuthorWindow = new AddAuthorWindow();
        AddAuthorWindow.Show();
    }

    private void LoadAuthors()
    {
        using var db = new MehrisbookstoreContext();

        Authors = new ObservableCollection<Author>(
            db.Authors.ToList()
        );

    }

    public void LoadAuthorsBooks()
    {
        using var db = new MehrisbookstoreContext();

        AuthorsBooks = new ObservableCollection<OriginalBook>(
        db.OriginalBooks
        .Where(ob => ob.Authors.Contains(SelectedAuthor))
        .ToList()
        );

        RaisePropertyChanged("AuthorsBooks");
    }
}


