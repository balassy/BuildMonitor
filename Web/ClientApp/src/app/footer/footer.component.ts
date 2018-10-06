import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../shared/services/dashboard/dashboard.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {
  public timestampHumanized: Observable<string>;

  constructor(private _dashboardService: DashboardService) {
  }

  ngOnInit() {
    this.timestampHumanized = this._dashboardService.onUpdated;
  }
}
