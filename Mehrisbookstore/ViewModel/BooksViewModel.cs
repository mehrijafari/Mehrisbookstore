using Mehrisbookstore.Command;
using Mehrisbookstore.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace Mehrisbookstore.ViewModel;

internal class BooksViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    public ObservableCollection<Book> Books { get; set; }
    public ObservableCollection<OriginalBook> OriginalBooks { get; set; }
    public ObservableCollection<Publisher> Publishers { get; set; }
    public ObservableCollection<Publisher> PublishersInEdit { get; set; }
    public ObservableCollection<Genre> PossibleGenresToAdd { get; set; }
    public ObservableCollection<Genre> Genres { get; set; }
    public ObservableCollection<Genre> GenresInEdit { get; set; }
    public ObservableCollection<Genre> GenresToAddInEdit { get; set; }
    public ObservableCollection<Author> AuthorsInEdit { get; set; }
    public ObservableCollection<Genre> GenresAdded { get; set; }
    public ObservableCollection<Author> Authors { get; set; }
    public AddNewBookWindow AddNewBookWindow { get; set; }
    public EditBookWindow EditBookWindow { get; set; }
    public DelegateCommand OpenEditBookCommand { get; }
    public DelegateCommand OpenAddNewBookCommand { get; }
    public DelegateCommand CloseAddNewBookCommand { get; }
    public DelegateCommand AddGenreCommand { get; }
    public DelegateCommand RemoveGenreCommand { get; }
    public DelegateCommand AddNewBookCommand { get; }
    public DelegateCommand DeleteBookCommand { get; }
    public DelegateCommand AddGenreInEditCommand { get; }
    public DelegateCommand RemoveGenreInEditCommand { get; }
    public DelegateCommand CloseEditBookCommand { get; }
    public DelegateCommand EditBookCommand { get; }

    private Genre _genreToAddInEdit;

    public Genre GenreToAddInEdit
    {
        get => _genreToAddInEdit;
        set
        {
            _genreToAddInEdit = value;
            RaisePropertyChanged();
        }
    }
    private Genre _genreInEdit;

    public Genre GenreInEdit
    {
        get => _genreInEdit;
        set
        {
            _genreInEdit = value;
            RaisePropertyChanged();
        }
    }

    private Genre _genreToAdd;

    public Genre GenreToAdd
    {
        get => _genreToAdd;
        set
        {
            _genreToAdd = value;
            RaisePropertyChanged();
        }
    }

    private Visibility _booksVisibility;

    public Visibility BooksVisibility
    {
        get => _booksVisibility;
        set
        {
            _booksVisibility = value;
            RaisePropertyChanged();
        }
    }

    private Book _newBook;

    public Book NewBook
    {
        get => _newBook;
        set
        {
            _newBook = value;
            RaisePropertyChanged();

        }
    }
    private OriginalBook _newBookTitle;

    public OriginalBook NewBookTitle
    {
        get => _newBookTitle;
        set
        {
            _newBookTitle = value;
            RaisePropertyChanged();
            LoadAuthorsForBook();
        }
    }
    public DateTime? PublishdateDateTimeEdit
    {
        get
        {
            if (BookBeingEdited?.PublishDate != null)
            {
                return new DateTime(
                    BookBeingEdited.PublishDate.Value.Year,
                    BookBeingEdited.PublishDate.Value.Month,
                    BookBeingEdited.PublishDate.Value.Day
                );
            }
            return null;
        }
        set
        {
            if (value != null)
            {
                BookBeingEdited.PublishDate = DateOnly.FromDateTime(value.Value);
            }
            else
            {
                BookBeingEdited.PublishDate = null;
            }

            RaisePropertyChanged();
        }
    }

    public DateTime? PublishdateDateTime
    {
        get
        {
            if (NewBook?.PublishDate != null)
            {
                return new DateTime(
                    NewBook.PublishDate.Value.Year,
                    NewBook.PublishDate.Value.Month,
                    NewBook.PublishDate.Value.Day
                );
            }
            return null;
        }
        set
        {
            if (value != null)
            {
                NewBook.PublishDate = DateOnly.FromDateTime(value.Value);
            }
            else
            {
                NewBook.PublishDate = null;
            }

            RaisePropertyChanged();
        }
    }

    private Publisher _selectedPublisher;

    public Publisher SelectedPublisher
    {
        get => _selectedPublisher;
        set
        {
            _selectedPublisher = value;
            RaisePropertyChanged();
        }
    }
    private Publisher _publisherInEdit;

    public Publisher PublisherInEdit
    {
        get => _publisherInEdit;
        set
        {
            _publisherInEdit = value;
            RaisePropertyChanged();
        }
    }

    private Book _selectedBook;

    public Book SelectedBook
    {
        get => _selectedBook;
        set
        {
            _selectedBook = value;
            RaisePropertyChanged();
            DeleteBookCommand.RaiseCanExecuteChanged();
            OpenEditBookCommand.RaiseCanExecuteChanged();
        }
    }

    private Book _bookBeingEdited;

    public Book BookBeingEdited
    {
        get => _bookBeingEdited;
        set
        {
            _bookBeingEdited = value;
            RaisePropertyChanged();
        }
    }

    private Genre _genre;

    public Genre Genre
    {
        get => _genre;
        set
        {
            _genre = value;
            RaisePropertyChanged();
        }
    }

    public BooksViewModel(MainWindowViewModel mainWindowViewModel) //konstruktor
    {
        this._mainWindowViewModel = mainWindowViewModel;
        BooksVisibility = Visibility.Hidden;
        OpenAddNewBookCommand = new DelegateCommand(OpenAddNewBook);
        AddGenreCommand = new DelegateCommand(AddGenre);
        RemoveGenreCommand = new DelegateCommand(RemoveGenre);
        AddNewBookCommand = new DelegateCommand(AddNewBook);
        DeleteBookCommand = new DelegateCommand(DeleteBook, CanDeleteBook);
        OpenEditBookCommand = new DelegateCommand(OpenEditBook, CanOpenEditBook);
        CloseAddNewBookCommand = new DelegateCommand(CloseAddNewBook);
        AddGenreInEditCommand = new DelegateCommand(AddGenreInEdit);
        RemoveGenreInEditCommand = new DelegateCommand(RemoveGenreInEdit);
        CloseEditBookCommand = new DelegateCommand(CloseEditBook);
        EditBookCommand = new DelegateCommand(EditBook);
        LoadBooks();
        LoadGenres();
    }
    private void EditBook(object obj)
    {
        using var db = new MehrisbookstoreContext();

        var book = db.Books.Find(SelectedBook.Isbn13);

        if (string.IsNullOrEmpty(BookBeingEdited.Isbn13))
        {
            MessageBox.Show("ISBN13 can not be empty");
            return;
        }

        if (!GenresInEdit?.Any() ?? true)
        {
            MessageBox.Show("Genres can not be empty");
            return;
        }

        if (db.Books.Any(b => b.Isbn13 == BookBeingEdited.Isbn13))
        {
            if (SelectedBook.Isbn13 == BookBeingEdited.Isbn13)
            {
                if (book != null)
                {
                    foreach (var genre in GenresInEdit) //kollar om genrerna man ska lägga till redan tillhör boken eller inte
                    {
                        var bookGenre = db.Genres
                            .Include(bg => bg.Books)
                            .FirstOrDefault(bg => bg.Id == genre.Id);

                        if (bookGenre.Books.Any(bg => bg.Isbn13 == BookBeingEdited.Isbn13)) //om boken HAR genren, fortsätt loopen
                        {
                            continue;
                        }
                        else //om boken INTE har den genren, lägg till genren
                        {
                            db.Database.ExecuteSqlRaw("INSERT INTO BookGenres values({0}, {1})", BookBeingEdited.Isbn13, genre.Id);
                        }
                    }

                    foreach (var genre in GenresToAddInEdit) //kollar genrerna man INTE ska lägga till tillhör boken eller inte
                    {
                        var bookGenre = db.Genres
                            .Include(bg => bg.Books)
                            .FirstOrDefault(bg => bg.Id == genre.Id);

                        if (bookGenre.Books.Any(bg => bg.Isbn13 == BookBeingEdited.Isbn13)) // om boken HAR genren, ta bort genren 
                        {
                            db.Database.ExecuteSqlRaw("DELETE FROM BookGenres WHERE BookID = {0} AND GenreID = {1}", BookBeingEdited.Isbn13, genre.Id);
                            
                        }
                        else //om boken INTE har genren, fortsätt loopen
                        {
                            continue;
                            
                        }
                    }

                    book.Isbn13 = BookBeingEdited.Isbn13;
                    book.Title = BookBeingEdited.Title;
                    book.Language = BookBeingEdited.Language;
                    book.Price = BookBeingEdited.Price;
                    book.PublishDate = BookBeingEdited.PublishDate;
                    book.Pages = BookBeingEdited.Pages;
                    book.PublisherId = PublisherInEdit.Id;
                    book.OriginalTitleId = BookBeingEdited.OriginalTitleId;

                    db.SaveChanges();

                    SelectedBook.Isbn13 = BookBeingEdited.Isbn13;
                    SelectedBook.Title = BookBeingEdited.Title;
                    SelectedBook.Language = BookBeingEdited.Language;
                    SelectedBook.Price = BookBeingEdited.Price;
                    SelectedBook.PublishDate = BookBeingEdited.PublishDate;
                    SelectedBook.Pages = BookBeingEdited.Pages;
                    SelectedBook.PublisherId = PublisherInEdit.Id;
                    SelectedBook.OriginalTitleId = BookBeingEdited.OriginalTitleId;

                    LoadBooks();

                    EditBookWindow.Close();
                    return;
                    
                }

            }

            MessageBox.Show("There is already another book with that ISBN13 in the system");
        }

    }

    private void CloseEditBook(object obj)
    {
        EditBookWindow.Close();
    }

    private void RemoveGenreInEdit(object obj)
    {
        GenresToAddInEdit.Add(GenreInEdit);
        GenresInEdit.Remove(GenreInEdit);
    }

    private void AddGenreInEdit(object obj)
    {
        if (GenresInEdit == null)
        {
            GenresInEdit = new ObservableCollection<Genre>();
        }

        GenresInEdit.Add(GenreToAddInEdit);
        GenresToAddInEdit.Remove(GenreToAddInEdit);
        GenreToAddInEdit = GenresToAddInEdit.FirstOrDefault();
    }

    private void CloseAddNewBook(object obj)
    {
        AddNewBookWindow.Close();
    }

    private bool CanOpenEditBook(object? arg)
    {
        if (SelectedBook != null) return true;
        else return false;
    }

    private void OpenEditBook(object obj)
    {
        EditBookWindow = new EditBookWindow();
        BookBeingEdited = new Book
        {
            Isbn13 = SelectedBook.Isbn13,
            Title = SelectedBook.Title,
            Language = SelectedBook.Language,
            Price = SelectedBook.Price,
            PublishDate = SelectedBook.PublishDate,
            Pages = SelectedBook.Pages,
            PublisherId = SelectedBook.PublisherId,
            OriginalTitleId = SelectedBook.OriginalTitleId
        };
        LoadPublishersInEdit();
        LoadAuthorsInEdit();
        LoadGenresInEdit();
        LoadGenresToAddInEdit();
        EditBookWindow.Show();
    }

    public void LoadPublishersInEdit()
    {
        using var db = new MehrisbookstoreContext();

        PublishersInEdit = new ObservableCollection<Publisher>(
            db.Publishers.ToList()
            );


        if (BookBeingEdited != null)
        {
            PublisherInEdit = PublishersInEdit.FirstOrDefault(p => p.Id == BookBeingEdited.PublisherId);
        }
        else
        {
            PublisherInEdit = null;
        }//TODO: lade till denna 

        // PublisherInEdit = PublishersInEdit.FirstOrDefault(p => p.Id == BookBeingEdited.PublisherId);

        RaisePropertyChanged("PublishersInEdit");
    }
    public void LoadGenres()
    {
        using var db = new MehrisbookstoreContext();

        var genresList = db.Genres.ToList();
        Genres = new ObservableCollection<Genre>(genresList);

        RaisePropertyChanged("Genres");
    }
    private bool CanDeleteBook(object? arg)
    {
        if (SelectedBook != null) return true;
        else return false;
    }

    private void DeleteBook(object obj)
    {
        using var db = new MehrisbookstoreContext();

        var result = MessageBox.Show($"Are you sure you want to delete book {SelectedBook.Title}?",
            "Confirm Deletion",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            var inventoryBalanceToRemove = db.InventoryBalances
                .Where(ib => ib.Isbn == SelectedBook.Isbn13)
                .ToList();

            db.InventoryBalances.RemoveRange(inventoryBalanceToRemove);

            db.Database.ExecuteSqlRaw("DELETE FROM BookGenres WHERE BookID = {0}", SelectedBook.Isbn13);

            db.Books.Remove(SelectedBook);
            db.SaveChanges();
            LoadBooks();
            RaisePropertyChanged("Books");

        }

        _mainWindowViewModel.StoresViewModel.LoadBooksInSelectedStore();
        _mainWindowViewModel.StoresViewModel.LoadStoreInventory();
    }
    private void AddNewBook(object obj)
    {
        using var db = new MehrisbookstoreContext();

        if (NewBook.Isbn13 == null || NewBookTitle.OriginalTitle == null || NewBook.Language == null || SelectedPublisher.Id == null || NewBookTitle.Id == null || GenresAdded == null)
        {
            MessageBox.Show("ISBN, Title, Language, Publisher or Genre can not be empty");
            return;
        }

        if (db.Books.Any(b => b.Isbn13 == NewBook.Isbn13))
        {
            MessageBox.Show("There is already a book with this ISBN13 in the system.");
            return;
        }

        var newBook = new Book()
        {
            Isbn13 = NewBook.Isbn13,
            Title = NewBookTitle.OriginalTitle,
            Language = NewBook.Language,
            Price = NewBook.Price,
            PublishDate = PublishdateDateTime.HasValue ? DateOnly.FromDateTime(PublishdateDateTime.Value) : null,
            Pages = NewBook.Pages,
            PublisherId = SelectedPublisher.Id,
            OriginalTitleId = NewBookTitle.Id
        };

        db.Books.Add(newBook);
        db.SaveChanges();

        foreach (var genre in GenresAdded)
        {
            db.Database.ExecuteSqlRaw("INSERT INTO BookGenres values({0}, {1})", newBook.Isbn13, genre.Id);
        }

        LoadBooks();
        RaisePropertyChanged("Books");

        AddNewBookWindow.Close();
    }
    private void AddGenre(object obj)
    {
        if (GenresAdded == null)
        {
            GenresAdded = new ObservableCollection<Genre>();
        }
        GenresAdded.Add(GenreToAdd);
        Genres.Remove(GenreToAdd);
        GenreToAdd = Genres.FirstOrDefault();
        RaisePropertyChanged("GenresAdded");
    }
    private void RemoveGenre(object obj)
    {
        Genres.Add(Genre);
        GenresAdded.Remove(Genre);
        RaisePropertyChanged("GenresAdded");
    }

    private void OpenAddNewBook(object obj)
    {
        NewBook = new Book();
        NewBookTitle = new OriginalBook();
        LoadOriginalBooks();
        LoadPublishers();
        LoadGenres();
        AddNewBookWindow = new AddNewBookWindow();
        AddNewBookWindow.Show();
    }

    public void LoadPublishers()
    {
        using var db = new MehrisbookstoreContext();

        Publishers = new ObservableCollection<Publisher>(
            db.Publishers.ToList()
            );

        RaisePropertyChanged("Publishers");

    }

    public void LoadBooks()
    {
        using var db = new MehrisbookstoreContext();

        Books = new ObservableCollection<Book>(
            db.Books
            .ToList()
            );
        RaisePropertyChanged("Books");
    }

    public void LoadOriginalBooks()
    {
        using var db = new MehrisbookstoreContext();

        OriginalBooks = new ObservableCollection<OriginalBook>(
            db.OriginalBooks.ToList()
            );

        RaisePropertyChanged("OriginalBooks");

    }

    public void LoadAuthorsForBook()
    {
        using var db = new MehrisbookstoreContext();

        if (NewBookTitle == null)
        {
            NewBookTitle = new OriginalBook();
        }

        var authorsList = db.OriginalBooks
            .Where(b => b.Id == NewBookTitle.Id)
            .SelectMany(b => b.Authors)
            .ToList();

        Authors = new ObservableCollection<Author>(authorsList);

        RaisePropertyChanged("Authors");
    }

    public void LoadAuthorsInEdit()
    {
        using var db = new MehrisbookstoreContext();

        AuthorsInEdit = new ObservableCollection<Author>(
            db.OriginalBooks
            .Where(ob => ob.Id == BookBeingEdited.OriginalTitleId)
            .SelectMany(ob => ob.Authors)
            .ToList()
            );

        RaisePropertyChanged("AuthorsInEdit");
    }
    public void LoadGenresInEdit()
    {
        using var db = new MehrisbookstoreContext();

        GenresInEdit = new ObservableCollection<Genre>(
            db.Genres
            .Where(g => g.Books.Contains(BookBeingEdited))
            .ToList()
            );

        RaisePropertyChanged("GenresInEdit");
    }
    public void LoadGenresToAddInEdit()
    {
        using var db = new MehrisbookstoreContext();

        var genresOfCurrentBook = GenresInEdit.Select(g => g.Id).ToList();

        GenresToAddInEdit = new ObservableCollection<Genre>(
            db.Genres
            .Where(g => !genresOfCurrentBook.Contains(g.Id))
            .ToList()
            );

        GenreToAddInEdit = GenresToAddInEdit.FirstOrDefault();

        RaisePropertyChanged("GenresToAddInEdit");
    }
}
