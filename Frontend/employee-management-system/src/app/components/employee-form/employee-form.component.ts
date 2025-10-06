import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService } from 'src/app/services/employee.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.scss']
})
export class EmployeeFormComponent implements OnInit {
  employeeForm: FormGroup;
  isEdit = false;
  employeeId: number | null = null;
  userRole: string = '';

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.employeeForm = this.createForm();
    this.userRole = this.authService.getUserRole();
  }

  ngOnInit() {
    this.employeeId = this.route.snapshot.params['id'];
    this.isEdit = !!this.employeeId;

    if (this.isEdit && this.employeeId) {
      this.loadEmployee(this.employeeId);
    }
  }

  createForm(): FormGroup {
    return this.fb.group({
      employeeCode: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      position: ['', Validators.required],
      department: ['', Validators.required],
      salary: [0, [Validators.required, Validators.min(0)]],
      hireDate: ['']
    });
  }

  loadEmployee(id: number) {
    this.employeeService.getEmployee(id).subscribe({
      next: (employee) => {
        this.employeeForm.patchValue(employee);
      },
      error: (error) => {
        console.error('Error loading employee:', error);
        alert('Error loading employee data');
      }
    });
  }

  onSubmit() {
    if (this.employeeForm.valid) {
      console.log('Form submitted:', this.employeeForm.value);
      
      if (this.isEdit && this.employeeId) {
        this.employeeService.updateEmployee(this.employeeId, this.employeeForm.value).subscribe({
          next: () => {
            console.log('Employee updated successfully');
            this.navigateBack();
          },
          error: (error) => {
            console.error('Error updating employee:', error);
            alert('Error updating employee');
          }
        });
      } else {
        this.employeeService.createEmployee(this.employeeForm.value).subscribe({
          next: () => {
            console.log('Employee created successfully');
            this.navigateBack();
          },
          error: (error) => {
            console.error('Error creating employee:', error);
            alert('Error creating employee');
          }
        });
      }
    } else {
      console.log('Form is invalid');
      // Mark all fields as touched to show validation errors
      Object.keys(this.employeeForm.controls).forEach(key => {
        this.employeeForm.get(key)?.markAsTouched();
      });
    }
  }

  onCancel() {
    this.navigateBack();
  }

  navigateBack() {
    if (this.userRole === 'Admin') {
      this.router.navigate(['/admin/employees']);
    } else if (this.userRole === 'Manager') {
      this.router.navigate(['/manager/employees']);
    } else {
      this.router.navigate(['/user/employees']);
    }
  }
}