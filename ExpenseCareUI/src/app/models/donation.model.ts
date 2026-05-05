// models/donation.model.ts
export interface DonationDetailsDto {
  id?: number;
  userId: number;
  donorName: string;
  amount: number;
  donorAddress?: string;
  donorMobile?: string;
  donationFor?: string;
  donationDate?: string;
  paymentMode: string;
  notes?: string;
  entryBy: string;
  entryAt: string;
  status: string;
  approvedBy?: string;
  approvedAt?: string;
}