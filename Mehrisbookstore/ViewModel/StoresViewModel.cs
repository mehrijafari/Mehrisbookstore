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
    public AddBookWindow AddBookWindow { get; set; }
    public DeleteBookWindow DeleteBookWindow { get; set; }
    public DelegateCommand OpenTransferBooksCommand { get; }
    public DelegateCommand OpenEditQuantityCommand { get;}
    public DelegateCommand CloseEditQuantityCommand { get; }
    public DelegateCommand OpenAddBooksCommand { get; }
    public DelegateCommand CloseAddBooksCommand { get; set; }
    public DelegateCommand EditQuantityCommand { get; }
    public DelegateCommand AddQuantityCommand { get; }
    public DelegateCommand SubtractQuantityCommand { get; }
    public DelegateCommand AddBookCommand { get; }
    public DelegateCommand OpenDeleteBookCommand { get; }
    public DelegateCommand DeleteBookCommand { get; }
    public DelegateCommand CloseDeleteBookCommand { get; }
    public ObservableCollection<Store> Stores { get; private set; }
    public ObservableCollection<Book> PossibleBooksToAdd { get; set; }
    public ObservableCollection<Book> PossibleIsbnToAdd { get; set; }
    public ObservableCollection<string> PossibleRecipientStores { get; set; } 
    public ObservableCollection<Book> BooksInSelectedStore { get; set; }
    public ObservableCollection<Book> IsbnInSelectedStore { get; set; }

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


    private Store _selectedStore;

    public Store SelectedStore
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

    private Book _selectedIsbn;

    public Book SelectedIsbn
    {
        get => _selectedIsbn;
        set 
        { 
            _selectedIsbn = value;
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
    private Visibility _storesVisibility;

    public Visibility StoresVisibility
    {
        get => _storesVisibility;
        set 
        {
            _storesVisibility = value;
            RaisePropertyChanged();
        }
    }


    public ObservableCollection<StoreInventorySummary> StoreInventory { get; private set; }
    public StoresViewModel(MainWindowViewModel? mainWindowViewModel) //konstruktor
    {
        this.mainWindowViewModel = mainWindowViewModel;
        StoresVisibility = Visibility.Visible;
        AmountOfBooksToEdit = 1;
        OpenEditQuantityCommand = new DelegateCommand(OpenEditQuantity);
        EditQuantityCommand = new DelegateCommand(EditAmountOfBooks);
        AddQuantityCommand = new DelegateCommand(AddQuantity);
        SubtractQuantityCommand = new DelegateCommand(SubtractQuantity);
        CloseEditQuantityCommand = new DelegateCommand(CloseEditQuantity);
        OpenAddBooksCommand = new DelegateCommand(OpenAddBooks);
        CloseAddBooksCommand = new DelegateCommand(CloseAddBooksWindow);
        AddBookCommand = new DelegateCommand(AddBook);
        OpenDeleteBookCommand = new DelegateCommand(OpenDeleteBook);
        DeleteBookCommand = new DelegateCommand(DeleteBook);
        CloseDeleteBookCommand = new DelegateCommand(CloseDeleteBook);
        LoadStores();
    }

    private void CloseDeleteBook(object obj)
    {
        DeleteBookWindow.Close();
    }

    private void DeleteBook(object obj)
    {
        using var db = new MehrisbookstoreContext();

        var recordToDelete = db.InventoryBalances
            .Where(ib => ib.Isbn == SelectedIsbn.Isbn13 && ib.Store.StoreName == SelectedStore.StoreName)
            .FirstOrDefault();

        if (recordToDelete != null)
        {
            db.InventoryBalances.Remove(recordToDelete);
            db.SaveChanges();
            LoadStoreInventory();
            RaisePropertyChanged("StoreInventory");
            LoadBooksInSelectedStore();
            RaisePropertyChanged("BooksInSelectedStore");
            LoadPossibleBooksToAdd();
            RaisePropertyChanged("PossibleBooksToAdd");
            DeleteBookWindow.Close();
        }
    }

    private void OpenDeleteBook(object obj)
    {
        DeleteBookWindow = new DeleteBookWindow();
        DeleteBookWindow.Show();
    }

    private void AddBook(object obj)
    {
        using var db = new MehrisbookstoreContext();

        var store = db.Stores
            .Where(s => s.StoreName == SelectedStore.StoreName)
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
        LoadBooksInSelectedStore();
        RaisePropertyChanged("BooksInSelectedStore");
        LoadPossibleBooksToAdd();
        RaisePropertyChanged("PossibleBooksToAdd");
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
            //var inventoryBalance = new InventoryBalance();

            var inventoryBalance = db.InventoryBalances
            .Include(ib => ib.IsbnNavigation)
            .Include(ib => ib.Store)
            .Where(ib => ib.IsbnNavigation.Title.Equals(SelectedBook.Title) && ib.Store.StoreName.Equals(SelectedStore.StoreName) && ib.Isbn.Equals(SelectedIsbn.Isbn13))
            .FirstOrDefault();
        

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

        Stores = new ObservableCollection<Store>(
            db.Stores.ToList()
            );

        SelectedStore = Stores.FirstOrDefault();
    }
    private void LoadPossibleRecipientStores()
    {
        using var db = new MehrisbookstoreContext();

        PossibleRecipientStores = new ObservableCollection<string>(
            db.Stores.Where(s => s.StoreName != SelectedStore.StoreName).Select(s => s.StoreName).ToList()
        );
    }
    private void LoadPossibleIsbnToAdd() 
    {
        using var db = new MehrisbookstoreContext();

        PossibleIsbnToAdd = new ObservableCollection<Book>();

        var allBooks = db.Books
            .Where(b => b.OriginalTitleId == SelectedBookToAdd.OriginalTitleId)
            .ToList();

        var existingIsbn = db.InventoryBalances
            .Where(ib => ib.Store.StoreName == SelectedStore.StoreName)
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
    private void LoadIsbnInSelectedStore()
    {
        using var db = new MehrisbookstoreContext();

        IsbnInSelectedStore = new ObservableCollection<Book>(
            db.InventoryBalances
            .Include(ib => ib.IsbnNavigation)
            .Include(ib => ib.Store)
            .Where(ib => ib.IsbnNavigation.OriginalTitleId == SelectedBook.OriginalTitleId && ib.Store.StoreName.Equals(SelectedStore.StoreName))
            .Select(ib => ib.IsbnNavigation)
            .Distinct()
            .ToList()

        );
        SelectedIsbn = IsbnInSelectedStore.FirstOrDefault();
    }
    public void LoadPossibleBooksToAdd()
    {
        using var db = new MehrisbookstoreContext();

        var booksNotInStore = db.Books
            .Where(b => !db.InventoryBalances
                .Include(ib => ib.Store)
                .Include(ib => ib.IsbnNavigation)
                .Where(ib => ib.Store.StoreName == SelectedStore.StoreName)
                .Select(ib => ib.Isbn)
                .Contains(b.Isbn13))
                .Distinct()
            .ToList();

        PossibleBooksToAdd = new ObservableCollection<Book>(booksNotInStore);
        SelectedBookToAdd = PossibleBooksToAdd.FirstOrDefault();
        RaisePropertyChanged("PossibleBooksToAdd");
       
    }
    public void LoadBooksInSelectedStore()
    {
        using var db = new MehrisbookstoreContext();

        BooksInSelectedStore = new ObservableCollection<Book>(
                db.Books
                .Include(b => b.InventoryBalances)
                .Where(b => b.InventoryBalances.Any(ib => ib.Store.StoreName == SelectedStore.StoreName))
                .Distinct()
                .ToList()
            );
        SelectedBook = BooksInSelectedStore.FirstOrDefault();
        RaisePropertyChanged("BooksInSelectedStore");
    }
    public void LoadStoreInventory()
    {
        using var db = new MehrisbookstoreContext();

        StoreInventory = new ObservableCollection<StoreInventorySummary>(
            db.InventoryBalances
                .Include(ib => ib.Store)
                .Include(ib => ib.IsbnNavigation)
                .Include(ib => ib.IsbnNavigation.OriginalTitle.Authors) 
                .Where(ib => ib.Store.StoreName == SelectedStore.StoreName)
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
        RaisePropertyChanged("StoreInventory");
    }
}
