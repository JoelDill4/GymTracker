import { Component, Input, Output, EventEmitter, ContentChild, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';

export interface FilterOption {
  id: string;
  name: string;
}

@Component({
  selector: 'app-list-card',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  template: `
    <div class="list-container">
      <div class="row mb-4" *ngIf="showSearch || showFilter">
        <div class="col-md-6 mb-3 mb-md-0" *ngIf="showSearch">
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

      <div class="row g-4">
        <div *ngFor="let item of items" class="col-md-6 col-lg-4">
          <div class="card h-100" (click)="onCardClick($event, item)">
            <div class="card-body d-flex flex-column">
              <h5 class="card-title mb-3">
                <i [class]="'bi ' + icon + ' me-2'"></i>{{ getItemTitle(item) }}
              </h5>
              <div class="flex-grow-1">
                <p class="card-text text-muted mb-0" *ngIf="getItemDescription(item)">{{ getItemDescription(item) }}</p>
              </div>
              <div class="d-flex align-items-center mt-3 pt-3 border-top">
                <ng-container *ngIf="getItemBadge(item)">
                  <span class="badge bg-primary" (click)="onBadgeClick($event, item)" [style.cursor]="getItemBadge(item)?.routerLink ? 'pointer' : 'default'">
                    <i [class]="'bi ' + getItemBadge(item)?.icon + ' me-1'" *ngIf="getItemBadge(item)?.icon"></i>{{ getItemBadge(item)?.text }}
                  </span>
                </ng-container>
                <div class="ms-auto action-buttons">
                  <ng-container *ngTemplateOutlet="actionButtons; context: { $implicit: item }"></ng-container>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .list-container {
      width: 100%;
    }

    .card {
      transition: transform 0.3s ease, box-shadow 0.3s ease;
      cursor: pointer;
    }

    .card:hover {
      transform: translateY(-5px);
      box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1);
    }

    .card-text {
      overflow: hidden;
      display: -webkit-box;
      -webkit-line-clamp: 3;
      -webkit-box-orient: vertical;
      line-height: 1.5;
    }

    .badge {
      font-size: 0.9rem;
      padding: 0.5rem 0.75rem;
      border-radius: 6px;
    }

    .action-buttons {
      z-index: 1;
    }

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
export class ListCardComponent<T> {
  @Input() items: T[] = [];
  @Input() showSearch = false;
  @Input() showFilter = false;
  @Input() searchPlaceholder = 'Search...';
  @Input() filterPlaceholder = 'All';
  @Input() filterIcon = 'bi-funnel';
  @Input() filterOptions: FilterOption[] = [];
  @Input() icon: string = '';
  @Input() getItemTitle: (item: T) => string = () => '';
  @Input() getItemDescription: (item: T) => string | undefined = () => undefined;
  @Input() getItemBadge: (item: T) => { text: string; icon?: string; routerLink?: any[] } | undefined = () => undefined;
  @Input() getItemRouterLink: (item: T) => any[] = () => [];

  @Output() search = new EventEmitter<string>();
  @Output() filter = new EventEmitter<string | null>();
  @Output() itemClick = new EventEmitter<T>();
  @Output() badgeClick = new EventEmitter<T>();

  @ContentChild(TemplateRef) actionButtons!: TemplateRef<any>;

  searchTerm = '';
  selectedFilterId: string | null = null;

  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) {}

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

  onCardClick(event: MouseEvent, item: T): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.action-buttons') && !target.closest('.badge')) {
      const routerLink = this.getItemRouterLink(item);
      if (routerLink.length > 0) {
        this.router.navigate(routerLink, { relativeTo: this.route });
      }
      this.itemClick.emit(item);
    }
  }

  onBadgeClick(event: MouseEvent, item: T): void {
    event.stopPropagation();
    const badge = this.getItemBadge(item);
    if (badge?.routerLink) {
      this.router.navigate(badge.routerLink, { relativeTo: this.route });
    }
    this.badgeClick.emit(item);
  }
} 
