using System.Globalization;
using System.ServiceModel.Channels;
using TipoCambioMoneda.Data;
using TipoCambioMoneda.Dto.Request;
using TipoCambioMoneda.Dto.Response;
using TipoCambioMoneda.Entities;
using TipoCambioMoneda.Services;
using TipoCambioReference;

namespace TipoCambioMoneda.Services;

public class TasaCambioService : ITasaCambioService
{
    private readonly TasaCambioDBContext _db;

    public TasaCambioService(TasaCambioDBContext db)
    {
        _db = db;
    }

    public async Task<ActionResultResponse> TipoCambioDia()
    {
        try
        {
            var cliente = new TipoCambioSoapClient(
                TipoCambioSoapClient.EndpointConfiguration.TipoCambioSoap
            );
            var response = await cliente.TipoCambioDiaAsync();
            var result = response.Body.TipoCambioDiaResult;
            if (result?.CambioDolar == null || result.CambioDolar.Length == 0)
            {
                return new ErrorResponse("No se encontraron datos de tipo de cambio.", 400);
            }
            var dolar = result.CambioDolar.First();
            var data = new
            {
                message = "Consulta exitosa",
                fecha = dolar.fecha,
                tasa = dolar.referencia
            };
            var nuevo = new BitacoraTasaCambio
            {
                FechaTipoCambio = DateTime.Parse(dolar.fecha),
                TipoCambio = Convert.ToDecimal(dolar.referencia),
                OrigenApi = "TipoCambioSoap"
            };


            _db.BitacoraTasaCambios.Add(nuevo);
            await _db.SaveChangesAsync();
            return new SuccessResponse<object>(data);

        }
        catch (Exception ex)
        {
            return new ErrorResponse($"Error al consultar tipo de cambio: {ex.Message}", 500);
        }
    }
    public async Task<ActionResultResponse> TipoCambioMonedaRango(TipoCambioMonedaRequest request)
    {
        try
        {
            if (request is null)
                return new ErrorResponse("El cuerpo de la solicitud es requerido.", 400);

            if (request.fechaInicio == default || request.fechaFin == default)
                return new ErrorResponse("Las fechas de inicio y fin son obligatorias y deben ser válidas.", 400);

            if (request.fechaFin < request.fechaInicio)
                return new ErrorResponse("La fecha de fin no puede ser menor que la fecha de inicio.", 400);

            if (request.fechaInicio > DateTime.Today || request.fechaFin > DateTime.Today)
                return new ErrorResponse("Las fechas no pueden ser posteriores al día de hoy.", 400);

            var cliente = new TipoCambioSoapClient(
                TipoCambioSoapClient.EndpointConfiguration.TipoCambioSoap
            );

            string fechainit = request.fechaInicio.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            string fechafin = request.fechaFin.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            var response = await cliente.TipoCambioRangoMonedaAsync(fechainit, fechafin, request.moneda);
            var result = response.Body.TipoCambioRangoMonedaResult;

            if (result?.Vars == null || result.Vars.Length == 0 || result.TotalItems == 0)
                return new ErrorResponse("No se encontraron datos de tipo de cambio en el rango especificado.", 404);

            var bitacoras = new List<BitacoraTasaCambio>();

            foreach (var item in result.Vars)
            {
                if (!DateTime.TryParseExact(item.fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var fechaTipoCambio))
                {
                    return new ErrorResponse($"Fecha inválida en los datos del proveedor: '{item.fecha}'.", 500);
                }

                bitacoras.Add(new BitacoraTasaCambio
                {
                    FechaTipoCambio = fechaTipoCambio,
                    TipoCambio = Convert.ToDecimal(item.venta),
                    OrigenApi = $"TipoCambioRangoMoneda:{request.moneda}"
                });
            }

            if (bitacoras.Count == 0)
                return new ErrorResponse("No se pudieron procesar los datos del proveedor.", 500);

            _db.BitacoraTasaCambios.AddRange(bitacoras);
            await _db.SaveChangesAsync();

            var data = new
            {
                message = "Consulta de rango ejecutada correctamente",
                totalItems = result.TotalItems,
                rango = new
                {
                    fechaInicio = fechainit,
                    fechaFin = fechafin,
                    moneda = request.moneda
                },
                items = result.Vars.Select(v => new
                {
                    v.moneda,
                    v.fecha,
                    v.venta,
                    v.compra
                }).ToList()
            };

            return new SuccessResponse<object>(data);
        }
        catch (TaskCanceledException)
        {
            return new ErrorResponse("El servicio externo no respondió a tiempo.", 504);
        }
        catch
        {
            return new ErrorResponse("Error interno al consultar tipo de cambio por rango.", 500);
        }
    }


}