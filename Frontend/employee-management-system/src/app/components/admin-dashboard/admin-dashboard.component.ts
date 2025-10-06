import { Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/services/employee.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  stats: any = {
    totalEmployees: 0,
    totalDepartments: 0,
    averageSalary: 0,
    totalSalary: 0,
    recentHires: 0
  };
  isLoading: boolean = true;

  constructor(
    private employeeService: EmployeeService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadStatistics();
  }

  loadStatistics() {
    this.isLoading = true;
    this.employeeService.getEmployees().subscribe({
      next: (employees) => {
        this.calculateStatistics(employees);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading statistics:', error);
        this.isLoading = false;
      }
    });
  }

  private calculateStatistics(employees: any[]) {
    if (!employees || employees.length === 0) {
      this.stats = {
        totalEmployees: 0,
        totalDepartments: 0,
        averageSalary: 0,
        totalSalary: 0,
        recentHires: 0
      };
      return;
    }

    // Calculate statistics from employee data
    const departments = new Set(employees.map(emp => emp.department));
    const totalSalary = employees.reduce((sum, emp) => sum + (emp.salary || 0), 0);
    const averageSalary = employees.length > 0 ? totalSalary / employees.length : 0;
    
    // Calculate recent hires (last 30 days)
    const thirtyDaysAgo = new Date();
    thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);
    const recentHires = employees.filter(emp => {
      if (!emp.hireDate) return false;
      const hireDate = new Date(emp.hireDate);
      return hireDate >= thirtyDaysAgo;
    }).length;

    this.stats = {
      totalEmployees: employees.length,
      totalDepartments: departments.size,
      averageSalary: Math.round(averageSalary),
      totalSalary: totalSalary,
      recentHires: recentHires,
      // Additional stats
      activeEmployees: employees.filter(emp => emp.status !== 'Inactive').length,
      highestSalary: Math.max(...employees.map(emp => emp.salary || 0)),
      lowestSalary: Math.min(...employees.map(emp => emp.salary || 0))
    };
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}