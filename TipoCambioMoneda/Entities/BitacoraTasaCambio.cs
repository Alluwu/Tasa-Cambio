using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipoCambioMoneda.Entities;

[Table("bitacora_tasa_cambio")]
public class BitacoraTasaCambio
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("fecha_consulta")]
    public DateTime FechaConsulta { get; set; } = DateTime.Now;

    [Column("fecha_tipo_cambio", TypeName = "date")]
    public DateTime FechaTipoCambio { get; set; } = DateTime.Today;

    [Column("tipo_cambio", TypeName = "numeric(18,6)")]
    public decimal TipoCambio { get; set; }

    [Column("origen_api")]
    [StringLength(200)]
    public string? OrigenApi { get; set; }
}