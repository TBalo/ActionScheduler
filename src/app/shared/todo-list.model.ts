import { getLocaleDateTimeFormat } from "@angular/common";

export class TodoList {
    listId:number=0;
    taskDescription:string='';
    status:boolean=false;
    dueDate:Date;
}
