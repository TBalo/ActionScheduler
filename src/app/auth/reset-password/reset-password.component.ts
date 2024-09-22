import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
})
export class ResetPasswordComponent {
  password: string = '';
  confirmPassword: string = '';

  constructor(private router: Router) {}

  onSubmitResetPassword(): void {
    if (this.password == this.confirmPassword) {
      console.log('Password reset successful!');
      this.router.navigate(['/login']);
    } else {
      alert('Password entered do not match!');
    }
  }
}
