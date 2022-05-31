using NotesWPF.Model;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NotesWPF.ViewModel
{
    public class FileController
    {
        private string notesFileName = "notes.csv".FullPath();

        public List<NoteModel>? Notes { get; private set; }

        public FileController()
        {
            LoadFromFile();
        }

        public void SaveToFile()
        {
            if (Notes != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (NoteModel note in Notes)
                {
                    sb.Append($"{note.Date.ToString()}||");
                    sb.Append($"{note.Title}||");
                    sb.Append($"{note.Content}||");

                    sb.AppendLine();
                }
                File.WriteAllText(notesFileName, sb.ToString());
            }
        }

        public void LoadFromFile()
        {
            try
            {
                Notes = notesFileName.LoadFromFile().ConvertToNotes();
            }
            catch
            {

            }
        }
    }
}
