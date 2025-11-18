using Microsoft.AspNetCore.Mvc;

namespace TipoCambioMoneda.Dto.Response;

public abstract class ActionResultResponse
{
    public abstract IActionResult ToHttpResponse(ControllerBase controller);
}