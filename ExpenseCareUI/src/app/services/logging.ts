import { Injectable } from '@angular/core';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LoggingService {
  private appInsights: ApplicationInsights;

  constructor() {
    this.appInsights = new ApplicationInsights({
      config: {
        connectionString: environment.appInsights.connectionString,
        enableAutoRouteTracking: true // Automatically tracks page views when a user clicks links
      }
    });
    
    this.appInsights.loadAppInsights(); // Initializes connection to Azure
  }

  // Custom function to track button clicks or form inputs
  logEvent(name: string, properties?: { [key: string]: any }) {
    this.appInsights.trackEvent({ name: name }, properties);
  }

  // Custom function to log UI errors
  logException(error: Error) {
    this.appInsights.trackException({ error: error });
  }
}
