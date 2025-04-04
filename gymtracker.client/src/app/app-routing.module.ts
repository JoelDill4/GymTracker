import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExercisesComponent } from './features/exercises/pages/exercises/exercises.component';
import { RoutinesComponent } from './features/routines/pages/routines/routines.component';

const routes: Routes = [
  { path: '', redirectTo: '/routines', pathMatch: 'full' },
  { path: 'exercises', component: ExercisesComponent },
  { path: 'routines', component: RoutinesComponent },
  { path: '**', redirectTo: '/exercises' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
