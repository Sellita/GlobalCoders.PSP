import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrganizationService {

  constructor(private api: ApiService) {}

  getOrganizations(): Observable<any> {
    return this.api.get<any>(`Organization`);
  }

  getOrganization(id: number): Observable<any> {
    return this.api.get<any>(`Organization/${id}`);
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
