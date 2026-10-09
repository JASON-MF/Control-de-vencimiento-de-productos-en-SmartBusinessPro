using System;

namespace SmartBusinessPro.Estandarizacion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Trama de entrada sucia delimitada por comas (formato tipo CSV)
            string registroBruto = "   lApToP dElL xPs 15  ,  SOPORTE.VENTAS@TECHPRO.COM.SV  ,  (503) 2250-9988  ,   dIsTrIbUiDoRa  gLoBaL  ";

            // Separación mediante Split por la coma ','
            string[] campos = registroBruto.Split(',');

            // Formateo de Producto (Tipo Título con Substring, ToUpper y ToLower)
            string rawProducto = campos[0].Trim();
            while (rawProducto.Contains("  ")) rawProducto = rawProducto.Replace("  ", " ");
            string[] palabrasProducto = rawProducto.Split(' ');
            string productoEstandar = "";
            foreach (string p in palabrasProducto)
            {
                if (p.Length > 0)
                    productoEstandar += p.Substring(0, 1).ToUpper() + p.Substring(1).ToLower() + " ";
            }
            productoEstandar = productoEstandar.Trim();

            // Limpieza de correo, teléfono y proveedor
            string correoContacto = campos[1].Trim().ToLower();
            string telefonoSanitizado = campos[2].Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            string proveedorOficial = campos[3].Trim().ToUpper();

            // Validaciones funcionales de negocio
            bool correoValido = correoContacto.Contains("@") && (correoContacto.EndsWith(".com") || correoContacto.EndsWith(".sv"));
            bool esLaptop = productoEstandar.ToUpper().Contains("LAPTOP");

            // Salida limpia en pantalla
            Console.WriteLine("=== REGISTRO PROCESADO Y ESTANDARIZADO (SmartBusinessPro) ===");
            Console.WriteLine($"Producto Estándar : {productoEstandar}");
            Console.WriteLine($"Correo de Contacto: {correoContacto}");
            Console.WriteLine($"Teléfono Limpio   : {telefonoSanitizado}");
            Console.WriteLine($"Proveedor Oficial : {proveedorOficial}");
            Console.WriteLine($"¿Correo Válido?   : {correoValido}");
            Console.WriteLine($"¿Categoría Laptop?: {esLaptop}");

            Console.ReadKey();
        }
    }
}