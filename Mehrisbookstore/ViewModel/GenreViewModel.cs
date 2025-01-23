using Mehrisbookstore.Command;
using Mehrisbookstore.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrisbookstore.ViewModel;

internal class GenreViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    public DelegateCommand AddNewGenreCommand { get; }
    public DelegateCommand CancelAddNewGenreCommand { get; }
    public DelegateCommand OpenAddNewGenreCommand { get; }
    public AddNewGenre AddNewGenreWindow { get; set; }

    private Genre _newGenre;

    public Genre NewGenre
    {
        get => _newGenre;
        set 
        {
            _newGenre = value;
            RaisePropertyChanged();
        }
    }

    public GenreViewModel(MainWindowViewModel mainWindowViewModel) //konstruktor
    {
        this._mainWindowViewModel = mainWindowViewModel;
        AddNewGenreCommand = new DelegateCommand(AddNewGenre);
        CancelAddNewGenreCommand = new DelegateCommand(CancelAddNewGenre);
        OpenAddNewGenreCommand = new DelegateCommand(OpenAddNewGenre);
    }

    private void OpenAddNewGenre(object obj)
    {
        NewGenre = new Genre();
        AddNewGenreWindow = new AddNewGenre();
        AddNewGenreWindow.Show();
    }

    private void CancelAddNewGenre(object obj)
    {
        AddNewGenreWindow.Close();
    }

    public void AddNewGenre(object obj)
    {
        using var db = new MehrisbookstoreContext();

        db.Genres.Add(NewGenre);

        db.SaveChanges();

        _mainWindowViewModel.BooksViewModel.LoadGenres();
        _mainWindowViewModel.BooksViewModel.LoadGenresInEdit();
        _mainWindowViewModel.BooksViewModel.LoadGenresToAddInEdit();

        AddNewGenreWindow.Close();
    }
}
