
npm install powerbi-client-angular

import { PowerBIModule } from 'powerbi-client-angular';

@NgModule({
  imports: [
    // other imports...
    PowerBIModule
  ],
  // declarations, providers, bootstrap...
})
export class AppModule { }


import { Component } from '@angular/core';

@Component({
  selector: 'app-report',
  template: `
    <powerbi-report
      [embedConfig]="embedConfig"
      (loaded)="reportLoaded($event)"
      (error)="errorOccurred($event)">
    </powerbi-report>
  `
})
export class ReportComponent {
  public embedConfig: pbi.IEmbedConfiguration = {
    type: 'report',
    tokenType: pbi.models.TokenType.Aad,
    accessToken: '', // Access token from Azure Service Principal
    embedUrl: '', // Embed URL of the PowerBI report
    permissions: pbi.models.Permissions.All,
    settings: {
      filterPaneEnabled: true,
      navContentPaneEnabled: true
    }
  };

  reportLoaded(event: pbi.Embed): void {
    console.log('Report loaded: ', event);
  }

  errorOccurred(event: pbi.Embed): void {
    console.error('Error occurred: ', event);
  }
}
