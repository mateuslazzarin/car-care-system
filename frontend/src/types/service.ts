export interface ServiceDto {
  id: string;
  clientId: string;
  name: string;
  description?: string;
  category: ServiceCategoryDto;
  price: number;
  durationMinutes: number;
  createdAt: string;
  updatedAt?: string;
  isActive: boolean;
}

export interface CreateServiceDto {
  clientId: string;
  name: string;
  description?: string;
  category: ServiceCategoryDto;
  price: number;
  durationMinutes: number;
}

export interface UpdateServiceDto {
  name: string;
  description?: string;
  category: ServiceCategoryDto;
  price: number;
  durationMinutes: number;
}

export enum ServiceCategoryDto {
  Aesthetics = 1,
  Audio = 2,
  Accessories = 3,
  Film = 4,
}

export const ServiceCategoryLabels: Record<ServiceCategoryDto, string> = {
  [ServiceCategoryDto.Aesthetics]: 'Estética',
  [ServiceCategoryDto.Audio]: 'Som',
  [ServiceCategoryDto.Accessories]: 'Acessórios',
  [ServiceCategoryDto.Film]: 'Películas',
};
