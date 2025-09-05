using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace WpfApp1
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public partial class MainWindow : Window
    {
        static string connectionString =
            "Data Source=LAB1502-001\\SQLEXPRESS;" +
            "Initial Catalog=Tecsup2023DB;" +
            "User Id=fabricioam; Pwd=fabricioam; " +
            "TrustServerCertificate=true;";

        public MainWindow()
        {
            InitializeComponent();
            CargarEstudiantes(); // Muestra todos al iniciar
        }

        // 7. Cargar estudiantes usando DataReader
        private List<Student> ObtenerEstudiantes(string filtroNombre = "")
        {
            var estudiantes = new List<Student>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT StudentId, FirstName, LastName FROM dbo.Students";
                if (!string.IsNullOrWhiteSpace(filtroNombre))
                {
                    query += " WHERE FirstName LIKE @nombre OR LastName LIKE @nombre";
                }
                using (var cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(filtroNombre))
                        cmd.Parameters.AddWithValue("@nombre", "%" + filtroNombre + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            estudiantes.Add(new Student
                            {
                                StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName"))
                            });
                        }
                    }
                }
            }
            return estudiantes;
        }

        private void CargarEstudiantes(string filtroNombre = "")
        {
            dgEstudiantes.ItemsSource = ObtenerEstudiantes(filtroNombre);
        }

        // 8. Buscar por nombre
        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            CargarEstudiantes(nombre);
        }

        // Mostrar todos
        private void BtnMostrarTodo_Click(object sender, RoutedEventArgs e)
        {
            txtNombre.Text = "";
            CargarEstudiantes();
        }
    }
}