import { Component, OnInit, signal, inject } from '@angular/core';
import { TaskResponse } from '../../models/task-response';
import { Task } from '../../services/task';

@Component({
  selector: 'app-task-list',
  imports: [],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})
export class TaskList implements OnInit{
  private taskService = inject(Task);

  tasks = signal<TaskResponse[]>([]);

  ngOnInit(): void {
    this.taskService.getTasks().subscribe((tasks) => {
      this.tasks.set(tasks);
    });
  }
}
