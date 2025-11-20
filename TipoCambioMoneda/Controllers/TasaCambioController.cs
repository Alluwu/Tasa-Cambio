using Microsoft.AspNetCore.Mvc;
using TipoCambioMoneda.Dto.Request;
using TipoCambioMoneda.Services;

namespace TipoCambioMoneda.Controllers;

[Route("api/TasaCambio")]
[ApiController]
public class TasaCambioController : ControllerBase
{
    private ITasaCambioService _service;

    public TasaCambioController(ITasaCambioService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> TasaCambioMonedaGet()
    {
        var resultado = await _service.TipoCambioDia();
        return resultado.ToHttpResponse(this);
    }
     [HttpGet("rango")]
    public async Task<IActionResult> TasaCambioMonedaRangoGet([FromQuery]TipoCambioMonedaRequest  request)
    {
        var resultado = await _service.TipoCambioMonedaRango(request);
        return resultado.ToHttpResponse(this);
    }

    [HttpGet("bitacora")]
    public async Task<IActionResult> BitacoraPaginado([FromQuery]PaginadoRequest  request)
    {
        var resultado = await _service.ObtenerBitacoraPaginado(request);
        return resultado.ToHttpResponse(this);
    }
}