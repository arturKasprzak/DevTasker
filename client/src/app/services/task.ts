import { Service, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TaskResponse } from '../models/task-response';

@Service()
export class Task {
    private http = inject(HttpClient);

    getTasks(): Observable<TaskResponse[]> {
        return this.http.get<TaskResponse[]>('https://localhost:7187/api/tasks');
    }
}
