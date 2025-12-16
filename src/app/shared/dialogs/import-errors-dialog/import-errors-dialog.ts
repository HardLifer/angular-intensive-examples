import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { FailedImportLoanData } from '../../../core/index';

export interface ImportErrorsDialogData {
  errors: FailedImportLoanData[];
  fileName: string;
  totalErrors: number;
}

@Component({
  selector: 'app-import-errors-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatChipsModule
  ],
  templateUrl: './import-errors-dialog.html',
  styleUrls: ['./import-errors-dialog.scss']
})
export class ImportErrorsDialogComponent {
  displayedColumns: string[] = ['rowNumber', 'columnName', 'rowData', 'validationMessage'];

  // Get last 10 errors for display
  displayErrors: FailedImportLoanData[];
  remainingCount: number;

  // Add this property at the top of the class
  showTextFormat = false;

  constructor(
    public dialogRef: MatDialogRef<ImportErrorsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ImportErrorsDialogData
  ) {
    // Show only last 10 errors
    this.displayErrors = this.data.errors.slice(-10);
    this.remainingCount = Math.max(0, this.data.totalErrors - 10);
  }

  close(): void {
    this.dialogRef.close();
  }

  downloadFullReport(): void {
    this.dialogRef.close('download');
  }

  retryImport(): void {
    this.dialogRef.close('retry');
  }

  formatAsText(): string {
    let text = `IMPORT VALIDATION ERRORS\n`;
    text += `File: ${this.data.fileName}\n`;
    text += `Total Errors: ${this.data.totalErrors}\n`;
    text += `Showing Last: ${this.displayErrors.length} errors\n`;
    text += `${'='.repeat(80)}\n\n`;

    this.displayErrors.forEach((error, index) => {
      text += `Error ${index + 1}:\n`;
      text += `  Row Number: ${error.rowNumber}\n`;
      text += `  Column: ${error.columnName || 'General'}\n`;
      text += `  Invalid Data: ${error.rowData || 'N/A'}\n`;
      text += `  Message: ${error.validationMessage}\n`;
      text += `${'-'.repeat(80)}\n\n`;
    });

    return text;
  }
}