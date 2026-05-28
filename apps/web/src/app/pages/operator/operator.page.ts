import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { catchError, forkJoin, of } from 'rxjs';
import { ApiService } from '../../core/http/api.service';
import { PayfastSubscribeComponent } from '../../shared/payfast/payfast-subscribe.component';

type SectionKey =
  | 'home'
  | 'tools'
  | 'studio'
  | 'workflows'
  | 'tasks'
  | 'outputs'
  | 'library'
  | 'notifications'
  | 'billing'
  | 'account'
  | 'admin'
  | 'workers'
  | 'audit'
  | 'settings'
  | 'webhooks';

interface StoredUser {
  email: string;
  displayName: string;
  roles: string[];
  companyName?: string;
}

interface SectionDef {
  key: SectionKey;
  label: string;
  adminOnly?: boolean;
}

interface AiTool {
  key: string;
  name: string;
  group: string;
  description: string;
  taskCategory: string | number;
  fields: string[];
  examples: string[];
}

interface TaskSummary {
  id: string;
  title: string;
  category: string | number;
  priority: string | number;
  status: string | number;
  progress: number;
  createdDate: string;
  completedAt?: string | null;
  queue?: string | null;
}

interface PagedResult<T> {
  items?: T[];
  totalCount?: number;
}

interface TemplateDto {
  id: string;
  name: string;
  description?: string;
  category: string | number;
  defaultPriority?: string | number;
  promptTemplate?: string;
  defaultInputJson?: string;
}

interface OutputItem {
  taskId: string;
  title: string;
  category: string;
  status: string;
  summary?: string | null;
  result?: string | null;
  createdOrCompletedAt: string;
  artifacts: Array<{ id: string; fileName: string; contentType: string; kind?: string | null; sizeBytes: number; previewUrl?: string | null }>;
}

interface NotificationItem {
  id: string;
  title: string;
  body: string;
  severity: string;
  createdAt: string;
  readAt?: string | null;
  dismissedAt?: string | null;
}

interface UsageDto {
  planName: string;
  tasksThisMonth: number;
  completedThisMonth: number;
  includedTasks: number;
  storageUsedMb: number;
  upgradeRecommended: boolean;
  message: string;
}

interface FileItem {
  id: string;
  name: string;
  contentType: string;
  sizeBytes: number;
  createdDate: string;
}

interface DataBlock {
  title: string;
  endpoint: string;
  value: unknown;
  error?: string;
}

const STARTER_PLAYBOOKS = [
  {
    name: 'Content Campaign',
    description: 'Research a topic, draft the core article, then create social posts and a report.',
    toolKeys: ['research-hub', 'blog-writer', 'social-studio', 'report-builder'],
  },
  {
    name: 'Lead Research Pack',
    description: 'Collect company context, analyze prospects, and prepare outreach assets for MADLeads or MADRecruiting.',
    toolKeys: ['research-hub', 'data-analyst', 'social-studio'],
  },
  {
    name: 'Website Audit',
    description: 'Review a website, summarize UX issues, create test notes, and prepare an improvement plan.',
    toolKeys: ['research-hub', 'ux-ui-studio', 'test-planner', 'report-builder'],
  },
  {
    name: 'Document Production',
    description: 'Turn a rough idea into a structured document, review notes, and export-ready copy.',
    toolKeys: ['doc-assistant', 'research-hub', 'report-builder'],
  },
  {
    name: 'App Integration',
    description: 'Map an integration, define API contracts, prepare implementation tasks, and create verification notes.',
    toolKeys: ['integration-architect', 'dotnet-helper', 'angular-helper', 'test-planner'],
  },
  {
    name: 'MAD Universe Handoff',
    description: 'Prepare a cross-product handoff plan for MAD Prospects, MADAuthor, MADCreate, MADLeads, or another MAD app.',
    toolKeys: ['integration-architect', 'automation-builder', 'report-builder'],
  },
];

