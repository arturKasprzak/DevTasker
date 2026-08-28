import { Routes } from '@angular/router';
import { TaskList } from './components/task-list/task-list';
import { NotFound } from './components/not-found/not-found';

export const routes: Routes = [
    { path: 'tasks', component: TaskList },
    { path: '', redirectTo: 'tasks', pathMatch: 'full' },
    { path: '**', component: NotFound }
];
