using Abstracciones.Interfaces.DA;
using Abstracciones.Modelo;
using Dapper;
using System.Data;

namespace DA
{
    public class ProveedoresDA : IProveedoresDA
    {
        private readonly DBContext _dbContext;

        public ProveedoresDA(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ProveedorResponse>> Obtener()
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryAsync<ProveedorResponse>(
                "ObtenerProveedores",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<ProveedorResponse?> Obtener(int IdProveedor)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryFirstOrDefaultAsync<ProveedorResponse>(
                "ObtenerProveedor",
                new { IdProveedor },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Agregar(ProveedorRequest proveedor)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<int>(
                "AgregarProveedor",
                proveedor,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Editar(int IdProveedor, ProveedorRequest proveedor)
        {
            using var conexion = _dbContext.CrearConexion();

            var parametros = new
            {
                IdProveedor,
                proveedor.Empresa,
                proveedor.CedulaJuridica,
                proveedor.Contacto,
                proveedor.Telefono,
                proveedor.Correo,
                proveedor.Direccion,
                proveedor.Estado
            };

            return await conexion.ExecuteScalarAsync<int>(
                "EditarProveedor",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> Eliminar(int IdProveedor)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<int>(
                "EliminarProveedor",
                new { IdProveedor },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ProductoProveedorResponse>> ObtenerProductos(int IdProveedor)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.QueryAsync<ProductoProveedorResponse>(
                "ObtenerProductosPorProveedor",
                new { IdProveedor },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AgregarProducto(int IdProveedor, ProductoProveedorRequest productoProveedor)
        {
            using var conexion = _dbContext.CrearConexion();

            var parametros = new
            {
                IdProveedor,
                productoProveedor.IdProducto,
                productoProveedor.PrecioCompra,
                productoProveedor.UnidadMedida
            };

            return await conexion.ExecuteScalarAsync<int>(
                "AgregarProductoProveedor",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> EditarProducto(int IdProveedor, ProductoProveedorRequest productoProveedor)
        {
            using var conexion = _dbContext.CrearConexion();

            var parametros = new
            {
                IdProveedor,
                productoProveedor.IdProducto,
                productoProveedor.PrecioCompra,
                productoProveedor.UnidadMedida
            };

            return await conexion.ExecuteScalarAsync<int>(
                "EditarProductoProveedor",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> EliminarProducto(int IdProveedor, int IdProducto)
        {
            using var conexion = _dbContext.CrearConexion();

            return await conexion.ExecuteScalarAsync<int>(
                "EliminarProductoProveedor",
                new { IdProveedor, IdProducto },
                commandType: CommandType.StoredProcedure);
        }
    }
}
