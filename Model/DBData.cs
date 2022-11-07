using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NotesWPF.Model
{
    public static class DBData
    {
        public static async Task<List<NoteModel>> SelectAll()
        {

            try
            {
                using (SqlCommand command = new SqlCommand($"select * from [Notes]", DBConnection.Connection()))
                {
                    try
                    {
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        List<NoteModel> notes = new List<NoteModel>();

                        while (reader.Read())
                        {
                            notes.Add(new NoteModel(reader.GetInt32(0), reader.GetDateTime(1), reader.GetString(2), reader.GetString(3)));
                        }

                        reader.Close();

                        return notes;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("SQL command error", ex);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot connect to the database", ex);
            }
        }

        private static bool InsertNote(SqlConnection connection, NoteModel note)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("insert into [Notes] ([Date], [Title], [Content]) values (@p0, @p1, @p2);", connection))
                {
                    SetParameters(command, note);

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error of the SQL command", ex);
            }
        }

        public static void Insert(List<NoteModel> notes)
        {
            if (notes.Count > 0)
            {
                try
                {

                    for (int i = 0; i < notes.Count; i++)
                    {
                        InsertNote(DBConnection.Connection(), notes[i]);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Cannot connect to the database", ex);
                }
            }
        }

        public static bool Update(NoteModel note)
        {
            if (note.Id == null)
            {
                return false;
            }


            try
            {
                using (SqlCommand command = new SqlCommand($"update [Notes] set [Date]=@p0, [Title]=@p1, [Content]=@p2 where id={note.Id};", DBConnection.Connection()))
                {
                    SetParameters(command, note);

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error of the SQL command", ex);
            }
        }

        public static bool Delete(NoteModel note)
        {
            if (note.Id == null)
            {
                return false;
            }

            try
            {

                using (SqlCommand command = new SqlCommand($"delete from[Notes] where [id]=@p0;", DBConnection.Connection()))
                {
                    command.Parameters.AddWithValue("@p0", note.Id);

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error of the SQL command", ex);
            }

        }


        private static void SetParameters(SqlCommand command, NoteModel note)
        {
            command.Parameters.AddWithValue("@p0", note.Date);
            if (note.Title != null)
            {
                command.Parameters.AddWithValue("@p1", note.Title);
            }
            else
            {
                command.Parameters.AddWithValue("@p1", DBNull.Value);
            }

            if (note.Content != null)
            {
                command.Parameters.AddWithValue("@p2", note.Content);
            }
            else
            {
                command.Parameters.AddWithValue("@p2", DBNull.Value);
            }
        }
    }
}
