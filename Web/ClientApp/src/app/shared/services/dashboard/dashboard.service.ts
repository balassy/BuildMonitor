import { Injectable } from '@angular/core';
import { Dashboard, DashboardLink } from './dashboard.types';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  constructor(private _http: HttpClient) {
  }

  public GetDashboards(): Observable<DashboardLink[]> {
    const url = '/api/dashboards';
    return this._http.get<DashboardLink[]>(url);
  }

  public GetDashBoard(slug: string): Observable<Dashboard> {
    if (!slug) {
      throw new Error('Please specify the slug!');
    }

    const url = `/api/dashboards/${slug}`;
    return this._http.get<Dashboard>(url);
  }
}
