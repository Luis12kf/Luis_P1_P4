namespace Luis_P1_P4.Models;

public record AutorRecordSet(string Nombre,
 string Nacionalidad, 
 string FechaNacimiento, 
 long Sueldo);
public record AutorRecordGet{
public long idAutor { get; init; }   
public string Nombre { get; init; } = string.Empty;
public string Nacionalidad { get; init; } = string.Empty;
public string FechaNacimiento { get; init; } = string.Empty;
public long Sueldo { get; init; }
}
