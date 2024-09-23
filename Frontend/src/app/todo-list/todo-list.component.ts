import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { TodoListService } from '../shared/todo-list.service';
import { TodoList } from '../shared/todo-list.model';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: [],
  providers: [DatePipe]
})
export class TodoListComponent implements OnInit {

  userName: string = localStorage.getItem('userName') || 'Guest';


  constructor(public service: TodoListService,
    private toastr: ToastrService,
    private datePipe: DatePipe 

  ) { }

  ngOnInit(): void {
    this.service.refreshList()
  }

  populateForm(selectedRecord: TodoList) {
    this.service.formData = Object.assign({}, selectedRecord);
  }

  onDelete(id: number) {
    if (confirm('Are you sure you want to delete this record?'))
      this.service.deleteTodoList(id).subscribe(
        res => {
          this.service.refreshList();
          this.toastr.error("Deleted successfully", "Task Scheduler");
        },
        err => {
          console.log(err);
        });
  }

  orderDesc() {
    this.service.getListDesc()
    this.toastr.success('Listed in descending order', 'Task Scheduler');
  }

  orderAsc() {
    this.service.getListAsc();
    this.toastr.success('Listed in ascending order', 'Task Scheduler');
  }

  formatDateWithSuffix(date: Date | string): string {
    if (!date) return ''; 
    if (typeof date === 'string') {
      date = new Date(date);
    }
  
    if (!(date instanceof Date) || isNaN(date.getTime())) {
      return ''; 
    }
    const day = date.getDate();
    const suffix = this.getOrdinalSuffix(day);
    const formattedDate = `${day}${suffix} of ${this.getMonthName(date.getMonth())}, ${date.getFullYear()}`;
    return formattedDate;
  }
  
  private getOrdinalSuffix(day: number): string {
    if (day === 1 || day === 21 || day === 31) {
      return 'st';
    } else if (day === 2 || day === 22) {
      return 'nd';
    } else if (day === 3 || day === 23) {
      return 'rd';
    } else {
      return 'th';
    }
  }
  
  private getMonthName(month: number): string {
    const months = [
      'January', 'February', 'March', 'April', 'May', 'June',
      'July', 'August', 'September', 'October', 'November', 'December'
    ];
    return months[month];
  }
}

