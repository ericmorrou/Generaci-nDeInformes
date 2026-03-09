using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace GeneracionDeInformes
{
    public static class DatabaseHelper
    {
        private const string DbName = "ProductosRA5";
        private static readonly string DbFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{DbName}.sqlite");
        
        private static readonly string ConnectionString = $"Data Source={DbFileName};Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists(DbFileName))
            {
                SQLiteConnection.CreateFile(DbFileName);
                CreateAndSeedTable();
            }
        }

        private static void CreateAndSeedTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string createTableQuery = @"
                        CREATE TABLE Productos (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Nombre TEXT,
                            Precio REAL,
                            Categoria TEXT,
                            Stock INTEGER
                        )";

                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insert seed data
                    string insertQuery = @"
                        INSERT INTO Productos (Nombre, Precio, Categoria, Stock) VALUES 
                        ('Portátil Gaming', 1200.50, 'Electrónica', 15),
                        ('Ratón Inalámbrico', 25.99, 'Electrónica', 50),
                        ('Monitor 27', 250.00, 'Electrónica', 20),
                        ('Teclado Mecánico', 85.00, 'Electrónica', 30),
                        ('Auriculares USB', 45.00, 'Electrónica', 40),
                        
                        ('Silla de Oficina', 150.00, 'Mobiliario', 10),
                        ('Mesa Escritorio', 120.00, 'Mobiliario', 8),
                        ('Lámpara de Pie', 35.50, 'Mobiliario', 25),
                        ('Estantería', 65.00, 'Mobiliario', 12),
                        ('Armario', 210.00, 'Mobiliario', 5),

                        ('Cuaderno A4', 5.50, 'Papelería', 100),
                        ('Bolígrafo Azul', 1.20, 'Papelería', 500),
                        ('Paquete Folios', 6.00, 'Papelería', 80),
                        ('Rotuladores', 8.50, 'Papelería', 45),
                        ('Carpeta', 3.00, 'Papelería', 60)";

                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error insertando datos: " + ex.Message);
            }
        }

        public static DataTable GetProductos(string categoria = null)
        {
            DataTable table = new DataTable("Productos");
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT Nombre, Precio, Categoria, Stock FROM Productos";
                    
                    if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
                    {
                        query += " WHERE Categoria = @Categoria";
                    }

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
                        {
                            command.Parameters.AddWithValue("@Categoria", categoria);
                        }

                        using (var adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(table);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error obteniendo datos: " + ex.Message);
            }
            return table;
        }
    }
}
