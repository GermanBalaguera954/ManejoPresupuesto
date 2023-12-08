using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
    }
    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        private readonly string connectionString;
        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Crear (TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                                            (@"INSERT INTO TiposCuentas(Nombre, UsuarioId, Orden)
                                                VALUES(@Nombre, @UsuarioId, 0); 
                                                SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var exite = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM TiposCuentas
                                    WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;",
                                    new { nombre, usuarioId });
            return exite == 1;
        }
    }
}
