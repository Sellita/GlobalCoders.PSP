import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AuthService {


  constructor(private api: ApiService) {}

  login(email: string, password: string): Observable<any> {
    const credentials = { email, password };
    return this.api.post<any>(`Account/Login`, credentials);
  }

  isloggedIn(): boolean {
    if (typeof window !== 'undefined') {
      return !!localStorage.getItem('accessToken');
    }else {
      return false;
    }
    
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
  }

}
