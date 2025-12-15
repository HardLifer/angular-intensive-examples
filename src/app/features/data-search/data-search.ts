import { Component, signal, computed, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { LoanApiService } from '../../core/services/loan-api.service'
import { LoanDetailDto } from '../../core';

@Component({
  selector: 'app-data-search',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatCardModule
  ],
  templateUrl: './data-search.html',
  styleUrls: ['./data-search.scss']
})
export class DataSearchComponent implements OnInit {
  private loanApiService = inject(LoanApiService);

  // Signals for reactive state
  searchText = signal('');
  allData = signal<LoanDetailDto[]>([]);
  loading = signal(false);
  selectedItem = signal<LoanDetailDto | null>(null);

  // Pagination signals
  pageSize = signal(10);
  pageIndex = signal(0);
  totalItems = signal(0);

  // Table columns - updated to match LoanDetailDto
  displayedColumns: string[] = ['id', 'loanId', 'balance', 'ltv', 'insuranceType'];

  // Computed signal for filtered and paginated data
  displayedData = computed(() => {
    const search = this.searchText().toLowerCase();
    const data = this.allData();

    // Filter data based on search
    const filtered = data.filter(item =>
      item.loanId.toString().includes(search) ||
      item.insuranceType?.toLowerCase().includes(search) || ''
    );

    // Update total items for pagination
    this.totalItems.set(filtered.length);

    // Apply pagination
    const startIndex = this.pageIndex() * this.pageSize();
    const endIndex = startIndex + this.pageSize();

    return filtered.slice(startIndex, endIndex);
  });

  ngOnInit(): void {
    this.loadData();
  }

  // Load data - simulated for now, replace with actual API call
  loadData(): void {
    this.loading.set(true);

    // TODO: Replace with actual API call when backend supports listing
    // this.loanApiService.searchLoans('', 0, 100).subscribe({
    //   next: (data) => {
    //     this.allData.set(data);
    //     this.loading.set(false);
    //   },
    //   error: (err) => {
    //     console.error('Error loading loans:', err);
    //     this.loading.set(false);
    //   }
    // });

    // Simulated data matching LoanDetailDto structure
    setTimeout(() => {
      const mockData: LoanDetailDto[] = Array.from({ length: 50 }, (_, i) => ({
        id: i + 1,
        loanId: 10000 + i,
        balance: Math.random() * 500000,
        ltv: Math.random() * 100,
        insuranceType: i % 3 === 0 ? 'CMHC' : i % 3 === 1 ? 'Genworth' : 'Conventional'
      }));

      this.allData.set(mockData);
      this.loading.set(false);
    }, 1000);
  }

  // Handle search input
  onSearchChange(value: string): void {
    this.searchText.set(value);
    this.pageIndex.set(0);
  }

  // Handle pagination change
  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  // Handle row click - fetch item by ID using real API
  onRowClick(item: LoanDetailDto): void {
    this.loading.set(true);

    this.loanApiService.getLoanById(item.loanId).subscribe({
      next: (data) => {
        this.selectedItem.set(data);
        this.loading.set(false);
        console.log('Fetched loan by ID:', data);
      },
      error: (err) => {
        console.error('Error fetching loan:', err);
        this.loading.set(false);
      }
    });
  }

  // Close detail view
  closeDetail(): void {
    this.selectedItem.set(null);
  }

  // Format currency
  formatCurrency(value: number | null): string {
    if (value === null) return 'N/A';
    return `$${value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
  }

  // Format percentage
  formatPercentage(value: number | null): string {
    if (value === null) return 'N/A';
    return `${value.toFixed(2)}%`;
  }
}