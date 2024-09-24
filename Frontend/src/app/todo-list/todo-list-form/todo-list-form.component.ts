import { Component, OnInit  } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { TodoList } from 'src/app/shared/todo-list.model';
import { TodoListService } from 'src/app/shared/todo-list.service';

@Component({
  selector: 'app-todo-list-form',
  templateUrl: './todo-list-form.component.html',
  styleUrls: []
})
export class TodoListFormComponent implements OnInit {

  constructor(public service:TodoListService,
    private toastr: ToastrService){ }

ngOnInit(): void {
  }

  onSubmit(form:NgForm){
    if(this.service.formData.listId == 0)
      this.insertRecord(form);
    else
    this.updateRecord(form);
  }

  insertRecord(form:NgForm){
    this.service.postTodoList().subscribe(
      res =>{
              this.service.refreshList();
              this.resetForm(form);
              this.toastr.success('Submitted successfully', 'Task Scheduler');
            },
            err => {
              console.log(err);
              this.toastr.error('Failed to submit', 'Task Scheduler');
            }
      );}

  updateRecord(form:NgForm){
    this.service.putTodoList().subscribe(
      res =>{
              this.service.refreshList();
              this.resetForm(form);
              this.toastr.info('Updated Successfully', 'Task Scheduler');
            },
      err =>{
        console.log(err);
            }
      );
  }

  resetForm(form:NgForm){
    form.form.reset();
    this.service.formData = new TodoList();
  }
}
