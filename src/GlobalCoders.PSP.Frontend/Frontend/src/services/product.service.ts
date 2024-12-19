import { Injectable, Provider } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Product } from '../models/product';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private productSubject = new BehaviorSubject<Product[]>([]);
  products$ = this.productSubject.asObservable();

  token: string = localStorage.getItem('accessToken') || '';
  headers: HttpHeaders = new HttpHeaders(
    {
      Authorization: `Bearer ${this.token}`,
    }
  );

  constructor(private http: HttpClient) { }

  getProducts(): Observable<any> {
    const body = {
      page: 1,
      itemsPerPage: 100,
    };
    return this.http
      .post('http://localhost:9001/Product/All', body, { headers: this.headers })
      .pipe(
        tap((response: any) => {
          const products = response.items || [];
          this.productSubject.next(products);
        })
      );
  }


  // Crear un servicio y sincronizar con el BehaviorSubject
  createProduct(product: any): Observable<any> {
    return this.http
      .post('http://localhost:9001/Product/Create', product, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshServices(); // Actualiza el listado
        })
      );
  }

  // Actualizar un servicio y sincronizar con el BehaviorSubject
  updateProduct(product: any): Observable<any> {
    return this.http
      .put('http://localhost:9001/Product/Update', product, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshServices(); // Actualiza el listado
        })
      );
  }

  // Eliminar un servicio y sincronizar con el BehaviorSubject
  deleteProduct(id: string): Observable<any> {
    return this.http
      .delete(`http://localhost:9001/Product/Delete/${id}`, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshServices(); // Actualiza el listado
        })
      );
  }

  // Obtener un servicio específico
  getProduct(id: string): Observable<Product> {
    return this.http.get<Product>(`http://localhost:9001/Product/Id/${id}`, {
      headers: this.headers,
    });
  }

  private refreshServices() {
    this.getProducts().subscribe();
  }


}
