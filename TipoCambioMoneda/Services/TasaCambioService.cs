using System.Globalization;
using System.ServiceModel.Channels;
using Microsoft.EntityFrameworkCore;
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


            var promedio = result.Vars
                .Select(v => Convert.ToDecimal(v.venta))
                .Average();

            var bitacora = new BitacoraTasaCambio
            {
                FechaTipoCambio = request.fechaInicio,
                TipoCambio = promedio,
                OrigenApi = $"TipoCambioRango:{request.moneda}"
            };

            _db.BitacoraTasaCambios.Add(bitacora);
            await _db.SaveChangesAsync();


            var data = new
            {
                message = "Consulta de rango ejecutada correctamente",
                totalItems = result.TotalItems,
                promedioVenta = promedio,
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
    public async Task<ActionResultResponse> ObtenerBitacoraPaginado(PaginadoRequest request)
    {
        try
        {
            if (request.Page <= 0) request.Page = 1;
            if (request.PageSize <= 0 || request.PageSize > 100) request.PageSize = 20;

            DateTime? fi = request.FechaInicio?.Date;
            DateTime? ff = request.FechaFin?.Date;

            if (fi.HasValue && ff.HasValue && ff < fi)
                return new ErrorResponse("La fecha de fin no puede ser menor que la fecha de inicio.", 400);

            var query = _db.BitacoraTasaCambios
                .AsNoTracking()
                .AsQueryable();

            if (fi.HasValue)
                query = query.Where(b => b.FechaTipoCambio >= fi.Value);

            if (ff.HasValue)
            {
                var fechaFinInclusive = ff.Value.AddDays(1).AddTicks(-1);
                query = query.Where(b => b.FechaTipoCambio <= fechaFinInclusive);
            }

            var totalItems = await query.CountAsync();
            var totalPages = totalItems == 0
                ? 0
                : (int)Math.Ceiling(totalItems / (double)request.PageSize);

            if (totalItems == 0)
            {
                var emptyData = new
                {
                    message = "No hay registros en la bitácora para los filtros especificados.",
                    totalItems = 0,
                    totalPages = 0,
                    page = 1,
                    pageSize = request.PageSize,
                    rango = new
                    {
                        fechaInicio = fi?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        fechaFin = ff?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                    },
                    items = Array.Empty<object>()
                };

                return new SuccessResponse<object>(emptyData);
            }

            if (request.Page > totalPages)
                request.Page = totalPages;

            var items = await query
                .OrderByDescending(b => b.Id)
                .ThenByDescending(b => b.FechaConsulta)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(b => new
                {
                    b.Id,
                    b.FechaConsulta,
                    b.FechaTipoCambio,
                    b.TipoCambio,
                    b.OrigenApi
                })
                .ToListAsync();

            var data = new
            {
                message = "Consulta de bitácora ejecutada correctamente.",
                totalItems,
                totalPages,
                page = request.Page,
                pageSize = request.PageSize,
                rango = new
                {
                    fechaInicio = fi?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    fechaFin = ff?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                },
                items
            };

            return new SuccessResponse<object>(data);
        }
        catch (Exception)
        {
            return new ErrorResponse("Error interno al consultar la bitácora de tipo de cambio.", 500);
        }
    }
}