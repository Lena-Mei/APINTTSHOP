using System;
using System.Collections.Generic;

namespace APINTTShop.Models.Entities;

public partial class Gestionusuario
{
    public int IdUsuario { get; set; }

    public string Inicio { get; set; } = null!;

    public string? Contrasenya { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string? Apellido2 { get; set; }

    public string Email { get; set; } = null!;

    public string? IsoIdioma { get; set; }
}
