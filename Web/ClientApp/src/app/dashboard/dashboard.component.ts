import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../shared/services/dashboard/dashboard.service';
import { Dashboard } from '../shared/services/dashboard/dashboard.types';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  public dashboard: Observable<Dashboard>;

  constructor(
    private _route: ActivatedRoute,
    private _dashboardService: DashboardService) {
  }

  public ngOnInit() {
    const slug: string = this._route.snapshot.paramMap.get('slug');
    this.dashboard = this._dashboardService.getDashboard(slug);
  }
}
