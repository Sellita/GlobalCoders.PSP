import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {


  constructor(private http: HttpClient, private router: Router) { }

  login(email: string, password: string) {
    const loginData = { email, password };
    this.http.post('http://localhost:9001/Account/Login', loginData).subscribe(
      (response: any) => {
        if (typeof localStorage !== 'undefined') {
          localStorage.setItem('accessToken', response.accessToken);
          localStorage.setItem('refreshToken', response.refreshToken);
        } else {
          console.error('localStorage is not available');
        }
        this.router.navigate(['/order']);
    },
      (error) => {
        console.log("Login failed: ", error);
      }
    );
  }

  isloggedIn() {
    if (typeof localStorage === 'undefined') {
      return false;
    }else{
      return localStorage.getItem('accessToken') !== null;
    }
  }


  logout() {
    if (typeof localStorage !== 'undefined'){
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
    }
    this.router.navigate(['/login']);
  }

}
