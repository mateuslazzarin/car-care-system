import api from './api';
import { ClientDto, CreateClientDto, UpdateClientDto } from '../types/client';

export const clientService = {
  getAll: async (): Promise<ClientDto[]> => {
    const response = await api.get('/clients');
    return response.data;
  },

  getById: async (id: string): Promise<ClientDto> => {
    const response = await api.get(`/clients/${id}`);
    return response.data;
  },

  getActive: async (): Promise<ClientDto[]> => {
    const response = await api.get('/clients/active');
    return response.data;
  },

  search: async (term: string): Promise<ClientDto[]> => {
    const response = await api.get('/clients/search', { params: { term } });
    return response.data;
  },

  create: async (data: CreateClientDto): Promise<ClientDto> => {
    const response = await api.post('/clients', data);
    return response.data;
  },

  update: async (id: string, data: UpdateClientDto): Promise<ClientDto> => {
    const response = await api.put(`/clients/${id}`, data);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/clients/${id}`);
  },
};
