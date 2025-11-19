using TipoCambioMoneda.Dto.Request;
using TipoCambioMoneda.Dto.Response;

namespace TipoCambioMoneda.Services;
public interface ITasaCambioService
{
    public Task<ActionResultResponse> TipoCambioDia();
    public Task<ActionResultResponse> TipoCambioMonedaRango(TipoCambioMonedaRequest request);
}