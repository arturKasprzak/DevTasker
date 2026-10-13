import { Component, OnInit, signal, inject } from '@angular/core';
import { TaskResponse } from '../../models/task-response';
import { Task } from '../../services/task';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-task-list',
  imports: [RouterLink],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})
export class TaskList implements OnInit{
  private taskService = inject(Task);

  tasks = signal<TaskResponse[]>([]);

  ngOnInit(): void {
    this.loadTasks();
  }

  onDelete(id: string): void {
    this.taskService.deleteTask(id).subscribe({
      next: () => {
        this.loadTasks();
      },
      error: (err) => {
        if(err.status === 404){
          this.loadTasks();          
          alert('This task has already been deleted.');
        }
        else {
          alert('An error occurred during deletion.');
        }
      }
    });
  }

  loadTasks(): void {
    this.taskService.getTasks().subscribe((tasks) => {
      this.tasks.set(tasks);
    });
  }
}
