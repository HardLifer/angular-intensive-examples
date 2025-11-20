import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-import-file',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatSnackBarModule
  ],
  templateUrl: './import-file.html',
  styleUrl: './import-file.scss',
})
export class ImportFile {
  selectedFile = signal<File | null>(null);
  uploading = signal(false);
  uploadProgress = signal(0);

  private readonly MAX_FILE_SIZE = 50 * 1024 * 1024; // 50MB in bytes
  private readonly ALLOWED_EXTENSIONS = ['.xlsx', '.xls'];

  constructor(private snackBar: MatSnackBar) {}

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      
      // Validate file size
      if (file.size > this.MAX_FILE_SIZE) {
        this.snackBar.open(
          `File size exceeds 50MB. Selected file is ${(file.size / (1024 * 1024)).toFixed(2)}MB`,
          'Close',
          { duration: 5000, panelClass: ['error-snackbar'] }
        );
        this.selectedFile.set(null);
        input.value = ''; // Reset input
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

    // Create FormData to send file
    const formData = new FormData();
    formData.append('file', file);

    // TODO: Replace with your actual API endpoint
    // Example implementation with HttpClient:
    /*
    this.http.post('your-api-endpoint/upload', formData, {
      reportProgress: true,
      observe: 'events'
    }).subscribe({
      next: (event) => {
        if (event.type === HttpEventType.UploadProgress) {
          const progress = event.total 
            ? Math.round((100 * event.loaded) / event.total)
            : 0;
          this.uploadProgress.set(progress);
        } else if (event.type === HttpEventType.Response) {
          this.snackBar.open('File uploaded successfully!', 'Close', { duration: 3000 });
          this.uploading.set(false);
          this.removeFile();
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
    */

    // Simulated upload for demonstration
    this.simulateUpload();
  }

  private simulateUpload(): void {
    const interval = setInterval(() => {
      const currentProgress = this.uploadProgress();
      
      if (currentProgress >= 100) {
        clearInterval(interval);
        this.uploading.set(false);
        this.snackBar.open('File uploaded successfully!', 'Close', { duration: 3000 });
        this.removeFile();
      } else {
        this.uploadProgress.set(currentProgress + 10);
      }
    }, 300);
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  }
}
