import { Injectable } from '@angular/core';
import { Dashboard, DashboardLink } from './dashboard.types';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private _onUpdatedSource = new Subject<string>();

  public onUpdated: Observable<string> = this._onUpdatedSource.asObservable();

  constructor(private _http: HttpClient) {
  }

  public getDashboards(): Observable<DashboardLink[]> {
    const url = '/api/dashboards';
    return this._http.get<DashboardLink[]>(url);
  }

  public getDashboard(slug: string): Observable<Dashboard> {
    if (!slug) {
      throw new Error('Please specify the slug!');
    }

    const url = `/api/dashboards/${slug}`;
    return this._http.get<Dashboard>(url).pipe(tap((d: Dashboard) => this.notifyUpdate(d.timestampUtc)));
  }

  private notifyUpdate(timestampUtc: string) {
    this._onUpdatedSource.next(timestampUtc);
  }
}
