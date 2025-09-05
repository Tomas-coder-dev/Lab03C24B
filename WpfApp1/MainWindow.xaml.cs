using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly string connectionString =
             "Data Source=LAB1502-001\\SQLEXPRESS;" +
             "Initial Catalog=Tecsup2023DB;" +
             "User Id=fabricioam;Pwd=fabricioam;" +
             "TrustServerCertificate=true;";

        public MainWindow()
        {
            InitializeComponent();
            LoadAllStudents();
        }

        private void LoadAllStudents()
        {
            var students = new List<Student>();

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT StudentId, FirstName, LastName FROM dbo.Students ORDER BY StudentId", conn))
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        students.Add(new Student
                        {
                            StudentId = rdr.GetInt32(rdr.GetOrdinal("StudentId")),
                            FirstName = rdr.GetString(rdr.GetOrdinal("FirstName")),
                            LastName = rdr.GetString(rdr.GetOrdinal("LastName"))
                        });
                    }
                }
            }

            dgEstudiantes.ItemsSource = students;
        }

        private List<Student> SearchStudents(string term)
        {
            var list = new List<Student>();

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(
                @"SELECT StudentId, FirstName, LastName
                  FROM dbo.Students
                  WHERE FirstName LIKE @q OR LastName LIKE @q
                  ORDER BY StudentId", conn))
            {
                cmd.Parameters.AddWithValue("@q", $"%{term}%");
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new Student
                        {
                            StudentId = rdr.GetInt32(rdr.GetOrdinal("StudentId")),
                            FirstName = rdr.GetString(rdr.GetOrdinal("FirstName")),
                            LastName = rdr.GetString(rdr.GetOrdinal("LastName"))
                        });
                    }
                }
            }

            return list;
        }

        // Evento Buscar
        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string term = txtNombre.Text.Trim();
            if (string.IsNullOrEmpty(term))
            {
                LoadAllStudents();
            }
            else
            {
                dgEstudiantes.ItemsSource = SearchStudents(term);
            }
        }
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}