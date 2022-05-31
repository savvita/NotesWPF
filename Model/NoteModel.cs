using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NotesWPF.Model
{
    public class NoteModel :INotifyPropertyChanged
    {
        private string title;

        public string Title
        {
            get => title;

            set 
            { 
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private DateTime date;

        public DateTime Date
        {
            get => date;

            set 
            { 
                date = value;
                OnPropertyChanged("Date");
            }
        }

        private string content;

        public string Content
        {
            get => content;

            set 
            { 
                content = value;
                OnPropertyChanged("Content");
            }
        }

        public NoteModel()
        {
            date = DateTime.Now;
            Title = String.Empty;
            Content = String.Empty;
        }

        public NoteModel(DateTime date, string title, string content)
        {
            Title = title;
            Date = date;
            Content = content;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
