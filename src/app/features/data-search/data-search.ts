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
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { LoanApiService, LoanDetailDto } from '../../core/index';

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
    MatCardModule,
    MatSnackBarModule
  ],
  templateUrl: './data-search.html',
  styleUrls: ['./data-search.scss']
})
export class DataSearchComponent implements OnInit {
  private loanDetailsService = inject(LoanApiService);
  private snackBar = inject(MatSnackBar);

  // Signals for reactive state
  searchText = signal('');
  allData = signal<LoanDetailDto[]>([]);
  loading = signal(false);
  selectedItem = signal<LoanDetailDto | null>(null);

  // Pagination signals
  pageSize = signal(10);
  pageIndex = signal(0);
  totalItems = signal(0);

  // Table columns
  displayedColumns: string[] = ['id', 'loanId', 'balance', 'ltv', 'insuranceType'];

  // Computed signal for filtered and paginated data
  displayedData = computed(() => {
    const search = this.searchText().toLowerCase();
    const data = this.allData();

    // Filter data based on search
    const filtered = data.filter(item =>
      item.loanId.toString().includes(search)
    );

    // Update total items for pagination
    //this.totalItems.set(data.length);

    // Apply pagination
    // const startIndex = this.pageIndex() * this.pageSize();
    // const endIndex = startIndex + this.pageSize();

    return filtered;
  });

  ngOnInit(): void {
    this.loadData();
  }

  // Load mock data - replace with actual API when available
  loadData(): void {
    this.loading.set(true);

    // TODO: Replace with actual API call when backend supports listing
    // this.loanDetailsService.getAllLoans(0, 100).subscribe({
    //   next: (data) => {
    //     this.allData.set(data);
    //     this.loading.set(false);
    //   },
    //   error: (err) => {
    //     this.snackBar.open('Error loading loans: ' + err.message, 'Close', { 
    //       duration: 5000 
    //     });
    //     this.loading.set(false);
    //   }
    // });

    // Simulated data matching backend structure
    setTimeout(() => {
      const mockData: LoanDetailDto[] = Array.from({ length: 50 }, (_, i) => ({
        id: i + 1,
        loanId: 10000 + i,
        balance: Math.random() * 500000,
        ltv: Math.random() * 100,
        insuranceType: i % 3 === 0 ? 'CMHC' : i % 3 === 1 ? 'Genworth' : 'Conventional',
        obs: Math.random() * 100,
        productBusinessGroupCode: `PBG${i}`,
        loanClass: i % 2 === 0 ? 'Residential' : 'Commercial',
        variableRateFlag: i % 2 === 0 ? 'Y' : 'N',
        province: ['ON', 'BC', 'AB', 'QC'][i % 4],
        purpose: 'Purchase',
        beacon: Math.floor(Math.random() * 200) + 600,
        riskScore: Math.random() * 100,
        amortizationTermInMonths: 300,
        termInMonths: 60,
        securityTypeCode: 1,
        securityTypeDescription: 'First Mortgage',
        commitmentDate: new Date(2024, 0, i + 1).toISOString(),
        maturityDate: new Date(2029, 0, i + 1).toISOString(),
        applicantCifNumber: `CIF${10000 + i}`,
        brokerCompany: `Broker ${i}`,
        effectiveRate: Math.random() * 5 + 2,
        exceptionFlag: null,
        exceptionType: null,
        originalTdsRatio: Math.random() * 50,
        loanConformingIndicator: 'Y',
        reasonCode: null,
        scoringValue: Math.random() * 100,
        employmentType: 'Full Time',
        occupation: 'Professional',
        income: Math.random() * 150000 + 50000,
        productText: `Product ${i}`,
        fundingDate: new Date(2024, 0, i + 1).toISOString(),
        priorEncCount: 0,
        priorEncAmount: 0,
        branchOfficeId: `BR${i}`,
        branchOfficeText: `Branch ${i}`,
        borrowerName: `Borrower ${i + 1}`,
        underwriterSkey: `UW${i}`,
        partnerName: null,
        partnerId: null,
        director: null,
        date1: null,
        reviewType: null,
        nonConformingReason: null,
        mortgageOfficerBp: null,
        mortgageOfficerName: null,
        mortgageOfficerSkey: null,
        aggRating: null,
        prRating: null,
        spRating: null,
        userName: null,
        stressTds: null,
        isLocked: 0,
        createdAt: new Date(2024, 0, i + 1),
        updatedAt: null,
        deletedAt: null
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

    this.loanDetailsService.getLoanById(item.loanId).subscribe({
      next: (data) => {
        this.selectedItem.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        this.snackBar.open('Error fetching loan: ' + err.message, 'Close', {
          duration: 5000
        });
        this.loading.set(false);
        // Fallback to showing the row data
        this.selectedItem.set(item);
      }
    });
  }

  // Close detail view
  closeDetail(): void {
    this.selectedItem.set(null);
  }

  // Format currency
  formatCurrency(value: number | null | undefined): string {
    if (value === null || value === undefined) return 'N/A';
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 2
    }).format(value);
  }

  // Format percentage
  formatPercentage(value: number | null | undefined): string {
    if (value === null || value === undefined) return 'N/A';
    return `${value.toFixed(2)}%`;
  }

  // Format date
  formatDate(value: Date | string | null | undefined): string {
    if (!value) return 'N/A';
    const date = typeof value === 'string' ? new Date(value) : value;
    return date.toLocaleDateString('en-US');
  }
}