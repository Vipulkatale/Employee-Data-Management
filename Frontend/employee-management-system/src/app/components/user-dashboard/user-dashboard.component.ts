import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.scss']
})
export class UserDashboardComponent {
  userRole: string = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.userRole = this.authService.getUserRole();
  }

  // Add logout method
  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}