export interface ReviewTemplateDto {
  id: number;
  name: string;
  createdAt: Date | null;
}

export enum ReviewTemplateType {
  Residential = 1,
  Commercial = 2
}