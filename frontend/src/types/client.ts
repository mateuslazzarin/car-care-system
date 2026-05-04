export interface ClientDto {
  id: string;
  name: string;
  email: string;
  phoneNumber: string;
  document?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  createdAt: string;
  updatedAt?: string;
  isActive: boolean;
}

export interface CreateClientDto {
  name: string;
  email: string;
  phoneNumber: string;
  document?: string;
  address?: string;
  city?: string;
  state?: string;
  zipCode?: string;
}

export interface UpdateClientDto extends CreateClientDto {}
