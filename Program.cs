

using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Data.SqlClient;

namespace MongoDB_SQLServerExample
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("--- Conectando C# a MongoDB Server ---");

            // 1. Configuración de la cadena de conexión
            string connectionString = "mongodb://localhost:27017";


            // 2. Crear el cliente de MongoDB para conectarse al servidor
            var client = new MongoClient(connectionString);

            try
            {
                // 3. Acceder a la base de datos y a la colección
                var database = client.GetDatabase("social_network");
                var collection = database.GetCollection<BsonDocument>("posts");

                Console.WriteLine("--- Conectando a MongoDB  ---");
                Console.WriteLine("Listando contenido de la colección 'posts':\n");

                // 4. Obtener todos los documentos (await válido dentro de método async)
                var documents = await collection.Find(new BsonDocument()).ToListAsync();

                if (documents.Count == 0)
                {
                    Console.WriteLine("No se encontraron documentos en la colección posts.");
                }
                else
                {
                    foreach (var doc in documents)
                    {
                        Console.WriteLine(doc.ToJson(new MongoDB.Bson.IO.JsonWriterSettings { Indent = true }));
                        Console.WriteLine(new string('-', 30));
                    }
                    //Console.WriteLine("\n");
                    //Console.WriteLine("\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
            }




            
             Console.WriteLine("\n--- Conectando a SQL Server Express ---");

             // Configuración de la cadena de conexión para SQL Server local
             string sqlConnectionString = "Server=DESKTOP-R9RN64N\\SQLEXPRESS;Database=social_network;Integrated Security=True;Encrypt=False;";

             try
             {
                 using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                 {
                     await connection.OpenAsync();
                     Console.WriteLine("Conexión a SQL Server exitosa.");

                     // Consulta para listar el contenido de la tabla posts
                     string query = "SELECT * FROM posts";
                     using (SqlCommand command = new SqlCommand(query, connection))
                     {
                         using (SqlDataReader reader = await command.ExecuteReaderAsync())
                         {
                             Console.WriteLine("Listando contenido de la tabla 'posts':\n");

                             if (!reader.HasRows)
                             {
                                 Console.WriteLine("No se encontraron registros en la tabla.");
                             }
                             else
                             {
                                 while (await reader.ReadAsync())
                                 {
                                     // Asumiendo que la tabla posts tiene columnas como id, title, content, etc.
                                     // Ajusta según la estructura real de la tabla
                                     Console.WriteLine($"title: {reader["title"]}, category: {reader["category"]}, likes: {reader["likes"]}");
                                     Console.WriteLine("\n");
                                     Console.WriteLine("\n");
                                     Console.WriteLine("\n");

                                 }
                             }
                         }
                     }
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Error de conexión a SQL Server: {ex.Message}");
             }
             
        }
    }
}