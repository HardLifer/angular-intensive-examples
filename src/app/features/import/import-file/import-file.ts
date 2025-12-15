import { Component, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { ImportApiService } from '../../../core/services/import-api.service';
import { ReviewTemplateType } from '../../../core/models/review-template.model';

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
    MatFormFieldModule
  ],
  templateUrl: './import-file.html',
  styleUrl: './import-file.scss',
})
export class ImportFile {
  private importApiService = inject(ImportApiService);
  private snackBar = inject(MatSnackBar);

  selectedFile = signal<File | null>(null);
  uploading = signal(false);
  uploadProgress = signal(0);
  selectedTemplate = signal<number>(ReviewTemplateType.Residential);

  private readonly MAX_FILE_SIZE = 50 * 1024 * 1024; // 50MB in bytes

  templates = [
    { value: ReviewTemplateType.Residential, label: 'Residential' },
    { value: ReviewTemplateType.Commercial, label: 'Commercial' }
  ];

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      
      if (file.size > this.MAX_FILE_SIZE) {
        this.snackBar.open(
          `File size exceeds 50MB. Selected file is ${(file.size / (1024 * 1024)).toFixed(2)}MB`,
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

    this.importApiService.importExcelFile(file, this.selectedTemplate()).subscribe({
      next: (result) => {
        if (typeof result === 'number') {
          this.uploadProgress.set(result);
        } else {
          // Result is FailedImportLoanData[]
          if (result.length === 0) {
            this.snackBar.open('File uploaded successfully!', 'Close', { duration: 3000 });
            this.removeFile();
          } else {
            this.snackBar.open(
              `Upload completed with ${result.length} errors. Check console for details.`,
              'Close',
              { duration: 5000, panelClass: ['error-snackbar'] }
            );
            console.error('Import errors:', result);
          }
          this.uploading.set(false);
        }
      },
      error: (error) => {
        this.snackBar.open('Upload failed: ' + error.message, 'Close', { 
          duration: 5000,
          panelClass: ['error-snackbar']
        });
        this.uploading.set(false);
      }
    });
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  }
}