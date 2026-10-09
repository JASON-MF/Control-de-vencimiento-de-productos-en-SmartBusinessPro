using System;
using System.Collections.Generic;
using System.Data.SqlClient; 
using System.Linq;

namespace ColeccionProductosSmartBusiness
{
    // Modelo de datos mapeado con la base de datos SmartBusinessPro
    public class Producto
    {
        public int ProductoId { get; set; }
        public int CategoriaId { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
    }

    internal class Program
    {
        // Cadena de conexión hacia tu instancia de SQL Server
        private static readonly string connectionString = @"Server=JASON;Database=SmartBusinessPro;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";



        static void Main(string[] args)
        {
            // 1. Carga inicial desde la Base de Datos SQL Server
            List<Producto> productos = CargarProductosDesdeSQL();

            if (productos.Count == 0)
            {
                Console.WriteLine("⚠️ No se cargaron productos o hubo un fallo de conexión a SQL Server.");
                Console.WriteLine("Compruebe que el servicio de SQL Server esté activo y presione una tecla para salir...");
                Console.ReadKey();
                return;
            }

            int opcion = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("==========================================================");
                Console.WriteLine("       CONSULTA A LA LISTA DE PRODUCTOS (SmartBusinessPro)");
                Console.WriteLine("==========================================================");
                Console.WriteLine("1- Mostrar todos los productos");
                Console.WriteLine("2- Ordenar productos por nombre (A-Z)");
                Console.WriteLine("3- Ordenar productos por precio (Mayor a Menor)");
                Console.WriteLine("4- Reporte de Inventario Crítico (Stock < 6)");
                Console.WriteLine("5- Filtrar productos por categoría");
                Console.WriteLine("6- Ver estadísticas del inventario (Más costoso y Promedio)");
                Console.WriteLine("7- Salir");
                Console.WriteLine("==========================================================");
                Console.Write("Seleccione una opción: ");

                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine();
                    switch (opcion)
                    {
                        case 1:
                            MostrarLista("LISTA GENERAL DE PRODUCTOS EN INVENTARIO", productos);
                            break;

                        case 2:
                            // Ordenamiento LINQ por nombre (A-Z)
                            var porNombre = productos.OrderBy(p => p.Nombre).ToList();
                            MostrarLista("PRODUCTOS ORDENADOS POR NOMBRE (A-Z)", porNombre);
                            break;

                        case 3:
                            // Ordenamiento LINQ descendente por precio
                            var porPrecio = productos.OrderByDescending(p => p.Precio).ToList();
                            MostrarLista("PRODUCTOS ORDENADOS POR PRECIO (MAYOR A MENOR)", porPrecio);
                            break;

                        case 4:
                            // Filtro LINQ: Inventario Crítico (Stock < 6) con orden por Categoría y Stock
                            var inventarioCritico = productos
                                .Where(p => p.Stock < 6)
                                .OrderBy(p => p.Categoria)
                                .ThenBy(p => p.Stock)
                                .ToList();
                            MostrarLista("REPORTE DE INVENTARIO CRÍTICO (STOCK < 6)", inventarioCritico);
                            break;

                        case 5:
                            FiltrarPorCategoria(productos);
                            break;

                        case 6:
                            MostrarEstadisticas(productos);
                            break;

                        case 7:
                            Console.WriteLine("Saliendo del sistema SmartBusinessPro...");
                            break;

                        default:
                            Console.WriteLine("❌ Opción no válida. Intente de nuevo.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("❌ Entrada inválida. Por favor ingrese un número entero.");
                }

                if (opcion != 7)
                {
                    Console.WriteLine("\nPRESIONE UNA TECLA PARA CONTINUAR...");
                    Console.ReadKey();
                }

            } while (opcion != 7);
        }

