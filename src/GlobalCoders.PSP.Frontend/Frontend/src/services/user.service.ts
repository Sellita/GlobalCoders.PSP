import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Service } from '../models/service';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private userSubject = new BehaviorSubject<Service[]>([]);
  users$ = this.userSubject.asObservable();

  token: string = localStorage.getItem('accessToken') || '';
  headers: HttpHeaders = new HttpHeaders(
    {
      Authorization: `Bearer ${this.token}`,
    }
  );

  constructor(private http: HttpClient) { }

  getUsers(): Observable<any> {
    const body = {
      page: 1,
      itemsPerPage: 100,
    };
    return this.http
      .post('http://localhost:9001/Employee/All', body, { headers: this.headers })
      .pipe(
        tap((response: any) => {
          const users = response.items || [];
          this.userSubject.next(users);
        })
      );
  }


  // Crear un servicio y sincronizar con el BehaviorSubject
  createUser(user: any): Observable<any> {
    return this.http
      .post('http://localhost:9001/Employee/Create', user, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshUsers(); // Actualiza el listado
        })
      );
  }

  // Actualizar un servicio y sincronizar con el BehaviorSubject
  updateUser(user: any): Observable<any> {
    return this.http
      .put('http://localhost:9001/Employee/Update', user, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshUsers(); // Actualiza el listado
        })
      );
  }

  // Eliminar un servicio y sincronizar con el BehaviorSubject
  deleteUser(id: string): Observable<any> {
    return this.http
      .delete(`http://localhost:9001/Employee/Delete/${id}`, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshUsers(); // Actualiza el listado
        })
      );
  }

  // Obtener un servicio específico
  getUser(id: string): Observable<User> {
    return this.http.get<User>(`http://localhost:9001/Employee/Id/${id}`, {
      headers: this.headers,
    });
  }

  private refreshUsers() {
    this.getUsers().subscribe();
  }

}
