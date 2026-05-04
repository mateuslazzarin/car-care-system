import api from './api';
import { AppointmentDto, CreateAppointmentDto, UpdateAppointmentDto } from '../types/appointment';

export const appointmentService = {
  getAll: async (): Promise<AppointmentDto[]> => {
    const response = await api.get('/appointments');
    return response.data;
  },

  getById: async (id: string): Promise<AppointmentDto> => {
    const response = await api.get(`/appointments/${id}`);
    return response.data;
  },

  getByClientId: async (clientId: string): Promise<AppointmentDto[]> => {
    const response = await api.get(`/appointments/client/${clientId}`);
    return response.data;
  },

  getByStatus: async (status: number): Promise<AppointmentDto[]> => {
    const response = await api.get(`/appointments/status/${status}`);
    return response.data;
  },

  getByDateRange: async (startDate: string, endDate: string): Promise<AppointmentDto[]> => {
    const response = await api.get('/appointments/range', { params: { startDate, endDate } });
    return response.data;
  },

  create: async (data: CreateAppointmentDto): Promise<AppointmentDto> => {
    const response = await api.post('/appointments', data);
    return response.data;
  },

  update: async (id: string, data: UpdateAppointmentDto): Promise<AppointmentDto> => {
    const response = await api.put(`/appointments/${id}`, data);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/appointments/${id}`);
  },
};
