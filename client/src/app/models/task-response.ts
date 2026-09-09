export interface TaskResponse{
    id: string;
    title: string;
    description: string | null;
    status: boolean;
    userName: string;
}