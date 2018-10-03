import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../shared/services/dashboard/dashboard.service';
import { DashboardLink } from '../shared/services/dashboard/dashboard.types';
import { Observable } from 'rxjs';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public dashboards: Observable<DashboardLink[]>;

  constructor(private _service: DashboardService) {
  }

  ngOnInit() {
    this.dashboards = this._service.GetDashboards();
  }

}
