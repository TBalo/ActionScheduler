import { Injectable } from '@angular/core';
import { TodoList } from './todo-list.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TodoListService {

  constructor(private http:HttpClient) {}

  readonly baseURL = 'http://localhost:5093/api/TodoList'
  readonly baseURLgetAsc = 'http://localhost:5093/api/TodoList/GetTodoListsAsc'
  readonly baseURLgetDesc = 'http://localhost:5093/api/TodoList/GetTodoListsDesc'

  formData:TodoList = new TodoList();
  list : TodoList[];

  postTodoList(){
    return this.http.post(this.baseURL, this.formData);
  }

  putTodoList(){
    return this.http.put(`${this.baseURL}/${this.formData.listId}`, this.formData);
  }

  deleteTodoList(id:number){
    return this.http.delete(`${this.baseURL}/${id}`);
  }
 
	refreshList(){
		this.http.get(this.baseURLgetAsc)
		.toPromise()
			.then(res => this.list = res as TodoList[]);
		}

    getListAsc(){
      this.http.get(this.baseURLgetAsc)
      .toPromise()
        .then(res => this.list = res as TodoList[]);
      }

      getListDesc(){
        this.http.get(this.baseURLgetDesc)
        .toPromise()
          .then(res => this.list = res as TodoList[]);
        }
    
  

}
