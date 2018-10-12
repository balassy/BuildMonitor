import { TestBed } from '@angular/core/testing';

import { DashboardService } from './dashboard.service';
import { HttpClientModule } from '@angular/common/http';
import { ConfigService } from '../config/config.service';

describe('DashboardService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientModule
    ],
    providers: [
      ConfigService
    ]
  }));

  it('should be created', () => {
    const service: DashboardService = TestBed.get(DashboardService);
    expect(service).toBeTruthy();
  });
});
