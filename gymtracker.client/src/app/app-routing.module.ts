import { RouterModule, Routes } from '@angular/router';
import { ExercisesComponent } from './features/exercises/pages/exercises/exercises.component';
import { NgModule } from '@angular/core';

export const routes: Routes = [
  { path: '', redirectTo: '/exercises', pathMatch: 'full' },
  { path: 'exercises', component: ExercisesComponent },
  { path: '**', redirectTo: '/exercises' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
