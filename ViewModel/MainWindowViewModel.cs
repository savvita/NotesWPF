using System.Linq;
using System.Collections.ObjectModel;
using NotesWPF.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight.Command;
using System;

namespace NotesWPF.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private FileController controller;
        private ObservableCollection<NoteModel>? notes;
        public ObservableCollection<NoteModel>? Notes 
        {
            get => notes;

            private set
            {
                notes = value;
                OnPropertyChanged("Notes");
            }
        }

        private NoteModel? currentNote;
        public NoteModel? CurrentNote 
        {
            get => currentNote;
                
            set
            {
                currentNote = value;
                OnPropertyChanged("CurrentNote");
            }
        }

        public MainWindowViewModel()
        {
            controller = new FileController();

            if(controller.Notes != null)
                Notes = new ObservableCollection<NoteModel>(controller.Notes);

            addCommand = null;
            deleteCommand = null;
            okCommand = null;
            searchCommand = null;
            sortByDateCommand = null;
            sortByTitleCommand = null;
            sortByDefaultCommand = null;
            closeCommand = null;
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
                    if (controller.Notes != null)
                    {
                        NoteModel note = new NoteModel();
                        controller.Notes.Add(note);

                        if (Notes != null)
                        {
                            Notes.Add(note);
                            CurrentNote = Notes.Last();
                        }
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
                    if (controller.Notes != null && CurrentNote != null)
                    {
                        controller.Notes.Remove(CurrentNote);

                        if(Notes != null)
                            Notes.Remove(CurrentNote);
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
                    if (Notes != null && currentNote != null)
                    {
                        int idx = Notes.IndexOf(currentNote);

                        if (idx != -1)
                        {
                            string[] values = ((string)note).Split("||");
                            Notes[idx].Title = values[1];
                            Notes[idx].Content = values[2];
                        }
                    }
                });
            }
        }

        public RelayCommand<object> SearchCommand
        {
            get
            {
                return searchCommand ?? new RelayCommand<object>((obj) =>
                {
                    if (controller.Notes != null)
                    {
                        string text = (string)obj;

                    if (text.Equals(string.Empty))
                    {
                        Notes = new ObservableCollection<NoteModel>(controller.Notes);
                    }
                    else
                    {
                        Notes = new ObservableCollection<NoteModel>(
                            controller.Notes.Where((note) =>
                            {
                                return note != null ?
                                note.Title.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                                note.Content.Contains(text, StringComparison.OrdinalIgnoreCase) :
                                false;
                            }));
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
                    if(Notes != null)
                        Notes = new ObservableCollection<NoteModel>(Notes.OrderBy(note => note.Date));
                });
            }
        }

        public RelayCommand SortByTitleCommand
        {
            get
            {
                return sortByTitleCommand ?? new RelayCommand(() =>
                {
                    if(Notes != null)
                        Notes = new ObservableCollection<NoteModel>(Notes.OrderBy(note => note.Title));
                });
            }
        }

        public RelayCommand SortByDefaultCommand
        {
            get
            {
                return sortByDefaultCommand ?? new RelayCommand(() =>
                {
                    if(controller.Notes != null)
                        Notes = new ObservableCollection<NoteModel>(controller.Notes);
                });
            }
        }

        public RelayCommand CloseCommand
        {
            get
            {
                return closeCommand ?? new RelayCommand(() =>
                {
                    controller.SaveToFile();
                });
            }
        }

        #endregion
    }
}