@Component({
  selector: 'app-operator-page',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, PayfastSubscribeComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  styles: [`
    :host { display: block; min-height: 100vh; background: #f7f8fd; color: #172033; font-family: Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif; }
    :host * { box-sizing: border-box; letter-spacing: 0; }
    .shell { display: grid; grid-template-columns: 276px minmax(0, 1fr); min-height: 100vh; }
    aside { position: sticky; top: 0; height: 100vh; overflow: auto; padding: 18px; background: linear-gradient(180deg, #ffffff 0%, #f0f4ff 100%); border-right: 1px solid #dde5f3; }
    .brand { display: flex; align-items: center; gap: 12px; color: #111827; text-decoration: none; font-weight: 950; font-size: 22px; margin-bottom: 18px; }
    .brand img { width: 42px; height: 42px; object-fit: contain; }
    nav { display: grid; gap: 6px; }
    nav button, nav a, .signout { width: 100%; min-height: 42px; border: 0; border-radius: 8px; background: transparent; color: #46546b; cursor: pointer; display: flex; align-items: center; justify-content: flex-start; padding: 9px 11px; font: inherit; font-weight: 850; text-decoration: none; }
    nav button.is-active { background: #111827; color: #fff; box-shadow: 0 14px 32px rgba(17, 24, 39, .16); }
    nav button:hover, nav a:hover, .signout:hover { background: #e8eef9; color: #111827; }
    .admin-link { margin-top: 8px; color: #5b21b6 !important; }
    .user-card { margin: 18px 0; padding: 14px; border: 1px solid #dbe4f3; border-radius: 8px; background: #fff; }
    .user-card strong { display: block; color: #111827; word-break: break-word; }
    .user-card span { color: #667085; font-size: 12px; }
    .powered { margin-top: 12px; color: #7b8798; font-size: 11px; line-height: 1.45; }
    main { min-width: 0; padding: 24px; }
    .hero { position: relative; overflow: hidden; display: grid; grid-template-columns: minmax(0, 1fr) minmax(280px, 420px); gap: 24px; align-items: stretch; margin-bottom: 18px; border-radius: 8px; background: linear-gradient(135deg, #101827 0%, #2838a8 52%, #ff3fb4 100%); color: #fff; padding: clamp(22px, 5vw, 40px); }
    .hero::after { content: ""; position: absolute; inset: auto -10% -35% 42%; height: 280px; background: radial-gradient(circle, rgba(255,255,255,.32), transparent 68%); pointer-events: none; }
    .hero > * { position: relative; z-index: 1; }
    .eyebrow { margin: 0 0 8px; color: #7b61ff; text-transform: uppercase; font-size: 11px; font-weight: 950; }
    .hero .eyebrow { color: #ffe6fb; }
    h1, h2, h3, p { margin: 0; }
    h1 { max-width: 720px; font-size: clamp(38px, 6vw, 72px); line-height: .95; font-weight: 950; }
    h2 { font-size: 22px; }
    h3 { font-size: 16px; }
    .lead { margin-top: 18px; max-width: 720px; color: #f4efff; font-size: 18px; line-height: 1.6; }
    .hero-panel { display: grid; gap: 10px; align-content: center; border: 1px solid rgba(255,255,255,.22); border-radius: 8px; background: rgba(255,255,255,.14); padding: 18px; backdrop-filter: blur(10px); }
    .hero-panel div { display: flex; justify-content: space-between; gap: 12px; border-bottom: 1px solid rgba(255,255,255,.16); padding-bottom: 10px; }
    .hero-panel div:last-child { border-bottom: 0; padding-bottom: 0; }
    .hero-panel span { color: #f4efff; }
    .hero-panel strong { font-size: 26px; }
    .top-actions, .button-row { display: flex; flex-wrap: wrap; align-items: center; gap: 8px; }
    .top-actions { margin-top: 24px; }
    button, a.button, input, select, textarea { border-radius: 8px; font: inherit; }
    button, a.button { min-height: 40px; border: 1px solid #d5dfef; background: #fff; color: #172033; cursor: pointer; display: inline-flex; align-items: center; justify-content: center; padding: 8px 13px; font-weight: 850; text-decoration: none; }
    button.primary, a.primary { border-color: #111827; background: #111827; color: #fff; }
    button.accent { border-color: #ff3fb4; background: #ff3fb4; color: #fff; }
    button.ghost { background: #f8fafc; }
    button.danger { border-color: #fecaca; color: #b42318; }
    button:disabled { opacity: .55; cursor: not-allowed; }
    .section-head { display: flex; align-items: flex-start; justify-content: space-between; gap: 16px; margin: 20px 0 12px; }
    .muted { color: #667085; line-height: 1.55; }
    .grid { display: grid; grid-template-columns: repeat(3, minmax(0, 1fr)); gap: 14px; }
    .grid.two { grid-template-columns: repeat(2, minmax(0, 1fr)); }
    .card, .panel, .output-card, .task-row { border: 1px solid #dfe7f3; border-radius: 8px; background: #fff; box-shadow: 0 18px 45px rgba(15, 23, 42, .07); }
    .card { display: grid; gap: 12px; padding: 18px; min-height: 180px; }
    .card strong { display: block; font-size: 17px; }
    .card small { color: #667085; line-height: 1.45; }
    .tool-card { cursor: pointer; text-align: left; transition: transform 160ms ease, border-color 160ms ease, box-shadow 160ms ease; }
    .tool-card:hover, .tool-card.is-active { transform: translateY(-2px); border-color: #7b61ff; box-shadow: 0 18px 40px rgba(123, 97, 255, .16); }
    .pill-row { display: flex; flex-wrap: wrap; gap: 6px; }
    .pill { border-radius: 999px; background: #eff4ff; color: #365ac9; padding: 5px 9px; font-size: 11px; font-weight: 900; }
    .pill.success { background: #ecfdf3; color: #027a48; }
    .pill.warning { background: #fff8e6; color: #a15c00; }
    .pill.dark { background: #111827; color: #fff; }
    .composer { display: grid; grid-template-columns: minmax(280px, .85fr) minmax(0, 1.15fr); gap: 14px; align-items: start; }
    .panel { padding: 18px; }
    form { display: grid; gap: 12px; margin-top: 14px; }
    label { display: grid; gap: 6px; color: #344054; font-size: 13px; font-weight: 850; }
    input, select, textarea { width: 100%; min-height: 42px; border: 1px solid #cfd9ea; background: #fff; color: #172033; padding: 10px 12px; }
    textarea { min-height: 150px; resize: vertical; }
    .status-line { margin-top: 12px; color: #4d5c73; font-weight: 800; }
    .status-line.error { color: #b42318; }
    .tasks { display: grid; gap: 10px; }
    .task-row { display: grid; grid-template-columns: minmax(0, 1fr) auto; gap: 14px; padding: 14px; align-items: center; }
    .progress { height: 8px; overflow: hidden; border-radius: 999px; background: #edf2f7; margin-top: 10px; }
    .progress span { display: block; height: 100%; background: linear-gradient(90deg, #7b61ff, #ff3fb4); }
    .output-card { display: grid; gap: 12px; padding: 16px; }
    .output-card pre { margin: 0; white-space: pre-wrap; word-break: break-word; color: #344054; font-family: inherit; max-height: 220px; overflow: auto; }
    .empty { border: 1px dashed #cbd5e1; border-radius: 8px; background: #fff; padding: 28px; color: #667085; text-align: center; }
    .list { display: grid; gap: 10px; }
    .list-item { display: grid; gap: 8px; border: 1px solid #e2e8f0; border-radius: 8px; background: #fff; padding: 14px; }
    .admin-grid { display: grid; grid-template-columns: minmax(260px, .8fr) minmax(0, 1.2fr); gap: 14px; align-items: start; }
    table { width: 100%; border-collapse: collapse; min-width: 640px; }
    th, td { border-bottom: 1px solid #edf2f7; padding: 9px; text-align: left; vertical-align: top; font-size: 12px; }
    th { color: #667085; text-transform: uppercase; font-size: 10px; }
    .table-wrap { overflow: auto; }
    @media (max-width: 1080px) { .shell { grid-template-columns: 1fr; } aside { position: static; height: auto; } nav { grid-template-columns: repeat(auto-fit, minmax(145px, 1fr)); } .hero, .composer, .admin-grid { grid-template-columns: 1fr; } .grid, .grid.two { grid-template-columns: repeat(2, minmax(0, 1fr)); } }
    @media (max-width: 700px) { main { padding: 14px; } .grid, .grid.two { grid-template-columns: 1fr; } .task-row { grid-template-columns: 1fr; } h1 { font-size: 38px; } }
  `],
  template: `
    <div class="shell">
      <aside>
        <a routerLink="/dashboard" class="brand">
          <img src="/icon-MADai.png" alt="MADai">
          <span>MADai</span>
        </a>
        <nav aria-label="MADai app navigation">
          @for (section of visibleSections(); track section.key) {
            <button type="button" [class.is-active]="activeSection() === section.key" (click)="open(section.key)">{{ section.label }}</button>
          }
          @if (isSuperAdmin()) {
            <a routerLink="/ai" class="admin-link">MADCloud /ai</a>
          }
        </nav>
        <div class="user-card">
          <strong>{{ user()?.displayName || user()?.email || 'MADai user' }}</strong>
          <span>{{ user()?.companyName || 'Subscriber workspace' }}</span>
          <p class="powered">Powered by MADCloud infrastructure. Your workflow stays inside MADai.</p>
        </div>
        <button type="button" class="signout" (click)="logout()">Sign out</button>
      </aside>

      <main>
        <section class="hero">
          <div>
            <p class="eyebrow">Subscriber AI workspace</p>
            <h1>{{ heroTitle() }}</h1>
            <p class="lead">{{ heroCopy() }}</p>
            <div class="top-actions">
              <button type="button" class="primary" (click)="open('tools')">Launch an AI tool</button>
              <button type="button" class="accent" (click)="open('studio')">Open Studio</button>
              <button type="button" (click)="refresh()">Refresh</button>
            </div>
          </div>
          <div class="hero-panel" aria-label="MADai usage summary">
            <div><span>Plan</span><strong>{{ usage()?.planName || 'Starter' }}</strong></div>
            <div><span>Runs this month</span><strong>{{ usage()?.tasksThisMonth ?? taskCount() }}</strong></div>
            <div><span>Completed</span><strong>{{ usage()?.completedThisMonth ?? completedCount() }}</strong></div>
            <div><span>Storage</span><strong>{{ usage()?.storageUsedMb ?? 0 }} MB</strong></div>
          </div>
        </section>

        @if (statusMessage()) {
          <p class="status-line" [class.error]="statusError()">{{ statusMessage() }}</p>
        }

        @switch (activeSection()) {
          @case ('home') { <ng-container [ngTemplateOutlet]="homeSection"></ng-container> }
          @case ('tools') { <ng-container [ngTemplateOutlet]="toolsSection"></ng-container> }
          @case ('studio') { <ng-container [ngTemplateOutlet]="studioSection"></ng-container> }
          @case ('workflows') { <ng-container [ngTemplateOutlet]="workflowsSection"></ng-container> }
          @case ('tasks') { <ng-container [ngTemplateOutlet]="tasksSection"></ng-container> }
          @case ('outputs') { <ng-container [ngTemplateOutlet]="outputsSection"></ng-container> }
          @case ('library') { <ng-container [ngTemplateOutlet]="librarySection"></ng-container> }
          @case ('notifications') { <ng-container [ngTemplateOutlet]="notificationsSection"></ng-container> }
          @case ('billing') { <ng-container [ngTemplateOutlet]="billingSection"></ng-container> }
          @case ('account') { <ng-container [ngTemplateOutlet]="accountSection"></ng-container> }
          @default { <ng-container [ngTemplateOutlet]="adminSection"></ng-container> }
        }

        <ng-template #homeSection>
          <div class="section-head">
            <div><p class="eyebrow">Start here</p><h2>Choose a tool, launch a workflow, or review what MADai has produced.</h2></div>
          </div>
          <section class="grid">
            <button type="button" class="card tool-card" (click)="open('tools')"><strong>AI Tools</strong><small>Guided tools for writing, creative work, research, automation, integrations, and software help.</small><span class="pill dark">{{ tools().length }} tools</span></button>
            <button type="button" class="card tool-card" (click)="open('studio')"><strong>Creative Studio</strong><small>A focused request composer with presets, examples, attachments, and launch controls.</small><span class="pill">Fresh workspace</span></button>
            <button type="button" class="card tool-card" (click)="open('workflows')"><strong>Workflow Studio</strong><small>Starter playbooks convert larger goals into repeatable MADai task runs.</small><span class="pill">{{ playbooks.length }} playbooks</span></button>
            <button type="button" class="card tool-card" (click)="open('tasks')"><strong>My Tasks</strong><small>Track active, completed, failed, and queued requests without seeing backend machinery.</small><span class="pill">{{ taskCount() }} requests</span></button>
            <button type="button" class="card tool-card" (click)="open('outputs')"><strong>Outputs</strong><small>Browse generated content, reports, artifacts, files, and completed AI results.</small><span class="pill success">{{ outputs().length }} outputs</span></button>
            <button type="button" class="card tool-card" (click)="open('billing')"><strong>Billing</strong><small>Soft usage prompts, Payfast-only checkout, USD default, and ZAR support for South Africa.</small><span class="pill warning">Soft upsell</span></button>
          </section>
        </ng-template>

        <ng-template #toolsSection>
          <div class="section-head">
            <div><p class="eyebrow">AI Tools</p><h2>All subscriber AI functionality, grouped by the work people actually want done.</h2></div>
            <select [(ngModel)]="toolGroup" name="toolGroup"><option value="All">All groups</option><option>Writing</option><option>Creative</option><option>Intelligence</option><option>Automation</option></select>
          </div>
          <section class="grid">
            @for (tool of filteredTools(); track tool.key) {
              <button type="button" class="card tool-card" [class.is-active]="selectedTool()?.key === tool.key" (click)="selectTool(tool)">
                <div class="pill-row"><span class="pill">{{ tool.group }}</span><span class="pill">{{ displayEnum(tool.taskCategory) }}</span></div>
                <strong>{{ tool.name }}</strong>
                <small>{{ tool.description }}</small>
                <div class="pill-row">@for (example of tool.examples.slice(0, 3); track example) { <span class="pill">{{ example }}</span> }</div>
              </button>
            }
          </section>
        </ng-template>

        <ng-template #studioSection>
          <div class="section-head">
            <div><p class="eyebrow">Creative Studio</p><h2>Describe what you need. MADai turns it into a trackable AI request.</h2></div>
          </div>
          <section class="composer">
            <aside class="panel">
              <p class="eyebrow">Selected tool</p>
              <h2>{{ selectedTool()?.name || 'Choose a tool' }}</h2>
              <p class="muted">{{ selectedTool()?.description || 'Pick a tool from AI Tools or choose one below.' }}</p>
              <div class="pill-row" style="margin-top: 12px;">
                @for (tool of tools().slice(0, 8); track tool.key) {
                  <button type="button" class="ghost" (click)="selectTool(tool)">{{ tool.name }}</button>
                }
              </div>
            </aside>
            <form class="panel" (ngSubmit)="launchRequest()">
              <label>Request title<input name="requestTitle" [(ngModel)]="requestTitle" placeholder="Example: Launch campaign for MADai"></label>
              <label>What should MADai create or solve?<textarea name="requestPrompt" [(ngModel)]="requestPrompt" required placeholder="Give MADai the goal, audience, source material, constraints, and expected output."></textarea></label>
              <label>Extra context<input name="requestContext" [(ngModel)]="requestContext" placeholder="Audience, format, brand voice, URL, product, or app name"></label>
              <label>Priority<select name="requestPriority" [(ngModel)]="requestPriority"><option [ngValue]="25">Low</option><option [ngValue]="50">Normal</option><option [ngValue]="75">High</option><option [ngValue]="100">Critical</option></select></label>
              <label>Attach a source file<input type="file" (change)="onFilePicked($event)"></label>
              <div class="button-row"><button type="submit" class="primary" [disabled]="saving()">Launch MADai AI</button><button type="button" (click)="loadExample()">Use example</button></div>
            </form>
          </section>
        </ng-template>

        <ng-template #workflowsSection>
          <div class="section-head">
            <div><p class="eyebrow">Workflow Studio</p><h2>Starter playbooks for bigger outcomes.</h2><p class="muted">Each playbook launches the first guided request now and preserves the full multi-step plan for the user.</p></div>
          </div>
          <section class="grid two">
            @for (playbook of playbooks; track playbook.name) {
              <article class="card">
                <strong>{{ playbook.name }}</strong>
                <small>{{ playbook.description }}</small>
                <div class="pill-row">@for (key of playbook.toolKeys; track key) { <span class="pill">{{ toolName(key) }}</span> }</div>
                <button type="button" class="primary" (click)="startPlaybook(playbook)">Start playbook</button>
              </article>
            }
          </section>
          <div class="section-head"><div><p class="eyebrow">Saved playbooks</p><h2>Template library</h2></div></div>
          <section class="grid two">
            @for (template of templates(); track template.id) {
              <article class="card">
                <strong>{{ template.name }}</strong>
                <small>{{ template.description || template.promptTemplate || 'Reusable MADai workflow template.' }}</small>
                <div class="pill-row"><span class="pill">{{ displayEnum(template.category) }}</span><span class="pill">{{ displayEnum(template.defaultPriority || 'Normal') }}</span></div>
                <button type="button" (click)="useTemplate(template)">Use template</button>
              </article>
            } @empty {
              <div class="empty">No saved templates yet. Use starter playbooks while your library grows.</div>
            }
          </section>
        </ng-template>

        <ng-template #tasksSection>
          <div class="section-head"><div><p class="eyebrow">My Tasks</p><h2>Track every MADai request from idea to output.</h2></div><button type="button" (click)="refresh()">Refresh</button></div>
          <section class="tasks">
            @for (task of tasks(); track task.id) {
              <article class="task-row">
                <div>
                  <div class="pill-row"><span class="pill">{{ displayEnum(task.category) }}</span><span class="pill" [class.success]="isDone(task)" [class.warning]="isActive(task)">{{ displayEnum(task.status) }}</span></div>
                  <h3>{{ task.title }}</h3>
                  <p class="muted">Created {{ task.createdDate | date:'medium' }}</p>
                  <div class="progress"><span [style.width.%]="task.progress || (isDone(task) ? 100 : 8)"></span></div>
                </div>
                <div class="button-row"><button type="button" (click)="viewTask(task.id)">Details</button><button type="button" (click)="retryTask(task.id)">Retry</button><button type="button" class="danger" (click)="cancelTask(task.id)">Cancel</button></div>
              </article>
            } @empty {
              <div class="empty">No requests yet. Launch an AI tool to create your first MADai task.</div>
            }
          </section>
        </ng-template>

        <ng-template #outputsSection>
          <div class="section-head"><div><p class="eyebrow">Outputs</p><h2>Generated work, evidence, and downloadable artifacts.</h2></div></div>
          <section class="grid two">
            @for (output of outputs(); track output.taskId) {
              <article class="output-card">
                <div class="pill-row"><span class="pill success">{{ displayEnum(output.status) }}</span><span class="pill">{{ output.category }}</span></div>
                <h3>{{ output.title }}</h3>
                <pre>{{ output.summary || output.result || 'Output is stored as artifacts or task result data.' }}</pre>
                <div class="pill-row">@for (artifact of output.artifacts; track artifact.id) { <span class="pill">{{ artifact.fileName }}</span> }</div>
              </article>
            } @empty {
              <div class="empty">Completed AI outputs will appear here as soon as MADai finishes work.</div>
            }
          </section>
        </ng-template>

        <ng-template #librarySection>
          <div class="section-head"><div><p class="eyebrow">Library</p><h2>Files and reusable source material.</h2></div></div>
          <section class="panel">
            <label>Upload source material<input type="file" (change)="onFilePicked($event)"></label>
            <button type="button" class="primary" (click)="uploadFile()">Upload to library</button>
          </section>
          <section class="list" style="margin-top: 14px;">
            @for (file of files(); track file.id) {
              <article class="list-item"><strong>{{ file.name }}</strong><span class="muted">{{ file.contentType }} - {{ formatBytes(file.sizeBytes) }} - {{ file.createdDate | date:'medium' }}</span><a class="button" [href]="downloadUrl(file.id)" target="_blank">Download</a></article>
            } @empty {
              <div class="empty">Upload briefs, transcripts, CSVs, PDFs, or source files for MADai to use in future requests.</div>
            }
          </section>
        </ng-template>

        <ng-template #notificationsSection>
          <div class="section-head"><div><p class="eyebrow">Notifications</p><h2>Updates that need attention.</h2></div><button type="button" (click)="markAllRead()">Mark all read</button></div>
          <section class="list">
            @for (note of notifications(); track note.id) {
              <article class="list-item"><div class="pill-row"><span class="pill">{{ note.severity }}</span><span class="pill" [class.success]="note.readAt"> {{ note.readAt ? 'Read' : 'New' }} </span></div><strong>{{ note.title }}</strong><p class="muted">{{ note.body }}</p><div class="button-row"><button type="button" (click)="notificationAction(note.id, 'read')">Read</button><button type="button" (click)="notificationAction(note.id, 'dismiss')">Dismiss</button></div></article>
            } @empty {
              <div class="empty">No notifications right now.</div>
            }
          </section>
        </ng-template>

        <ng-template #billingSection>
          <div class="section-head"><div><p class="eyebrow">Billing</p><h2>Soft usage, simple upgrades, Payfast only.</h2><p class="muted">{{ usage()?.message || 'You can keep exploring MADai. Upgrade prompts appear when your team needs more capacity.' }}</p></div></div>
          <section class="grid">
            <article class="card"><strong>{{ usage()?.tasksThisMonth ?? 0 }} / {{ usage()?.includedTasks ?? 25 }}</strong><small>Included monthly AI runs in the current soft-launch plan.</small></article>
            <article class="card"><strong>{{ usage()?.completedThisMonth ?? 0 }}</strong><small>Completed outputs this month.</small></article>
            <article class="card"><strong>{{ usage()?.storageUsedMb ?? 0 }} MB</strong><small>Library and artifact storage currently tracked.</small></article>
          </section>
          <app-payfast-subscribe productName="MADai" headline="Upgrade MADai with Payfast" lead="Use Payfast checkout when your team is ready for more capacity. USD is the default; ZAR is used for South African visitors." [compact]="true"></app-payfast-subscribe>
        </ng-template>

        <ng-template #accountSection>
          <div class="section-head"><div><p class="eyebrow">Account</p><h2>Your profile and workspace access.</h2></div></div>
          <section class="grid two">
            <article class="card"><strong>{{ user()?.displayName || 'MADai user' }}</strong><small>{{ user()?.email }}</small><div class="pill-row">@for (role of user()?.roles || []; track role) { <span class="pill">{{ role }}</span> }</div></article>
            <article class="card"><strong>Workspace</strong><small>{{ user()?.companyName || 'Subscriber workspace' }}</small><button type="button" (click)="logout()">Sign out</button></article>
          </section>
        </ng-template>

        <ng-template #adminSection>
          <div class="section-head"><div><p class="eyebrow">Admin command center</p><h2>{{ activeLabel() }}</h2><p class="muted">Technical operations are restricted to admin users and kept away from the subscriber workspace.</p></div><button type="button" (click)="refreshAdmin()">Refresh</button></div>
          <section class="admin-grid">
            <aside class="panel">
              <h3>Admin tools</h3>
              <p class="muted">Use these areas for diagnostics, settings, worker health, audit findings, and integration administration.</p>
              @if (isSuperAdmin()) { <a routerLink="/ai" class="button primary" style="margin-top: 12px;">Open MADCloud /ai</a> }
            </aside>
            <div class="list">
              @for (block of adminBlocks(); track block.title) {
                <article class="panel">
                  <h3>{{ block.title }}</h3>
                  <p class="muted">{{ block.endpoint }}</p>
                  @if (block.error) {
                    <p class="status-line error">{{ block.error }}</p>
                  } @else if (rows(block.value).length) {
                    <div class="table-wrap"><table><thead><tr>@for (key of rowKeys(rows(block.value)[0]); track key) { <th>{{ key }}</th> }</tr></thead><tbody>@for (row of rows(block.value); track rowId(row, $index)) { <tr>@for (key of rowKeys(row); track key) { <td>{{ display(row[key]) }}</td> }</tr> }</tbody></table></div>
                  } @else {
                    <p class="muted">No records returned.</p>
                  }
                </article>
              }
            </div>
          </section>
        </ng-template>
      </main>
    </div>
  `
})
export class OperatorPage implements OnInit {
  private readonly api = inject(ApiService);
  private readonly rawUser = signal(localStorage.getItem('madai.user') || sessionStorage.getItem('madai.user'));

