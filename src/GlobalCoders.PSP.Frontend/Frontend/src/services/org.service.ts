
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OrgService {

  constructor(private http: HttpClient) {}

  getOrganizations(){
    const token = localStorage.getItem('accessToken');
    console.log(token);
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`, // Incluye el token en el encabezado de autorización
    });

    const body = {
        "page": 1,
        "itemsPerPage": 100,
    };

    return this.http.post('http://localhost:9001/Organization/All',body, { headers });
  }

  
}
