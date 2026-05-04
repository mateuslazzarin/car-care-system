export interface AppointmentDto {
  id: string;
  clientId: string;
  scheduledDate: string;
  completedDate?: string;
  status: AppointmentStatusDto;
  notes?: string;
  totalPrice: number;
  createdAt: string;
  updatedAt?: string;
  services: AppointmentServiceDto[];
}

export interface AppointmentServiceDto {
  serviceId: string;
  serviceName: string;
  price: number;
  quantity: number;
}

export interface CreateAppointmentDto {
  clientId: string;
  scheduledDate: string;
  notes?: string;
  services: AppointmentServiceDto[];
}

export interface UpdateAppointmentDto {
  scheduledDate: string;
  status: AppointmentStatusDto;
  notes?: string;
}

export enum AppointmentStatusDto {
  Scheduled = 1,
  InProgress = 2,
  Completed = 3,
  Cancelled = 4,
}