  readonly sections: SectionDef[] = [
    { key: 'home', label: 'Home' },
    { key: 'tools', label: 'AI Tools' },
    { key: 'studio', label: 'Studio' },
    { key: 'workflows', label: 'Workflows' },
    { key: 'tasks', label: 'My Tasks' },
    { key: 'outputs', label: 'Outputs' },
    { key: 'library', label: 'Library' },
    { key: 'notifications', label: 'Notifications' },
    { key: 'billing', label: 'Billing' },
    { key: 'account', label: 'Account' },
    { key: 'admin', label: 'Admin', adminOnly: true },
    { key: 'workers', label: 'Workers', adminOnly: true },
    { key: 'audit', label: 'Audit', adminOnly: true },
    { key: 'settings', label: 'Settings', adminOnly: true },
    { key: 'webhooks', label: 'Webhooks', adminOnly: true },
  ];

  readonly activeSection = signal<SectionKey>('home');
  readonly tools = signal<AiTool[]>([]);
  readonly tasks = signal<TaskSummary[]>([]);
  readonly outputs = signal<OutputItem[]>([]);
  readonly templates = signal<TemplateDto[]>([]);
  readonly files = signal<FileItem[]>([]);
  readonly notifications = signal<NotificationItem[]>([]);
  readonly usage = signal<UsageDto | null>(null);
  readonly adminBlocks = signal<DataBlock[]>([]);
  readonly saving = signal(false);
  readonly statusMessage = signal('');
  readonly statusError = signal(false);
  readonly user = computed<StoredUser | null>(() => {
    const raw = this.rawUser();
    if (!raw) return null;
    try { return JSON.parse(raw) as StoredUser; } catch { return null; }
  });
  readonly isSuperAdmin = computed(() => {
    const user = this.user();
    const evidence = [user?.email, user?.displayName, ...(user?.roles ?? [])].join(' ').toLowerCase();
    return evidence.includes('admin@madprospects.com') || evidence.includes('systemadmin') || evidence.includes('superadmin');
  });
  readonly visibleSections = computed(() => this.sections.filter(section => !section.adminOnly || this.isSuperAdmin()));
  readonly filteredTools = computed(() => this.toolGroup === 'All' ? this.tools() : this.tools().filter(tool => tool.group === this.toolGroup));
  readonly selectedTool = signal<AiTool | null>(null);
  readonly taskCount = computed(() => this.tasks().length);
  readonly completedCount = computed(() => this.tasks().filter(task => this.isDone(task)).length);

