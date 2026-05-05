export interface ExpenseDetailsDto {
  id?: number;
  userId: number;
  expenseType: string;
  amount: number;
  expenseDate?: string;
  description: string;
  status: string;
  approvedBy?: string;
  approvedAt?: string;
  attachImage?: string;
  notes?: string;
  entryBy: string;
  entryAt: string;
}