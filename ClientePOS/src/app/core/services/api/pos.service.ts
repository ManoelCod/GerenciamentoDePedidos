import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PosService {

  private apiUrl = 'http://localhost:5013/api/Order';  

  constructor(private http: HttpClient) { }

  create(data: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/create`, data);
  }

  update(id: number, data: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/update/${id}`, data);
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/delete/${id}`);
  }

  filterOrders(date?: string, customerName?: string): Observable<any[]> {
    let params = new HttpParams();
    if (date) {
      params = params.set('date', date);
    }
    if (customerName) {
      params = params.set('customerName', customerName);
    }
    return this.http.get<any[]>(`${this.apiUrl}/filter`, { params });
  }

}
