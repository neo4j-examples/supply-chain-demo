export interface Incident {
  id: number;
  description: string;
  detailedDescription: string;
  solution: string;
  sensorsState: { name: string; failure: boolean }[];
}
