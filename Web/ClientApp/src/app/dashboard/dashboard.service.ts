import { Injectable } from '@angular/core';
import { Dashboard, GaugeGroup, Gauage } from './dashboard.types';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  constructor() {
  }

  public GetDashBoard(slug: string): Dashboard {
    return {
      'title': 'Open-source projects',
      'groups': [
        {
          'title': 'Eclipse',
          'gauges': [
            {
              'title': 'Check Style',
              'status': 'Success',
              'buildId': '1626845',
              'buildNumber': '673',
              'finishDateHumanized': '12 days ago',
              'branchName': 'master',
              'triggeredBy': 'Git',
              'lastChangeBy': 'collections-bot',
              'passedTestCount': 0,
              'failedTestCount': 0,
              'ignoredTestCount': 0
            },
            {
              'title': 'Compile, Test',
              'status': 'Success',
              'buildId': '1626851',
              'buildNumber': '429',
              'finishDateHumanized': '12 days ago',
              'branchName': 'master',
              'triggeredBy': 'Git',
              'lastChangeBy': 'collections-bot',
              'passedTestCount': 159058,
              'failedTestCount': 0,
              'ignoredTestCount': 1
            },
            {
              'title': 'Nightly Coverage',
              'status': 'Success',
              'buildId': '1647318',
              'buildNumber': '1505',
              'finishDateHumanized': '19 hours ago',
              'branchName': 'master',
              'triggeredBy': null,
              'lastChangeBy': null,
              'passedTestCount': 159058,
              'failedTestCount': 0,
              'ignoredTestCount': 1
            }
          ]
        },
        {
          'title': 'Storybook',
          'gauges': [
            {
              'title': 'Angular',
              'status': 'Success',
              'buildId': '1646662',
              'buildNumber': '1864',
              'finishDateHumanized': '2 days ago',
              'branchName': 'master',
              'triggeredBy': null,
              'lastChangeBy': 'filipp.riabchun',
              'passedTestCount': 0,
              'failedTestCount': 0,
              'ignoredTestCount': 0
            },
            {
              'title': 'Build',
              'status': 'Success',
              'buildId': '1646801',
              'buildNumber': '2178',
              'finishDateHumanized': '2 days ago',
              'branchName': 'master',
              'triggeredBy': null,
              'lastChangeBy': null,
              'passedTestCount': 818,
              'failedTestCount': 0,
              'ignoredTestCount': 0
            },
            {
              'title': 'Examples',
              'status': 'Success',
              'buildId': '1646794',
              'buildNumber': '2084',
              'finishDateHumanized': '2 days ago',
              'branchName': 'master',
              'triggeredBy': null,
              'lastChangeBy': null,
              'passedTestCount': 2,
              'failedTestCount': 0,
              'ignoredTestCount': 0
            }
          ]
        },
        {
          'title': 'EasyHttp',
          'gauges': [
            {
              'title': 'PR Build',
              'status': 'Success',
              'buildId': '996794',
              'buildNumber': '1.8.0-alpha.9',
              'finishDateHumanized': 'one year ago',
              'branchName': 'develop',
              'triggeredBy': 'Git',
              'lastChangeBy': 'david',
              'passedTestCount': 74,
              'failedTestCount': 0,
              'ignoredTestCount': 1
            },
            {
              'title': 'Release Build',
              'status': 'Success',
              'buildId': '965554',
              'buildNumber': '1.7.0',
              'finishDateHumanized': 'one year ago',
              'branchName': 'master',
              'triggeredBy': 'Git',
              'lastChangeBy': 'dalpert',
              'passedTestCount': 76,
              'failedTestCount': 0,
              'ignoredTestCount': 1
            }
          ]
        }
      ]
    };
  }
}
