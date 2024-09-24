import { getLocaleDateTimeFormat } from "@angular/common";

export class TodoList {
    listId:number=0;
    task:string='';
    status:boolean=false;
    dueDate:Date;
    userId:number;
}
