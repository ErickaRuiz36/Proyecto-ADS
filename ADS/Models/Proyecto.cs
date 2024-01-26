using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADS.Models;

public partial class Proyecto
{
    public int IdProyecto { get; set; }
	[Required(ErrorMessage = "El campo Nombre es requerido.")]
	public string? Nombre { get; set; }

    public string? Descripcion { get; set; }
	[Required(ErrorMessage = "El campo Fecha de inicio es requerido.")]
	public DateTime? FechaInicio { get; set; }
	[Required(ErrorMessage = "El campo Fecha estimada de termino es requerido.")]
	public DateTime? FechaEstimadaTermino { get; set; }
	[Required(ErrorMessage = "El campo RFC del contratista es requerido.")]
	public string? RFCContratista { get; set; }
	[Required(ErrorMessage = "El campo Numero de contrato es requerido.")]
	public int NumContrato { get; set; }
	[Required(ErrorMessage = "El campo Fecha de contrato es requerido.")]
	public DateTime? FechaContrato { get; set; }

    public bool? Disponible { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<FrenteObra> FrenteObras { get; set; } = new List<FrenteObra>();
}