        // Método que realiza la conexión y consulta a SQL Server mediante ADO.NET
        private static List<Producto> CargarProductosDesdeSQL()
        {
            List<Producto> lista = new List<Producto>();

            string sql = @"SELECT p.ProductoId, p.CategoriaId, p.Nombre, c.Nombre AS Categoria, p.Precio, p.Stock, p.Activo
                           FROM Productos AS p
                           INNER JOIN Categorias AS c ON p.CategoriaId = c.CategoriaId
                           WHERE p.Activo = 1;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto p = new Producto
                            {
                                ProductoId = reader["ProductoId"] != DBNull.Value ? Convert.ToInt32(reader["ProductoId"]) : 0,
                                CategoriaId = reader["CategoriaId"] != DBNull.Value ? Convert.ToInt32(reader["CategoriaId"]) : 0,
                                Nombre = reader["Nombre"] != DBNull.Value ? reader["Nombre"].ToString() : "Sin Nombre",
                                Categoria = reader["Categoria"] != DBNull.Value ? reader["Categoria"].ToString() : "Sin Categoría",
                                Precio = reader["Precio"] != DBNull.Value ? Convert.ToDecimal(reader["Precio"]) : 0m,
                                Stock = reader["Stock"] != DBNull.Value ? Convert.ToInt32(reader["Stock"]) : 0,
                                Activo = reader["Activo"] != DBNull.Value && Convert.ToBoolean(reader["Activo"])
                            };
                            lista.Add(p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al conectar o consultar SQL Server: {ex.Message}");
            }

            return lista;
        }

        // Muestra la lista de productos formateada en consola
        private static void MostrarLista(string titulo, List<Producto> lista)
        {
            Console.WriteLine($"----------- {titulo} -----------");
            Console.WriteLine();

            if (lista == null || lista.Count == 0)
            {
                Console.WriteLine("⚠️ No se encontraron registros para mostrar.");
                return;
            }

            Console.WriteLine($"{"ID",-5} | {"PRODUCTO",-25} | {"CATEGORÍA",-15} | {"PRECIO",-10} | {"STOCK",-5}");
            Console.WriteLine(new string('-', 70));

            foreach (var p in lista)
            {
                Console.WriteLine($"{p.ProductoId,-5} | {p.Nombre,-25} | {p.Categoria,-15} | {p.Precio,10:C2} | {p.Stock,5}");
            }
        }

        // Búsqueda y filtrado dinámico por categoría
        private static void FiltrarPorCategoria(List<Producto> productos)
        {
            Console.Write("Ingrese la categoría a buscar: ");
            string catBuscada = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(catBuscada))
            {
                Console.WriteLine("⚠️ Debe ingresar el nombre de una categoría.");
                return;
            }

            var filtrados = productos
                .Where(p => p.Categoria != null && p.Categoria.IndexOf(catBuscada, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            MostrarLista($"PRODUCTOS DE LA CATEGORÍA: {catBuscada.ToUpper()}", filtrados);
        }

        // Método de estadísticas utilizando métodos de extensión LINQ
        private static void MostrarEstadisticas(List<Producto> productos)
        {
            Console.WriteLine("----------- ESTADÍSTICAS DEL INVENTARIO -----------");

            if (productos == null || productos.Count == 0)
            {
                Console.WriteLine("⚠️ No hay datos para calcular estadísticas.");
                return;
            }

            // LINQ: FirstOrDefault para obtener el producto más costoso
            var masCostoso = productos.OrderByDescending(p => p.Precio).FirstOrDefault();
            if (masCostoso != null)
            {
                Console.WriteLine($"⭐ Producto más costoso : {masCostoso.Nombre} ({masCostoso.Categoria}) - {masCostoso.Precio:C2}");
            }

            // LINQ: Average para calcular el precio promedio
            decimal precioPromedio = productos.Average(p => p.Precio);
            Console.WriteLine($"📊 Precio Promedio General : {precioPromedio:C2}");

            // LINQ: Sum para calcular existencias totales
            int totalStock = productos.Sum(p => p.Stock);
            Console.WriteLine($"📦 Stock Total en Almacén  : {totalStock} unidades");
        }
    }
}