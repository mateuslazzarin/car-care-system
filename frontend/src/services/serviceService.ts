import api from './api';
import { ServiceDto, CreateServiceDto, UpdateServiceDto } from '../types/service';

export const serviceService = {
  getAll: async (): Promise<ServiceDto[]> => {
    const response = await api.get('/services');
    return response.data;
  },

  getById: async (id: string): Promise<ServiceDto> => {
    const response = await api.get(`/services/${id}`);
    return response.data;
  },

  getByClientId: async (clientId: string): Promise<ServiceDto[]> => {
    const response = await api.get(`/services/client/${clientId}`);
    return response.data;
  },

  getByCategory: async (category: number): Promise<ServiceDto[]> => {
    const response = await api.get(`/services/category/${category}`);
    return response.data;
  },

  getActive: async (): Promise<ServiceDto[]> => {
    const response = await api.get('/services/active');
    return response.data;
  },

  create: async (data: CreateServiceDto): Promise<ServiceDto> => {
    const response = await api.post('/services', data);
    return response.data;
  },

  update: async (id: string, data: UpdateServiceDto): Promise<ServiceDto> => {
    const response = await api.put(`/services/${id}`, data);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/services/${id}`);
  },
};
