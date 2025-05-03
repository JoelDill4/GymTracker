import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-upper-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './upper-navbar.component.html',
  styleUrls: ['./upper-navbar.component.scss']
})
export class UpperNavbarComponent {
  segment = '';
  segments: string[] = [];
  selectedSegment = '';

  constructor(private router: Router) {
    this.router.events.subscribe(() => {
      this.updateSegments();
    });
    this.updateSegments();
  }

  updateSegments() {
    const url = this.router.url;
    if (url.startsWith('/routines')) {
      this.segments = ['My routines'];
      this.selectedSegment = 'My routines';
    } else if (url.startsWith('/exercises')) {
      this.segments = ['My exercises', 'Progression chart'];
      if (!this.selectedSegment || !this.segments.includes(this.selectedSegment)) {
        this.selectedSegment = 'My exercises';
      }
    } else if (url.startsWith('/workouts')) {
      this.segments = ['My workouts'];
      this.selectedSegment = 'My workouts';
    } else {
      this.segments = [];
      this.selectedSegment = '';
    }
  }

  selectSegment(segment: string) {
    this.selectedSegment = segment;
    const url = this.router.url;
    
    if (url.startsWith('/exercises')) {
      if (segment === 'My exercises') {
        this.router.navigate(['/exercises']);
      } else if (segment === 'Progression chart') {
        this.router.navigate(['/exercises/progression']);
      }
    } else if (url.startsWith('/routines')) {
      if (segment === 'My routines') {
        this.router.navigate(['/routines']);
      }
    } else if (url.startsWith('/workouts')) {
      if (segment === 'My workouts') {
        this.router.navigate(['/workouts']);
      }
    }
  }
} 
