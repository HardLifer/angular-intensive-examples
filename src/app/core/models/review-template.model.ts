export interface ReviewTemplateDto {
  id: number;
  name: string;
  createdAt: Date | null;
  updatedAt: Date | null;
}

export interface ReviewTemplateItemDto {
  id: number;
  templateId: number | null;
  itemOptionTypeId: number | null;
  hasComment: boolean;
  isDisabled: boolean;
  createdAt: Date | null;
  updatedAt: Date | null;
}

export interface ReviewOptionDto {
  id: number;
  optionTypeId: number;
  name: string;
  createdAt: Date | null;
  updatedAt: Date | null;
}

export interface ReviewOptionTypeDto {
  id: number;
  optionName: string;
  createdAt: Date | null;
  updatedAt: Date | null;
}