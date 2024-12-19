import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Org } from '../models/org';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrgService {

  private orgSubject = new BehaviorSubject<Org[]>([]);
  organizations$ = this.orgSubject.asObservable();

  token: string = localStorage.getItem('accessToken') || '';
  headers: HttpHeaders = new HttpHeaders(
    {
      Authorization: `Bearer ${this.token}`, // Incluye el token en el encabezado de autorización
    }
  );

  constructor(private http: HttpClient) {}

  getOrganizations(): Observable<any> {
      const body = {
        page: 1,
        itemsPerPage: 100,
      };
      return this.http
        .post('http://localhost:9001/Organization/All', body, { headers: this.headers })
        .pipe(
          tap((response: any) => {
            const organizations = response.items || [];
            this.orgSubject.next(organizations);
          })
        );
    }
  
  
    // Crear un servicio y sincronizar con el BehaviorSubject
    createOrganization(org: any): Observable<any> {
      return this.http
        .post('http://localhost:9001/Organization/Create', org, {
          headers: this.headers,
        })
        .pipe(
          tap(() => {
            this.refreshOrgs(); // Actualiza el listado
          })
        );
    }
  
    // Actualizar un servicio y sincronizar con el BehaviorSubject
    updateOrganization(org: any): Observable<any> {
      return this.http
        .put('http://localhost:9001/Organization/Update', org, {
          headers: this.headers,
        })
        .pipe(
          tap(() => {
            this.refreshOrgs(); // Actualiza el listado
          })
        );
    }
  
    // Eliminar un servicio y sincronizar con el BehaviorSubject
    deleteOrganization(id: string): Observable<any> {
      return this.http
        .delete(`http://localhost:9001/Organization/Delete/${id}`, {
          headers: this.headers,
        })
        .pipe(
          tap(() => {
            this.refreshOrgs(); // Actualiza el listado
          })
        );
    }
  
    // Obtener un servicio específico
    getOrganization(id: string): Observable<Org> {
      return this.http.get<Org>(`http://localhost:9001/Organization/Id/${id}`, {
        headers: this.headers,
      });
    }
  
    private refreshOrgs() {
      this.getOrganizations().subscribe();
    }
  

}
