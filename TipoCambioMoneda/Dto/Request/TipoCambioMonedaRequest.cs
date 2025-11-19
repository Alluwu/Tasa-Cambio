namespace TipoCambioMoneda.Dto.Request;

public class TipoCambioMonedaRequest
{
    public DateTime fechaInicio {get; set;}
    public DateTime fechaFin {get; set;}
    public int moneda {get;set;}
}