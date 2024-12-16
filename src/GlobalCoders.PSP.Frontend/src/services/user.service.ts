import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {


  constructor(private api: ApiService) {}

  getUsers(): Observable<any> {
    return this.api.post<any>(`Employee/All`, {});
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
