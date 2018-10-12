import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject, timer } from 'rxjs';
import { tap, switchMap } from 'rxjs/operators';
import { Dashboard, DashboardLink } from './dashboard.types';
import { ConfigService } from '../config/config.service';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private _onUpdatedSource = new Subject<string>();

  public onUpdated: Observable<string> = this._onUpdatedSource.asObservable();

  constructor(
    private _http: HttpClient,
    private _config: ConfigService) {
  }

  public getDashboards(): Observable<DashboardLink[]> {
    const url = '/api/dashboards';
    return this._http.get<DashboardLink[]>(url);
  }

  public pollDashboard(slug: string): Observable<Dashboard> {
    if (!slug) {
      throw new Error('Please specify the slug of the dashboard to poll!');
    }

    // Start immediately, then poll in the specified intervals.
    return timer(0, this._config.pollingIntervalSeconds * 1000)
      .pipe(
        switchMap(_ =>
          this.getDashboard(slug)
        )
      );
  }

  private getDashboard(slug: string): Observable<Dashboard> {
    if (!slug) {
      throw new Error('Please specify the slug of the dashboard to download!');
    }

    const url = `/api/dashboards/${slug}`;
    return this._http.get<Dashboard>(url)
      .pipe(
        tap((d: Dashboard) =>
          this.notifyUpdate(d.timestampUtc)
        )
      );
  }

  private notifyUpdate(timestampUtc: string) {
    this._onUpdatedSource.next(timestampUtc);
  }
}
