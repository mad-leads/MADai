import { CommonModule } from '@angular/common';
import { PayfastSubscribeComponent } from '../../shared/payfast/payfast-subscribe.component';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';

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

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, RouterLink, PayfastSubscribeComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  styles: [`
    :host {
      display: block;
      min-height: 100vh;
      background: #07110f;
      color: #f8fafc;
      font-family: Inter, Segoe UI, Arial, sans-serif;
      letter-spacing: 0;
    }

    :host * {
      box-sizing: border-box;
      letter-spacing: 0;
    }

    a {
      color: inherit;
      text-decoration: none;
    }

    .home-shell {
      min-height: 100vh;
      background:
        radial-gradient(circle at 13% 8%, rgba(34, 197, 94, 0.22), transparent 28rem),
        radial-gradient(circle at 78% 0%, rgba(20, 184, 166, 0.18), transparent 26rem),
        linear-gradient(135deg, #07110f 0%, #0f1f1d 56%, #03111b 100%);
      overflow: hidden;
    }

    .nav {
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 18px;
      width: min(1160px, calc(100vw - 36px));
      margin: 0 auto;
      padding: 22px 0;
    }

    .brand {
      display: inline-flex;
      align-items: center;
      gap: 12px;
      font-weight: 950;
    }

    .brand img {
      width: 42px;
      height: 42px;
      object-fit: contain;
    }

    .nav-actions {
      display: flex;
      align-items: center;
      gap: 10px;
    }

    .btn {
      display: inline-flex;
      align-items: center;
      justify-content: center;
      min-height: 44px;
      border-radius: 8px;
      padding: 0 17px;
      font-weight: 900;
      transition: transform 160ms ease, box-shadow 160ms ease, border-color 160ms ease;
    }

    .btn:hover {
      transform: translateY(-1px);
    }

    .btn-primary {
      background: linear-gradient(135deg, #651FFF, #FF00D4);
      color: #03111b;
      box-shadow: 0 18px 42px rgba(20, 184, 166, 0.26);
    }

    .btn-secondary {
      border: 1px solid rgba(203, 213, 225, 0.28);
      color: #FFB8F3;
    }

    .hero {
      display: grid;
      align-items: center;
      min-height: calc(100vh - 88px);
      width: min(1160px, calc(100vw - 36px));
      margin: 0 auto;
      padding: 42px 0 72px;
    }

    .hero-grid {
      display: grid;
      grid-template-columns: minmax(0, 1fr) minmax(320px, 0.72fr);
      gap: clamp(28px, 6vw, 72px);
      align-items: center;
    }

    .eyebrow {
      display: inline-flex;
      width: max-content;
      max-width: 100%;
      border: 1px solid rgba(94, 234, 212, 0.28);
      border-radius: 999px;
      background: rgba(20, 184, 166, 0.12);
      color: #FF00D4;
      padding: 8px 12px;
      font-size: 12px;
      font-weight: 900;
      text-transform: uppercase;
    }

    h1 {
      margin: 24px 0 0;
      max-width: 820px;
      font-size: clamp(46px, 8vw, 92px);
      line-height: 0.9;
      font-weight: 950;
    }

    h1 span {
      color: #FF00D4;
    }

    .lead {
      max-width: 690px;
      margin: 26px 0 0;
      color: #DDCEFF;
      font-size: clamp(18px, 2vw, 22px);
      line-height: 1.65;
    }

    .hero-actions {
      display: flex;
      flex-wrap: wrap;
      gap: 12px;
      margin-top: 34px;
    }

    .proof {
      display: grid;
      grid-template-columns: repeat(3, minmax(0, 1fr));
      gap: 12px;
      margin-top: 34px;
      max-width: 720px;
    }

    .proof-card {
      border: 1px solid rgba(203, 213, 225, 0.16);
      border-radius: 8px;
      background: rgba(2, 6, 23, 0.36);
      padding: 16px;
    }

    .proof-card strong {
      display: block;
      color: #ffffff;
      font-size: 19px;
      margin-bottom: 4px;
    }

    .proof-card span {
      color: #a7c4bf;
      font-size: 13px;
      line-height: 1.5;
    }

    .command-panel {
      border: 1px solid rgba(203, 213, 225, 0.16);
      border-radius: 8px;
      background: rgba(2, 6, 23, 0.52);
      box-shadow: 0 34px 90px rgba(0, 0, 0, 0.32);
      overflow: hidden;
    }

    .panel-bar {
      display: flex;
      align-items: center;
      gap: 8px;
      height: 44px;
      padding: 0 14px;
      border-bottom: 1px solid rgba(203, 213, 225, 0.12);
      color: #94a3b8;
      font-size: 12px;
      font-weight: 800;
    }

    .panel-dot {
      width: 10px;
      height: 10px;
      border-radius: 999px;
      background: #651FFF;
    }

    .panel-body {
      display: grid;
      gap: 12px;
      padding: 18px;
    }

    .signal-row {
      display: grid;
      grid-template-columns: 1fr auto;
      gap: 16px;
      align-items: center;
      border: 1px solid rgba(203, 213, 225, 0.12);
      border-radius: 8px;
      background: rgba(15, 23, 42, 0.62);
      padding: 14px;
    }

    .signal-row b {
      display: block;
      color: #f8fafc;
      margin-bottom: 4px;
    }

    .signal-row span {
      color: #94a3b8;
      font-size: 13px;
    }

    .signal-pill {
      border-radius: 999px;
      background: rgba(34, 197, 94, 0.12);
      color: #FF00D4;
      padding: 7px 10px;
      font-size: 12px;
      font-weight: 900;
      white-space: nowrap;
    }

    /* --- Features section --- */
    .features {
      background: #0b1a17;
      border-top: 1px solid rgba(148, 163, 184, 0.1);
      padding: 72px 18px;
    }

    .features .section-inner {
      width: min(1120px, 100%);
      margin: 0 auto;
    }

    .features-grid {
      display: grid;
      grid-template-columns: repeat(3, minmax(0, 1fr));
      gap: 20px;
      margin-top: 34px;
    }

    .feature-card {
      border: 1px solid rgba(203, 213, 225, 0.12);
      border-radius: 8px;
      background: rgba(2, 6, 23, 0.42);
      padding: 28px 22px;
    }

    .feature-icon {
      display: inline-flex;
      align-items: center;
      justify-content: center;
      width: 44px;
      height: 44px;
      border-radius: 8px;
      background: rgba(101, 31, 255, 0.14);
      color: #FF00D4;
      font-size: 20px;
      margin-bottom: 16px;
    }

    .feature-card h3 {
      margin: 0 0 8px;
      font-size: 18px;
      font-weight: 900;
      color: #f8fafc;
    }

    .feature-card p {
      margin: 0;
      color: #a7c4bf;
      font-size: 14px;
      line-height: 1.65;
    }

    /* --- How it works section --- */
    .how-it-works {
      background: #07110f;
      padding: 72px 18px;
    }

    .how-it-works .section-inner {
      width: min(1120px, 100%);
      margin: 0 auto;
    }

    .steps-grid {
      display: grid;
      grid-template-columns: repeat(4, minmax(0, 1fr));
      gap: 16px;
      margin-top: 34px;
    }

    .step-card {
      position: relative;
      border: 1px solid rgba(203, 213, 225, 0.1);
      border-radius: 8px;
      background: rgba(15, 23, 42, 0.38);
      padding: 24px 18px;
    }

    .step-num {
      display: inline-flex;
      align-items: center;
      justify-content: center;
      width: 32px;
      height: 32px;
      border-radius: 999px;
      background: linear-gradient(135deg, #651FFF, #FF00D4);
      color: #fff;
      font-size: 14px;
      font-weight: 900;
      margin-bottom: 14px;
    }

    .step-card h3 {
      margin: 0 0 8px;
      font-size: 16px;
      font-weight: 900;
      color: #f8fafc;
    }

    .step-card p {
      margin: 0;
      color: #94a3b8;
      font-size: 13px;
      line-height: 1.6;
    }

    /* --- Marquee strip --- */
    .mad-universe-strip {
      position: relative;
      overflow: hidden;
      background: #0d1628;
      border-top: 1px solid rgba(148, 163, 184, 0.16);
      border-bottom: 1px solid rgba(148, 163, 184, 0.16);
      padding: 10px 0;
    }

    .mad-universe-inner {
      display: flex;
      align-items: center;
      gap: 18px;
      width: min(1080px, calc(100vw - 32px));
      margin: 0 auto;
    }

    .mad-universe-kicker {
      flex: 0 0 auto;
      display: inline-flex;
      align-items: center;
      gap: 6px;
      margin: 0;
      color: #FF00D4;
      font-size: 9px;
      font-weight: 800;
      text-transform: uppercase;
      white-space: nowrap;
    }

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
      gap: 28px;
      width: max-content;
      animation: madUniverseScroll 44s linear infinite;
    }

    .mad-universe-link {
      display: inline-flex;
      align-items: center;
      justify-content: center;
      min-width: max-content;
      opacity: 0.72;
      transition: opacity 160ms ease, transform 160ms ease;
    }

    .mad-universe-link:hover {
      opacity: 1;
      transform: translateY(-1px);
    }

    .mad-universe-link img {
      display: block;
      width: auto;
      max-width: 120px;
      height: 17px;
      object-fit: contain;
      filter: drop-shadow(0 0 12px rgba(255,255,255,0.08));
    }

    .mad-universe-text {
      color: #cbd5e1;
      font-size: 10px;
      font-weight: 900;
      text-transform: uppercase;
      white-space: nowrap;
    }

    @keyframes madUniverseScroll {
      from { transform: translateX(0); }
      to { transform: translateX(-50%); }
    }

    /* --- Pricing --- */
    .pricing {
      background: #f6fbf9;
      color: #0f172a;
      padding: 72px 18px;
    }

    .section-inner {
      width: min(1120px, 100%);
      margin: 0 auto;
    }

    .section-label {
      color: #651FFF;
      font-size: 12px;
      font-weight: 950;
      text-transform: uppercase;
    }

    h2 {
      margin: 12px 0 0;
      max-width: 740px;
      font-size: clamp(34px, 5vw, 58px);
      line-height: 1;
      font-weight: 950;
    }

    .section-copy {
      max-width: 680px;
      margin: 16px 0 0;
      color: #475569;
      font-size: 18px;
      line-height: 1.7;
    }

    .plans {
      display: grid;
      grid-template-columns: repeat(3, minmax(0, 1fr));
      gap: 16px;
      margin-top: 34px;
    }

    .plan {
      display: flex;
      flex-direction: column;
      border: 1px solid #EDE4FF;
      border-radius: 8px;
      background: #ffffff;
      padding: 24px;
      box-shadow: 0 18px 42px rgba(15, 23, 42, 0.08);
    }

    .plan.featured {
      background: #07110f;
      color: #f8fafc;
      border-color: rgba(20, 184, 166, 0.5);
      transform: translateY(-8px);
    }

    .plan-name {
      font-weight: 950;
      font-size: 20px;
    }

    .plan-desc {
      margin: 6px 0 0;
      color: #64748b;
      font-size: 13px;
      line-height: 1.5;
    }

    .plan.featured .plan-desc {
      color: #D4C0FF;
    }

    .plan-price {
      margin-top: 18px;
      font-size: 42px;
      line-height: 1;
      font-weight: 950;
    }

    .plan-price small {
      color: #64748b;
      font-size: 14px;
      font-weight: 800;
    }

    .plan.featured .plan-price small,
    .plan.featured li {
      color: #D4C0FF;
    }

    .plan ul {
      display: grid;
      gap: 10px;
      margin: 22px 0;
      padding: 0;
      list-style: none;
      color: #475569;
      font-size: 14px;
      line-height: 1.5;
    }

    .plan li::before {
      content: '';
      display: inline-flex;
      width: 7px;
      height: 7px;
      border-radius: 999px;
      background: #651FFF;
      margin-right: 8px;
      transform: translateY(-1px);
    }

    .plan .btn {
      margin-top: auto;
    }

    .pricing .btn-secondary {
      border-color: #DDCEFF;
      color: #0f172a;
    }

    /* --- Final CTA --- */
    .final-cta {
      background: #07110f;
      color: #f8fafc;
      padding: 70px 18px;
      text-align: center;
    }

    .final-cta .section-copy {
      color: #D4C0FF;
    }

    .final-cta .btn-secondary {
      border-color: rgba(203, 213, 225, 0.28);
      color: #FFB8F3;
    }

    .final-cta h2,
    .final-cta p {
      margin-left: auto;
      margin-right: auto;
    }

    /* --- Responsive --- */
    @media (max-width: 900px) {
      .hero-grid,
      .proof,
      .plans,
      .features-grid {
        grid-template-columns: 1fr;
      }

      .steps-grid {
        grid-template-columns: repeat(2, 1fr);
      }

      .command-panel {
        max-width: 520px;
      }

      .plan.featured {
        transform: none;
      }
    }

    @media (max-width: 700px) {
      .nav {
        align-items: flex-start;
        flex-direction: column;
      }

      .nav-actions,
      .hero-actions {
        width: 100%;
      }

      .btn {
        flex: 1 1 auto;
      }

      .steps-grid {
        grid-template-columns: 1fr;
      }

      .mad-universe-inner {
        align-items: stretch;
        flex-direction: column;
        gap: 8px;
      }

      .mad-universe-kicker {
        justify-content: center;
      }

      .mad-universe-track {
        gap: 22px;
        animation-duration: 52s;
      }

      .mad-universe-link img {
        height: 15px;
        max-width: 110px;
      }
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
    <div class="home-shell">
      <nav class="nav" aria-label="MADai main navigation">
        <a routerLink="/home" class="brand" aria-label="MADai home">
          <img src="/icon-MADai.png" alt="MADai logo">
          <span>MADai</span>
        </a>
        <div class="nav-actions">
          <a routerLink="/login" class="btn btn-secondary">Sign in</a>
          <a routerLink="/login" class="btn btn-primary">Launch command center</a>
        </div>
      </nav>

      <section class="hero" data-section="cta" aria-label="MADai hero">
        <div class="hero-grid">
          <div>
            <span class="eyebrow">AI tools and workflows for subscribers</span>
            <h1>Create, automate, <span>and ship with MADai.</span></h1>
            <p class="lead">
              MADai gives subscribers one fresh workspace for writing, research, creative briefs,
              reports, automation, integrations, and reusable AI workflows. Launch guided tools,
              track every request, and collect polished outputs without juggling chat windows.
            </p>
            <div class="hero-actions">
              <a routerLink="/login" class="btn btn-primary">Start with MADai free</a>
              <a href="#pricing" class="btn btn-secondary">See plans &amp; pricing</a>
            </div>
            <div class="proof" aria-label="MADai value propositions">
              <div class="proof-card"><strong>Centralize</strong><span>Keep product prompts, task context, and execution history in one place instead of scattered across tools.</span></div>
              <div class="proof-card"><strong>Accelerate</strong><span>Move from idea to action with repeatable AI workflows, operator review, and one-click task runs.</span></div>
              <div class="proof-card"><strong>Control</strong><span>Give every team a calmer, auditable way to run AI-assisted work with human approval gates.</span></div>
            </div>
          </div>

          <aside class="command-panel" aria-label="MADai command center preview">
            <div class="panel-bar"><span class="panel-dot" aria-hidden="true"></span> madai://subscriber/studio</div>
            <div class="panel-body">
              <div class="signal-row"><div><b>AI Tools</b><span>Writing, creative, intelligence, and automation launchers.</span></div><span class="signal-pill">Ready</span></div>
              <div class="signal-row"><div><b>Workflow Studio</b><span>Reusable playbooks for bigger outcomes.</span></div><span class="signal-pill">Fresh</span></div>
              <div class="signal-row"><div><b>Output Gallery</b><span>Generated work, files, and evidence in one place.</span></div><span class="signal-pill">Tracked</span></div>
            </div>
          </aside>
        </div>
      </section>
    </div>

    <section class="features" data-section="features" aria-label="MADai platform capabilities">
      <div class="section-inner">
        <span class="section-label">Platform capabilities</span>
        <h2>Everything subscribers need to put AI to work.</h2>
        <p class="section-copy" style="color: #a7c4bf;">MADai is built for people who want useful AI outcomes, not another empty chat box. Choose a tool, give context, and keep every request, file, and output organized.</p>
        <div class="features-grid">
          <article class="feature-card">
            <div class="feature-icon" aria-hidden="true">&#9881;</div>
            <h3>Guided AI tools</h3>
            <p>Launch writing, creative, research, analysis, transcription, reporting, automation, integration, and software helper tools from one friendly workspace.</p>
          </article>
          <article class="feature-card">
            <div class="feature-icon" aria-hidden="true">&#9878;</div>
            <h3>Trackable requests</h3>
            <p>Every request becomes a clear MADai task with status, history, outputs, and files, so subscribers always know what is running and what is ready.</p>
          </article>
          <article class="feature-card">
            <div class="feature-icon" aria-hidden="true">&#9872;</div>
            <h3>Review &amp; approval lanes</h3>
            <p>Every AI output passes through a human review step before it goes live. Set up approval workflows per workspace, per team, or per task type. Full audit history means you always know who approved what and when.</p>
          </article>
          <article class="feature-card">
            <div class="feature-icon" aria-hidden="true">&#128202;</div>
            <h3>Workspace dashboards</h3>
            <p>Each workspace gets its own dashboard showing active tasks, pending reviews, execution metrics, and team activity. See at a glance what is running, what needs attention, and where bottlenecks are forming.</p>
          </article>
          <article class="feature-card">
            <div class="feature-icon" aria-hidden="true">&#128279;</div>
            <h3>MAD product integration</h3>
            <p>MADai connects natively to the MAD universe of products including MAD Prospects, MADAuthor, MADCreate, and more. Push AI outputs directly into the tools your team already uses without context-switching.</p>
          </article>
          <article class="feature-card">
            <div class="feature-icon" aria-hidden="true">&#128274;</div>
            <h3>Role-based access control</h3>
            <p>Assign team roles with granular permissions for who can create prompts, run tasks, approve outputs, or manage workspaces. Keep sensitive operations locked down while giving contributors the access they need.</p>
          </article>
        </div>
      </div>
    </section>

    <section class="how-it-works" data-section="how-it-works" aria-label="How MADai works">
      <div class="section-inner">
        <span class="section-label">How it works</span>
        <h2>From prompt to production in four steps.</h2>
        <p class="section-copy" style="color: #94a3b8;">MADai turns the messy process of running AI across a product team into a clean, repeatable workflow. Here is how it works.</p>
        <div class="steps-grid">
          <article class="step-card">
            <div class="step-num">1</div>
            <h3>Create a workspace</h3>
            <p>Set up a workspace for your team, project, or product line. Add members, configure approval rules, and import your existing prompts to get started in minutes.</p>
          </article>
          <article class="step-card">
            <div class="step-num">2</div>
            <h3>Build your prompt library</h3>
            <p>Write, save, and tag prompts that your team will reuse. Include variables for dynamic inputs so anyone can run a prompt without rewriting it from scratch every time.</p>
          </article>
          <article class="step-card">
            <div class="step-num">3</div>
            <h3>Run tasks &amp; review outputs</h3>
            <p>Queue tasks from the command center. AI runs in the background while you work on other things. When results are ready, review them in the approval lane before they go live.</p>
          </article>
          <article class="step-card">
            <div class="step-num">4</div>
            <h3>Ship &amp; iterate</h3>
            <p>Approved outputs flow into your MAD products or export as files. Every run is logged with full context so your team can learn from what worked and refine what didn't.</p>
          </article>
        </div>
      </div>
    </section>

    <section class="mad-universe-strip" data-section="marquee" aria-label="Explore the MAD universe of products">
      <div class="mad-universe-inner">
        <p class="mad-universe-kicker"><span aria-hidden="true">*</span> MAD universe</p>
        <div class="mad-universe-marquee">
          <div class="mad-universe-track">
            @for (app of madUniverseApps.concat(madUniverseApps); track app.name + $index) {
              <a class="mad-universe-link" [href]="app.url" target="_blank" rel="noopener" [attr.aria-label]="'Visit ' + app.name">
                @if (app.logo) {
                  <img [src]="app.logo" [alt]="app.name + ' logo'" loading="lazy" decoding="async">
                } @else {
                  <span class="mad-universe-text">{{ app.name }}</span>
                }
              </a>
            }
          </div>
        </div>
      </div>
    </section>

    <section id="pricing" class="pricing" data-section="pricing" aria-label="MADai pricing plans">
      <div class="section-inner">
        <span class="section-label">Simple pricing</span>
        <h2>Start lean. Scale when AI becomes your operating layer.</h2>
        <p class="section-copy">Every plan includes the command center, prompt library, workspace history, MADCloud-powered AI operations, and secure Payfast checkout. No hidden fees, no per-seat surcharges on Starter.</p>
        <div class="plans">
          <article class="plan">
            <div class="plan-name">Starter</div>
            <p class="plan-desc">For individuals and small teams getting started with AI-powered workflows.</p>
            <div class="plan-price">Free <small>forever</small></div>
            <ul>
              <li>1 workspace</li>
              <li>25 AI task runs per month</li>
              <li>Prompt library starter kit</li>
              <li>Basic execution history</li>
              <li>Email support</li>
            </ul>
            <a routerLink="/login" class="btn btn-secondary">Start free</a>
          </article>
          <article class="plan featured">
            <div class="plan-name">Operator</div>
            <p class="plan-desc">For teams running AI workflows daily with review and approval needs.</p>
            <div class="plan-price">R699 <small>/mo</small></div>
            <ul>
              <li>5 workspaces</li>
              <li>Unlimited AI task runs</li>
              <li>Unlimited saved prompts</li>
              <li>Execution review lanes</li>
              <li>Priority support</li>
            </ul>
            <a routerLink="/login" class="btn btn-primary">Start 14-day trial</a>
          </article>
          <article class="plan">
            <div class="plan-name">Studio</div>
            <p class="plan-desc">For organisations scaling AI across multiple teams and product lines.</p>
            <div class="plan-price">R1,999 <small>/mo</small></div>
            <ul>
              <li>Unlimited workspaces</li>
              <li>Team roles and audit history</li>
              <li>Custom workflow setup</li>
              <li>Dedicated launch support</li>
              <li>SSO and advanced security</li>
            </ul>
            <a routerLink="/login" class="btn btn-secondary">Contact sales</a>
          </article>
        </div>
      </div>
    </section>

    <app-payfast-subscribe productName="MADai" headline="Subscribe to MADai with Payfast" lead="Pick a monthly plan and open secure onsite checkout without leaving MADai." [compact]="true"></app-payfast-subscribe>

    <section class="final-cta" data-section="cta-final" aria-label="Get started with MADai">
      <div class="section-inner">
        <span class="section-label">Ready to move</span>
        <h2>Give your team one place to think, prompt, review, and act.</h2>
        <p class="section-copy">MADai is the execution cockpit for product work that needs speed without losing control. Join teams already running smarter AI workflows across the MAD universe.</p>
        <div class="hero-actions" style="justify-content: center; margin-top: 28px;">
          <a routerLink="/login" class="btn btn-primary">Launch MADai free</a>
          <a href="https://madprospects.com/" target="_blank" rel="noopener" class="btn btn-secondary">Explore MAD Prospects</a>
        </div>
      </div>
    </section>
  `,
})
export class HomePage {
  protected readonly madUniverseApps = MAD_UNIVERSE_APPS;
}
