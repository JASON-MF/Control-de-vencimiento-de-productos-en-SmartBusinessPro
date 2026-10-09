using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBusinessPro
{
    // Entidad Producto con la estructura de las diapositivas (CLS07 - Diapositiva 145)
    class Producto
    {
        private DateTime _fechaVencimiento;

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int Stock { get; set; }
        public DateTime FechaIngreso { get; set; }

        // Indicación 10: Validación para evitar FechaVencimiento < FechaIngreso
        public DateTime FechaVencimiento
        {
            get => _fechaVencimiento;
            set
            {
                if (value.Date < FechaIngreso.Date)
                {
                    throw new ArgumentException($"Error en '{Nombre}': La fecha de vencimiento ({value:dd/MM/yyyy}) no puede ser anterior a la de ingreso ({FechaIngreso:dd/MM/yyyy}).");
                }
                _fechaVencimiento = value;
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SmartBusinessPro - Control de Vencimientos";

            // Diapositiva 148: Establecimiento de fecha actual y cálculo del límite con AddDays()
            DateTime fechaActual = DateTime.Today;
            DateTime limite = fechaActual.AddDays(30); // Indicación 7: Próximos 30 días

            // Diapositivas 146-147: Inicialización de la colección de productos
            List<Producto> productos = new List<Producto>();

            try
            {
                productos.Add(new Producto { Id = 101, Nombre = "Leche Entera 1L", Categoria = "Lácteos", Stock = 45, FechaIngreso = fechaActual.AddDays(-30), FechaVencimiento = fechaActual.AddDays(-5) });
                productos.Add(new Producto { Id = 102, Nombre = "Yogurt Griego 500g", Categoria = "Lácteos", Stock = 20, FechaIngreso = fechaActual.AddDays(-15), FechaVencimiento = fechaActual.AddDays(7) });
                productos.Add(new Producto { Id = 103, Nombre = "Queso Gouda 250g", Categoria = "Lácteos", Stock = 15, FechaIngreso = fechaActual.AddDays(-20), FechaVencimiento = fechaActual.AddDays(25) });
                productos.Add(new Producto { Id = 104, Nombre = "Cereal Avena 800g", Categoria = "Abarrotes", Stock = 60, FechaIngreso = fechaActual.AddDays(-60), FechaVencimiento = fechaActual.AddDays(120) });
                productos.Add(new Producto { Id = 105, Nombre = "Pan Molde Integral", Categoria = "Panadería", Stock = 12, FechaIngreso = fechaActual.AddDays(-10), FechaVencimiento = fechaActual.AddDays(-1) });
                productos.Add(new Producto { Id = 106, Nombre = "Jugo de Naranja 1L", Categoria = "Bebidas", Stock = 38, FechaIngreso = fechaActual.AddDays(-12), FechaVencimiento = fechaActual.AddDays(18) });
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR DE VALIDACIÓN]: {ex.Message}\n");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=================================================");
            Console.WriteLine("    SMARTBUSINESSPRO - CONTROL DE VENCIMIENTOS    ");
            Console.WriteLine("=================================================");
            Console.ResetColor();

            // Diapositiva 148 y 141: Presentación y formato de fecha con ToString()
            Console.WriteLine($"Fecha de análisis : {fechaActual:dd/MM/yyyy}");
            Console.WriteLine($"Proyección 30 días: {limite:dd/MM/yyyy}\n");

            // Diapositiva 148: Consulta LINQ para ordenar por fecha de vencimiento
            var productosOrdenados = productos.OrderBy(p => p.FechaVencimiento);

            Console.WriteLine("--- EVALUACIÓN DETALLADA DE INVENTARIO ---");

            // Diapositivas 149-150: Bucle de procesamiento, resta de TimeSpan y evaluación lógica
            foreach (Producto producto in productosOrdenados)
            {
                // Resta con DateTime que devuelve un TimeSpan (Diapositivas 136 y 149)
                TimeSpan diferenciaVencimiento = producto.FechaVencimiento - fechaActual;
                TimeSpan tiempoAlmacenado = fechaActual - producto.FechaIngreso;

                Console.WriteLine($"ID: {producto.Id} | Producto: {producto.Nombre}");
                Console.WriteLine($"Categoría : {producto.Categoria}");
                Console.WriteLine($"F. Ingreso: {producto.FechaIngreso:dd/MM/yyyy} ({tiempoAlmacenado.Days} días almacenado)");
                Console.WriteLine($"F. Vence  : {producto.FechaVencimiento:dd/MM/yyyy}");
                Console.WriteLine($"Stock     : {producto.Stock} unidades");

                // Evaluaciones condicionales iguales a las Diapositivas 137 y 150
                if (producto.FechaVencimiento < fechaActual)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Estado    : VENCIDO");
                    Console.WriteLine($"Detalle   : Venció hace {Math.Abs(diferenciaVencimiento.Days)} día(s). Retirar de estantería.");
                }
                else if (producto.FechaVencimiento <= limite)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Estado    : VENCE PRÓXIMAMENTE (Próximos 30 días)");
                    Console.WriteLine($"Detalle   : Quedan {diferenciaVencimiento.Days} día(s) para su vencimiento.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Estado    : VIGENTE");
                    Console.WriteLine($"Detalle   : Quedan {diferenciaVencimiento.Days} día(s) de vida útil.");
                }

                Console.ResetColor();
                Console.WriteLine(new string('-', 50)); // Separador usado en diapositiva 150
            }

            // Resumen de conteos utilizando sintaxis LINQ visto en la clase
            Console.WriteLine("\n--- RESUMEN EJECUTIVO DE INVENTARIO ---");
            Console.WriteLine($"Total Analizados : {productos.Count}");
            Console.WriteLine($"Vigentes         : {productos.Count(p => p.FechaVencimiento > limite)}");
            Console.WriteLine($"Por Vencer (<=30d): {productos.Count(p => p.FechaVencimiento >= fechaActual && p.FechaVencimiento <= limite)}");
            Console.WriteLine($"Vencidos         : {productos.Count(p => p.FechaVencimiento < fechaActual)}");

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}