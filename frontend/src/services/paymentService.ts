import api from './api';
import { PaymentDto, CreatePaymentDto, UpdatePaymentDto } from '../types/payment';

export const paymentService = {
  getAll: async (): Promise<PaymentDto[]> => {
    const response = await api.get('/payments');
    return response.data;
  },

  getById: async (id: string): Promise<PaymentDto> => {
    const response = await api.get(`/payments/${id}`);
    return response.data;
  },

  getByClientId: async (clientId: string): Promise<PaymentDto[]> => {
    const response = await api.get(`/payments/client/${clientId}`);
    return response.data;
  },

  getByStatus: async (status: number): Promise<PaymentDto[]> => {
    const response = await api.get(`/payments/status/${status}`);
    return response.data;
  },

  create: async (data: CreatePaymentDto): Promise<PaymentDto> => {
    const response = await api.post('/payments', data);
    return response.data;
  },

  update: async (id: string, data: UpdatePaymentDto): Promise<PaymentDto> => {
    const response = await api.put(`/payments/${id}`, data);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/payments/${id}`);
  },
};
