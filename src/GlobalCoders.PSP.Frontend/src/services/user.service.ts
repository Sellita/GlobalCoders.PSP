import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {


  constructor(private api: ApiService) {}

  private getAuthHeaders() {
    const token = localStorage.getItem('accessToken');
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }


  getUsers(): Observable<any> {
    const body = {
      page: 1,
      itemsPerPage: 100,
    };
    return this.api.post<any>(`Employee/All`, body, { headers: this.getAuthHeaders() });
  }

  getUser(id: number): Observable<any> {
    return this.api.get<any>(`Employee/${id}`);
  }

  createUser(user: any): Observable<any> {
    return this.api.post<any>(`Employee/Create`, user);
  }

  updateUser(user: any): Observable<any> {
    return this.api.put<any>(`Employee/Update`, user);
  }

  deleteUser(id: number): Observable<any> {
    return this.api.delete<any>(`Employee/delete/${id}`);
  }

}
