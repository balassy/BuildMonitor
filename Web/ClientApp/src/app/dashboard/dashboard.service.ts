import { Injectable } from '@angular/core';
import { Dashboard } from './dashboard.types';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  constructor(private _http: HttpClient) {
  }

  public async GetDashBoard(slug: string): Promise<Dashboard> {
    if (!slug) {
      throw new Error('Please specify the slug!');
    }

    const url: string = `/api/dashboards/${slug}`;
    return this._http.get<Dashboard>(url).toPromise();
  }
}
