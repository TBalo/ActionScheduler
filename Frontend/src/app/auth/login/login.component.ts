import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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

  private toasts = [
    { title: 'Login Error', content: 'Invalid credentials.', isOpen: false }
  ];
  
  private toastFlag = 0; 
  private timeOutDelay = 10000; 
  constructor(
    private authService: AuthService,
    private router: Router,
    private todoListService: TodoListService,
    private toastr: ToastrService
  ) {}

  private emailPattern: RegExp = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  private passwordPattern: RegExp = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;

  onLogin(): void {
    this.toastFlag = 0;

    if (!this.emailPattern.test(this.email)) {
      this.showToast(0); 
      return;
    }

    if (!this.passwordPattern.test(this.password)) {
      this.showToast(1); 
      return;
    }

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
      },
      err => {
        console.log('Login failed:', err);
        this.showToast(2); 
      }
    );
  }

  togglePasswordVisibility() {
    this.isPasswordVisible = !this.isPasswordVisible;
  }

  private showToast(index: number) {
    if (!this.toasts[index].isOpen) {
      this.toasts[index].isOpen = true;
      this.toastr.error(this.toasts[index].content, this.toasts[index].title);
      setTimeout(() => {
        this.toasts[index].isOpen = false; 
      }, this.timeOutDelay);
    }
  }
}
