import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Order } from '../models/order';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private orderSubject = new BehaviorSubject<Order[]>([]);
  orders$ = this.orderSubject.asObservable();

  token: string = localStorage.getItem('accessToken') || '';
  headers: HttpHeaders = new HttpHeaders(
    {
      Authorization: `Bearer ${this.token}`,
    }
  );

  constructor(private http: HttpClient) { }

  getOrders(): Observable<any> {
    const body = {
      page: 1,
      itemsPerPage: 100,
    };
    return this.http
      .post('http://localhost:9001/Order/All', body, { headers: this.headers })
      .pipe(
        tap((response: any) => {
          const services = response.items || [];
          this.orderSubject.next(services);
        })
      );
  }


  // Crear un servicio y sincronizar con el BehaviorSubject
  createOrder(order: any): Observable<any> {
    return this.http
      .post('http://localhost:9001/Order/Create', order, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshOrders(); // Actualiza el listado
        })
      );
  }

  // Actualizar un servicio y sincronizar con el BehaviorSubject
  updateOrder(order: any): Observable<any> {
    return this.http
      .put('http://localhost:9001/Order/Update', order, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshOrders(); // Actualiza el listado
        })
      );
  }

  // Eliminar un servicio y sincronizar con el BehaviorSubject
  deleteOrder(id: string): Observable<any> {
    return this.http
      .delete(`http://localhost:9001/Order/Delete/${id}`, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshOrders(); // Actualiza el listado
        })
      );
  }

  // Obtener un servicio específico
  getOrder(id: string): Observable<Order> {
    return this.http.get<Order>(`http://localhost:9001/Order/Id/${id}`, {
      headers: this.headers,
    });
  }

  private refreshOrders() {
    this.getOrders().subscribe();
  }


}
