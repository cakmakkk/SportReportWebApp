using Npgsql;
using System.Data;

namespace MemberManager.DataAccess
{
    public class LessonCRUD
    {
        readonly string connectionString = "Host=localhost;Username=postgres;Password=1234;Database=mydatabase;";

        public IEnumerable<Lesson> GetLessons()
        {
            var Lessons = new List<Lesson>();

            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM Lesson", con);
                command.Connection = con;
                command.CommandType = CommandType.Text;
                con.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var list = new Lesson();
                    list.Id = Convert.ToInt32(reader["id"].ToString());
                    list.Name = reader["name"].ToString();
                    list.Quota = Convert.ToInt32(reader["quota"].ToString());

                    Lessons.Add(list);
                }
                con.Close();
            }
            return Lessons;
        }

        public Lesson GetLessonsById(int Id)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                Lesson response = new Lesson();

                string query = $"SELECT * FROM Lesson WHERE id = {Id}";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Connection = con;
                command.CommandType = CommandType.Text;
                con.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Lesson read = new Lesson();
                    read.Id = Convert.ToInt32(reader["id"].ToString());
                    read.Name = reader["name"].ToString();
                    read.Quota = Convert.ToInt32(reader["quota"].ToString());

                    response = read;
                }
                con.Close();
                return response;
            }
        }

        public void CreateLessons(Lesson report)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                string query = "INSERT INTO Lesson (name, quota) VALUES (@name, @quota)";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@name", report.Name);
                command.Parameters.AddWithValue("@quota", report.Quota);

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public void UpdateLessons(Lesson report)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                string query = "UPDATE Lesson SET name = @name, quota = @quota WHERE id = @id";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@id", report.Id);
                command.Parameters.AddWithValue("@name", report.Name);
                command.Parameters.AddWithValue("@quota", report.Quota);

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public void Delete(int Id)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                string query = "DELETE FROM Lesson WHERE id = @id";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@id", Id);

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quota { get; set; }
    }
}