  readonly playbooks = STARTER_PLAYBOOKS;
  toolGroup = 'All';
  requestTitle = '';
  requestPrompt = '';
  requestContext = '';
  requestPriority = 50;
  selectedFile: File | null = null;

  ngOnInit(): void {
    this.loadSubscriberData();
  }

  open(section: SectionKey): void {
    if (this.sections.find(item => item.key === section)?.adminOnly && !this.isSuperAdmin()) {
      this.activeSection.set('home');
      return;
    }

    this.activeSection.set(section);
    if (this.isAdminSection(section)) {
      this.refreshAdmin();
    } else {
      this.loadSubscriberData();
    }
  }

  refresh(): void {
    this.isAdminSection(this.activeSection()) ? this.refreshAdmin() : this.loadSubscriberData();
  }

  loadSubscriberData(): void {
    forkJoin({
      tools: this.api.get<AiTool[]>('/ai-tools').pipe(catchError(() => of([]))),
      tasks: this.api.get<PagedResult<TaskSummary>>('/tasks?page=1&pageSize=80').pipe(catchError(() => of({ items: [] }))),
      outputs: this.api.get<OutputItem[]>('/outputs').pipe(catchError(() => of([]))),
      templates: this.api.get<TemplateDto[]>('/task-templates').pipe(catchError(() => of([]))),
      files: this.api.get<FileItem[]>('/files').pipe(catchError(() => of([]))),
      notifications: this.api.get<NotificationItem[]>('/notifications?take=40').pipe(catchError(() => of([]))),
      usage: this.api.get<UsageDto>('/me/usage').pipe(catchError(() => of(null))),
    }).subscribe(result => {
      this.tools.set(result.tools);
      this.tasks.set(result.tasks.items ?? []);
      this.outputs.set(result.outputs);
      this.templates.set(result.templates);
      this.files.set(result.files);
      this.notifications.set(result.notifications.filter(note => !note.dismissedAt));
      this.usage.set(result.usage);
      if (!this.selectedTool() && result.tools.length) {
        this.selectedTool.set(result.tools[0]);
      }
    });
  }

