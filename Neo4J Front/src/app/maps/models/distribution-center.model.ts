export interface DistributionCenter {
  id: string;
  description: string;
  city: string;
  address: string;
  country: string;
  phoneNumber: string;
  planning: number;
  sales: number;
  sourcing: number;
  warehouseAndDistribution: number;
  warehouseAndDistributionTotal: number;
  timeStart: string;
  timeEnd: string;
  reference: string;
  production: any[];
}
