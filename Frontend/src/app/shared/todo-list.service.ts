import { Injectable } from '@angular/core';
import { TodoList } from './todo-list.model'; 
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TodoListService {
  private userId: number = Number(localStorage.getItem('userId')); 

  readonly baseURL = `http://localhost:8080/api/TodoList`;
  readonly baseURLgetAsc = `http://localhost:8080/api/TodoList/user/asc/`;
  readonly baseURLgetDesc = `http://localhost:8080/api/TodoList/user/desc/`;

  formData: TodoList = new TodoList();
  list: TodoList[] = [];

  constructor(private http: HttpClient) {}

  private initializeUserId(): void {
    this.userId = Number(localStorage.getItem('userId')); 
    if (!this.userId) {
      console.error('No userId found in localStorage.');
    }
  }

  postTodoList(): Observable<TodoList> {
    this.initializeUserId(); 
    this.formData.userId = this.userId;
    return this.http.post<TodoList>(this.baseURL, this.formData);
  }

  putTodoList(): Observable<void> {
    this.initializeUserId(); 
    return this.http.put<void>(`${this.baseURL}/${this.formData.listId}`, this.formData);
  }

  deleteTodoList(id: number): Observable<void> {
    this.initializeUserId(); 
    return this.http.delete<void>(`${this.baseURL}/${id}`);
  }

  refreshList(): void {
    this.getListAsc().subscribe(res => this.list = res);
  }

  getListAsc(): Observable<TodoList[]> {
    this.initializeUserId(); 
    return this.userId ? this.http.get<TodoList[]>(`${this.baseURLgetAsc}${this.userId}`) : of([]);
  }

  getListDesc(): Observable<TodoList[]> {
    this.initializeUserId(); 
    return this.userId ? this.http.get<TodoList[]>(`${this.baseURLgetDesc}${this.userId}`) : of([]);
  }

  updateUserId(newUserId: number): void {
    this.userId = newUserId;
    localStorage.setItem('userId', newUserId.toString());
  }

  clearCache(): void {
    this.list = []; 
  }
}
