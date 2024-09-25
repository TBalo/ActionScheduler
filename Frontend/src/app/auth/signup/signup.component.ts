import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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

  private toasts = [
    { title: 'Invalid User Name', content: 'User Name is required.', isOpen: false },
    { title: 'Invalid Email', content: 'Please enter a valid email address.', isOpen: false },
    { title: 'Invalid Password', content: 'Password must be at least 8 characters long, contain one uppercase letter, one lowercase letter, one number, and one special character.', isOpen: false },
    { title: 'Signup Error', content: 'Signup failed. Please try again.', isOpen: false }
  ];

  private toastFlag = 0; 
  private timeOutDelay = 600; 

  constructor(private router: Router, private authService: AuthService, private toastr: ToastrService) {}

  onSignup(signupForm: any): void {
    this.toastFlag = 0;

    if (!this.userName) {
      this.showToast(0); 
      return;
    }

    if (!this.email) {
      this.showToast(1); 
      return;
    }

    if (!this.password) {
      this.showToast(2); 
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
        console.error('Signup failed:', error);
        this.showToast(3); 
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
