using Microsoft.AspNetCore.Mvc;
namespace TipoCambioMoneda.Dto.Response;

public class SuccessCreatedResponse<T> : ActionResultResponse
{
    public T Data { get; }
    public string Location { get; }

    public SuccessCreatedResponse(T data, string location = null)
    {
        Data = data;
        Location = location;
    }

    public override IActionResult ToHttpResponse(ControllerBase controller)
    {
        var result = new ObjectResult(Data)
        {
            StatusCode = StatusCodes.Status201Created
        };

        if (!string.IsNullOrEmpty(Location))
        {
            controller.Response.Headers.Append("Location", Location);
        }

        return result;
    }
}