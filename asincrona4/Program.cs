using System;
using System.Collections.Generic;
using System.Linq;

namespace EjemplosLinq
{
    // 1. Modelos de datos
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
    }

    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public decimal Monto { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 2. Colecciones de prueba
            List<Cliente> clientes = new List<Cliente>
            {
                new Cliente { Id = 1, Nombre = "Ana", Apellido = "García", Edad = 25 },
                new Cliente { Id = 2, Nombre = "Carlos", Apellido = "López", Edad = 17 },
                new Cliente { Id = 3, Nombre = "María", Apellido = "Rodríguez", Edad = 30 }
            };

            List<Pedido> pedidos = new List<Pedido>
            {
                new Pedido { Id = 101, ClienteId = 1, Monto = 150.50m },
                new Pedido { Id = 102, ClienteId = 1, Monto = 45.00m },
                new Pedido { Id = 103, ClienteId = 3, Monto = 210.00m }
            };

            Console.WriteLine("==================================================");
            Console.WriteLine("1. OPERADORES DE COMPARACIÓN (Filtrar mayores de edad >= 18)");
            Console.WriteLine("==================================================");

            // Uso del operador de comparación '>=' en el Where
            var clientesMayores = clientes.Where(c => c.Edad >= 18);

            foreach (var cliente in clientesMayores)
            {
                Console.WriteLine($"• {cliente.Nombre} {cliente.Apellido} - Edad: {cliente.Edad}");
            }

            Console.WriteLine("\n==================================================");
            Console.WriteLine("2. JOINS (Unir Clientes con sus Pedidos)");
            Console.WriteLine("==================================================");

            // Uso de 'join' relacionando IDs mediante 'equals'
            var pedidosPorCliente = from c in clientes
                                    join p in pedidos on c.Id equals p.ClienteId
                                    select new
                                    {
                                        ClienteNombre = c.Nombre,
                                        PedidoId = p.Id,
                                        Monto = p.Monto
                                    };

            foreach (var item in pedidosPorCliente)
            {
                Console.WriteLine($"• Cliente: {item.ClienteNombre} | Pedido #{item.PedidoId} | Monto: ${item.Monto}");
            }

            Console.WriteLine("\n==================================================");
            Console.WriteLine("3. GENERACIÓN DE LISTAS PERSONALIZADAS (Select / DTO)");
            Console.WriteLine("==================================================");

            // Proyección a una nueva lista con una estructura transformada
            var listaPersonalizada = clientes.Select(c => new
            {
                NombreCompleto = $"{c.Nombre} {c.Apellido}",
                EstadoEdad = c.Edad >= 18 ? "Mayor de edad" : "Menor de edad"
            }).ToList();

            foreach (var dto in listaPersonalizada)
            {
                Console.WriteLine($"• {dto.NombreCompleto} -> Clasificación: {dto.EstadoEdad}");
            }
        }
    }
}