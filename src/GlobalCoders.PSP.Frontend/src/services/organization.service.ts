import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OrganizationService {

  constructor(private api: ApiService) {}

  private getAuthHeaders() {
    let headers = new HttpHeaders();
    if (typeof window !== 'undefined' && localStorage.getItem('accessToken')) {
      const token = localStorage.getItem('accessToken');
      headers = headers.set('Authorization', `Bearer ${token}`);
    }
    return headers;
  }

  getOrganizations(): Observable<any> {
    const body = {
      page: 1,
      itemsPerPage: 100,
    };
    return this.api.post<any>(`Organization/All`, body,{ headers: this.getAuthHeaders()});
  }

  getOrganization(id: number): Observable<any> {
    return this.api.get<any>(`Organization/Id/${id}`, { headers: this.getAuthHeaders()});
  }

  createOrganization(organization: any): Observable<any> {
    return this.api.post<any>(`Organization`, organization);
  }

  updateOrganization(organization: any): Observable<any> {
    return this.api.put<any>(`Organization`, organization);
  }

  deleteOrganization(id: number): Observable<any> {
    return this.api.delete<any>(`Organization/${id}`);
  }
  
}
