import { Routes } from '@angular/router';
import { TaskList } from './components/task-list/task-list';
import { TaskForm } from './components/task-form/task-form';
import { NotFound } from './components/not-found/not-found';

export const routes: Routes = [
    { path: 'tasks', component: TaskList },
    { path: 'tasks/new', component: TaskForm },
    { path: '', redirectTo: 'tasks', pathMatch: 'full' },
    { path: '**', component: NotFound }
];
