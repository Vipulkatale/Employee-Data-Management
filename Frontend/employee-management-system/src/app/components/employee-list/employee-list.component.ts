import { Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/services/employee.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent implements OnInit {
  employees: any[] = [];
  displayedColumns: string[] = [];
  userRole: string = '';
  canEdit: boolean = false;
  canDelete: boolean = false;
  canAdd: boolean = false;
  canViewSalary: boolean = false;

  constructor(
    private employeeService: EmployeeService,
    private authService: AuthService,
    private router: Router
  ) {
    this.userRole = this.authService.getUserRole();
    this.canEdit = this.employeeService.canEdit();
    this.canDelete = this.employeeService.canDelete();
    this.canAdd = this.employeeService.canAdd();
    this.canViewSalary = this.employeeService.canViewSensitiveInfo();
  }

  ngOnInit() {
    this.setDisplayedColumns();
    this.loadEmployees();
  }

  setDisplayedColumns() {
    this.displayedColumns = [
      'employeeCode', 
      'name', 
      'contact',  // This column gets maximum space
      'department', 
      'employment', 
      'salary', 
      'actions'
    ];
  }

  loadEmployees() {
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = this.enrichEmployeeData(data);
        console.log('Employees loaded:', this.employees);
      },
      error: (error) => {
        console.error('Error loading employees:', error);
      }
    });
  }

  private enrichEmployeeData(employees: any[]): any[] {
    return employees.map(employee => {
      return {
        ...employee,
        // Ensure all contact fields exist
        mobile: employee.mobile || employee.phone,
        extension: employee.extension || '',
        location: employee.location || 'Main Office',
        employmentType: employee.employmentType || 'Full-time',
        status: employee.status || 'Active',
        salaryType: employee.salaryType || 'Annual'
      };
    });
  }

  addEmployee() {
    if (this.userRole === 'Admin') {
      this.router.navigate(['/admin/employees/add']);
    } else if (this.userRole === 'Manager') {
      this.router.navigate(['/manager/employees/add']);
    }
  }

  editEmployee(employee: any) {
    if (this.userRole === 'Admin') {
      this.router.navigate(['/admin/employees/edit', employee.id]);
    } else if (this.userRole === 'Manager') {
      this.router.navigate(['/manager/employees/edit', employee.id]);
    }
  }

  viewDetails(employee: any) {
    console.log('View employee details:', employee);
    alert(`Viewing details for: ${employee.firstName} ${employee.lastName}\nEmail: ${employee.email}\nPosition: ${employee.position}`);
  }

  deleteEmployee(id: number) {
    if (this.canDelete && confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.deleteEmployee(id).subscribe({
        next: () => {
          this.loadEmployees();
        },
        error: (error) => {
          console.error('Error deleting employee:', error);
        }
      });
    }
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
  viewEmployeeDetails(employee: any) {
  if (this.userRole === 'Admin') {
    this.router.navigate(['/admin/employees/view', employee.id]);
  } else if (this.userRole === 'Manager') {
    this.router.navigate(['/manager/employees/view', employee.id]);
  } else if (this.userRole === 'User') {
    this.router.navigate(['/user/employees/view', employee.id]);
  }
}
}