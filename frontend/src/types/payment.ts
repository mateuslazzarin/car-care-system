export interface PaymentDto {
  id: string;
  clientId: string;
  appointmentId?: string;
  amount: number;
  method: PaymentMethodDto;
  status: PaymentStatusDto;
  paymentDate: string;
  reference?: string;
  notes?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreatePaymentDto {
  clientId: string;
  appointmentId?: string;
  amount: number;
  method: PaymentMethodDto;
  reference?: string;
  notes?: string;
}

export interface UpdatePaymentDto {
  status: PaymentStatusDto;
  notes?: string;
}

export enum PaymentMethodDto {
  Cash = 1,
  CreditCard = 2,
  DebitCard = 3,
  BankTransfer = 4,
  Pix = 5,
}

export enum PaymentStatusDto {
  Pending = 1,
  Completed = 2,
  Failed = 3,
  Refunded = 4,
}

export const PaymentMethodLabels: Record<PaymentMethodDto, string> = {
  [PaymentMethodDto.Cash]: 'Dinheiro',
  [PaymentMethodDto.CreditCard]: 'Cartão de Crédito',
  [PaymentMethodDto.DebitCard]: 'Cartão de Débito',
  [PaymentMethodDto.BankTransfer]: 'Transferência Bancária',
  [PaymentMethodDto.Pix]: 'PIX',
};

export const PaymentStatusLabels: Record<PaymentStatusDto, string> = {
  [PaymentStatusDto.Pending]: 'Pendente',
  [PaymentStatusDto.Completed]: 'Concluído',
  [PaymentStatusDto.Failed]: 'Falhou',
  [PaymentStatusDto.Refunded]: 'Reembolsado',
};
