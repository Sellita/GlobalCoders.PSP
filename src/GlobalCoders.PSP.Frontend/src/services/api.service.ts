import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private apiURL = 'http://localhost:9001';

  constructor(private http: HttpClient) { }

  // GET
  get<T>(url: string, options: any = {}): Observable<any> {
    return this.http.get<T>(`${this.apiURL}/${url}`, options).pipe(catchError(this.handleError));
  }

  // POST
  post<T>(url: string, body: any, options: any = {}): Observable<any> {
    return this.http.post<T>(`${this.apiURL}/${url}`, body, options).pipe(catchError(this.handleError));
  }

  // PUT
  put<T>(url: string, body: any, options: any = {}): Observable<any> {
    return this.http.put<T>(`${this.apiURL}/${url}`, body, options).pipe(catchError(this.handleError));
  }

  // DELETE
  delete<T>(url: string, options: any = {}): Observable<any> {
    return this.http.delete<T>(`${this.apiURL}/${url}`, options).pipe(catchError(this.handleError));
  }

  // Manejo de errores
  private handleError(error: HttpErrorResponse) {
    console.error('Error en la API:', error);
    return throwError(() => new Error('Ocurrió un error al procesar la solicitud.'));
  }
  
}
