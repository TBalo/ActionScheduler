import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
})
export class ForgotPasswordComponent {
  email: string = '';

  constructor(private router: Router) {}

  onSubmitForgotPassword(): void {
    if (this.email) {
      console.log('Password reset email sent to:', this.email);

    } else {
      alert('Please enter your email address.');
    }
  }
}
