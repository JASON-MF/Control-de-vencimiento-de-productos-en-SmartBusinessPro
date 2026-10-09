using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBusinessPro
{
    // Definimos la clase Producto con sus atributos correspondientes
    class Producto
    {
        private DateTime _fechaVencimiento;

        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;

        // Utilizamos DateTime para almacenar la fecha de ingreso
        public DateTime FechaIngreso { get; set; }
        public int Cantidad { get; set; }

        // Encapsulamos la fecha de vencimiento validando que no sea menor a la fecha de ingreso
        public DateTime FechaVencimiento
        {
            get => _fechaVencimiento;
            set
            {
                if (value.Date < FechaIngreso.Date)
                {
                    throw new ArgumentException($"Error en '{Nombre}': La fecha de vencimiento ({value.ToString("dd/MM/yyyy")}) no puede ser anterior a la fecha de ingreso ({FechaIngreso.ToString("dd/MM/yyyy")}).");
                }
                _fechaVencimiento = value;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SmartBusinessPro - Control de Vencimientos";

            // Obtenemos la fecha actual del sistema
            DateTime fechaActual = DateTime.Now;

            // Calculamos la fecha límite para proyectar los próximos 30 días
            DateTime limite = fechaActual.AddDays(30);

            // Inicializamos la lista para almacenar los productos del inventario
            List<Producto> productos = new List<Producto>();

            try
            {
                // Registramos los productos con sus respectivas fechas de ingreso y vencimiento
                productos.Add(new Producto { Codigo = "PROD-101", Nombre = "Leche Entera 1L", FechaIngreso = fechaActual.AddDays(-30), FechaVencimiento = fechaActual.AddDays(-5), Cantidad = 45 });
                productos.Add(new Producto { Codigo = "PROD-102", Nombre = "Yogurt Griego 500g", FechaIngreso = fechaActual.AddDays(-15), FechaVencimiento = fechaActual.AddDays(7), Cantidad = 20 });
                productos.Add(new Producto { Codigo = "PROD-103", Nombre = "Queso Gouda 250g", FechaIngreso = fechaActual.AddDays(-20), FechaVencimiento = fechaActual.AddDays(25), Cantidad = 15 });
                productos.Add(new Producto { Codigo = "PROD-104", Nombre = "Cereal Avena 800g", FechaIngreso = fechaActual.AddDays(-60), FechaVencimiento = fechaActual.AddDays(120), Cantidad = 60 });
                productos.Add(new Producto { Codigo = "PROD-105", Nombre = "Pan Molde Integral", FechaIngreso = fechaActual.AddDays(-10), FechaVencimiento = fechaActual.AddDays(-1), Cantidad = 12 });
                productos.Add(new Producto { Codigo = "PROD-106", Nombre = "Jugo de Naranja 1L", FechaIngreso = fechaActual.AddDays(-12), FechaVencimiento = fechaActual.AddDays(18), Cantidad = 38 });
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[ERROR DE VALIDACIÓN]: {ex.Message}\n");
            }

            // Encabezado principal de la aplicación
            Console.WriteLine("=========================================================================================");
            Console.WriteLine("              SMARTBUSINESSPRO - CONTROL DE INVENTARIO Y VENCIMIENTOS                    ");
            Console.WriteLine("=========================================================================================");

            // Presentamos la fecha de análisis y la proyección formateadas
            Console.WriteLine($"Fecha de análisis : {fechaActual.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"Proyección 30 días: {limite.ToString("dd/MM/yyyy")}\n");

            // Ordenamos la lista de productos por fecha de vencimiento usando LINQ
            var productosOrdenados = productos.OrderBy(p => p.FechaVencimiento);

            // Presentamos los datos del inventario en formato de tabla
            Console.WriteLine("--- EVALUACIÓN DETALLADA DE INVENTARIO ---");
            Console.WriteLine("{0,-10} | {1,-20} | {2,-12} | {3,-12} | {4,-10} | {5,-12} | {6,-12} | {7,-12}",
                "Código", "Nombre", "F. Ingreso", "F. Vencim.", "Cantidad", "Almacenado", "Días Rest.", "Estado");
            Console.WriteLine(new string('-', 112));

            foreach (Producto producto in productosOrdenados)
            {
                // Calculamos el intervalo de tiempo entre fechas usando Subtract y TimeSpan
                TimeSpan diferenciaVencimiento = producto.FechaVencimiento.Subtract(fechaActual.Date);
                TimeSpan tiempoAlmacenado = fechaActual.Date.Subtract(producto.FechaIngreso.Date);

                // Evaluamos el estado del producto mediante comparaciones lógicas
                string estado;
                if (producto.FechaVencimiento < fechaActual.Date)
                {
                    estado = "VENCIDO";
                }
                else if (producto.FechaVencimiento <= limite.Date)
                {
                    estado = "POR VENCER";
                }
                else
                {
                    estado = "VIGENTE";
                }

                // Imprimimos la información formateando las fechas con ToString
                Console.WriteLine("{0,-10} | {1,-20} | {2,-12} | {3,-12} | {4,-10} | {5,-12} | {6,-12} | {7,-12}",
                    producto.Codigo,
                    producto.Nombre,
                    producto.FechaIngreso.ToString("dd/MM/yyyy"),
                    producto.FechaVencimiento.ToString("dd/MM/yyyy"),
                    producto.Cantidad,
                    $"{tiempoAlmacenado.Days} días",
                    $"{diferenciaVencimiento.Days} días",
                    estado);
            }

            Console.WriteLine(new string('-', 112));

            // Determinamos y mostramos los productos que se encuentran vencidos
            Console.WriteLine("\n--- PRODUCTOS VENCIDOS (RETIRAR DEL INVENTARIO) ---");
            foreach (Producto producto in productos)
            {
                if (producto.FechaVencimiento < fechaActual.Date)
                {
                    TimeSpan diferencia = producto.FechaVencimiento.Subtract(fechaActual.Date);
                    Console.WriteLine($"* [{producto.Codigo}] {producto.Nombre} | Vencido hace {Math.Abs(diferencia.Days)} día(s) | Cantidad afectada: {producto.Cantidad} unidades");
                }
            }

            // Determinamos y mostramos los productos que vencerán dentro de los próximos 30 días
            Console.WriteLine("\n--- PRODUCTOS QUE VENCERÁN EN LOS PRÓXIMOS 30 DÍAS ---");
            foreach (Producto producto in productos)
            {
                if (producto.FechaVencimiento >= fechaActual.Date && producto.FechaVencimiento <= limite.Date)
                {
                    TimeSpan diferencia = producto.FechaVencimiento.Subtract(fechaActual.Date);
                    Console.WriteLine($"* [{producto.Codigo}] {producto.Nombre} | Vence en {diferencia.Days} día(s) | Cantidad disponible: {producto.Cantidad} unidades");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}