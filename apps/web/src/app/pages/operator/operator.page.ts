import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

interface StoredUser {
  email: string;
  displayName: string;
  roles: string[];
}

@Component({
  selector: 'app-operator-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  styles: [`
    :host {
      display: block;
      min-height: 100vh;
      background:
        radial-gradient(circle at 15% 10%, rgba(34, 197, 94, 0.18), transparent 28rem),
        radial-gradient(circle at 85% 20%, rgba(20, 184, 166, 0.16), transparent 26rem),
        #07110f;
    }

    main {
      width: min(100%, 68rem);
      margin: 0 auto;
      padding: 2rem;
    }

    header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      gap: 1rem;
      margin-bottom: 2rem;
    }

    .brand {
      display: flex;
      align-items: center;
      gap: 0.75rem;
    }

    .brand img {
      width: 2.75rem;
      height: 2.75rem;
      object-fit: contain;
    }

    h1,
    h2,
    p {
      margin: 0;
    }

    .muted {
      color: #94a3b8;
    }

    .panel {
      border: 1px solid rgba(148, 163, 184, 0.22);
      border-radius: 8px;
      background: rgba(15, 23, 42, 0.86);
      padding: 1.25rem;
    }

    .grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(14rem, 1fr));
      gap: 1rem;
    }

    .badge {
      display: inline-flex;
      align-items: center;
      border-radius: 999px;
      background: rgba(34, 197, 94, 0.13);
      color: #bbf7d0;
      font-size: 0.8rem;
      font-weight: 700;
      padding: 0.35rem 0.7rem;
      margin-top: 0.75rem;
    }

    a,
    button {
      border: 1px solid rgba(148, 163, 184, 0.24);
      border-radius: 8px;
      background: rgba(15, 23, 42, 0.72);
      color: #e2e8f0;
      cursor: pointer;
      font-weight: 700;
      padding: 0.7rem 0.85rem;
      text-decoration: none;
    }
  `],
  template: `
    <main>
      <header>
        <div class="brand">
          <img src="/icon-MADai.png" alt="MADai">
          <div>
            <h1>MADai</h1>
            <p class="muted">Operator console</p>
          </div>
        </div>
        <button type="button" (click)="logout()">Sign out</button>
      </header>

      <section class="panel">
        <h2>Signed in</h2>
        <p class="muted">{{ user()?.displayName || user()?.email || 'Authenticated user' }}</p>
        @if (user()?.roles?.length) {
          <span class="badge">{{ user()!.roles.join(', ') }}</span>
        }
      </section>

      <div class="grid" style="margin-top: 1rem;">
        <section class="panel">
          <h2>Claude Tasks</h2>
          <p class="muted">SystemAdmin queue access is ready for operator workflows.</p>
        </section>
        <section class="panel">
          <h2>API Session</h2>
          <p class="muted">{{ hasToken() ? 'Access token stored for API calls.' : 'No active token.' }}</p>
          @if (!hasToken()) {
            <a routerLink="/login">Back to login</a>
          }
        </section>
      </div>
    </main>
  `
})
export class OperatorPage {
  private readonly rawUser = signal(localStorage.getItem('madai.user'));
  readonly hasToken = signal(Boolean(localStorage.getItem('madai.accessToken')));
  readonly user = computed<StoredUser | null>(() => {
    const raw = this.rawUser();
    if (!raw) return null;
    try {
      return JSON.parse(raw) as StoredUser;
    } catch {
      return null;
    }
  });

  logout(): void {
    localStorage.removeItem('madai.accessToken');
    localStorage.removeItem('madai.refreshToken');
    localStorage.removeItem('madai.user');
    window.location.assign('/login');
  }
}
