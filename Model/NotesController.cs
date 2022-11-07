using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NotesWPF.Model
{
    public class NotesController : IDisposable, INotifyPropertyChanged
    {
        private ObservableCollection<NoteModel>? notes;
        public ObservableCollection<NoteModel>? Notes
        {
            get => notes;

            private set
            {
                notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        public event Action? Loaded;

        public NotesController()
        {
            Initialize();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private async void Initialize()
        {
            Notes = new ObservableCollection<NoteModel>(await DBData.SelectAll());
            Loaded?.Invoke();
        }

        public void Add(NoteModel note)
        {
            Notes?.Add(note);
        }

        public async Task Remove(NoteModel note)
        {
            Notes?.Remove(note);

            try
            {
                await DBData.Delete(note);
            }
            catch { }
        }

        public async Task<bool> Update(NoteModel newNote)
        {
            try
            {
                return await DBData.Update(newNote);
            }
            catch { }

            return false;
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (Notes != null)
            {
                DBData.Insert(Notes.Where(note => note.Id == null).ToList());
            }
        }
    }
}
