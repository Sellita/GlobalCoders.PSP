import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Service } from '../models/service';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ServiceService {

  private servicesSubject = new BehaviorSubject<Service[]>([]);
  services$ = this.servicesSubject.asObservable();

  token: string = localStorage.getItem('accessToken') || '';
  headers: HttpHeaders = new HttpHeaders(
    {
      Authorization: `Bearer ${this.token}`,
    }
  );

  constructor(private http: HttpClient) { }

  getServices(): Observable<any> {
    const body = {
      page: 1,
      itemsPerPage: 100,
    };
    return this.http
      .post('http://localhost:9001/Service/All', body, { headers: this.headers })
      .pipe(
        tap((response: any) => {
          const services = response.items || [];
          this.servicesSubject.next(services);
        })
      );
  }


  // Crear un servicio y sincronizar con el BehaviorSubject
  createService(service: any): Observable<any> {
    return this.http
      .post('http://localhost:9001/Service/Create', service, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshServices(); // Actualiza el listado
        })
      );
  }

  // Actualizar un servicio y sincronizar con el BehaviorSubject
  updateService(service: any): Observable<any> {
    return this.http
      .put('http://localhost:9001/Service/Update', service, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshServices(); // Actualiza el listado
        })
      );
  }

  // Eliminar un servicio y sincronizar con el BehaviorSubject
  deleteService(id: string): Observable<any> {
    return this.http
      .delete(`http://localhost:9001/Service/Delete/${id}`, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshServices(); // Actualiza el listado
        })
      );
  }

  // Obtener un servicio específico
  getService(id: string): Observable<Service> {
    return this.http.get<Service>(`http://localhost:9001/Service/Id/${id}`, {
      headers: this.headers,
    });
  }

  private refreshServices() {
    this.getServices().subscribe();
  }


}
