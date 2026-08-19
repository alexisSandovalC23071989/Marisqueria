using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.ModeloProductos;

namespace Flujo
{
    public class ProductosFlujo : IProductosFlujo
    {
        private readonly IProductosDA _productosDA;

        public ProductosFlujo(IProductosDA productosDA)
        {
            _productosDA = productosDA;
        }

        public Task<IEnumerable<ProductoResponse>> Obtener()
        {
            return _productosDA.Obtener();
        }

        public Task<ProductoResponse?> Obtener(int IdProducto)
        {
            return _productosDA.Obtener(IdProducto);
        }

        public Task<int> Agregar(ProductoRequest producto)
        {
            return _productosDA.Agregar(producto);
        }

        public Task<int> Editar(int IdProducto, ProductoRequest producto)
        {
            return _productosDA.Editar(IdProducto, producto);
        }

        public Task<int> Eliminar(int IdProducto)
        {
            return _productosDA.Eliminar(IdProducto);
        }
    }
}
