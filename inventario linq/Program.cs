using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventario
{
    class Productos
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public double Precio { get; set; }
        public int Existencias { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Productos> ListaProductos = new List<Productos>();
            int Opcion = 0;

            while (Opcion != 6)
            {
                Console.Clear();
                Console.WriteLine("-------------- Gestión de Productos-----------");
                Console.WriteLine("1. Agregar Nuevo Producto");
                Console.WriteLine("2. Consultas sobre los productos");
                Console.WriteLine("3. Buscar Un Producto");
                Console.WriteLine("4. Modificar un Producto");
                Console.WriteLine("5. Eliminar un Producto");
                Console.WriteLine("6. Salir del Programa");
                Console.Write("Seleccione una opción: ");
                Opcion = Convert.ToInt32(Console.ReadLine());

                switch (Opcion)
                {
                    case 1:
                        AgregarProducto(ListaProductos);
                        break;
                    case 2:
                        MenuConsultas(ListaProductos);
                        break;
                    case 3:
                        BuscarProducto(ListaProductos);
                        break;
                    case 4:
                        ModificarProducto(ListaProductos);
                        break;
                    case 5:
                        EliminarProducto(ListaProductos);
                        break;
                    case 6:
                        Console.WriteLine("Saliendo del programa...");
                        break;
                    default:
                        Console.WriteLine("Opción no encontrada o no es válida");
                        break;
                }

                if (Opcion != 6)
                {
                    Console.WriteLine("\nPRESIONE UNA TECLA PARA CONTINUAR....");
                    Console.ReadKey();
                }
            }
        }

        // ---------------- Agregar ----------------
        static void AgregarProducto(List<Productos> lista)
        {
            Productos nuevo = new Productos();

            Console.WriteLine("Escriba el Código del Producto: ");
            nuevo.Codigo = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Ingrese el nombre del Producto: ");
            nuevo.Nombre = Console.ReadLine();

            Console.WriteLine("Ingrese el nombre de la categoría del Producto: ");
            nuevo.Categoria = Console.ReadLine();

            Console.WriteLine("Ingrese el Precio del Producto: ");
            nuevo.Precio = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Ingrese la cantidad de Existencias del Producto: ");
            nuevo.Existencias = Convert.ToInt32(Console.ReadLine());

            lista.Add(nuevo);
            Console.WriteLine("Producto agregado correctamente.");
        }

        // ---------------- Utilidad para mostrar ----------------
        static void MostrarLista(List<Productos> lista, string titulo)
        {
            Console.WriteLine($"\n ------------ {titulo} ----------------\n");
            if (lista.Count == 0)
            {
                Console.WriteLine("Sin registros en el inventario");
                return;
            }

            foreach (Productos p in lista)
            {
                Console.WriteLine($"Codigo: {p.Codigo}");
                Console.WriteLine($"Nombre: {p.Nombre}");
                Console.WriteLine($"Categoria: {p.Categoria}");
                Console.WriteLine($"Precio: {p.Precio:F2}");
                Console.WriteLine($"Existencias: {p.Existencias}");
                Console.WriteLine("------------------------------------------");
            }
        }

        // ---------------- Consultas ----------------
        static void MenuConsultas(List<Productos> lista)
        {
            Console.Clear();
            Console.WriteLine("---------- CONSULTA A LA LISTA DE PRODUCTOS --------------");
            Console.WriteLine("1- Mostrar todos los productos");
            Console.WriteLine("2- Ordenar productos por nombre");
            Console.WriteLine("3- Ordenar Productos por precio");
            Console.WriteLine("4- Ordenar productos por existencias");
            Console.WriteLine("5- Filtrar productos por categoría");
            Console.Write("Seleccione una opción: ");
            int opConsultas = Convert.ToInt32(Console.ReadLine());

            switch (opConsultas)
            {
                case 1:
                    MostrarLista(lista, "LISTA DE PRODUCTOS EN INVENTARIO");
                    break;

                case 2:
                    var porNombre = lista.OrderBy(p => p.Nombre).ToList();
                    MostrarLista(porNombre, "PRODUCTOS ORDENADOS POR NOMBRE");
                    break;

                case 3:
                    var porPrecio = lista.OrderBy(p => p.Precio).ToList();
                    MostrarLista(porPrecio, "PRODUCTOS ORDENADOS POR PRECIO");
                    break;

                case 4:
                    var porExistencias = lista.OrderByDescending(p => p.Existencias).ToList();
                    MostrarLista(porExistencias, "PRODUCTOS ORDENADOS POR EXISTENCIAS");
                    break;

                case 5:
                    Console.WriteLine("Ingrese el nombre de la categoría que desea consultar: ");
                    string categoriaBuscar = Console.ReadLine();
                    var porCategoria = lista
                        .Where(p => p.Categoria.Equals(categoriaBuscar, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (porCategoria.Count == 0)
                        Console.WriteLine("No existe la categoría o no existen productos en esa categoría");
                    else
                        MostrarLista(porCategoria, $"PRODUCTOS DE LA CATEGORÍA: {categoriaBuscar}");
                    break;

                default:
                    Console.WriteLine("Opción no encontrada o no es válida");
                    break;
            }
        }

        // ---------------- Buscar ----------------
        static void BuscarProducto(List<Productos> lista)
        {
            Console.Clear();
            Console.WriteLine("---------- BUSCAR UN PRODUCTO --------------");
            Console.WriteLine("1- Buscar por Código");
            Console.WriteLine("2- Buscar por Nombre");
            Console.WriteLine("3- Buscar por Categoría");
            Console.Write("Seleccione una opción: ");
            int opBuscar = Convert.ToInt32(Console.ReadLine());

            switch (opBuscar)
            {
                case 1:
                    Console.WriteLine("Ingrese el Código del Producto que desea buscar: ");
                    int codBuscar = Convert.ToInt32(Console.ReadLine());
                    Productos proCodigo = lista.FirstOrDefault(p => p.Codigo == codBuscar);

                    if (proCodigo != null)
                        MostrarLista(new List<Productos> { proCodigo }, "PRODUCTO ENCONTRADO");
                    else
                        Console.WriteLine("No se encontró el producto con el código ingresado");
                    break;

                case 2:
                    Console.WriteLine("Ingrese el Nombre del Producto que desea buscar: ");
                    string nomBuscar = Console.ReadLine();
                    var proNombre = lista
                        .Where(p => p.Nombre.Equals(nomBuscar, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (proNombre.Count > 0)
                        MostrarLista(proNombre, "PRODUCTOS ENCONTRADOS");
                    else
                        Console.WriteLine("No se encontró ningún producto con ese nombre");
                    break;

                case 3:
                    Console.WriteLine("Ingrese la Categoría del Producto que desea buscar: ");
                    string catBuscar = Console.ReadLine();
                    var proCategoria = lista
                        .Where(p => p.Categoria.Equals(catBuscar, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (proCategoria.Count > 0)
                        MostrarLista(proCategoria, "PRODUCTOS ENCONTRADOS");
                    else
                        Console.WriteLine("No se encontró ningún producto en esa categoría");
                    break;

                default:
                    Console.WriteLine("La opción que eligió no está dentro del menú de opciones");
                    break;
            }
        }

        // ---------------- Modificar ----------------
        static void ModificarProducto(List<Productos> lista)
        {
            Console.Clear();
            Console.WriteLine("---------- MODIFICAR UN PRODUCTO --------------");
            Console.WriteLine("Ingrese el Código del Producto que desea modificar: ");
            int codModificar = Convert.ToInt32(Console.ReadLine());
            Productos producto = lista.FirstOrDefault(p => p.Codigo == codModificar);

            if (producto == null)
            {
                Console.WriteLine("No se encontró el producto con el código ingresado");
                return;
            }

            Console.WriteLine($"Nombre actual: {producto.Nombre}. Nuevo nombre (Enter para dejar igual): ");
            string nombre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nombre)) producto.Nombre = nombre;

            Console.WriteLine($"Categoría actual: {producto.Categoria}. Nueva categoría (Enter para dejar igual): ");
            string categoria = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(categoria)) producto.Categoria = categoria;

            Console.WriteLine($"Precio actual: {producto.Precio:F2}. Nuevo precio (Enter para dejar igual): ");
            string precioStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(precioStr)) producto.Precio = Convert.ToDouble(precioStr);

            Console.WriteLine($"Existencias actuales: {producto.Existencias}. Nueva cantidad (Enter para dejar igual): ");
            string existStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(existStr)) producto.Existencias = Convert.ToInt32(existStr);

            Console.WriteLine("Producto modificado correctamente.");
        }

        // ---------------- Eliminar ----------------
        static void EliminarProducto(List<Productos> lista)
        {
            Console.Clear();
            Console.WriteLine("---------- ELIMINAR UN PRODUCTO --------------");
            Console.WriteLine("Ingrese el Código del Producto que desea eliminar: ");
            int codEliminar = Convert.ToInt32(Console.ReadLine());
            Productos producto = lista.FirstOrDefault(p => p.Codigo == codEliminar);

            if (producto == null)
            {
                Console.WriteLine("No se encontró el producto con el código ingresado");
                return;
            }

            Console.WriteLine($"¿Está seguro que desea eliminar el producto '{producto.Nombre}'? (S/N): ");
            string confirmacion = Console.ReadLine();

            if (confirmacion.Trim().Equals("S", StringComparison.OrdinalIgnoreCase))
            {
                lista.Remove(producto);
                Console.WriteLine("Producto eliminado correctamente.");
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
            }
        }
    }
}