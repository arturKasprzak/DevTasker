export interface CreateTaskRequest {
    title: string;
    description: string | null;
    status: boolean;
    userId: string | null;
  }