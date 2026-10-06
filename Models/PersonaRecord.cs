namespace Luis_P1_P4.Models;

public record PersonaRecordSet(string Nombre, string Nacionalidad, string FechaNacimiento, long Sueldo);
public record PersonaRecordGet{
public long IdAutor { get; init; }   
public string Nombre { get; init; } = string.Empty;
public string Nacionalidad { get; init; } = string.Empty;
public string FechaNacimiento { get; init; } = string.Empty;
public long Sueldo { get; init; }
}
