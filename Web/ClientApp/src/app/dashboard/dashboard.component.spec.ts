import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { fakeAsync, ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { DashboardComponent } from './dashboard.component';
import { DashboardService } from '../shared/services/dashboard/dashboard.service';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;

  beforeEach(fakeAsync(() => {
    const dashboardServiceMock = jasmine.createSpyObj('DashboardService', ['pollDashboard']);

    TestBed.configureTestingModule({
      declarations: [ DashboardComponent ],
      imports: [ HttpClientModule, RouterTestingModule ],
      providers: [
        { provide: DashboardService, useValue: dashboardServiceMock }
      ],
      schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
