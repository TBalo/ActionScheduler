import { getLocaleDateTimeFormat } from "@angular/common";

export class TodoList {
    listId:number=0;
    task:string='';
    status:boolean=false;
    dueDate:Date;
    userId:number;
}


export interface TodoListDto {
    listId: number;
    task: string;
    status: boolean;
    dueDate: string | null; 
    userId: number;
  }

  export interface UpdateTodoListPayload {
    todoListDto: TodoListDto; 
  }