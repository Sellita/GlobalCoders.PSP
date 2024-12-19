import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Tax } from '../models/tax';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TaxService {

  private servicesSubject = new BehaviorSubject<Tax[]>([]);
  services$ = this.servicesSubject.asObservable();

  token: string = localStorage.getItem('accessToken') || '';
  headers: HttpHeaders = new HttpHeaders(
    {
      Authorization: `Bearer ${this.token}`,
    }
  );

  constructor(private http: HttpClient) { }

  getTaxes(): Observable<any> {
    const body = {
      page: 1,
      itemsPerPage: 100,
    };
    return this.http
      .post('http://localhost:9001/Tax/All', body, { headers: this.headers })
      .pipe(
        tap((response: any) => {
          const services = response.items || [];
          this.servicesSubject.next(services);
        })
      );
  }


  // Crear un servicio y sincronizar con el BehaviorSubject
  createTax(tax: any): Observable<any> {
    return this.http
      .post('http://localhost:9001/Tax/Create', tax, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshServices(); // Actualiza el listado
        })
      );
  }

  // Actualizar un servicio y sincronizar con el BehaviorSubject
  updateTax(tax: any): Observable<any> {
    return this.http
      .put('http://localhost:9001/Tax/Update', tax, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshServices(); // Actualiza el listado
        })
      );
  }

  // Eliminar un servicio y sincronizar con el BehaviorSubject
  deleteTax(id: string): Observable<any> {
    return this.http
      .delete(`http://localhost:9001/Tax/Delete/${id}`, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshServices(); // Actualiza el listado
        })
      );
  }

  // Obtener un servicio específico
  getTax(id: string): Observable<Tax> {
    return this.http.get<Tax>(`http://localhost:9001/Tax/Id/${id}`, {
      headers: this.headers,
    });
  }

  private refreshServices() {
    this.getTaxes().subscribe();
  }


}
