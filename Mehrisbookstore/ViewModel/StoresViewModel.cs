using Mehrisbookstore.Command;
using Mehrisbookstore.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Mehrisbookstore.ViewModel;

internal class StoresViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;
    public EditQuantity EditQuantity { get; set; }
    public TransferBooks TransferBooks { get; set; }
    public AddBookWindow AddBookWindow { get; set; }
    public DelegateCommand OpenTransferBooksCommand { get; }
    public DelegateCommand OpenEditQuantityCommand { get;}
    public DelegateCommand CloseEditQuantityCommand { get; }
    public DelegateCommand OpenAddBooksCommand { get; }
    public DelegateCommand CloseAddBooksCommand { get; set; }
    public DelegateCommand EditQuantityCommand { get; }
    public DelegateCommand AddQuantityCommand { get; }
    public DelegateCommand SubtractQuantityCommand { get; }
    public DelegateCommand AddBookCommand { get; set; }
    public ObservableCollection<Author> AuthorsOfOriginalTitle { get; set; } //TODO:Ta bort om du ej ska ha
    public ObservableCollection<string> Stores { get; private set; }
    public ObservableCollection<Book> PossibleBooksToAdd { get; set; }
    public ObservableCollection<Book> PossibleIsbnToAdd { get; set; }
    public ObservableCollection<string> PossibleRecipientStores { get; set; } 
    public ObservableCollection<string> BooksInSelectedStore { get; set; }
    public ObservableCollection<string> IsbnInSelectedStore { get; set; }

    private int _amountOfBooksToEdit;

    public int AmountOfBooksToEdit
    {
        get => _amountOfBooksToEdit;
        set 
        {
            _amountOfBooksToEdit = value;
            RaisePropertyChanged();
        }
    }


    private string? _selectedStore;

    public string? SelectedStore
    {
        get => _selectedStore;
        set 
        { 
            _selectedStore = value;
            RaisePropertyChanged();
            LoadStoreInventory();
            RaisePropertyChanged("StoreInventory");
            LoadBooksInSelectedStore();
            RaisePropertyChanged("BooksInSelectedStore");
            LoadPossibleRecipientStores();
            RaisePropertyChanged("PossibleRecipientStores");
            LoadPossibleBooksToAdd();
            RaisePropertyChanged("PossibleBooksToAdd");
            
            
           
        }
    }

    private string _selectedIsbn;

    public string SelectedIsbn
    {
        get => _selectedIsbn;
        set 
        { 
            _selectedIsbn = value;
            RaisePropertyChanged();
        }
    }

    private string _selectedBook;

    public string SelectedBook
    {
        get => _selectedBook;
        set 
        { 
            _selectedBook = value;
            RaisePropertyChanged();
            LoadIsbnInSelectedStore();
            RaisePropertyChanged("IsbnInSelectedStore");
        }
    }

    private string _recipientStore;

    public string RecipientStore
    {
        get => _recipientStore;
        set 
        { 
            _recipientStore = value;
            RaisePropertyChanged();
        }
    }

    private Book _selectedBookToAdd;

    public Book SelectedBookToAdd
    {
        get => _selectedBookToAdd;
        set 
        {
            _selectedBookToAdd = value;
            RaisePropertyChanged();
            LoadPossibleIsbnToAdd();
            RaisePropertyChanged("PossibleIsbnToAdd");
            //TODO:ta bort om du ej ska ha kvar
            //LoadAuthorsOfOriginalTitle();
            //RaisePropertyChanged("AuthorsOfOriginalTitle");
        }
    }
    private Book _selectedIsbnToAdd;

    public Book SelectedIsbnToAdd
    {
        get => _selectedIsbnToAdd;
        set 
        { 
            _selectedIsbnToAdd = value;
            RaisePropertyChanged();
        }
    }
    //TODO:ta bort om du ej ska ha
    //private Author _selectedAuthor;

    //public Author SelectedAuthor
    //{
    //    get => _selectedAuthor;
    //    set 
    //    {
    //        _selectedAuthor = value;
    //        RaisePropertyChanged();
    //    }
    //}



    public ObservableCollection<StoreInventorySummary> StoreInventory { get; private set; }
    public StoresViewModel(MainWindowViewModel? mainWindowViewModel) 
    {
        this.mainWindowViewModel = mainWindowViewModel;
        OpenEditQuantityCommand = new DelegateCommand(OpenEditQuantity);
        EditQuantityCommand = new DelegateCommand(EditAmountOfBooks);
        AddQuantityCommand = new DelegateCommand(AddQuantity);
        SubtractQuantityCommand = new DelegateCommand(SubtractQuantity);
        AmountOfBooksToEdit = 1;
        CloseEditQuantityCommand = new DelegateCommand(CloseEditQuantity);
        OpenTransferBooksCommand = new DelegateCommand(OpenTransferBooks);
        OpenAddBooksCommand = new DelegateCommand(OpenAddBooks);
        CloseAddBooksCommand = new DelegateCommand(CloseAddBooksWindow);
        AddBookCommand = new DelegateCommand(AddBook);
        LoadStores();
        

    }

    private void AddBook(object obj)
    {
        using var db = new MehrisbookstoreContext();

        var store = db.Stores
            .Where(s => s.StoreName == SelectedStore)
            .FirstOrDefault();

        var newInventoryBalance = new InventoryBalance()
        {
            StoreId = store.Id,
            Isbn = SelectedIsbnToAdd.Isbn13,
            Quantity = 1
        };

        db.InventoryBalances.Add(newInventoryBalance);
        db.SaveChanges();
        LoadStoreInventory();
        RaisePropertyChanged("StoreInventory");
        AddBookWindow.Close();
    }

    private void CloseAddBooksWindow(object obj)
    {
        AddBookWindow.Close();
    }

    private void OpenAddBooks(object obj)
    {
        AddBookWindow = new AddBookWindow();
        AddBookWindow.Show();
    }

    private void OpenTransferBooks(object obj)
    {
        TransferBooks = new TransferBooks();
        TransferBooks.Show();
    }

    private void CloseEditQuantity(object obj)
    {
        AmountOfBooksToEdit = 1;
        EditQuantity.Close();
    }

    private void SubtractQuantity(object obj)
    {
        AmountOfBooksToEdit--;
    }

    private void AddQuantity(object obj)
    {
        AmountOfBooksToEdit++;
    }

    private void EditAmountOfBooks(object obj)
    {
        using var db = new MehrisbookstoreContext();

        //hitta rätt InventoryBalance
        var inventoryBalance = db.InventoryBalances
          .Include(ib => ib.IsbnNavigation)
          .Include(ib => ib.Store)
          .Where(ib => ib.IsbnNavigation.Title.Equals(SelectedBook) && ib.Store.StoreName.Equals(SelectedStore) && ib.Isbn.Equals(SelectedIsbn))
          .First();

        //Redigera Quantity på inventory balance

        if (AmountOfBooksToEdit < 0 && -AmountOfBooksToEdit > inventoryBalance.Quantity)
        {
            MessageBox.Show($"Can not remove {-AmountOfBooksToEdit} books. There are only {inventoryBalance.Quantity} books in stock.");
            return;
        }
       
        inventoryBalance.Quantity += AmountOfBooksToEdit;
        db.SaveChanges();

        LoadStoreInventory();
        RaisePropertyChanged("StoreInventory");

        AmountOfBooksToEdit = 1;
        EditQuantity.Close();
    }

    private void OpenEditQuantity(object obj)
    {
        EditQuantity = new EditQuantity();
        EditQuantity.Show();
    }


    private void LoadStores()
    {
        using var db = new MehrisbookstoreContext();

        Stores = new ObservableCollection<string>(
            db.Stores.Select(s => s.StoreName).ToList()

            );

        SelectedStore = Stores.FirstOrDefault();
    }
    private void LoadPossibleRecipientStores()
    {
        using var db = new MehrisbookstoreContext();

        PossibleRecipientStores = new ObservableCollection<string>(
            db.Stores.Where(s => s.StoreName != SelectedStore).Select(s => s.StoreName).ToList()
        );
    }
    private void LoadPossibleBooksToAdd()
    {
        using var db = new MehrisbookstoreContext();

        var booksNotInStore = db.Books
            .Where(b => !db.InventoryBalances
                .Include(ib => ib.Store)
                .Include(ib => ib.IsbnNavigation)
                .Where(ib => ib.Store.StoreName == SelectedStore)
                .Select(ib => ib.Isbn)
                .Contains(b.Isbn13))
                .Distinct()
            .ToList();

        PossibleBooksToAdd = new ObservableCollection<Book>(booksNotInStore);
        SelectedBookToAdd = PossibleBooksToAdd.FirstOrDefault();
        
       
    }
    private void LoadPossibleIsbnToAdd() 
    {
        using var db = new MehrisbookstoreContext();

        PossibleIsbnToAdd = new ObservableCollection<Book>();

        var allBooks = db.Books  
            .Where(b => b.Title == SelectedBookToAdd.Title)
            .ToList();

        var existingIsbn = db.InventoryBalances
            .Where(ib => ib.Store.StoreName == SelectedStore)
            .Select(ib => ib.Isbn)
            .ToList();

        foreach (var book in allBooks)
        {
            if (!existingIsbn.Contains(book.Isbn13))
            {
                PossibleIsbnToAdd.Add(book);
            }
        }

        SelectedIsbnToAdd = PossibleIsbnToAdd.FirstOrDefault();
    }

    //private void LoadAuthorsOfOriginalTitle()
    //{
    //    using var db = new MehrisbookstoreContext();

    //    AuthorsOfOriginalTitle = new ObservableCollection<Author>();

    //    var authors = db.Books
    //        .Where(b => b.Title == SelectedBookToAdd.Title)
    //        .SelectMany(b => b.OriginalTitle.Authors)
    //        .ToList();

    //    foreach (var author in authors)
    //    {
    //        AuthorsOfOriginalTitle.Add(author);
    //    }

    //    SelectedAuthor = AuthorsOfOriginalTitle.FirstOrDefault();
    //}
    private void LoadIsbnInSelectedStore()
    {
        using var db = new MehrisbookstoreContext();

        IsbnInSelectedStore = new ObservableCollection<string>(
            db.InventoryBalances
            .Include(ib => ib.IsbnNavigation)
            .Include(ib => ib.Store)
            .Where(ib => ib.IsbnNavigation.Title.Equals(SelectedBook) && ib.Store.StoreName.Equals(SelectedStore))
            .Select(ib => ib.Isbn)
            .Distinct()
            .ToList()

        );
        SelectedIsbn = IsbnInSelectedStore.FirstOrDefault();
    }

    
  
    private void LoadBooksInSelectedStore()
    {
        using var db = new MehrisbookstoreContext();

        BooksInSelectedStore = new ObservableCollection<string>(
            db.Books.Select(b => b.Title)
            .Distinct()
            .ToList()
        );
        SelectedBook = BooksInSelectedStore.FirstOrDefault();
    }
    private void LoadStoreInventory()
    {
        using var db = new MehrisbookstoreContext();

        StoreInventory = new ObservableCollection<StoreInventorySummary>(
            db.InventoryBalances
                .Include(ib => ib.Store)
                .Include(ib => ib.IsbnNavigation)
                .Include(ib => ib.IsbnNavigation.OriginalTitle.Authors) 
                .Where(ib => ib.Store.StoreName == SelectedStore)
                .Select(ib => new StoreInventorySummary()
                {
                    StoreID = ib.StoreId,
                    Title = ib.IsbnNavigation.Title,
                    ISBN = ib.Isbn,
                    Quantity = ib.Quantity,
                    Authors = string.Join(", ", ib.IsbnNavigation.OriginalTitle.Authors.Select(a => $"{a.FirstName} {a.LastName}"))
                })
                .ToList()
         );
    }
}
public class StoreInventorySummary
{
    public int StoreID { get; set; }
    public string? Title { get; set; }
    public string? ISBN { get; set; }
    public int Quantity { get; set; }
    public string? Authors { get; set; }
}
