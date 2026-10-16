import { Component, OnInit, inject, input } from '@angular/core';
import { Task } from '../../services/task';
import { UpdateTaskRequest } from '../../models/update-task-request';
import { TaskResponse } from '../../models/task-response';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-task-edit',
  imports: [ReactiveFormsModule],
  templateUrl: './task-edit.html',
  styleUrl: './task-edit.css',
})
export class TaskEdit implements OnInit {
  readonly id = input.required<string>();

  private taskService = inject(Task)
  private fb = inject(FormBuilder)
  private router = inject(Router)

  taskForm = this.fb.group({
    title: ['', Validators.required],
    description: [''],
    status: [false],
    userId: [null as string | null]
  });

  ngOnInit(): void {
    const taskId = this.id();

    this.taskService.getTask(taskId).subscribe({
      next: (task: TaskResponse) =>
      {
        this.taskForm.patchValue({
          title : task.title,
          description : task.description,
          status : task.status,
          userId : task.userId
        })
      },
      error(err) {
        console.error('Failed to retrieve the task.', err)
      }
    });
  }

  onSubmit(): void {
    if(this.taskForm.valid)
    {
      const requestPayload = this.taskForm.getRawValue() as UpdateTaskRequest;

      this.taskService.updateTask(this.id(), requestPayload).subscribe({
        next: () => {
          console.log('Task updated successfully.');
          this.router.navigate(['/tasks']);
        },
        error: (err) => {
          console.error('Error updating the task', err);
        }
      });
    }
  }
}
