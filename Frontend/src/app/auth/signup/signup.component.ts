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
  isSubmitting: boolean = false;  
  errorMessage: string = ''; 
  backendError: string = '';

  constructor(private router: Router, private authService: AuthService) {}

  onSignup(signupForm: any): void {
    this.isSubmitting = true;
    this.errorMessage = '';  

    if (signupForm.invalid) {
      this.isSubmitting = false;  
      return;
    }

    const newUser = {
      userName: this.userName,
      email: this.email,
      password: this.password,
    };

    this.authService.signup(newUser).subscribe(
      (response) => {
        console.log('Signup successful:', response);
        this.router.navigate(['/login']);
      },
      (error) => {
        this.isSubmitting = false; 
        if (error.error && error.error.message) {
          this.showBackendError(error.error.message);  
        } else {
          this.showBackendError('Signup failed. Please try again.');
        }
        console.error('Signup failed:', error);
      }
    );
  }

  togglePasswordVisibility() {
    this.isPasswordVisible = !this.isPasswordVisible;
  }

  isPasswordValid(): boolean {
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&_\-])[A-Za-z\d@$!%*?&_\-]{8,}$/;
    return passwordRegex.test(this.password);
  }

  showBackendError(message: string): void {
    this.backendError = message; 
  }
}
