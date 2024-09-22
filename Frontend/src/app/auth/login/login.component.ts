import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/shared/AuthService';
import { TodoListService } from 'src/app/shared/todo-list.service'; // Ensure the path is correct

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  email: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router, private todoListService: TodoListService) { }

  onLogin(): void {
    if (this.email && this.password) {
      const user = {
        email: this.email,
        password: this.password
      };

      this.authService.login(user).subscribe(
        res => {
          localStorage.setItem('token', res.data.token);
          localStorage.setItem('userId', res.data.user.userId.toString());
          localStorage.setItem('userName', res.data.user.userName)
          this.todoListService.clearCache();

          this.todoListService.updateUserId(res.data.user.userId);

          this.router.navigate(['/todo-list']);
        },
        err => {
          console.log('Login failed:', err);
        }
      );
    } else {
      alert('Please enter both email and password.');
    }
  }
}
