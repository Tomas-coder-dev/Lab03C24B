using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Lab03C24B
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"ID: {StudentId}, Nombre: {FirstName}, Apellido: {LastName}";
        }
    }

    internal class Program
    {
        // Usa la base de datos y tabla correctas según tu imagen Tecsup2023DB y dbo.Students
        static string connectionString =
            "Data Source=LAB1502-001\\SQLEXPRESS;" +
            "Initial Catalog=Tecsup2023DB;" +
            "User Id=fabricioam; Pwd=fabricioam; " +
            "TrustServerCertificate=true;";

        // Recuperar estudiantes usando DataTable (desconectado)
        public static DataTable GetStudentsWithDataTable()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT StudentId, FirstName, LastName FROM dbo.Students", conn);
                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        // Recuperar estudiantes usando DataReader y lista de objetos (conectado)
        public static List<Student> GetStudentsWithObjectList()
        {
            var students = new List<Student>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT StudentId, FirstName, LastName FROM dbo.Students", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName"))
                    });
                }
            }
            return students;
        }

        static void Main(string[] args)
        {
            // Mostrar estudiantes usando DataTable
            Console.WriteLine("Listado estudiantes usando DataTable:");
            var dt = GetStudentsWithDataTable();
            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine($"ID: {row["StudentId"]}, Nombre: {row["FirstName"]}, Apellido: {row["LastName"]}");
            }

            // Mostrar estudiantes usando lista de objetos
            Console.WriteLine("\nListado estudiantes usando lista de objetos:");
            var list = GetStudentsWithObjectList();
            foreach (var student in list)
            {
                Console.WriteLine(student.ToString());
            }
        }
    }
}