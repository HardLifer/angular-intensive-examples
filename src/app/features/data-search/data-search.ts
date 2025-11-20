import { Component, signal, computed, OnInit } from '@angular/core';
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

// Interface for table data
interface DataItem {
  id: number;
  name: string;
  email: string;
  status: string;
  createdDate: string;
}

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
  // Signals for reactive state
  searchText = signal('');
  allData = signal<DataItem[]>([]);
  loading = signal(false);
  selectedItem = signal<DataItem | null>(null);

  // Pagination signals
  pageSize = signal(10);
  pageIndex = signal(0);
  totalItems = signal(0);

  // Table columns
  displayedColumns: string[] = ['id', 'name', 'email', 'status', 'createdDate'];

  // Computed signal for filtered and paginated data
  displayedData = computed(() => {
    const search = this.searchText().toLowerCase();
    const data = this.allData();
    
    // Filter data based on search
    const filtered = data.filter(item => 
      item.name.toLowerCase().includes(search) ||
      item.email.toLowerCase().includes(search) ||
      item.status.toLowerCase().includes(search)
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

  // Simulate API call to load data
  loadData(): void {
    this.loading.set(true);

    // TODO: Replace with actual API call
    // Example: this.http.get<DataItem[]>('your-api-endpoint/data')
    
    // Simulated data
    setTimeout(() => {
      const mockData: DataItem[] = Array.from({ length: 50 }, (_, i) => ({
        id: i + 1,
        name: `User ${i + 1}`,
        email: `user${i + 1}@example.com`,
        status: i % 3 === 0 ? 'Active' : i % 3 === 1 ? 'Inactive' : 'Pending',
        createdDate: new Date(2024, Math.floor(Math.random() * 12), Math.floor(Math.random() * 28) + 1).toISOString().split('T')[0]
      }));

      this.allData.set(mockData);
      this.loading.set(false);
    }, 1000);
  }

  // Handle search input
  onSearchChange(value: string): void {
    this.searchText.set(value);
    this.pageIndex.set(0); // Reset to first page when searching
  }

  // Handle pagination change
  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
  }

  // Handle row click - fetch item by ID
  onRowClick(item: DataItem): void {
    this.loading.set(true);
    
    // TODO: Replace with actual API call
    // Example: this.http.get<DataItem>(`your-api-endpoint/data/${item.id}`)
    
    // Simulated API call
    setTimeout(() => {
      this.selectedItem.set(item);
      this.loading.set(false);
      console.log('Fetched item by ID:', item.id, item);
    }, 500);
  }

  // Close detail view
  closeDetail(): void {
    this.selectedItem.set(null);
  }
}