  selectTool(tool: AiTool): void {
    this.selectedTool.set(tool);
    this.activeSection.set('studio');
    this.requestTitle = this.requestTitle || tool.examples[0] || tool.name;
  }

  launchRequest(): void {
    const tool = this.selectedTool() ?? this.tools()[0];
    if (!tool || !this.requestPrompt.trim() || this.saving()) return;
    this.saving.set(true);
    this.statusMessage.set('Launching your MADai AI request...');
    const payload = {
      toolKey: tool.key,
      title: this.requestTitle || tool.name,
      prompt: this.requestPrompt,
      inputs: { context: this.requestContext, sourceFile: this.selectedFile?.name ?? null },
      attachments: this.selectedFile ? [{ fileName: this.selectedFile.name, sizeBytes: this.selectedFile.size, contentType: this.selectedFile.type }] : [],
      priority: this.requestPriority,
    };
    this.api.post<{ taskId: string; providerMessage: string }>('/ai-requests', payload).subscribe({
      next: response => {
        this.saving.set(false);
        this.statusError.set(false);
        this.statusMessage.set(response.providerMessage || 'Your request is now tracked in My Tasks.');
        this.requestPrompt = '';
        this.requestContext = '';
        this.loadSubscriberData();
        this.activeSection.set('tasks');
      },
      error: error => this.fail(error),
    });
  }

