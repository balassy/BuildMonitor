import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  public get pollingIntervalSeconds(): number {
    if (!environment.pollingIntervalSeconds) {
      throw new Error('The pollingIntervalSeconds configuration setting is missing!');
    }

    return environment.pollingIntervalSeconds;
  }
}
