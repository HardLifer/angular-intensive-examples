export interface ImportExcelRequest {
  file: File;
  templateId: number;
}

export interface ImportExcelResponse {
  success: boolean;
  failedLoans: FailedImportLoanData[];
}

export interface FailedImportLoanData {
  rowNumber: number;
  columnName: string | null;
  rowData: string | null;
  isValid: boolean;
  validationMessage: string;
}