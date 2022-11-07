using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NotesWPF.Model
{
    public class NoteModel :INotifyPropertyChanged
    {
        public int? Id { get; }

        private string? title;

        public string? Title
        {
            get => title;

            set 
            { 
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private DateTime? date;

        public DateTime? Date
        {
            get => date;

            set 
            { 
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private string? content;

        public string? Content
        {
            get => content;

            set 
            { 
                content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public NoteModel()
        {
            Date = DateTime.Now;
            Title = String.Empty;
            Content = String.Empty;
        }

        public NoteModel(DateTime? date, string? title, string? content)
        {
            Title = title;
            Date = date;
            Content = content;
        }

        public NoteModel(int id, DateTime? date, string? title, string? content) : this(date, title, content)
        {
            Id = id;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
