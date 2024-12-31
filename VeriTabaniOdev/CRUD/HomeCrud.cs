using System.Data;
using Npgsql;

namespace VeriTabaniOdev.CRUD
{
    public class HomeCrud
    {
        readonly string connectionString = "Host=localhost;Username=postgres;Password=1234;Database=mydatabase;";

        public IEnumerable<Instructor> GetInstructors()
        {
            var Instructors = new List<Instructor>();

            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM Instructor", con);
                command.Connection = con;
                command.CommandType = CommandType.Text;
                con.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var list = new Instructor();
                    list.Id = Convert.ToInt32(reader["id"].ToString());
                    list.Name = reader["name"].ToString();
                    list.LastName = reader["lastname"].ToString();
                    list.Branch = reader["branch"].ToString();

                    Instructors.Add(list);
                }
                con.Close();
            }
            return Instructors;
        }

        public Instructor GetInstructorsById(int Id)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                Instructor response = new Instructor();

                string query = $"SELECT * FROM Instructor WHERE id = {Id}";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Connection = con;
                command.CommandType = CommandType.Text;
                con.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Instructor read = new Instructor();
                    read.Id = Convert.ToInt32(reader["id"].ToString());
                    read.Name = reader["name"].ToString();
                    read.LastName = reader["lastname"].ToString();
                    read.Branch = reader["branch"].ToString();

                    response = read;
                }
                con.Close();
                return response;
            }
        }

        public void CreateInstructors(Instructor report)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                string query = "INSERT INTO Instructor (name, lastname, branch) VALUES (@name, @lastname, @branch)";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@name", report.Name);
                command.Parameters.AddWithValue("@lastname", report.LastName);
                command.Parameters.AddWithValue("@branch", report.Branch);

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public void UpdateInstructors(Instructor report)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                string query = "UPDATE Instructor SET name = @name, lastname = @lastname, branch = @branch WHERE id = @id";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@id", report.Id);
                command.Parameters.AddWithValue("@name", report.Name);
                command.Parameters.AddWithValue("@lastname", report.LastName);
                command.Parameters.AddWithValue("@branch", report.Branch);

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public void Delete(int Id)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                string query = "DELETE FROM Instructor WHERE id = @id";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.Parameters.AddWithValue("@id", Id);

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public class Instructor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Branch { get; set; }
    }
}
