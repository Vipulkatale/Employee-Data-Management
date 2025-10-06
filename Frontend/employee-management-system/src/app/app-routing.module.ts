import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { EmployeeListComponent } from './components/employee-list/employee-list.component';
import { EmployeeFormComponent } from './components/employee-form/employee-form.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { UserDashboardComponent } from './components/user-dashboard/user-dashboard.component'; // Import this
import { AuthGuard } from './guards/auth.guard';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { EmployeeDetailsComponent } from './components/employee-details/employee-details.component';
import { RegisterComponent } from './components/register/register.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
   { path: 'register', component: RegisterComponent }, 
  // Admin routes
  {
    path: 'admin/dashboard',
    component: AdminDashboardComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Admin'] }
  },
  {
    path: 'admin/employees',
    component: EmployeeListComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Admin', 'Manager'] }
  },
  {
    path: 'admin/employees/add',
    component: EmployeeFormComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Admin', 'Manager'] }
  },
  {
    path: 'admin/employees/edit/:id',
    component: EmployeeFormComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Admin', 'Manager'] }
  },
  
  // Manager routes
  {
    path: 'manager/dashboard',
    component: AdminDashboardComponent, // You can create a separate ManagerDashboardComponent later
    canActivate: [AuthGuard],
    data: { roles: ['Manager'] }
  },
  {
    path: 'manager/employees',
    component: EmployeeListComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Manager'] }
  },
  {
    path: 'manager/employees/add',
    component: EmployeeFormComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Manager'] }
  },
  {
    path: 'manager/employees/edit/:id',
    component: EmployeeFormComponent,
    canActivate: [AuthGuard],
    data: { roles: ['Manager'] }
  },
  
  // User routes - Read only access
  {
    path: 'user/dashboard',  
    component: UserDashboardComponent,
    canActivate: [AuthGuard],
    data: { roles: ['User'] }
  },
  {
    path: 'user/employees', 
    component: EmployeeListComponent,
    canActivate: [AuthGuard],
    data: { roles: ['User'] }
  },
  {
    path: 'user/profile', 
    component: UserProfileComponent,
    canActivate: [AuthGuard],
    data: { roles: ['User'] }
  },
  {
  path: 'admin/employees/view/:id',
  component: EmployeeDetailsComponent,
  canActivate: [AuthGuard],
  data: { roles: ['Admin'] }
},
{
  path: 'manager/employees/view/:id',
  component: EmployeeDetailsComponent,
  canActivate: [AuthGuard],
  data: { roles: ['Manager'] }
},
{
  path: 'user/employees/view/:id',
  component: EmployeeDetailsComponent,
  canActivate: [AuthGuard],
  data: { roles: ['User'] }
},
  
  { path: '**', redirectTo: '/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }