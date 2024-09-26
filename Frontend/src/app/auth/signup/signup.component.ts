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

  handleBlur(field: any): void {
    if (field.invalid) {
      this.showBackendError(this.getErrorMessage(field));
    } else {
      this.backendError = ''; // Clear error message if the field is valid
    }
  }
  
  getErrorMessage(field: any): string {
    if (field.errors?.['required']) {
      return `${field.name} is required.`;
    }
    if (field.errors?.['email']) {
      return 'Enter a valid email address.';
    }
    if (field.errors?.['pattern']) {
      return 'Password must be at least 8 characters long, contain one uppercase letter, one lowercase letter, one number, and one special character.';
    }
    return '';
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
