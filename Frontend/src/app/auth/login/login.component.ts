import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/shared/AuthService';
import { TodoListService } from 'src/app/shared/todo-list.service'; 

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  isPasswordVisible: boolean = false;
  errorMessage: string = '';  
  backendError: string = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private todoListService: TodoListService
  ) {}

  onLogin(): void {
    const user = {
      email: this.email,
      password: this.password
    };

    this.authService.login(user).subscribe(
      res => {
        localStorage.setItem('token', res.data.token);
        localStorage.setItem('userId', res.data.user.userId.toString());
        localStorage.setItem('userName', res.data.user.userName);
        this.todoListService.clearCache();
        this.todoListService.updateUserId(res.data.user.userId);
        this.router.navigate(['/todo-list']);
        this.errorMessage = '';  
      },
      (error) => {
        if (error.error && error.error.message) {
          this.showBackendError(error.error.message);  
        } else {
          this.showBackendError('Login failed. Please try again.');
        }
        console.error('Login failed:', error);
      }
    );
  }

  togglePasswordVisibility() {
    this.isPasswordVisible = !this.isPasswordVisible;
  }

  showBackendError(message: string): void {
    this.backendError = message; 
  }
}
