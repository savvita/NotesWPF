using System.Linq;
using NotesWPF.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Data;

namespace NotesWPF.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public NotesController Controller { get; set; } = new NotesController();

        private NoteModel? currentNote;
        public NoteModel? CurrentNote
        {
            get => currentNote;

            set
            {
                currentNote = value;
                OnPropertyChanged(nameof(CurrentNote));
            }
        }

        private ICollectionView? notesView;
        private SortDescription sortByTitle;
        private SortDescription sortByDate;

        private string? filter;

        public string? Filter
        {
            get
            {
                return filter;
            }
            set
            {
                if (value != filter)
                {
                    filter = value;
                    notesView?.Refresh();
                    OnPropertyChanged(nameof(Filter));
                }
            }
        }

        public MainWindowViewModel()
        {
            Controller.Loaded += Initialize;
            sortByTitle = new SortDescription("Title", ListSortDirection.Ascending);
            sortByDate = new SortDescription("Date", ListSortDirection.Ascending);
        }

        private void Initialize()
        {
            notesView = CollectionViewSource.GetDefaultView(Controller.Notes);
            notesView.Filter = obj =>
            {
                if (String.IsNullOrEmpty(Filter) == true)
                {
                    return true;
                }

                if (obj is NoteModel note)
                {
                    bool isTitleContains = false;
                    bool isContentContains = false;

                    if (note.Title != null)
                    {
                        isTitleContains = note.Title.Contains(Filter, StringComparison.OrdinalIgnoreCase);
                    }

                    if (note.Content != null)
                    {
                        isContentContains = note.Content.Contains(Filter, StringComparison.OrdinalIgnoreCase);
                    }
                    return isTitleContains || isContentContains;
                }

                return false;
            };
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Commands
        private readonly RelayCommand<object>? addCommand;
        private readonly RelayCommand? deleteCommand;
        private readonly RelayCommand<object>? okCommand;
        public readonly RelayCommand<object>? searchCommand;
        public readonly RelayCommand? sortByDateCommand;
        public readonly RelayCommand? sortByTitleCommand;
        public readonly RelayCommand? sortByDefaultCommand;
        public readonly RelayCommand? closeCommand;

        public RelayCommand<object> AddCommand
        {
            get
            {
                return addCommand ?? new RelayCommand<object>((_) =>
                {
                    if (Controller.Notes != null)
                    {
                        NoteModel note = new NoteModel();
                        Controller.Notes.Add(note);

                        CurrentNote = Controller.Notes.Last();
                    }
                });

            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? new RelayCommand(() =>
                {
                    if (Controller.Notes != null && CurrentNote != null)
                    {
                        Controller.Remove(CurrentNote);
                        Controller.Notes.Remove(CurrentNote);
                    }
                });
            }
        }

        public RelayCommand<object> OkCommand
        {
            get
            {
                return okCommand ?? new RelayCommand<object>((note) =>
                {
                    if (Controller.Notes != null && CurrentNote != null)
                    {
                        int idx = Controller.Notes.IndexOf(CurrentNote);

                        if (idx != -1)
                        {
                            string[] values = ((string)note).Split("||");
                            Controller.Notes[idx].Title = values[1];
                            Controller.Notes[idx].Content = values[2];
                            OnPropertyChanged(nameof(NotesController.Notes));
                            Controller.Update(CurrentNote);
                        }
                    }
                });
            }
        }

        public RelayCommand SortByDateCommand
        {
            get
            {
                return sortByDateCommand ?? new RelayCommand(() =>
                {
                    if (Controller.Notes != null)
                    {
                        notesView?.SortDescriptions.Clear();
                        notesView?.SortDescriptions.Add(sortByDate);
                    }
                });
            }
        }

        public RelayCommand SortByTitleCommand
        {
            get
            {
                return sortByTitleCommand ?? new RelayCommand(() =>
                {
                    if (Controller.Notes != null)
                    {
                        notesView?.SortDescriptions.Clear();
                        notesView?.SortDescriptions.Add(sortByTitle);
                    }
                });
            }
        }

        public RelayCommand SortByDefaultCommand
        {
            get
            {
                return sortByDefaultCommand ?? new RelayCommand(() =>
                {
                    if (Controller.Notes != null)
                    {
                        notesView?.SortDescriptions.Clear();
                    }
                });
            }
        }

        public RelayCommand CloseCommand
        {
            get
            {
                return closeCommand ?? new RelayCommand(() =>
                {
                    Controller.Dispose();
                });
            }
        }

        #endregion
    }
}
