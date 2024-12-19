import { Injectable, Provider } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Product } from '../models/product';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ProductType } from '../models/product-type';

@Injectable({
  providedIn: 'root'
})
export class ProductTypeService {

  private productTypeSubject = new BehaviorSubject<Product[]>([]);
  productsType$ = this.productTypeSubject.asObservable();

  token: string = localStorage.getItem('accessToken') || '';
  headers: HttpHeaders = new HttpHeaders(
    {
      Authorization: `Bearer ${this.token}`,
    }
  );

  constructor(private http: HttpClient) { }

  getProductsTypes(): Observable<any> {
    const body = {
      page: 1,
      itemsPerPage: 100,
    };
    return this.http
      .post('http://localhost:9001/ProductType/All', body, { headers: this.headers })
      .pipe(
        tap((response: any) => {
          const productsTypes = response.items || [];
          this.productTypeSubject.next(productsTypes);
        })
      );
  }


  // Crear un servicio y sincronizar con el BehaviorSubject
  createProductType(productType: any): Observable<any> {
    return this.http
      .post('http://localhost:9001/ProductType/Create', productType, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshServices(); // Actualiza el listado
        })
      );
  }

  // Actualizar un servicio y sincronizar con el BehaviorSubject
  updateProductType(productType: any): Observable<any> {
    return this.http
      .put('http://localhost:9001/ProductType/Update', productType, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshServices(); // Actualiza el listado
        })
      );
  }

  // Eliminar un servicio y sincronizar con el BehaviorSubject
  deleteProductType(id: string): Observable<any> {
    return this.http
      .delete(`http://localhost:9001/ProductType/Delete/${id}`, {
        headers: this.headers,
      })
      .pipe(
        tap(() => {
          this.refreshServices(); // Actualiza el listado
        })
      );
  }

  // Obtener un servicio específico
  getProduct(id: string): Observable<ProductType> {
    return this.http.get<ProductType>(`http://localhost:9001/ProductType/Id/${id}`, {
      headers: this.headers,
    });
  }

  private refreshServices() {
    this.getProductsTypes().subscribe();
  }

}
