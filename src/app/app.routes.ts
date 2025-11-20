import { Routes } from '@angular/router';
import { MainLayoutComponent } from './layout/main-page/main-layout/main-layout.component';

export const routes: Routes = [
{
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'import', loadComponent: () => import('./features/import/import-file/import-file').then(m => m.ImportFile) },
      { path: 'search', loadComponent: () => import('./features/data-search/data-search').then(m => m.DataSearchComponent) }
      //{ path: 'dashboard', loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) },
      //{ path: 'data-management', loadComponent: () => import('./features/data-management/data-management.component').then(m => m.DataManagementComponent) },
      // Add more routes...
    ]
  }
];
