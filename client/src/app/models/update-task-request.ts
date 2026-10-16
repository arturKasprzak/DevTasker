export interface UpdateTaskRequest {
    title: string;
    description: string | null;
    status: boolean;
    userId: string | null;
  }