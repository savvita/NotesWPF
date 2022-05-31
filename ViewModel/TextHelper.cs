using NotesWPF.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NotesWPF.ViewModel
{
    public static class TextHelper
    {
        public static string FullPath(this string fileName)
        {
            return Path.Combine(Settings.FilePath, fileName);
        }

        public static List<string> LoadFromFile(this string fileName)
        {
            if (!File.Exists(fileName))
            {
                return new List<string>();
            }

            return File.ReadAllLines(fileName).ToList();
        }

        public static List<NoteModel> ConvertToNotes(this List<string> lines)
        {
            List<NoteModel> notes = new List<NoteModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split("||");

                DateTime date = DateTime.Parse(cols[0]);

                notes.Add(new NoteModel(date, cols[1], cols[2]));
            }
            return notes;
        }
    }
}
