import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../../core/http/api.service';

const MAD_UNIVERSE_APPS = [
  { name: 'MAD Prospects', url: 'https://madprospects.com/', logo: 'https://madprospects.com/media/logo-wide-madprospects.png' },
  { name: 'MADai', url: 'https://madai.madprospects.com/', logo: 'https://madprospects.com/media/logo-wide-MADai.png' },
  { name: 'MADAuthor', url: 'https://madauthor.madprospects.com/', logo: 'https://madprospects.com/media/logo-wide-MADAuthor.png' },
  { name: 'MAD Cloud', url: 'https://madcloud.madprospects.com/', logo: '' },
  { name: 'MADCreate', url: 'https://madcreate.madprospects.com/', logo: 'https://madprospects.com/media/logo-wide-MADCreate.png' },
  { name: 'MADHub', url: 'https://madhub.madprospects.com/', logo: 'https://madprospects.com/media/logo-wide-MADHub.png' },
  { name: 'MADLeads', url: 'https://madleads.madprospects.com/', logo: 'https://madprospects.com/media/logo-wide-MADLeads.png' },
  { name: 'MADLearn', url: 'https://madlearn.madprospects.com/', logo: 'https://madprospects.com/media/logo-wide-MADLearn.png' },
  { name: 'MADLove', url: 'https://madlove.madprospects.com/', logo: 'https://madprospects.com/media/logo-wide-MADLove.png' },
  { name: 'MADMultisciple', url: 'https://madmultisciple.madprospects.com/', logo: 'https://madprospects.com/media/logo-wide-MADMultisciple.png' },
  { name: 'MADPulse', url: 'https://madpulse.madprospects.com/', logo: 'https://madprospects.com/media/logo-wide-MADPulse.png' },
  { name: 'MADRecruiting', url: 'https://madrecruiting.madprospects.com/', logo: 'https://madprospects.com/media/logo-wide-MADRecruiting.png' },
] as const;

interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  user: {
    email: string;
    displayName: string;
    roles: string[];
  };
}

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  styles: [`
    :host {
      display: block;
      min-height: 100vh;
      background:
        radial-gradient(circle at 10% 0%, rgba(34, 197, 94, 0.18), transparent 28rem),
        radial-gradient(circle at 90% 15%, rgba(20, 184, 166, 0.16), transparent 24rem),
        #07110f;
    }

    .page {
      min-height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      flex-direction: column;
      gap: 1.25rem;
      padding: 2rem;
    }

    .panel {
      width: min(100%, 26rem);
      border: 1px solid rgba(148, 163, 184, 0.22);
      border-radius: 8px;
      background: rgba(15, 23, 42, 0.9);
      box-shadow: 0 24px 80px rgba(2, 6, 23, 0.38);
      padding: 1.5rem;
    }

    .login-copy {
      max-width: 42rem;
      text-align: center;
    }

    .login-copy p {
      margin: 0 0 0.6rem;
      color: #5eead4;
      font-size: 0.75rem;
      font-weight: 800;
      letter-spacing: 0.18em;
      text-transform: uppercase;
    }

    .login-copy h2 {
      margin: 0;
      color: #f8fafc;
      font-size: clamp(2rem, 5vw, 4rem);
      line-height: 1;
    }

    .brand {
      display: flex;
      align-items: center;
      gap: 0.75rem;
      margin-bottom: 1.5rem;
    }

    .brand img {
      width: 2.75rem;
      height: 2.75rem;
      object-fit: contain;
    }

    h1 {
      margin: 0;
      font-size: 1.35rem;
      line-height: 1.2;
    }

    p {
      margin: 0.25rem 0 0;
      color: #94a3b8;
      font-size: 0.9rem;
    }

    form {
      display: grid;
      gap: 1rem;
    }

    label {
      display: grid;
      gap: 0.45rem;
      color: #cbd5e1;
      font-size: 0.8rem;
      font-weight: 700;
      text-transform: uppercase;
      letter-spacing: 0;
    }

    input {
      width: 100%;
      border: 1px solid rgba(148, 163, 184, 0.3);
      border-radius: 8px;
      background: rgba(2, 6, 23, 0.72);
      color: #f8fafc;
      padding: 0.85rem 0.9rem;
      outline: none;
    }

    input:focus {
      border-color: #14b8a6;
      box-shadow: 0 0 0 3px rgba(20, 184, 166, 0.18);
    }

    button {
      border: 0;
      border-radius: 8px;
      background: linear-gradient(135deg, #22c55e, #14b8a6);
      color: #04111a;
      cursor: pointer;
      font-weight: 800;
      padding: 0.9rem 1rem;
    }

    button:disabled {
      cursor: not-allowed;
      opacity: 0.58;
    }

    .error {
      border-radius: 8px;
      background: rgba(244, 63, 94, 0.12);
      color: #fecdd3;
      padding: 0.75rem 0.85rem;
      font-size: 0.9rem;
    }

    .mad-universe-strip {
      position: relative;
      overflow: hidden;
      width: min(100%, 62rem);
      background: #0d1628;
      border: 1px solid rgba(148, 163, 184, 0.16);
      border-radius: 8px;
      padding: 14px 0;
    }
    .mad-universe-inner {
      display: flex;
      align-items: center;
      gap: 20px;
      margin: 0 auto;
      padding: 0 18px;
    }
    .mad-universe-kicker {
      flex: 0 0 auto;
      display: inline-flex;
      align-items: center;
      gap: 8px;
      margin: 0;
      color: #7dd3fc;
      font-size: 10px;
      font-weight: 700;
      letter-spacing: 0.18em;
      line-height: 1;
      text-transform: uppercase;
      white-space: nowrap;
    }
    .mad-universe-kicker span { color: #38bdf8; }
    .mad-universe-marquee {
      flex: 1 1 auto;
      min-width: 0;
      overflow: hidden;
      -webkit-mask-image: linear-gradient(90deg, transparent, #000 8%, #000 92%, transparent);
      mask-image: linear-gradient(90deg, transparent, #000 8%, #000 92%, transparent);
    }
    .mad-universe-track {
      display: flex;
      align-items: center;
      gap: 32px;
      width: max-content;
      animation: madUniverseScroll 44s linear infinite;
    }
    .mad-universe-link {
      display: inline-flex;
      align-items: center;
      justify-content: center;
      min-width: max-content;
      opacity: 0.78;
      text-decoration: none;
      transition: opacity 160ms ease, transform 160ms ease;
    }
    .mad-universe-link:hover { opacity: 1; transform: translateY(-1px); }
    .mad-universe-link img {
      display: block;
      width: auto;
      max-width: 156px;
      height: 22px;
      object-fit: contain;
      filter: drop-shadow(0 0 12px rgba(255,255,255,0.08));
    }
    .mad-universe-text {
      color: #cbd5e1;
      font-size: 11px;
      font-weight: 800;
      letter-spacing: 0.1em;
      line-height: 1;
      text-transform: uppercase;
      white-space: nowrap;
    }
    @keyframes madUniverseScroll {
      from { transform: translateX(0); }
      to { transform: translateX(-50%); }
    }
    @media (max-width: 760px) {
      .mad-universe-inner {
        align-items: stretch;
        flex-direction: column;
        gap: 10px;
      }
      .mad-universe-kicker { justify-content: center; }
      .mad-universe-track { gap: 26px; animation-duration: 52s; }
      .mad-universe-link img { height: 19px; max-width: 136px; }
    }
    @media (prefers-reduced-motion: reduce) {
      .mad-universe-track {
        animation: none;
        flex-wrap: wrap;
        justify-content: center;
        width: auto;
      }
      .mad-universe-marquee {
        -webkit-mask-image: none;
        mask-image: none;
      }
    }
  `],
  template: `
    <main class="page">
      <section class="login-copy" aria-label="MADai introduction">
        <p>AI command center</p>
        <h2>Run the work, not just the prompts.</h2>
      </section>
      <section class="panel" aria-labelledby="login-title">
        <div class="brand">
          <img src="/icon-MADai.png" alt="MADai">
          <div>
            <h1 id="login-title">MADai</h1>
            <p>Sign in and launch the command center</p>
          </div>
        </div>

        <form [formGroup]="form" (ngSubmit)="login()">
          <label>
            Email
            <input type="email" autocomplete="username" formControlName="email">
          </label>
          <label>
            Password
            <input type="password" autocomplete="current-password" formControlName="password">
          </label>

          @if (error()) {
            <div class="error" role="alert">{{ error() }}</div>
          }

          <button type="submit" [disabled]="form.invalid || loading()">
            {{ loading() ? 'Signing in...' : 'Launch MADai' }}
          </button>
        </form>
      </section>
      <section class="mad-universe-strip" aria-label="Explore the MAD universe">
        <div class="mad-universe-inner">
          <p class="mad-universe-kicker"><span aria-hidden="true">*</span> The MAD universe</p>
          <div class="mad-universe-marquee">
            <div class="mad-universe-track">
              @for (app of madUniverseApps.concat(madUniverseApps); track app.name + $index) {
                <a class="mad-universe-link" [href]="app.url" target="_blank" rel="noopener" [attr.aria-label]="app.name">
                  @if (app.logo) {
                    <img [src]="app.logo" [alt]="app.name" loading="lazy" decoding="async" />
                  } @else {
                    <span class="mad-universe-text">{{ app.name }}</span>
                  }
                </a>
              }
            </div>
          </div>
        </div>
      </section>
    </main>
  `
})
export class LoginPage {
  protected readonly madUniverseApps = MAD_UNIVERSE_APPS;

  private readonly fb = inject(FormBuilder);
  private readonly api = inject(ApiService);
  private readonly router = inject(Router);

  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly form = this.fb.nonNullable.group({
    email: ['admin@madprospects.com', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  login(): void {
    if (this.form.invalid || this.loading()) return;
    this.loading.set(true);
    this.error.set(null);
    this.api.post<AuthResponse>('/auth/login', {
      email: this.form.controls.email.value,
      password: this.form.controls.password.value,
      mfaCode: null
    }).subscribe({
      next: response => {
        localStorage.setItem('madai.accessToken', response.accessToken);
        localStorage.setItem('madai.refreshToken', response.refreshToken);
        localStorage.setItem('madai.user', JSON.stringify(response.user));
        this.router.navigateByUrl('/app/claude');
      },
      error: err => {
        this.error.set(err?.message ?? 'Sign in failed');
        this.loading.set(false);
      }
    });
  }
}
