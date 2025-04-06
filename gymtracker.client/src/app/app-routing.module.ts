import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExercisesComponent } from './features/exercises/pages/exercises.component';
import { RoutinesComponent } from './features/routines/pages/routines/routines.component';
import { RoutineDetailComponent } from './features/routines/pages/routine-detail/routine-detail.component';
import { WorkoutDayExercisesComponent } from './features/workoutDays/pages/workout-day-exercises/workout-day-exercises.component';

const routes: Routes = [
  { path: '', redirectTo: '/routines', pathMatch: 'full' },
  { path: 'exercises', component: ExercisesComponent },
  { path: 'routines', component: RoutinesComponent },
  { path: 'routines/:id', component: RoutineDetailComponent },
  { path: 'routines/:id/workoutday/:id', component: WorkoutDayExercisesComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
