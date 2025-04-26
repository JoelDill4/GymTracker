import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

export interface FilterOption {
  id: string;
  name: string;
}

@Component({
  selector: 'app-list-search-filter',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="row mb-4">
      <div class="col-md-6" *ngIf="showSearch">
        <div class="input-group">
          <span class="input-group-text bg-white">
            <i class="bi bi-search"></i>
          </span>
          <input type="text"
                 class="form-control"
                 [placeholder]="searchPlaceholder"
                 [(ngModel)]="searchTerm"
                 (ngModelChange)="onSearch()"
                 (keyup.enter)="onSearch()">
          <button class="btn btn-outline-secondary"
                  type="button"
                  (click)="onSearch()">
            Search
          </button>
          <button class="btn btn-outline-secondary"
                  type="button"
                  (click)="clearSearch()"
                  *ngIf="searchTerm">
            <i class="bi bi-x-lg"></i>
          </button>
        </div>
      </div>
      <div class="col-md-6" *ngIf="showFilter && filterOptions">
        <div class="input-group">
          <span class="input-group-text bg-white">
            <i [class]="'bi ' + filterIcon"></i>
          </span>
          <select class="form-select"
                  [(ngModel)]="selectedFilterId"
                  (ngModelChange)="onFilterChange()">
            <option [ngValue]="null">{{ filterPlaceholder }}</option>
            <option *ngFor="let option of filterOptions" [value]="option.id">
              {{ option.name }}
            </option>
          </select>
          <button class="btn btn-outline-secondary"
                  type="button"
                  (click)="clearFilter()"
                  *ngIf="selectedFilterId">
            <i class="bi bi-x-lg"></i>
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .input-group {
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
    }
    
    .input-group-text {
      border-right: none;
    }
    
    .form-control, .form-select {
      border-left: none;
    }
    
    .form-control:focus, .form-select:focus {
      box-shadow: none;
      border-color: #ced4da;
    }
  `]
})
export class ListSearchFilterComponent {
  @Input() showSearch = true;
  @Input() showFilter = true;
  @Input() searchPlaceholder = 'Search...';
  @Input() filterPlaceholder = 'All';
  @Input() filterIcon = 'bi-funnel';
  @Input() filterOptions: FilterOption[] = [];

  @Output() search = new EventEmitter<string>();
  @Output() filter = new EventEmitter<string | null>();
  @Output() clear = new EventEmitter<void>();

  searchTerm = '';
  selectedFilterId: string | null = null;

  onSearch(): void {
    this.search.emit(this.searchTerm);
  }

  onFilterChange(): void {
    this.filter.emit(this.selectedFilterId);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.search.emit('');
  }

  clearFilter(): void {
    this.selectedFilterId = null;
    this.filter.emit(null);
  }
} 