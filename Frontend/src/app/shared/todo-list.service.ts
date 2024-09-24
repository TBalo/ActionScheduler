import { Injectable } from '@angular/core';
import { TodoList } from './todo-list.model'; 
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TodoListService {
  private userId: number = Number(localStorage.getItem('userId')); 

  private baseURL = `https://todolist-qlng.onrender.com/api/TodoList`;
  readonly baseURLgetAsc = `https://todolist-qlng.onrender.com/api/TodoList/user/asc/`;
  readonly baseURLgetDesc = `https://todolist-qlng.onrender.com/api/TodoList/user/desc/`;

  formData: TodoList = new TodoList();
  list: TodoList[] = [];

  constructor(private http: HttpClient) { }
  private getHttpOptions() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': '*/*'
      })
    };
  }
  private initializeUserId(): void {
    this.userId = Number(localStorage.getItem('userId')); 
    if (!this.userId) {
      console.error('No userId found in localStorage.');
    }
  }

  postTodoList(): Observable<TodoList> {
    this.initializeUserId(); 
    this.formData.userId = this.userId;
    return this.http.post<TodoList>(`${this.baseURL}`, this.formData, this.getHttpOptions());
  }

  putTodoList(): Observable<void> {
    this.initializeUserId(); 
    return this.http.put<void>(`${this.baseURL}/${this.formData.listId}`, this.formData, this.getHttpOptions());
  }

  deleteTodoList(id: number): Observable<void> {
    this.initializeUserId(); 
    return this.http.delete<void>(`${this.baseURL}/${id}`, this.getHttpOptions());
  }

  refreshList(): void {
    this.getListAsc().subscribe(res => this.list = res);
  }

  getListAsc(): Observable<TodoList[]> {
    this.initializeUserId(); 
    return this.userId ? this.http.get<TodoList[]>(`${this.baseURLgetAsc}${this.userId}`) : of([]);
  }

  // getListDesc(): Observable<TodoList[]> {
  //   this.initializeUserId(); 
  //   return this.userId ? this.http.get<TodoList[]>(`${this.baseURLgetDesc}${this.userId}`) : of([]);
  // }

  updateUserId(newUserId: number): void {
    this.userId = newUserId;
    localStorage.setItem('userId', newUserId.toString());
  }

  clearCache(): void {
    this.list = []; 
  }
}
