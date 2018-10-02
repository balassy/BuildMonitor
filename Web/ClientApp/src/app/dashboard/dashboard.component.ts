import { Component } from '@angular/core';
import { DashboardService } from './dashboard.service';
import { Dashboard, GaugeGroup, Gauage} from './dashboard.types';

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  public dashboard: Dashboard;

  constructor(private _dashboardService: DashboardService) {
    this.dashboard = this._dashboardService.GetDashBoard('TODO');
  }
}