  startPlaybook(playbook: typeof STARTER_PLAYBOOKS[number]): void {
    const firstTool = this.tools().find(tool => tool.key === playbook.toolKeys[0]) ?? this.tools()[0];
    if (firstTool) this.selectedTool.set(firstTool);
    this.requestTitle = playbook.name;
    this.requestPrompt = `${playbook.description}\n\nCreate the first task and include the full plan for: ${playbook.toolKeys.map(key => this.toolName(key)).join(' -> ')}.`;
    this.requestContext = 'Workflow Studio starter playbook';
    this.activeSection.set('studio');
  }

  useTemplate(template: TemplateDto): void {
    const tool = this.tools().find(item => String(item.taskCategory) === String(template.category)) ?? this.tools()[0];
    if (tool) this.selectedTool.set(tool);
    this.requestTitle = template.name;
    this.requestPrompt = template.promptTemplate || template.description || `Run the ${template.name} workflow.`;
    this.requestContext = template.defaultInputJson || 'Saved playbook template';
    this.activeSection.set('studio');
  }

  loadExample(): void {
    const tool = this.selectedTool();
    this.requestTitle = tool?.examples[0] || 'MADai request';
    this.requestPrompt = `Create a polished ${tool?.name || 'AI'} output for a MADProspects subscriber. Include clear sections, practical next steps, and a concise summary.`;
    this.requestContext = tool?.fields.join(', ') || '';
  }

