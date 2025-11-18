using Microsoft.AspNetCore.Mvc;
namespace TipoCambioMoneda.Dto.Response;

public class SuccessResponse<T> : ActionResultResponse
{
    public T Data { get; }

    public SuccessResponse(T data)
    {
        Data = data;
    }

    public override IActionResult ToHttpResponse(ControllerBase controller)
    {
        return controller.Ok(Data);
    }
}