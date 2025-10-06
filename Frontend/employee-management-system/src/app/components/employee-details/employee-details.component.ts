import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService } from 'src/app/services/employee.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-employee-details',
  templateUrl: './employee-details.component.html',
  styleUrls: ['./employee-details.component.scss']
})
export class EmployeeDetailsComponent implements OnInit {
  employee: any = null;
  employeeId: number = 0;
  userRole: string = '';
  canEdit: boolean = false;
  canDelete: boolean = false;
  canViewSalary: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private employeeService: EmployeeService,
    private authService: AuthService
  ) {
    this.userRole = this.authService.getUserRole();
    this.canEdit = this.employeeService.canEdit();
    this.canDelete = this.employeeService.canDelete();
    this.canViewSalary = this.employeeService.canViewSensitiveInfo();
  }

  ngOnInit() {
    this.employeeId = +this.route.snapshot.params['id'];
    this.loadEmployeeDetails();
  }

  loadEmployeeDetails() {
    this.employeeService.getEmployee(this.employeeId).subscribe({
      next: (employee) => {
        this.employee = this.enrichEmployeeData(employee);
        console.log('Employee details loaded:', this.employee);
      },
      error: (error) => {
        console.error('Error loading employee details:', error);
        alert('Error loading employee details');
      }
    });
  }

  private enrichEmployeeData(employee: any): any {
    return {
      ...employee,
      employmentType: employee.employmentType || 'Full-time',
      status: employee.status || 'Active',
      salaryType: employee.salaryType || 'Annual',
      location: employee.location || 'Main Office'
    };
  }

  goBack() {
    if (this.userRole === 'Admin') {
      this.router.navigate(['/admin/employees']);
    } else if (this.userRole === 'Manager') {
      this.router.navigate(['/manager/employees']);
    } else {
      this.router.navigate(['/user/employees']);
    }
  }

  editEmployee() {
    if (this.userRole === 'Admin') {
      this.router.navigate(['/admin/employees/edit', this.employeeId]);
    } else if (this.userRole === 'Manager') {
      this.router.navigate(['/manager/employees/edit', this.employeeId]);
    }
  }

  deleteEmployee() {
    if (this.canDelete && confirm(`Are you sure you want to delete ${this.employee.firstName} ${this.employee.lastName}?`)) {
      this.employeeService.deleteEmployee(this.employeeId).subscribe({
        next: () => {
          alert('Employee deleted successfully');
          this.goBack();
        },
        error: (error) => {
          console.error('Error deleting employee:', error);
          alert('Error deleting employee');
        }
      });
    }
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}