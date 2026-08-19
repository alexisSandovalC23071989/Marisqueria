using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IProveedoresController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> Obtener(int IdProveedor);
        Task<IActionResult> Agregar(ProveedorRequest proveedor);
        Task<IActionResult> Editar(int IdProveedor, ProveedorRequest proveedor);
        Task<IActionResult> Eliminar(int IdProveedor);

        Task<IActionResult> ObtenerProductos(int IdProveedor);
        Task<IActionResult> AgregarProducto(int IdProveedor, ProductoProveedorRequest productoProveedor);
        Task<IActionResult> EditarProducto(int IdProveedor, int IdProducto, ProductoProveedorRequest productoProveedor);
        Task<IActionResult> EliminarProducto(int IdProveedor, int IdProducto);
    }
}
