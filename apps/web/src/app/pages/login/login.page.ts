import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../../core/http/api.service';

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
      display: grid;
      place-items: center;
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
  `],
  template: `
    <main class="page">
      <section class="panel" aria-labelledby="login-title">
        <div class="brand">
          <img src="/icon-MADai.png" alt="MADai">
          <div>
            <h1 id="login-title">MADai</h1>
            <p>Operator sign in</p>
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
            {{ loading() ? 'Signing in...' : 'Sign in' }}
          </button>
        </form>
      </section>
    </main>
  `
})
export class LoginPage {
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
