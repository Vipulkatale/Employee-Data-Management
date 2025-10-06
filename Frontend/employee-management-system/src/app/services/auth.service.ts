import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7168/api/Auth';
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('currentUser') || 'null'));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  // Register new user
  register(userData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, userData);
  }

  login(username: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { username, password });
  }

  // ... rest of your existing methods remain the same
  setUser(user: any, token: string) {
    console.log('Setting user in auth service:', user);
    console.log('Setting token:', token);
    
    localStorage.setItem('currentUser', JSON.stringify(user));
    localStorage.setItem('token', token);
    this.currentUserSubject.next(user);
    
    // Verify storage
    console.log('Stored user:', localStorage.getItem('currentUser'));
    console.log('Stored token:', localStorage.getItem('token'));
  }

  getToken(): string {
    const token = localStorage.getItem('token') || '';
    console.log('Retrieved token:', token);
    return token;
  }

  getUserRole(): string {
    const user = this.currentUserValue;
    const role = user ? user.role : '';
    console.log('User role:', role);
    return role;
  }

  public get currentUserValue() {
    return this.currentUserSubject.value;
  }

  isLoggedIn(): boolean {
    const isLoggedIn = !!this.currentUserValue;
    console.log('Is logged in:', isLoggedIn);
    return isLoggedIn;
  }

  logout() {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }
}