import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { TodoList, TodoListDto, UpdateTodoListPayload } from 'src/app/shared/todo-list.model';
import { TodoListService } from 'src/app/shared/todo-list.service';

@Component({
  selector: 'app-todo-list-form',
  templateUrl: './todo-list-form.component.html',
  styleUrls: [],
})
export class TodoListFormComponent implements OnInit {
  constructor(public service: TodoListService, private toastr: ToastrService,  private datePipe: DatePipe,
  ) {}

  ngOnInit(): void {}

  onSubmit(form: NgForm) {
    if (!this.service.formData.listId) {
      this.service.formData.listId = 0;
    }
  
    const formattedDueDate = this.datePipe.transform(this.service.formData.dueDate, 'yyyy-MM-ddTHH:mm:ss.SSSZ') || '1970-01-01T00:00:00.000Z'; // Use a default date
  
    const payload: UpdateTodoListPayload = {
      todoListDto: {
        listId: this.service.formData.listId,
        task: this.service.formData.task,
        status: this.service.formData.status,
        dueDate: formattedDueDate,  // Ensure this is a string
        userId: this.service.formData.userId
      }
    };
  
    // Pass the payload to your updateRecord method
    if (payload.todoListDto.listId === 0) {
      this.insertRecord(payload, form);
    } else {
      this.updateRecord(payload, form);
    }
  }
  

  insertRecord(payload: UpdateTodoListPayload, form: NgForm) {
    this.service.postTodoList(payload).subscribe(
      (res) => {
        this.resetForm(form); 
        this.toastr.success('Task added successfully', 'Task Scheduler');
        this.service.refreshList();
      },
      (err) => {
        console.log(err);
        this.toastr.error('Failed to submit', 'Task Scheduler');
      }
    );
  }

  updateRecord(payload: UpdateTodoListPayload, form: NgForm) {
    this.service.putTodoList(payload).subscribe(
      (res) => {
        this.resetForm(form);
        this.toastr.success('Task updated successfully', 'Task Scheduler');
        this.service.refreshList();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  resetForm(form: NgForm) {
    form.reset();
    this.service.formData = new TodoList();
  }
}
