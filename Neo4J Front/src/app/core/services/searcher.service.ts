import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SearcherService {
  searched = new EventEmitter<number[]>();

  constructor(private http: HttpClient) {}

  search(keyworks: string): Observable<any> {
    return this.http.get<any>(`${environment.urlApi}/neo4j/searchlocation`, {
      params: { keyworks },
    });
  }
}
