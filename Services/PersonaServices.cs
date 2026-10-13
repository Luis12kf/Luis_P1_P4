using Dapper;
using Luis_P1_P4.Models;
using Microsoft.Data.Sqlite;

namespace Luis_P1_P4.Services;

public class PersonaServices(IConfiguration configuration)
{
    readonly IConfiguration _configuration = configuration;

    private SqliteConnection CreateConnection() =>
        new SqliteConnection(_configuration.GetConnectionString("DbSqlite_Ds"));

    public async Task InitializeAsync()
    {
        const string query = @"
            CREATE TABLE IF NOT EXISTS Personas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL,
                Nacionalidad TEXT NOT NULL,
                FechaNacimiento TEXT NOT NULL,
                Sueldo INTEGER NOT NULL
            );";

        using var conexion = CreateConnection();
        await conexion.ExecuteAsync(query);
    }

    public async Task<bool> SaveAsync(AutorRecordSet persona)
    {
        const string query = @"INSERT INTO Personas (Nombre, Nacionalidad, FechaNacimiento, Sueldo)
                               VALUES (@Nombre, @Nacionalidad, @FechaNacimiento, @Sueldo)";

        using var conexion = CreateConnection();

        var parametros = new
        {
            Nombre = persona.Nombre,
            Nacionalidad = persona.Nacionalidad,
            FechaNacimiento = persona.FechaNacimiento,
            Sueldo = persona.Sueldo
        };

        int filasAfectadas = await conexion.ExecuteAsync(query, parametros);
        return filasAfectadas > 0;
    }

    public async Task<AutorRecordGet?> UpdateAsync(int id, AutorRecordSet persona)
    {
        const string query = @"
            UPDATE Personas
            SET Nombre = COALESCE(@Nombre, Nombre),
                Nacionalidad =   COALESCE(@Nacionalidad, Nacionalidad) ,
                FechaNacimiento =  COALESCE(@FechaNacimiento, FechaNacimiento),
                Sueldo =  COALESCE(@Sueldo, Sueldo)
            WHERE id = @Id";

        using var conexion = CreateConnection();

        int filasAfectadas = await conexion.ExecuteAsync(query, new {
            Id = id,
            Nombre = persona.Nombre,
            Nacionalidad = persona.Nacionalidad,
            FechaNacimiento = persona.FechaNacimiento,
            Sueldo = persona.Sueldo
        });

        return filasAfectadas > 0 ? new AutorRecordGet
        {
            IdAutor = id,
            Nombre = persona.Nombre,
            Nacionalidad = persona.Nacionalidad,
            FechaNacimiento = persona.FechaNacimiento,
            Sueldo = persona.Sueldo
        } : null;
    }
    public async Task<AutorRecordGet?> DeleteAsync(int id)
    {
        const string query = @"
            DELETE FROM Personas
            WHERE id = @Id";

        using var conexion = CreateConnection();

        int filasAfectadas = await conexion.ExecuteAsync(query, new { IdAutor = id });
        return filasAfectadas > 0 ? new AutorRecordGet
        {
            IdAutor = id
        } : null;
    }
    public async Task<AutorRecordGet?> GetByIdAsync(int id)
    {
        const string query = @"
            SELECT Id AS IdAutor, Nombre, Nacionalidad, FechaNacimiento, Sueldo
            FROM Personas WHERE id = @Id";
        using var conexion = CreateConnection();
        return await conexion.QueryFirstOrDefaultAsync<AutorRecordGet>(query, new { Id = id });
    }

    public async Task<IEnumerable<AutorRecordGet>> GetListAsync()
    {
        const string query = "SELECT Id AS IdAutor, Nombre, Nacionalidad, FechaNacimiento, Sueldo FROM Personas";

        using var conexion = CreateConnection();
        return await conexion.QueryAsync<AutorRecordGet>(query);
    }
}