import { Component, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ImportApiService, ReviewTemplate, FailedImportLoanData } from '../../../core/index';
import { ImportErrorsDialogComponent } from '../../../shared//dialogs/import-errors-dialog/import-errors-dialog';

@Component({
  selector: 'app-import-file',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatSnackBarModule,
    MatSelectModule,
    MatFormFieldModule,
    MatDialogModule
  ],
  templateUrl: './import-file.html',
  styleUrl: './import-file.scss',
})
export class ImportFile {
  private importService = inject(ImportApiService);
  private snackBar = inject(MatSnackBar);
  private dialog = inject(MatDialog);

  selectedFile = signal<File | null>(null);
  uploading = signal(false);
  uploadProgress = signal(0);
  selectedTemplate = signal<number>(ReviewTemplate.Residential);

  private readonly MAX_FILE_SIZE = 50 * 1024 * 1024; // 50MB

  templates = [
    { value: ReviewTemplate.Residential, label: 'Residential' },
    { value: ReviewTemplate.Commercial, label: 'Commercial' }
  ];

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      const file = input.files[0];

      if (file.size > this.MAX_FILE_SIZE) {
        this.snackBar.open(
          `File size exceeds 50MB. Selected: ${(file.size / (1024 * 1024)).toFixed(2)}MB`,
          'Close',
          { duration: 5000, panelClass: ['error-snackbar'] }
        );
        this.selectedFile.set(null);
        input.value = '';
        return;
      }

      this.selectedFile.set(file);
      this.snackBar.open(`File selected: ${file.name}`, 'Close', { duration: 3000 });
    }
  }

  triggerFileInput(): void {
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    fileInput?.click();
  }

  removeFile(): void {
    this.selectedFile.set(null);
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }

  uploadFile(): void {
    const file = this.selectedFile();

    if (!file) {
      this.snackBar.open('Please select a file first', 'Close', { duration: 3000 });
      return;
    }

    this.uploading.set(true);
    this.uploadProgress.set(0);

    this.importService.importExcelFile(file, this.selectedTemplate()).subscribe({
      next: (result) => {
        if (result.type === 'progress') {
          this.uploadProgress.set(result.progress || 0);
        } else if (result.type === 'complete') {
          const errors = result.data || [];

          if (errors.length === 0) {
            this.snackBar.open('✓ File uploaded successfully!', 'Close', {
              duration: 3000,
              panelClass: ['success-snackbar']
            });
            this.removeFile();
          } else {
            // Open dialog with errors
            this.openErrorsDialog(errors, file.name);
          }
          this.uploading.set(false);
        }
      },
      error: (error) => {
        this.snackBar.open('✗ Upload failed: ' + error.message, 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
        this.uploading.set(false);
      }
    });
  }

  private openErrorsDialog(errors: FailedImportLoanData[], fileName: string): void {
    const dialogRef = this.dialog.open(ImportErrorsDialogComponent, {
      width: '900px',
      maxWidth: '95vw',
      maxHeight: '90vh',
      data: {
        errors: errors,
        fileName: fileName,
        totalErrors: errors.length
      },
      disableClose: false,
      panelClass: 'import-errors-dialog'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'download') {
        this.downloadErrorReport(errors);
      } else if (result === 'retry') {
        // User wants to fix and retry - keep the file selected
        this.snackBar.open('Please fix the errors in your file and upload again', 'Close', {
          duration: 4000
        });
      }
    });
  }

  private downloadErrorReport(errors: FailedImportLoanData[]): void {
    // Create CSV content
    let csvContent = 'Row Number,Column Name,Invalid Data,Error Message\n';

    errors.forEach(error => {
      const row = [
        error.rowNumber,
        error.columnName || 'N/A',
        `"${(error.rowData || 'N/A').replace(/"/g, '""')}"`,
        `"${error.validationMessage.replace(/"/g, '""')}"`
      ].join(',');
      csvContent += row + '\n';
    });

    // Create blob and download
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);

    link.setAttribute('href', url);
    link.setAttribute('download', `import-errors-${new Date().getTime()}.csv`);
    link.style.visibility = 'hidden';

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    this.snackBar.open('✓ Error report downloaded', 'Close', { duration: 3000 });
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  }
}