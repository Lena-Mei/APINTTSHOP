using System;
using System.Collections.Generic;

namespace APINTTShop.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Inicio { get; set; } = null!;

    public string Contrasenya { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string? Apellido2 { get; set; }

    public string? Direccion { get; set; }

    public string? Provincia { get; set; }

    public string? Ciudad { get; set; }

    public string? CodigoPostal { get; set; }

    public string? Telefono { get; set; }

    public string Email { get; set; } = null!;

    public string? IsoIdioma { get; set; }

    public int? IdRate { get; set; }
}


