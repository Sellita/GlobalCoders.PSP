import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  token: string = localStorage.getItem('accessToken') || '';
  headers: HttpHeaders = new HttpHeaders(
    {
      Authorization: `Bearer ${this.token}`,
    }
  );

  constructor(private http: HttpClient) {}

  
  createEmployee(user: any){
    return this.http.post('http://localhost:9001/Employee/Create', user, { headers: this.headers });
  }

  updateEmployee(user: any){
    return this.http.put('http://localhost:9001/Employee/Update', user, { headers: this.headers });
  }
  
  deleteEmployee(id: string){
    return this.http.delete('http://localhost:9001/Employee/Delete/'+id, { headers: this.headers });
  }

  getEmployee(id: string){
    return this.http.get('http://localhost:9001/Employee/Id/'+id, { headers: this.headers });
  }

  getEmployees(){
    const body = {
      "page": 1,
      "itemsPerPage": 100,
    };
    return this.http.post('http://localhost:9001/Employee/All', body, { headers: this.headers });
  }
}
