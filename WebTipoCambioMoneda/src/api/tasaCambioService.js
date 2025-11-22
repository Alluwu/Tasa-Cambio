import api from './http';

export async function apiGetTasaDia() {
  const res = await api.get('api/TasaCambio');
  return res.data;
}

export async function apiGetTasaRango(params) {
  const res = await api.get('api/TasaCambio/rango', { params });
  return res.data;
}

export async function apiGetBitacora(params) {
  const res = await api.get('api/TasaCambio/bitacora', { params });
  return res.data;
}
