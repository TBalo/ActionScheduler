import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = 'https://todolist-qlng.onrender.com/api/Auth';

  constructor(private http: HttpClient, private toastr: ToastrService) { }
  private getHttpOptions() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': '*/*'
      })
    };
  }

  signup(user: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/signup`, user, this.getHttpOptions());
  }

  login(user: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/login`, user, this.getHttpOptions());
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUserId(): string | null {
    return localStorage.getItem('userId');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}