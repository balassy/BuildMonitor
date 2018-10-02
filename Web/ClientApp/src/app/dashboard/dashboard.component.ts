import { Component, OnInit } from '@angular/core';
import { DashboardService } from './dashboard.service';
import { Dashboard, GaugeGroup, Gauage} from './dashboard.types';

@Component({
  selector: 'dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  public dashboard: Dashboard;

  constructor(private _dashboardService: DashboardService) {
  }

  public async ngOnInit() {
    this.dashboard = await this._dashboardService.GetDashBoard('oss');
  }
}
