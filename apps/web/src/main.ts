import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';

const legacyOperatorPath = window.location.pathname.toLowerCase().replace(/\/+$/, '');
if (legacyOperatorPath === '/app/claude' || legacyOperatorPath === '/claude') {
  window.history.replaceState(null, '', '/dashboard');
}

bootstrapApplication(AppComponent, appConfig).catch(err => console.error(err));
