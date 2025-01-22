import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PosService {

  private apiUrl = 'http://localhost:5013/api/Pedido';  

  constructor(private http: HttpClient) { }

  create(data: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/create`, data);
  }
}
