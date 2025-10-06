import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';
  isLoading: boolean = false;
  showDemoInfo: boolean = true; // Set to false if you don't want demo accounts shown

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';

      this.authService.login(
        this.loginForm.value.username,
        this.loginForm.value.password
      ).subscribe({
        next: (response: any) => {
          this.isLoading = false;
          console.log('Login response:', response);

          // Create user object from response
          const user = {
            username: this.loginForm.value.username,
            role: response.role,
            email: response.email || 'user@company.com'
          };

          // Store user and token
          this.authService.setUser(user, response.token);
          
          // Redirect based on role
          if (response.role === 'Admin') {
            this.router.navigate(['/admin/dashboard']);
          } else if (response.role === 'Manager') {
            this.router.navigate(['/manager/dashboard']);
          } else if (response.role === 'User') {
            this.router.navigate(['/user/dashboard']);
          } else {
            this.router.navigate(['/dashboard']);
          }
        },
        error: (error) => {
          this.isLoading = false;
          console.error('Login error:', error);
          this.errorMessage = error.error?.message || 'Invalid username or password';
        }
      });
    }
  }
}