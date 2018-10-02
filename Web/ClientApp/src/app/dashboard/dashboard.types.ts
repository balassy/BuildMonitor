export interface Gauage {
  title: string;
  status: string;
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
  gauges: Gauage[];
}

export interface Dashboard {
  title: string;
  groups: GaugeGroup[];
}