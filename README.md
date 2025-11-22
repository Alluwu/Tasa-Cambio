TIPO DE CAMBIO GUATEMALA.

Proyecto: Consulta y Bitacora de Tipo de Cambio

La aplicacion esta compuesta por .NET CORE 8 + Vue 3 para consultar la tasa de cambio del Banco de Guatemala mediante el servicio SOAP TipoCambioMoneda, almacenar las consultas en la base de datos y mostrar la bitácora. 
Tecnologias utilizadas 

Backend (.NET 8)
ASP.NET Core 8 Web API
Entity Framework Core 8
PostgreSQL (Npgsql)
SOAP Client (TipoCambioMoneda Banguat)
CORS habilitado para frontend
Arquitectura con Services + Controllers + DTOs + Responses personalizados

Frontend (Vue 3)

Vue 3 + Vite
Pinia (store global)
Axios (consumo de API)
Vue Router
vue3-toastify (notificaciones)
Componentes reutilizables (cards, tablas, formularios y paginación)

Funciones principales
Consulta de Tasa de Cambio del Día
Llamada al servicio SOAP del Banco de Guatemala.
Registro automático en bitácora.
Vista en Vue mostrando fecha, tasa y mensaje.

Consulta de Tipo de Cambio por Rango
Consulta por moneda y rango de fechas.
Validaciones de fecha en backend.
Cálculo promedio de la tasa.
Registro en bitácora.

Vista en Vue con formulario y tabla de resultados.
Bitácora Paginada (Backend + Frontend)
Filtros opcionales por fecha inicio / fin.
Paginación real desde base de datos.
Vista en Vue con tabla y componente de paginación.

Como ejecutar el proyecto:
Backend:
cd TipoCambioMoneda
dotnet restore
dotnet ef database update
dotnet run
Frontend
cd WebTipoCambioMoneda
npm install
npm run dev