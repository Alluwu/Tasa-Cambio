namespace TipoCambioMoneda.Dto.Request;
public class PaginadoRequest
{
    public DateTime? FechaInicio {get;set;}
    public DateTime? FechaFin {get;set;}
    public int Page {get;set;} = 1;
    public int PageSize {get;set;} = 20;
}
