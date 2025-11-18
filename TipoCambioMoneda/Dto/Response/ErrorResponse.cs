using Microsoft.AspNetCore.Mvc;
namespace TipoCambioMoneda.Dto.Response;

public class ErrorResponse : ActionResultResponse
{
    public string mensaje { get; }
    public int code { get; set; }
    public int StatusCode { get; set; }

    public ErrorResponse(string message, int statusCode = 400, int Code = 400)
    {
        mensaje = message;
        code = Code;
        StatusCode = statusCode;
    }

    public override IActionResult ToHttpResponse(ControllerBase controller)
    {
        var response = new { mensaje, code };
        var objectResult = new ObjectResult(response)
        {
            StatusCode = StatusCode
        };
        objectResult.ContentTypes.Add("application/json");
        return objectResult;
    }
}