  viewTask(id: string): void {
    this.api.get<unknown>(`/tasks/${id}`).subscribe({
      next: value => {
        const task = value as { title?: string; outputSummary?: string; resultPayload?: string };
        this.statusError.set(false);
        this.statusMessage.set(`${task.title || 'Task'} loaded. ${task.outputSummary || task.resultPayload || 'Details are available in the task record.'}`);
      },
      error: error => this.fail(error),
    });
  }

  retryTask(id: string): void {
    this.api.post(`/tasks/${id}/retry`).subscribe({ next: () => this.afterAction('Task retry requested.'), error: error => this.fail(error) });
  }

  cancelTask(id: string): void {
    this.api.post(`/tasks/${id}/cancel`, { reason: 'Cancelled by subscriber.' }).subscribe({ next: () => this.afterAction('Task cancelled.'), error: error => this.fail(error) });
  }

  onFilePicked(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input.files?.[0] ?? null;
  }

  uploadFile(): void {
    if (!this.selectedFile) return;
    const form = new FormData();
    form.append('file', this.selectedFile);
    this.api.upload('/files/upload', form).subscribe({ next: () => this.afterAction('File uploaded to your library.'), error: error => this.fail(error) });
  }

  notificationAction(id: string, action: 'read' | 'dismiss'): void {
    this.api.post(`/notifications/${id}/${action}`).subscribe({ next: () => this.afterAction('Notification updated.'), error: error => this.fail(error) });
  }

