import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { CreateTaskRequest } from '../../models/create-task-request';
import { Task } from '../../services/task'
import { Router } from '@angular/router';

@Component({
  selector: 'app-task-form',
  imports: [ReactiveFormsModule],
  templateUrl: './task-form.html',
  styleUrl: './task-form.css',
})
export class TaskForm {
  private fb = inject(FormBuilder);
  private taskService = inject(Task);
  private router = inject(Router);

  taskForm = this.fb.group({
    title: ['', Validators.required],
    description: [null as string | null],
    status: [false],
    userId: [null as string | null]
  });

  onSubmit(): void {
    if(this.taskForm.invalid)
    {
      this.taskForm.markAllAsTouched();
      return;
    }

    const requestPayload = this.taskForm.getRawValue() as CreateTaskRequest

    this.taskService.createTask(requestPayload).subscribe({
      next: (newTaskId: string) => {
        this.router.navigate(['/tasks']);
      },
      error: (err) => {
        console.error('Error:', err);
      }
    })
  }
}
