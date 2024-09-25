import { Injectable } from '@angular/core';
import { TodoList } from './todo-list.model'; 
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TodoListService {
  private userId: number = Number(localStorage.getItem('userId')); 

  private baseURL = 'https://todolist-qlng.onrender.com/api/TodoList';

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
    return this.http.post<TodoList>(`${this.baseURL}/CreateTask`, this.formData, this.getHttpOptions());
  }

  putTodoList(): Observable<void> {
    this.initializeUserId(); 
    return this.http.put<void>(`${this.baseURL}/UpdateTask?id=${this.formData.listId}`, this.formData, this.getHttpOptions());
  }

  deleteTodoList(id: number): Observable<void> {
    this.initializeUserId(); 
    return this.http.delete<void>(`${this.baseURL}/Delete?id=${id}`, this.getHttpOptions());
  }

  refreshList(): void {
    this.getListAsc().subscribe(
      (res: TodoList[]) => { 
        this.list = res; 
        console.log(this.list); 
      },
      error => {
        console.error('Error fetching list:', error); 
      }
    );
  }

  resetFormData() {
    this.formData = new TodoList(); 
  }

  getListAsc(): Observable<TodoList[]> {
    this.initializeUserId(); 
    if (!this.userId) {
      return of([]); 
    }
  
    return this.http.get<{ statusCode: number, message: string, data: TodoList[] }>(
      `${this.baseURL}/GetUserTasksAsc?userId=${this.userId}`, 
      this.getHttpOptions()
    ).pipe(
      map(response => response.data || []), 
      catchError(err => {
        console.error('Error fetching list ascending:', err);
        return of([]); 
      })
    );
  }

  getListDesc(): Observable<TodoList[]> {
    this.initializeUserId(); 
    if (!this.userId) {
      return of([]); 
    }
  
    return this.http.get<{ statusCode: number, message: string, data: TodoList[] }>(
      `${this.baseURL}/GetUserTasksDesc?userId=${this.userId}`, 
      this.getHttpOptions()
    ).pipe(
      map(response => response.data || []), 
      catchError(err => {
        console.error('Error fetching list ascending:', err);
        return of([]); 
      })
    );
  }

  


  updateUserId(newUserId: number): void {
    this.userId = newUserId;
    localStorage.setItem('userId', newUserId.toString());
  }

  clearCache(): void {
    this.list = []; 
  }
}