  markAllRead(): void {
    this.api.post('/notifications/read-all').subscribe({ next: () => this.afterAction('All notifications marked read.'), error: error => this.fail(error) });
  }

  refreshAdmin(): void {
    if (!this.isSuperAdmin()) return;
    const endpoints = this.adminEndpoints(this.activeSection());
    forkJoin(endpoints.map(([, endpoint]) => this.api.get(endpoint).pipe(catchError(error => of({ __error: error?.message ?? 'Request failed' }))))).subscribe(values => {
      this.adminBlocks.set(values.map((value, index) => {
        const [title, endpoint] = endpoints[index];
        const error = this.isErrorValue(value) ? String((value as { __error: string }).__error) : undefined;
        return { title, endpoint, value: error ? [] : value, error };
      }));
    });
  }

  activeLabel(): string {
    return this.sections.find(section => section.key === this.activeSection())?.label ?? 'MADai';
  }

  heroTitle(): string {
    const map: Record<string, string> = {
      home: 'Your AI workbench for every kind of business task.',
      tools: 'Choose what you want done. MADai handles the workflow.',
      studio: 'Create, brief, attach, and launch in one focused studio.',
      workflows: 'Turn bigger goals into repeatable AI playbooks.',
      tasks: 'Every request is tracked from launch to result.',
      outputs: 'Your generated work lives here.',
      library: 'Keep source material ready for future AI work.',
      billing: 'Start softly. Upgrade when the value is obvious.',
    };
    return map[this.activeSection()] ?? 'Admin tools stay separate from subscriber work.';
  }

  heroCopy(): string {
    return this.isAdminSection(this.activeSection())
      ? 'Technical diagnostics are visible only to admins. Subscribers see a clean MADai workspace.'
      : 'MADai gives subscribers guided AI tools, creative workflows, reusable playbooks, trackable history, and outputs they can actually use.';
  }

  toolName(key: string): string {
    return this.tools().find(tool => tool.key === key)?.name ?? key;
  }

  displayEnum(value: unknown): string {
    return String(value ?? '').replace(/([a-z])([A-Z])/g, '$1 $2');
  }

  isDone(task: TaskSummary): boolean {
    return String(task.status).toLowerCase() === 'completed' || Number(task.status) === 50;
  }

  isActive(task: TaskSummary): boolean {
    return ['queued', 'scheduled', 'claimed', 'running', 'awaitingvalidation'].includes(String(task.status).toLowerCase()) || [10, 15, 20, 30, 40].includes(Number(task.status));
  }

  formatBytes(value: number): string {
    if (!value) return '0 KB';
    if (value < 1024 * 1024) return `${Math.ceil(value / 1024)} KB`;
    return `${(value / 1024 / 1024).toFixed(1)} MB`;
  }

  downloadUrl(id: string): string {
    return this.api.absolute(`/files/${id}/download`);
  }

  rows(value: unknown): Array<Record<string, unknown>> {
    if (Array.isArray(value)) return value.filter(item => item && typeof item === 'object') as Array<Record<string, unknown>>;
    if (value && typeof value === 'object') {
      const obj = value as PagedResult<unknown> & Record<string, unknown>;
      const rows = obj.items ?? obj['data'] ?? [];
      return Array.isArray(rows) ? rows.filter(item => item && typeof item === 'object') as Array<Record<string, unknown>> : [];
    }
    return [];
  }

  rowKeys(row: Record<string, unknown>): string[] {
    return Object.keys(row).slice(0, 8);
  }

  rowId(row: Record<string, unknown>, index: number): string {
    return String(row['id'] ?? row['key'] ?? row['name'] ?? index);
  }

  display(value: unknown): string {
    if (value == null) return '';
    if (typeof value === 'object') return JSON.stringify(value);
    return String(value);
  }

  logout(): void {
    localStorage.removeItem('madai.accessToken');
    localStorage.removeItem('madai.refreshToken');
    localStorage.removeItem('madai.user');
    sessionStorage.removeItem('madai.accessToken');
    sessionStorage.removeItem('madai.refreshToken');
    sessionStorage.removeItem('madai.user');
    window.location.assign('/login');
  }

  private adminEndpoints(section: SectionKey): Array<[string, string]> {
    const map: Partial<Record<SectionKey, Array<[string, string]>>> = {
      admin: [['Users', '/admin/users'], ['Companies', '/admin/companies'], ['Plans', '/admin/plans'], ['Feature flags', '/admin/feature-flags']],
      workers: [['Worker fleet', '/workers'], ['Native processes', '/persistent-workers/native-processes']],
      audit: [['Findings', '/audit/findings'], ['Runs', '/audit/runs'], ['Recommendations', '/audit/recommendations']],
      settings: [['Settings', '/settings']],
      webhooks: [['Webhook endpoints', '/webhooks']],
    };
    return map[section] ?? map.admin!;
  }

  private isAdminSection(section: SectionKey): boolean {
    return ['admin', 'workers', 'audit', 'settings', 'webhooks'].includes(section);
  }

  private afterAction(message: string): void {
    this.saving.set(false);
    this.statusError.set(false);
    this.statusMessage.set(message);
    this.loadSubscriberData();
  }

  private fail(error: unknown): void {
    this.saving.set(false);
    this.statusError.set(true);
    this.statusMessage.set(error instanceof Error ? error.message : 'Action failed.');
  }

  private isErrorValue(value: unknown): boolean {
    return Boolean(value && typeof value === 'object' && '__error' in value);
  }
}
