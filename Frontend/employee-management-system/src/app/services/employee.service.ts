import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'https://localhost:7168/api/Employees';

  constructor(private http: HttpClient, private authService: AuthService) { }

  getEmployees(): Observable<any> {
    // For ALL roles (Admin, Manager, User) - return all employees
    return this.http.get(this.apiUrl);
  }

  getEmployee(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  createEmployee(employee: any): Observable<any> {
    return this.http.post(this.apiUrl, employee);
  }

  updateEmployee(id: number, employee: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, employee);
  }

  deleteEmployee(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  // Check if current user can perform actions
  canEdit(): boolean {
    const role = this.authService.getUserRole();
    return role === 'Admin' || role === 'Manager';
  }

  canDelete(): boolean {
    const role = this.authService.getUserRole();
    return role === 'Admin'; // Only Admin can delete
  }

  canAdd(): boolean {
    const role = this.authService.getUserRole();
    return role === 'Admin' || role === 'Manager'; // User cannot add
  }

  // Method to check if user can view sensitive information
  canViewSensitiveInfo(): boolean {
    const role = this.authService.getUserRole();
    return role === 'Admin' || role === 'Manager'; // User cannot see salary
  }
}