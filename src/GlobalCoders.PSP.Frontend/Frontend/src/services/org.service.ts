
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Org } from '../models/org';

@Injectable({
  providedIn: 'root'
})
export class OrgService {

  token: string = localStorage.getItem('accessToken') || '';
  headers: HttpHeaders = new HttpHeaders(
    {
      Authorization: `Bearer ${this.token}`, // Incluye el token en el encabezado de autorización
    }
  );

  constructor(private http: HttpClient) {}

  getOrganizations(){

    const body = {
        "page": 1,
        "itemsPerPage": 100,
    };
    return this.http.post('http://localhost:9001/Organization/All', body, { headers: this.headers });
  }

  createOrganization(organization: any){
    return this.http.post('http://localhost:9001/Organization/Create', organization, { headers: this.headers });
  }

  updateOrganization(organization: any){
    return this.http.put('http://localhost:9001/Organization/Update', organization,  { headers: this.headers });
  }

  deleteOrganization(id: string){
    return this.http.delete('http://localhost:9001/Organization/Delete/'+id, { headers: this.headers });
  }

  getOrganization(id: string): Org{
    let org: Org = {
      id: '',
      displayName: '',
      legalName: '',
      address: '',
      email: '',
      mainPhoneNumber: '',
      secondaryPhoneNumber: '',
      workingSchedule: []
    };

    this.http.get('http://localhost:9001/Organization/Id/'+id, { headers: this.headers }).subscribe(
      (data: any) => {
        org.address = data.address;
        org.displayName = data.displayName;
        org.email = data.email;
        org.id = data.id;
        org.legalName = data.legalName;
        org.mainPhoneNumber = data.mainPhoneNumber;
        org.secondaryPhoneNumber = data.secondaryPhoneNumber;
        org.workingSchedule = data.workingSchedule;
      }
    );

    return org;
  }

}
