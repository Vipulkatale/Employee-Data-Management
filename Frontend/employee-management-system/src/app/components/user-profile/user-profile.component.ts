import { Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/services/employee.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  userProfile: any = {};
  isLoading: boolean = true;
  currentUser: any = {};

  constructor(
    private employeeService: EmployeeService,
    private authService: AuthService,
    private router: Router
  ) {
    this.currentUser = this.authService.currentUserValue;
  }

  ngOnInit() {
    this.loadUserProfile();
  }

  loadUserProfile() {
    // For demo purposes, we'll get all employees and find the current user's profile
    // In a real app, you'd have an API endpoint like /api/Employees/my-profile
    this.employeeService.getEmployees().subscribe({
      next: (employees) => {
        // Find the employee record that matches the current user
        // This assumes the username matches the employee email or there's some relation
        this.userProfile = employees.find((emp: any) => 
          emp.email === this.currentUser.email || 
          emp.username === this.currentUser.username
        );
        
        // If no profile found, create a demo profile
        if (!this.userProfile) {
          this.userProfile = this.createDemoProfile();
        }
        
        this.isLoading = false;
        console.log('User profile loaded:', this.userProfile);
      },
      error: (error) => {
        console.error('Error loading user profile:', error);
        this.userProfile = this.createDemoProfile();
        this.isLoading = false;
      }
    });
  }

  // Create a demo profile if no employee record is found
  private createDemoProfile() {
    return {
      firstName: this.currentUser.username,
      lastName: 'User',
      email: this.currentUser.email || `${this.currentUser.username}@company.com`,
      employeeCode: `EMP${Math.random().toString(36).substr(2, 9).toUpperCase()}`,
      position: 'Employee',
      department: 'General',
      phone: '+1 (555) 000-0000',
      hireDate: new Date().toISOString().split('T')[0],
      salary: 50000,
      address: '123 Main Street, City, State 12345'
    };
  }

  goBack() {
    this.router.navigate(['/user/dashboard']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}