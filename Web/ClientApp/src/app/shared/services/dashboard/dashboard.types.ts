export interface Gauage {
  title: string;
  status: 'Success' | 'Running' | 'Failed';
  buildId: string;
  buildNumber: string;
  finishDateHumanized: string;
  branchName: string;
  triggeredBy: string;
  lastChangeBy: string;
  passedTestCount: number;
  failedTestCount: number;
  ignoredTestCount: number;
}

export interface GaugeGroup {
  title: string;
  columnCount: number;
  gauges: Gauage[];
}

export interface Dashboard {
  title: string;
  groups: GaugeGroup[];
  timestampUtc: string;
}

export interface DashboardLink {
  title: string;
  slug: string;
}
