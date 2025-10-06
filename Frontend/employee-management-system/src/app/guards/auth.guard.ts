import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {

    if (this.authService.isLoggedIn()) {
      const userRole = this.authService.getUserRole();
      console.log('Current User Role:', userRole);
      
      // Check if route requires specific roles
      if (route.data['roles'] && route.data['roles'].length > 0) {
        const hasRequiredRole = route.data['roles'].includes(userRole);
        console.log('Has Required Role:', hasRequiredRole);
        
        if (!hasRequiredRole) {
          console.log('Access Denied - Insufficient permissions');
          // Redirect to appropriate dashboard based on role
          this.redirectToDashboard(userRole);
          return false;
        }
      }
      
      console.log('Access Granted');
      return true;
    }

    console.log('User not logged in - Redirecting to login');
    this.router.navigate(['/login']);
    return false;
  }

  private redirectToDashboard(role: string): void {
    switch(role) {
      case 'Admin':
        this.router.navigate(['/admin/dashboard']);
        break;
      case 'Manager':
        this.router.navigate(['/manager/dashboard']);
        break;
      case 'User':
        this.router.navigate(['/user/dashboard']);
        break;
      default:
        this.router.navigate(['/login']);
    }
  }
}