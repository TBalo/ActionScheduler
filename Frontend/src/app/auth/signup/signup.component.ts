import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/shared/AuthService';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
})
export class SignupComponent {
  userName: string = '';
  email: string = '';
  password: string = '';
  isPasswordVisible: boolean = false;

  constructor(private router: Router, private authService: AuthService) {}

  onSignup(): void {
    if (this.userName && this.email && this.password) {
      const newUser = {
        userName: this.userName,
        email: this.email,
        password: this.password
      };

      this.authService.signup(newUser).subscribe(
        (response) => {
          console.log('Signup successful:', response);
          this.router.navigate(['/login']);
        },
        (error) => {
          console.error('Signup failed:', error);
          alert('Signup failed. Please try again.');
        }
      );
    } else {
      alert('Please fill out all fields.');
    }
  }

  togglePasswordVisibility() {
    this.isPasswordVisible = !this.isPasswordVisible;
  }
}
