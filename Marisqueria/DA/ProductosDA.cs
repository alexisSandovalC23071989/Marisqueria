using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Abstracciones.Modelos.ModeloProductos;
using Dapper;
using System.Data;

namespace DA
{
    public class ProductosDA : IProductosDA
    {
        private readonly DBContext _dbContext;

        public ProductosDA(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductoResponse>> Obtener()
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryAsync<ProductoResponse>(
                "ObtenerProductos",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<ProductoResponse?> Obtener(int IdProducto)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryFirstOrDefaultAsync<ProductoResponse>(
                "ObtenerProducto",
                new { IdProducto },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Agregar(ProductoRequest producto)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<int>(
                "AgregarProducto",
                producto,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Editar(int IdProducto, ProductoRequest producto)
        {
            using var conexion = _dbContext.CrearConexion();

            var parametros = new
            {
                IdProducto,
                producto.IdCategoria,
                producto.Nombre,
                producto.Descripcion,
                producto.PrecioVentaActual,
                producto.Imagen
            };

            return await conexion.ExecuteScalarAsync<int>(
                "EditarProducto",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Eliminar(int IdProducto)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<int>(
                "EliminarProducto",
                new { IdProducto },
                commandType: CommandType.StoredProcedure);
        }
    }
}
