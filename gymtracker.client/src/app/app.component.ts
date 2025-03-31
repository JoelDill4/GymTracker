import { Component } from '@angular/core';
import { ExercisesComponent } from './features/exercises/pages/exercises.component';

@Component({
  selector: 'app-root',
  template: `
    <app-exercises></app-exercises>
  `,
  standalone: true,
  imports: [ExercisesComponent]
})
export class AppComponent {
  title = 'gymtracker';
} 
