using Mehrisbookstore.Command;
using Mehrisbookstore.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace Mehrisbookstore.ViewModel;

internal class TitlesViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    public AddNewTitleWindow AddNewTitleWindow { get; set; }
    public EditTitleWindow EditTitleWindow { get; set; }
    public ObservableCollection<Author> AuthorsInOriginalBook { get; set; }
    public ObservableCollection<Author> AuthorsToAddInEdit { get; set; }
    public DelegateCommand OpenEditTitleCommand { get; }
    public DelegateCommand CloseEditTitleCommand { get; }
    public DelegateCommand EditTitleCommand { get; }
    public DelegateCommand AddAuthorToExistingTitleCommand { get; }
    public DelegateCommand RemoveAuthorInExistingTitleCommand { get; }
    public DelegateCommand OpenAddNewTitleCommand { get; }
    public DelegateCommand CloseAddNewTitleCommand { get; }
    public DelegateCommand AddNewOriginalBookCommand { get; }
    public DelegateCommand AddAuthorCommand { get; }
    public ObservableCollection<OriginalBook> OriginalBooks { get; set; }
    public ObservableCollection<Author> AuthorsToAdd { get; set; }
    public ObservableCollection<Author> Authors { get; set; }

    private OriginalBook _selectedOriginalBook;

    public OriginalBook SelectedOriginalBook
    {
        get => _selectedOriginalBook;
        set 
        {
            _selectedOriginalBook = value;
            RaisePropertyChanged();
            OpenEditTitleCommand.RaiseCanExecuteChanged();
        }
    }

    private OriginalBook _originalBookBeingEdited;

    public OriginalBook OriginalBookBeingEdited
    {
        get => _originalBookBeingEdited;
        set 
        {
            _originalBookBeingEdited = value;
            RaisePropertyChanged();
        }
    }


    private Visibility _titlesVisibility;

    public Visibility TitlesVisibility
    {
        get => _titlesVisibility;
        set 
        {
            _titlesVisibility = value;
            RaisePropertyChanged();
        }
    }

    private OriginalBook _newOriginalBook;

    public OriginalBook NewOriginalBook
    {
        get => _newOriginalBook;
        set 
        {
            _newOriginalBook = value;
            RaisePropertyChanged();
        }
    }

    private Author _authorToAdd;

    public Author AuthorToAdd
    {
        get => _authorToAdd;
        set 
        {
            _authorToAdd = value;
            RaisePropertyChanged();
        }
    }

    private Author _authorToAddInEdit;

    public Author AuthorToAddInEdit
    {
        get => _authorToAddInEdit;
        set 
        {
            _authorToAddInEdit = value;
            RaisePropertyChanged();
        }
    }

    private Author _authorToRemove;

    public Author AuthorToRemove
    {
        get => _authorToRemove;
        set 
        {
            _authorToRemove = value;
            RaisePropertyChanged();
        }
    }

    public TitlesViewModel(MainWindowViewModel mainWindowViewModel) //konstruktor
    {
        this._mainWindowViewModel = mainWindowViewModel;
        TitlesVisibility = Visibility.Hidden;
        OpenAddNewTitleCommand = new DelegateCommand(OpenAddNewTitle);
        CloseAddNewTitleCommand = new DelegateCommand(CloseAddNewTitle);
        AddNewOriginalBookCommand = new DelegateCommand(AddNewOriginalBook);
        AddAuthorCommand = new DelegateCommand(AddAuthor);
        OpenEditTitleCommand = new DelegateCommand(OpenEditTitle, CanOpenEditTitle);
        AddAuthorToExistingTitleCommand = new DelegateCommand(AddAuthorToExistingTitle);
        RemoveAuthorInExistingTitleCommand = new DelegateCommand(RemoveAuthorInExistingTitle);
        CloseEditTitleCommand = new DelegateCommand(CloseEditTitle);
        EditTitleCommand = new DelegateCommand(EditTitle);
        LoadOriginalBooks();
    }
 
    private void EditTitle(object obj)
    {
        using var db = new MehrisbookstoreContext();

        if (string.IsNullOrEmpty(OriginalBookBeingEdited.OriginalTitle))
        {
            MessageBox.Show("Original title can not be empty.");
            return;
        }
        if (!AuthorsInOriginalBook?.Any() ?? true)
        {
            MessageBox.Show("Authors can not be empty, please add at least one author.");
            return;
        }

        foreach (var author in AuthorsInOriginalBook) //kollar om författarna man ska lägga till redan har boken eller inte
        {
            var authorBooks = db.Authors
                .Include(a => a.OriginalBooks)
                .FirstOrDefault(a => a.Id == author.Id);

            if (authorBooks.OriginalBooks.Any(b => b.Id == OriginalBookBeingEdited.Id)) //om författaren har boken fortsätt loopen
            {
                continue;
            }
            else //om författaren inte har boken, lägg till boken
            {
                db.Database.ExecuteSqlRaw("INSERT INTO AuthorBook values({0}, {1})", author.Id, OriginalBookBeingEdited.Id);
            }
           
        }

        foreach (var author in AuthorsToAddInEdit) //kollar om författarna man INTE ska lägga till har boken eller inte
        {
            var authorBooks = db.Authors
                .Include(a => a.OriginalBooks)
                .FirstOrDefault(a => a.Id == author.Id);

            if (authorBooks.OriginalBooks.Any(b => b.Id == OriginalBookBeingEdited.Id)) //om författaren HAR boken, ta bort boken
            {
                db.Database.ExecuteSqlRaw("DELETE FROM AuthorBook WHERE [Author ID] = {0} AND OriginalBookID = {1}", author.Id, OriginalBookBeingEdited.Id);
            }
            else //om författaren inte har boken, fortsätt loopen
            {
                continue;
            }
        }

        var originalBook = db.OriginalBooks.Find(OriginalBookBeingEdited.Id);

        if (originalBook != null)
        {
            originalBook.OriginalTitle = OriginalBookBeingEdited.OriginalTitle;

            db.SaveChanges();

            SelectedOriginalBook.OriginalTitle = OriginalBookBeingEdited.OriginalTitle;

            var booksToEdit = db.Books
                .Where(b => b.OriginalTitleId != null)
                .Include(b => b.OriginalTitle)
                .ToList();

            foreach (var book in booksToEdit)
            {
                book.Title = book.OriginalTitle.OriginalTitle;
            }

            db.SaveChanges();
        }

        LoadOriginalBooks();
        _mainWindowViewModel.AuthorViewModel.LoadAuthorsBooks();
        _mainWindowViewModel.StoresViewModel.LoadBooksInSelectedStore();
        _mainWindowViewModel.StoresViewModel.LoadPossibleBooksToAdd();
        _mainWindowViewModel.StoresViewModel.LoadStoreInventory();
        _mainWindowViewModel.BooksViewModel.LoadBooks();

        EditTitleWindow.Close();
    }
    private void CloseEditTitle(object obj)
    {
        EditTitleWindow.Close();
    }
    private void RemoveAuthorInExistingTitle(object obj) 
    {
        AuthorsToAddInEdit.Add(AuthorToRemove);
        AuthorsInOriginalBook.Remove(AuthorToRemove);
        AuthorToAddInEdit = AuthorsToAddInEdit.FirstOrDefault();
    }
    private void AddAuthorToExistingTitle(object obj) 
    {
        AuthorsInOriginalBook.Add(AuthorToAddInEdit);
        AuthorsToAddInEdit.Remove(AuthorToAddInEdit);
        AuthorToAddInEdit = AuthorsToAddInEdit.FirstOrDefault();
    }
    private bool CanOpenEditTitle(object? arg)
    {
        if (SelectedOriginalBook != null) return true;
        else return false;
    }
    private void OpenEditTitle(object obj)
    {
        OriginalBookBeingEdited = new OriginalBook
        {
            Id = SelectedOriginalBook.Id,
            OriginalTitle = SelectedOriginalBook.OriginalTitle
        };
        LoadAuthorsInOriginalBook();
        LoadAuthorsToAddInEdit();
        EditTitleWindow = new EditTitleWindow();
        EditTitleWindow.Show();
    }
    private void AddAuthor(object obj) 
    {
        if (Authors == null)
        {
            Authors = new ObservableCollection<Author>();
        }
        Authors.Add(AuthorToAdd);
        AuthorsToAdd.Remove(AuthorToAdd);
        AuthorToAdd = AuthorsToAdd.FirstOrDefault();
        RaisePropertyChanged("Authors");

        
    }
    private void AddNewOriginalBook(object obj) 
    {
        using var db = new MehrisbookstoreContext();

        if (NewOriginalBook.OriginalTitle == null || Authors == null)
        {
            MessageBox.Show("Title and authors can not be empty");
            return;
        }

        db.OriginalBooks.Add(NewOriginalBook);
        db.SaveChanges();

        foreach (var author in Authors)
        {
             db.Database.ExecuteSqlRaw("INSERT INTO AuthorBook values({0}, {1})", author.Id, NewOriginalBook.Id);
        }

        LoadOriginalBooks(); 
        RaisePropertyChanged("OriginalBooks");

        _mainWindowViewModel.BooksViewModel.LoadOriginalBooks();

        Authors = null;
        AddNewTitleWindow.Close();
    }
    private void CloseAddNewTitle(object obj)
    {
        Authors = null;
        AddNewTitleWindow.Close();
       
    }
    private void OpenAddNewTitle(object obj)
    {
        LoadAuthors();
        Authors = null;
        NewOriginalBook = new OriginalBook();
        AddNewTitleWindow = new AddNewTitleWindow();
        AddNewTitleWindow.Show();
    }
    private void LoadOriginalBooks()
    {
        using var db = new MehrisbookstoreContext();

        var originalbookslist = db.OriginalBooks.ToList();

        OriginalBooks = new ObservableCollection<OriginalBook>(originalbookslist);

        RaisePropertyChanged("OriginalBooks");
        
    }
    public void LoadAuthors()
    {
        using var db = new MehrisbookstoreContext();
      
        var AuthorsToAddlist = db.Authors.ToList();

        AuthorsToAdd = new ObservableCollection<Author>(AuthorsToAddlist);

        AuthorToAdd = AuthorsToAdd.FirstOrDefault();

        RaisePropertyChanged("AuthorsToAdd");

    }
    private void LoadAuthorsInOriginalBook()
    {
        using var db = new MehrisbookstoreContext();

        AuthorsInOriginalBook = new ObservableCollection<Author>(
            db.Authors
            .Where(a => a.OriginalBooks.Contains(SelectedOriginalBook))
            );

        RaisePropertyChanged("AuthorsInOriginalBook");
    }
    public void LoadAuthorsToAddInEdit() 
    {
        using var db = new MehrisbookstoreContext();

        if (AuthorsInOriginalBook == null)
        {
            AuthorsInOriginalBook = new ObservableCollection<Author>();
        }

        var authorOfCurrentBook = AuthorsInOriginalBook.Select(a => a.Id).ToList();

        AuthorsToAddInEdit = new ObservableCollection<Author>(
            db.Authors
            .Where(a => !authorOfCurrentBook.Contains(a.Id))
            .ToList());

        AuthorToAddInEdit = AuthorsToAddInEdit.FirstOrDefault();

        RaisePropertyChanged("AuthorsToAddInEdit");
    }
}
