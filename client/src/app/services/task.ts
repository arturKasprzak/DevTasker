import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TaskResponse } from '../models/task-response';
import { CreateTaskRequest } from '../models/create-task-request';

@Injectable({
    providedIn: 'root'
})
export class Task {
    private http = inject(HttpClient);
    private apiUrl = 'https://localhost:7187/api/tasks'

    getTasks(): Observable<TaskResponse[]> {
        return this.http.get<TaskResponse[]>(this.apiUrl);
    }

    createTask(request: CreateTaskRequest): Observable<string> {
        return this.http.post<string>(this.apiUrl, request);
    }

    deleteTask(id: string): Observable<void> {
        return this.http.delete<void>(this.apiUrl + `/${id}`);
    }
}
