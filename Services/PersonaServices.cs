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

    public async Task<bool> SaveAsync(PersonaRecordSet persona)
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

    public async Task<PersonaRecordGet?> UpdateAsync(int id, PersonaRecordSet persona)
    {
        const string query = @"
            UPDATE Personas
            SET Nombre = @Nombre,
                Nacionalidad = @Nacionalidad,
                FechaNacimiento = @FechaNacimiento,
                Sueldo = @Sueldo
            WHERE Id = @Id";

        using var conexion = CreateConnection();

        int filasAfectadas = await conexion.ExecuteAsync(query, new {
            IdAutor = id,
            Nombre = persona.Nombre,
            Nacionalidad = persona.Nacionalidad,
            FechaNacimiento = persona.FechaNacimiento,
            Sueldo = persona.Sueldo
        });

        return filasAfectadas > 0 ? new PersonaRecordGet
        {
            IdAutor = id,
            Nombre = persona.Nombre,
            Nacionalidad = persona.Nacionalidad,
            FechaNacimiento = persona.FechaNacimiento,
            Sueldo = persona.Sueldo
        } : null;
    }
    public async Task<PersonaRecordGet?> DeleteAsync(int id)
    {
        const string query = @"
            DELETE FROM Personas
            WHERE Id = @Id";

        using var conexion = CreateConnection();

        int filasAfectadas = await conexion.ExecuteAsync(query, new { IdAutor = id });
        return filasAfectadas > 0 ? new PersonaRecordGet
        {
            IdAutor = id
        } : null;
    }
    public async Task<PersonaRecordGet?> GetByIdAsync(int id)
    {
        const string query = @"
            SELECT Id, Nombre, Nacionalidad, FechaNacimiento, Sueldo
            FROM Personas WHERE Id = @Id";

        using var conexion = CreateConnection();
        return await conexion.QueryFirstOrDefaultAsync<PersonaRecordGet>(query, new { IdAutor = id });
    }

    public async Task<IEnumerable<PersonaRecordGet>> GetListAsync()
    {
        const string query = "SELECT Id, Nombre, Nacionalidad, FechaNacimiento, Sueldo FROM Personas";

        using var conexion = CreateConnection();
        return await conexion.QueryAsync<PersonaRecordGet>(query);
    }
}