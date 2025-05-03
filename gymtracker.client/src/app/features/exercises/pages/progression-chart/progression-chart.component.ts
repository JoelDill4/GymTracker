import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgxApexchartsModule, ApexAxisChartSeries, ApexChart, ApexXAxis, ApexTitleSubtitle } from 'ngx-apexcharts';
import { WorkoutService } from '../../../workouts/services/workout.service';
import { ExerciseService } from '../../../exercises/services/exercise.service';
import { Workout } from '../../../workouts/models/workout.model';
import { Exercise } from '../../models/exercise.model';
import { ExerciseSet } from '../../../exercisesSets/models/exercise-set.model';

@Component({
  selector: 'app-progression-chart',
  standalone: true,
  imports: [CommonModule, FormsModule, NgxApexchartsModule],
  templateUrl: './progression-chart.component.html',
  styleUrls: ['./progression-chart.component.scss']
})
export class ProgressionChartComponent implements OnInit {
  workouts: Workout[] = [];
  exercises: Exercise[] = [];
  selectedExercise = '';
  startDate: string = '';
  endDate: string = '';
  error = '';
  loading = false;

  constructor(
    private workoutService: WorkoutService,
    private exerciseService: ExerciseService
  ) { }

  ngOnInit(): void {
    this.loadAllExercises();
  }

  refreshData(): void {
    if (!this.selectedExercise) {
      this.error = 'Please select an exercise first';
      return;
    }
    this.loadWorkouts();
  }

  loadWorkouts(): void {
    this.loading = true;
    this.error = '';
    this.workoutService.getWorkoutsByDateRange(this.startDate, this.endDate).subscribe({
      next: (data) => {
        this.workouts = data;
        this.updateChartData();
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load workouts';
        this.loading = false;
      }
    });
  }

  loadAllExercises(): void {
    this.exerciseService.getExercises().subscribe({
      next: (exercises) => {
        this.exercises = exercises;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load exercises';
      }
    });
  }

  private updateChartData(): void {
    if (!this.selectedExercise) {
      this.chartSeries = [{ name: 'Weight (kg)', data: [] }];
      this.chartXAxis.categories = [];
      return;
    }

    const exerciseSets: { date: string; weight: number }[] = [];
    
    this.workouts.forEach(workout => {
      workout.exerciseSets?.forEach(set => {
        if (set.exercise.id === this.selectedExercise) {
          exerciseSets.push({
            date: new Date(workout.workoutDate).toISOString().split('T')[0],
            weight: set.weight
          });
        }
      });
    });

    exerciseSets.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());

    this.chartSeries = [{
      name: 'Weight (kg)',
      data: exerciseSets.map(set => set.weight)
    }];

    this.chartXAxis = {
      categories: exerciseSets.map(set => set.date)
    };
  }

  public chartSeries: ApexAxisChartSeries = [
    {
      name: 'Weight (kg)',
      data: []
    }
  ];

  public chartDetails: ApexChart = {
    type: 'line',
    height: 350,
    toolbar: {
      show: true,
      tools: {
        download: true,
        selection: true,
        zoom: true,
        zoomin: true,
        zoomout: true,
        pan: true,
        reset: true
      }
    }
  };

  public chartXAxis: ApexXAxis = {
    categories: [],
    type: 'datetime'
  };

  public chartTitle: ApexTitleSubtitle = {
    text: 'Exercise Progression'
  };
} 
