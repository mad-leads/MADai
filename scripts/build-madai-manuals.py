from __future__ import annotations

from dataclasses import dataclass
from pathlib import Path
from typing import Iterable, Sequence

from reportlab.lib import colors
from reportlab.lib.enums import TA_CENTER, TA_LEFT
from reportlab.lib.pagesizes import letter
from reportlab.lib.styles import ParagraphStyle, getSampleStyleSheet
from reportlab.lib.units import inch
from reportlab.platypus import (
    BaseDocTemplate,
    Frame,
    KeepTogether,
    ListFlowable,
    ListItem,
    PageBreak,
    PageTemplate,
    Paragraph,
    Spacer,
    Table,
    TableStyle,
)


ROOT = Path(__file__).resolve().parents[1]


ACCENT = colors.HexColor("#14B8A6")
DARK = colors.HexColor("#07110F")
INK = colors.HexColor("#0F172A")
MUTED = colors.HexColor("#475569")
SOFT = colors.HexColor("#E8F5F2")
LINE = colors.HexColor("#CBD5E1")
LIGHT = colors.HexColor("#F8FAFC")


def stylesheet():
    base = getSampleStyleSheet()
    styles = {
        "Title": ParagraphStyle(
            "ManualTitle",
            parent=base["Title"],
            fontName="Helvetica-Bold",
            fontSize=28,
            leading=34,
            alignment=TA_CENTER,
            textColor=DARK,
            spaceAfter=14,
        ),
        "Subtitle": ParagraphStyle(
            "ManualSubtitle",
            parent=base["Normal"],
            fontName="Helvetica",
            fontSize=12,
            leading=17,
            alignment=TA_CENTER,
            textColor=MUTED,
            spaceAfter=18,
        ),
        "H1": ParagraphStyle(
            "H1",
            parent=base["Heading1"],
            fontName="Helvetica-Bold",
            fontSize=17,
            leading=21,
            textColor=DARK,
            spaceBefore=16,
            spaceAfter=8,
        ),
        "H2": ParagraphStyle(
            "H2",
            parent=base["Heading2"],
            fontName="Helvetica-Bold",
            fontSize=12.8,
            leading=16,
            textColor=colors.HexColor("#0F766E"),
            spaceBefore=12,
            spaceAfter=6,
        ),
        "H3": ParagraphStyle(
            "H3",
            parent=base["Heading3"],
            fontName="Helvetica-Bold",
            fontSize=11.2,
            leading=14,
            textColor=INK,
            spaceBefore=8,
            spaceAfter=4,
        ),
        "Body": ParagraphStyle(
            "Body",
            parent=base["BodyText"],
            fontName="Helvetica",
            fontSize=9.7,
            leading=14,
            textColor=INK,
            spaceAfter=7,
        ),
        "Small": ParagraphStyle(
            "Small",
            parent=base["BodyText"],
            fontName="Helvetica",
            fontSize=8.4,
            leading=11,
            textColor=MUTED,
            spaceAfter=5,
        ),
        "TableHead": ParagraphStyle(
            "TableHead",
            parent=base["BodyText"],
            fontName="Helvetica-Bold",
            fontSize=8.2,
            leading=10,
            textColor=colors.white,
        ),
        "TableBody": ParagraphStyle(
            "TableBody",
            parent=base["BodyText"],
            fontName="Helvetica",
            fontSize=8,
            leading=10.2,
            textColor=INK,
        ),
        "Callout": ParagraphStyle(
            "Callout",
            parent=base["BodyText"],
            fontName="Helvetica",
            fontSize=9.4,
            leading=13,
            textColor=INK,
            leftIndent=8,
            rightIndent=8,
            spaceBefore=4,
            spaceAfter=4,
        ),
    }
    return styles


STYLES = stylesheet()


class ManualDocTemplate(BaseDocTemplate):
    def __init__(self, filename: str, title: str):
        self.title_text = title
        super().__init__(
            filename,
            pagesize=letter,
            rightMargin=0.72 * inch,
            leftMargin=0.72 * inch,
            topMargin=0.72 * inch,
            bottomMargin=0.64 * inch,
            title=title,
            author="MADProspects",
        )
        frame = Frame(
            self.leftMargin,
            self.bottomMargin,
            self.width,
            self.height,
            id="normal",
        )
        self.addPageTemplates(
            [
                PageTemplate(id="main", frames=[frame], onPage=self._page),
            ]
        )

    def _page(self, canvas, doc):
        canvas.saveState()
        page = doc.page
        canvas.setStrokeColor(LINE)
        canvas.setLineWidth(0.5)
        canvas.line(doc.leftMargin, 0.48 * inch, letter[0] - doc.rightMargin, 0.48 * inch)
        canvas.setFont("Helvetica", 7.8)
        canvas.setFillColor(MUTED)
        canvas.drawString(doc.leftMargin, 0.32 * inch, self.title_text)
        canvas.drawRightString(letter[0] - doc.rightMargin, 0.32 * inch, f"Page {page}")
        canvas.restoreState()


def p(text: str, style: str = "Body"):
    return Paragraph(text, STYLES[style])


def h1(text: str):
    return Paragraph(text, STYLES["H1"])


def h2(text: str):
    return Paragraph(text, STYLES["H2"])


def h3(text: str):
    return Paragraph(text, STYLES["H3"])


def bullet(items: Sequence[str], level: int = 0):
    return ListFlowable(
        [ListItem(p(item), leftIndent=level * 10) for item in items],
        bulletType="bullet",
        start="circle",
        leftIndent=18 + level * 10,
        bulletFontName="Helvetica",
        bulletFontSize=6.5,
        bulletOffsetY=1,
    )


def numbered(items: Sequence[str]):
    return ListFlowable(
        [ListItem(p(item), leftIndent=0) for item in items],
        bulletType="1",
        leftIndent=18,
    )


def callout(title: str, text: str):
    data = [[p(f"<b>{title}</b><br/>{text}", "Callout")]]
    table = Table(data, colWidths=[6.42 * inch])
    table.setStyle(
        TableStyle(
            [
                ("BACKGROUND", (0, 0), (-1, -1), SOFT),
                ("BOX", (0, 0), (-1, -1), 0.6, ACCENT),
                ("LEFTPADDING", (0, 0), (-1, -1), 8),
                ("RIGHTPADDING", (0, 0), (-1, -1), 8),
                ("TOPPADDING", (0, 0), (-1, -1), 7),
                ("BOTTOMPADDING", (0, 0), (-1, -1), 7),
            ]
        )
    )
    return KeepTogether([table, Spacer(1, 8)])


def table(headers: Sequence[str], rows: Sequence[Sequence[str]], widths: Sequence[float]):
    data = [[p(h, "TableHead") for h in headers]]
    data.extend([[p(str(cell), "TableBody") for cell in row] for row in rows])
    tbl = Table(data, colWidths=[w * inch for w in widths], repeatRows=1)
    tbl.setStyle(
        TableStyle(
            [
                ("BACKGROUND", (0, 0), (-1, 0), DARK),
                ("TEXTCOLOR", (0, 0), (-1, 0), colors.white),
                ("GRID", (0, 0), (-1, -1), 0.35, LINE),
                ("VALIGN", (0, 0), (-1, -1), "TOP"),
                ("LEFTPADDING", (0, 0), (-1, -1), 5),
                ("RIGHTPADDING", (0, 0), (-1, -1), 5),
                ("TOPPADDING", (0, 0), (-1, -1), 5),
                ("BOTTOMPADDING", (0, 0), (-1, -1), 5),
                ("BACKGROUND", (0, 1), (-1, -1), colors.white),
                ("ROWBACKGROUNDS", (0, 1), (-1, -1), [colors.white, LIGHT]),
            ]
        )
    )
    return tbl


def cover(title: str, subtitle: str, version: str):
    return [
        Spacer(1, 1.0 * inch),
        Paragraph("MADai", STYLES["Title"]),
        Paragraph(title, STYLES["Title"]),
        Paragraph(subtitle, STYLES["Subtitle"]),
        Spacer(1, 0.18 * inch),
        table(
            ["Document", "Scope", "Version"],
            [[title, "MADai application, API, workers, operations, and MADProspects ecosystem", version]],
            [1.55, 3.65, 1.05],
        ),
        Spacer(1, 0.35 * inch),
        callout(
            "Important security note",
            "This document intentionally does not print any production password, API key, SMTP secret, database credential, or FTP credential. Operational users should obtain credentials through approved MADProspects channels.",
        ),
        PageBreak(),
    ]


def build_user_manual(path: Path):
    story = []
    story += cover(
        "User Manual",
        "A detailed guide to what the autonomous AI operations platform does, how the pieces fit together, and how operators use it.",
        "Prepared 2026-05-26",
    )

    story += [
        h1("1. Executive Overview"),
        p("MADai is an autonomous AI operations platform for the MADProspects ecosystem. Its core purpose is to accept AI work requests, store and manage those requests as trackable tasks, assign them to registered worker processes, collect progress and artifacts, and provide operators with real-time visibility into the health of the system."),
        p("The application is built around a .NET 8 API, SQL Server persistence, Hangfire-ready background processing, SignalR real-time updates, and an Angular frontend. It also includes a separate Claude operator queue used by SystemAdmin users and scheduled Claude/Codex workers to improve the application itself."),
        callout(
            "Current UI status",
            "The current deployed web frontend is a restored lightweight shell with a home page, login page, and operator console at /app/claude. The backend API and domain model contain a broader operations platform: task queues, workers, dashboards, admin, audit, files, notifications, webhooks, settings, templates, and Claude tasks.",
        ),
        h2("1.1 Main Outcomes"),
        bullet(
            [
                "Turn ad hoc AI requests into managed, auditable work items.",
                "Run work through registered desktop or server workers using secure worker API keys.",
                "Track task state from draft, queued, claimed, running, completed, failed, retried, cancelled, and dead-lettered.",
                "Expose real-time task, worker, notification, and dashboard events through SignalR hubs.",
                "Support SystemAdmin operations, role-based permissions, feature flags, settings, and company administration.",
                "Provide a Claude/Codex operator system for application self-improvement work.",
            ]
        ),
        h2("1.2 Application Surfaces"),
        table(
            ["Surface", "What it is for", "Primary users"],
            [
                ["Frontend", "Browser entry point for login and operator workflows. The current shell verifies authentication and SystemAdmin access to /app/claude.", "Operators, SystemAdmins"],
                ["REST API", "Main application contract for authentication, tasks, workers, dashboards, audit, files, notifications, admin, settings, webhooks, and Claude tasks.", "Frontend, automation, integrations"],
                ["SignalR hubs", "Pushes live task, worker, notification, and dashboard events to connected clients.", "Frontend dashboards and real-time clients"],
                ["Worker API", "Allows registered workers to heartbeat, claim tasks, report progress, log output, complete work, or fail work.", "MADai workers and trusted automation"],
                ["Claude operator queue", "Separate SystemAdmin queue for repository and product-improvement tasks executed by Claude/Codex schedulers.", "SystemAdmins, scheduled AI workers"],
                ["Deployment scripts", "Publish API and frontend to 1-grid/Plesk using FTP and environment injection.", "Developers, operators"],
            ],
            [1.2, 3.1, 1.7],
        ),
        PageBreak(),
    ]

    story += [
        h1("2. Users, Roles, and Access"),
        p("MADai uses ASP.NET Identity for users and roles. Human users authenticate with email and password, receive JWT access tokens and refresh tokens, and access endpoints according to their roles and permissions. Workers authenticate separately using X-API-Key so automated execution does not depend on a human JWT."),
        h2("2.1 Seeded Roles"),
        table(
            ["Role", "Typical purpose"],
            [
                ["SystemAdmin", "Full platform administration, Claude task management, system settings, feature flags, plans, companies, and operational control."],
                ["CompanyAdmin", "Company-level administration for tenant users, company settings, and business operations."],
                ["CompanyManager", "Operational management for company users and task workflows."],
                ["Worker", "Worker process identity and queue execution role."],
                ["User", "Standard authenticated user role."],
                ["Client", "External or customer-facing user role for limited interaction."],
                ["ReadOnly", "View-focused role for auditing, review, or support."],
            ],
            [1.45, 4.8],
        ),
        h2("2.2 Permission Areas"),
        table(
            ["Area", "Permissions represented in the platform"],
            [
                ["Tasks", "View, create, update, delete, cancel, retry, and assign tasks."],
                ["Workers", "View, manage, and drain worker nodes."],
                ["Companies", "View and manage tenant companies."],
                ["Users", "View and manage platform users."],
                ["Audit", "View and manage audit findings."],
                ["System", "System management, feature flags, and settings."],
            ],
            [1.2, 5.05],
        ),
        h2("2.3 Login Flow"),
        numbered(
            [
                "The user opens the frontend and signs in with an email address and password.",
                "The API validates the user, checks account status, applies lockout rules, and verifies the password.",
                "If MFA is enrolled, the API requires a valid authenticator code.",
                "The API records login history, updates last-login metadata, and issues an access token plus refresh token.",
                "The frontend stores the session locally and uses the access token on later API calls.",
                "On logout, the refresh token is revoked so it cannot be reused.",
            ]
        ),
        h2("2.4 MFA and Password Management"),
        p("MADai includes API operations for starting MFA enrollment, verifying MFA setup, disabling MFA, requesting a password reset, resetting a password, changing a password, and updating profile fields. Password reset emails are generated by the application service and delivered through the configured email sender."),
        h2("2.5 Superadmin Account"),
        p("The deployment is configured so DEV and PROD have a SystemAdmin account using the approved MADProspects administrator email. The seed process creates the account if it does not exist, confirms the email, ensures it is active, attaches it to the demo company, grants the SystemAdmin role, and resets the password when it differs from the canonical seed. Password values are intentionally excluded from this manual."),
    ]

    story += [
        h1("3. Task Management"),
        p("The task system is the operational center of MADai. A task stores the title, description, category, priority, status, queue name, scheduling information, prompt payload, input payload, output summary, result payload, error details, progress, retry counts, dependencies, comments, logs, artifacts, executions, assignments, and tags."),
        h2("3.1 Task Categories"),
        table(
            ["Category group", "Supported categories"],
            [
                ["Software and delivery", "SoftwareGeneration, AngularGeneration, DotNetGeneration, SqlGeneration, Debugging, Testing, Deployment, Documentation, IntegrationBuilding"],
                ["Creative and content", "BlogWriting, BookGeneration, ImageGeneration, VideoGeneration, SocialContent, UxUi"],
                ["Operations and intelligence", "Research, Automation, Scraping, Analysis, Transcription, Reporting, AiOrchestration"],
                ["Platform recovery", "SelfHealing"],
            ],
            [1.55, 4.7],
        ),
        h2("3.2 Task Status Lifecycle"),
        table(
            ["Status", "Meaning"],
            [
                ["Draft", "A task exists but is not yet queued for execution."],
                ["Queued", "Ready for a worker to claim."],
                ["Scheduled", "Queued for future execution based on ScheduledAt."],
                ["Claimed", "Reserved by a worker with a claim token."],
                ["Running", "A worker has started and is reporting progress."],
                ["Paused", "Temporarily stopped."],
                ["AwaitingValidation", "Completed by a worker but not yet accepted as final."],
                ["Completed", "Finished successfully."],
                ["Failed", "Failed without being dead-lettered."],
                ["Cancelled", "Cancelled by a user or operator."],
                ["TimedOut", "Exceeded allowed execution time."],
                ["DeadLetter", "Exceeded retry policy or considered terminal."],
                ["Recovered", "Recovered by audit or operator intervention."],
            ],
            [1.45, 4.8],
        ),
        h2("3.3 Priority and Queueing"),
        p("Tasks are prioritized using Lowest, Low, Normal, High, and Critical. Workers claim eligible queued or due scheduled tasks ordered by priority and creation date. Queue names allow different kinds of work to be routed to different workers. A worker can default to its configured queue or claim from a requested set of queues."),
        h2("3.4 Dependencies"),
        p("Tasks may depend on other tasks. Before a worker can claim a task, MADai checks whether hard dependencies have completed. This supports multi-step pipelines where research, drafting, validation, deployment, and reporting can be represented as separate but linked work items."),
        h2("3.5 Retries and Dead Letters"),
        p("When a worker reports a transient failure, the platform can requeue the task with exponential backoff. The current retry delay starts from seconds and grows up to a capped delay. Once retries are exhausted, the task can be marked as dead-lettered, preserving the failure reason and making it visible for operator action."),
        h2("3.6 Comments, Logs, Artifacts, and Templates"),
        bullet(
            [
                "Task comments allow collaboration or operator notes on a task.",
                "Task logs capture worker-provided log entries and execution detail.",
                "Task artifacts store files or outputs produced by work.",
                "Task templates allow common task patterns to be reused.",
                "Task recommendations can be generated, applied, or dismissed as operational suggestions.",
            ]
        ),
    ]

    story += [
        PageBreak(),
        h1("4. Worker Operations"),
        p("Workers are execution agents. They are registered separately from humans, receive an API key, heartbeat to the API, claim work, report progress, send logs, complete tasks, or fail tasks. Worker API keys are hashed in storage, and workers use the X-API-Key header for subsequent calls."),
        h2("4.1 Worker Registration and Identity"),
        numbered(
            [
                "A worker registers with metadata such as name, machine name, operating system, queue, capability labels, workspace root, and maximum concurrency.",
                "The API returns a worker ID and one-time API key.",
                "The worker stores the key and uses it for heartbeat and task execution calls.",
                "MADai stores a hash of the key and later authenticates the worker through the Worker authentication scheme.",
            ]
        ),
        h2("4.2 Heartbeats and Metrics"),
        p("Heartbeats update LastHeartbeatAt, worker status, active task count, and optional CPU, memory, disk, and notes. This feeds worker health views and real-time worker update events."),
        h2("4.3 Claim, Progress, Complete, Fail"),
        table(
            ["Step", "What happens"],
            [
                ["Claim", "Worker requests up to MaxTasks. MADai filters by queue, company, schedule, retry timing, dead-letter status, dependency completion, priority, and worker capacity."],
                ["Progress", "Worker reports progress from 0 to 100 and may attach status text or checkpoint JSON."],
                ["Log", "Worker sends execution log entries with timestamp, level, message, and source."],
                ["Complete", "MADai marks the task Completed or AwaitingValidation, stores output summary, result payload, validation report, and frees worker concurrency."],
                ["Fail", "MADai records failure details, decides retry vs terminal failure, updates retry timing, and frees worker concurrency."],
            ],
            [1.1, 5.15],
        ),
        h2("4.4 Worker Statuses"),
        p("Workers can be Offline, Starting, Idle, Busy, Draining, Errored, or Maintenance. Admin endpoints allow operators to drain or deactivate workers."),
    ]

    story += [
        h1("5. Claude Operator Queue"),
        p("MADai has a separate Claude task system for SystemAdmin application-improvement work. This is intentionally different from the end-user TaskItem queue. ClaudeTask records represent developer or operator work items for the repository itself, such as fixes, refactors, setup work, deployment tasks, or scanner findings."),
        h2("5.1 Claude Task Capabilities"),
        bullet(
            [
                "List, view, create, update, delete, and bulk-import Claude tasks.",
                "Fetch the next task for a worker or scheduled automation.",
                "Attach files to Claude tasks and delete attachments.",
                "Use prompt templates for repeatable operator instructions.",
                "Control state transitions through a state-machine gate.",
                "Receive real-time task update events through the SignalR infrastructure.",
            ]
        ),
        h2("5.2 Statuses and Priorities"),
        p("Claude tasks support statuses such as Pending, InProgress, ToBeDeployed, Completed, Failed, Cancelled, and Deferred. Priorities allow operators to order work and control attention."),
        h2("5.3 Scheduled Worker and Scanner"),
        p("The README describes a scheduled worker that polls for Claude work and a scanner that imports findings. The worker uses adaptive backoff so a busy queue is handled quickly while an idle queue produces very little API traffic. Scanner findings can be imported in bulk into the Claude queue for review or execution."),
        h2("5.4 Current Operator Console"),
        p("The current deployed UI at /app/claude confirms the authenticated SystemAdmin session and gives access to the operator surface. It is a lightweight restored shell and should be expanded into the richer Claude task board described by the API and README."),
    ]

    story += [
        PageBreak(),
        h1("6. Dashboards, Audit, Notifications, and Files"),
        h2("6.1 Dashboard Metrics"),
        p("The dashboard API computes total tasks, queued tasks, running tasks, completed-today count, failed-today count, dead-letter count, active workers, idle workers, offline workers, average duration, and success rate. Queue health groups work by queue name, and trend endpoints expose failure and completion trends."),
        h2("6.2 Audit and Self-Healing"),
        p("The audit domain stores audit runs, findings, recommendations, cleanup tasks, and optimization suggestions. The audit worker described by the README scans repeat failures, dead-letter backlog, stuck running tasks, orphaned artifacts, and queue backlog. High or critical findings can create self-healing tasks, closing the loop from detection to remediation."),
        h2("6.3 Notifications"),
        p("The notifications API lists notifications, marks individual notifications as read, marks all as read, dismisses notifications, sends notifications, and manages notification preferences. A SignalR notifications hub supports user-targeted real-time delivery."),
        h2("6.4 File Handling"),
        p("The files API supports upload, listing, and download. The domain contains file items, folders, versions, and access logs, allowing the application to evolve toward auditable file storage for task inputs, outputs, and attachments."),
        h2("6.5 Webhooks"),
        p("Webhook endpoints can be listed, created, updated, deleted, and secret-rotated. A background delivery worker polls pending events every 10 seconds, posts JSON payloads to active endpoints, signs each request with HMAC-SHA256, retries failures with exponential backoff, and marks delivery success or failure."),
    ]

    story += [
        h1("7. Administration and Tenant Management"),
        h2("7.1 Admin API"),
        p("System administration endpoints cover users, feature flags, plans, companies, and roles. Admins can list users, get a user, create users, update users, reset passwords, delete users, manage feature flags, manage plans, manage companies, and list roles."),
        h2("7.2 Companies and Tenancy"),
        p("Companies have names, slugs, legal names, contact fields, country, timezone, active status, plan relationships, branding, settings, and users. Task and dashboard queries can scope results by the current user's company where applicable."),
        h2("7.3 Plans and Billing Domain"),
        p("The domain model includes plans, company plans, subscriptions, invoices, payments, credit ledgers, and usage records. This gives MADai a foundation for commercial subscription tiers, credit accounting, usage limits, and invoices, even where the current UI does not yet expose the full billing experience."),
        h2("7.4 System Settings and Feature Flags"),
        p("Settings provide a key/value store used by operator controls such as worker active flags and deployment timing. Feature flags are managed through the admin API and can support staged rollouts, beta features, tenant-specific enablement, or kill-switches."),
    ]

    story += [
        PageBreak(),
        h1("8. API Reference by Capability"),
        table(
            ["Capability", "Representative endpoints"],
            [
                ["Auth", "POST /api/v1/auth/login, refresh, logout, register, forgot-password, reset-password, MFA start/verify/disable, change-password, profile"],
                ["Tasks", "GET/POST/PUT /api/v1/tasks, cancel, retry, import-bulk"],
                ["Task collaboration", "Comments under /api/v1/tasks/{taskId}/comments; recommendations under /api/v1/task-recommendations"],
                ["Workers", "POST /api/v1/worker/register, heartbeat, claim, progress, log, complete, fail; admin list/drain/deactivate"],
                ["Dashboard", "Overview, queues, workers, failure trends, completion trends"],
                ["Claude tasks", "List, next, import-bulk, get, create, update, delete, attachment upload/delete"],
                ["Prompt templates", "List, create, update, delete Claude prompt templates"],
                ["Audit", "Findings, runs, recommendations"],
                ["Files", "Upload, list, download"],
                ["Notifications", "List, read, read-all, dismiss, send, preferences"],
                ["Admin", "Users, feature flags, plans, companies, roles"],
                ["Webhooks", "List, create, update, delete, rotate secret"],
                ["Persistent workers", "Repository intelligence, task injection, session rotation, native processes"],
                ["System", "Ping, health, readiness, Swagger in development"],
            ],
            [1.55, 4.7],
        ),
        h2("8.1 SignalR Hubs"),
        table(
            ["Hub", "Purpose"],
            [
                ["/hubs/tasks", "Task lifecycle and Claude task update events."],
                ["/hubs/workers", "Worker heartbeat and status changes."],
                ["/hubs/notifications", "User-targeted notifications."],
                ["/hubs/dashboard", "Aggregate dashboard refresh signals."],
            ],
            [1.6, 4.65],
        ),
    ]

    story += [
        h1("9. Operational Runbook"),
        h2("9.1 DEV Startup"),
        p("Local development uses the API on http://localhost:3011 and the frontend on http://localhost:4211. The API can use LocalDB for development. The frontend is an Angular standalone app. pnpm is configured to use the shared store at C:/Code/.pnpm."),
        h2("9.2 Production Deployment"),
        p("The deploy scripts publish the .NET API, inject environment variables into web.config, and upload API and frontend files by FTP to the Plesk/1-grid host. The current production hosts are https://madaiapi.madprospects.com and https://madai.madprospects.com."),
        h2("9.3 Health Checks"),
        bullet(
            [
                "GET /health confirms application health.",
                "GET /health/ready checks readiness dependencies such as SQL.",
                "The frontend root and /login should return the Angular application.",
                "Unauthenticated Hangfire dashboard access should be denied.",
            ]
        ),
        h2("9.4 Common Operator Checks"),
        numbered(
            [
                "Confirm the API health endpoint returns 200.",
                "Sign in through the frontend and verify the expected role appears.",
                "Create or inspect tasks and verify queue counts.",
                "Confirm workers heartbeat and capacity look sane.",
                "Inspect failures and dead-letter counts before triggering retries.",
                "Check webhook delivery status when downstream integrations are not receiving events.",
                "Review audit findings and decide whether to apply, dismiss, or convert them to work.",
            ]
        ),
    ]

    story += [
        h1("10. Limitations and Next UI Work"),
        p("The backend is materially broader than the current restored frontend. The frontend should be expanded to expose dashboards, task management, workers, audit findings, notifications, files, admin, settings, webhooks, prompt templates, and a full Claude task board. Until those screens are rebuilt, many functions are API-first and require Swagger, direct API clients, scripts, or future UI work."),
        h2("10.1 Known Technical Caveats"),
        bullet(
            [
                "The baseline task executor described in the README produces deterministic output until connected to the intended Claude Code Desktop bridge.",
                "Hangfire is wired but recurring jobs need explicit scheduling where required.",
                "Production secrets must remain in environment-specific configuration, not committed files.",
                "The app should continue to avoid repo-local node_modules folders and use C:/Code/.pnpm for pnpm storage.",
            ]
        ),
    ]

    doc = ManualDocTemplate(str(path), "MADai User Manual")
    doc.build(story)


def build_ideas(path: Path):
    story = []
    story += cover(
        "Enhancement and Integration Ideas",
        "A strategic backlog for expanding MADai and connecting it to the MADProspects Universe.",
        "Prepared 2026-05-26",
    )

    story += [
        h1("1. Strategic Direction"),
        p("MADai should become the autonomous operations layer of MADProspects: a shared control plane where every MAD application can request work, publish events, receive generated artifacts, and expose operational health. The strongest opportunity is not only adding screens to MADai, but making MADai the queue, worker, workflow, and self-healing nervous system for the broader product universe."),
        callout(
            "North-star idea",
            "Every MADProspects product should be able to send a task to MADai, receive status updates, attach context, receive artifacts, and trigger follow-up actions without each product inventing its own worker system.",
        ),
        h2("1.1 Product Principles"),
        bullet(
            [
                "Use MADai as the cross-product task and automation backbone.",
                "Keep each application domain-specific, but standardize identity, eventing, health, task execution, and observability.",
                "Expose humans to simple workflows while allowing workers and integrations to use richer API contracts.",
                "Prefer configurable playbooks and templates over hard-coded one-off automations.",
                "Treat every task, artifact, webhook, deployment, and recommendation as auditable.",
            ]
        ),
    ]

    story += [
        h1("2. High-Impact Enhancements Inside MADai"),
        h2("2.1 Full Operator Dashboard"),
        p("Build the restored frontend into a complete operations cockpit. The dashboard should show queue health, live workers, task throughput, failed tasks, dead-letter backlog, average duration, success rate, audit findings, webhook failures, and deployment status. It should support drill-down from metric to affected task, worker, company, endpoint, or audit finding."),
        h2("2.2 Task Board and Task Detail Views"),
        bullet(
            [
                "Kanban-style columns for Draft, Queued, Scheduled, Claimed, Running, AwaitingValidation, Completed, Failed, DeadLetter, and Cancelled.",
                "Task detail with prompt payload, input payload, output summary, result payload, validation report, logs, comments, artifacts, retries, dependencies, and execution history.",
                "Bulk actions for retry, cancel, assign queue, change priority, add tags, and export.",
                "Saved filters for product, company, queue, category, status, priority, owner, worker, and created date.",
                "Timeline view showing status changes, logs, comments, and artifacts in order.",
            ]
        ),
        h2("2.3 Worker Fleet Management"),
        bullet(
            [
                "Live worker cards with status, queue, max concurrency, current concurrency, last heartbeat, CPU, memory, disk, labels, and capabilities.",
                "Drain, deactivate, maintenance mode, and restart-request workflows.",
                "Worker capability matching so tasks can require skills like Angular, .NET, SQL, PDF generation, image generation, scraping, or deployment.",
                "Worker performance scoring by success rate, average duration, category, and failure pattern.",
                "Workspace inspection showing recent logs, artifacts, and claimed tasks.",
            ]
        ),
        h2("2.4 Claude Task Board"),
        p("Turn /app/claude into a real SystemAdmin board for repository-improvement work. Add columns for Pending, InProgress, ToBeDeployed, Completed, Failed, Cancelled, and Deferred. Support attachments, prompt templates, priority, notes, scanner import review, deployment grouping, and verification checklists."),
        h2("2.5 Template and Playbook Library"),
        p("Create reusable playbooks that generate one or more tasks from a template. Examples: migrate one app to madprospects.com, deploy frontend and API, add superadmin seed, create PDF documentation, recover missing published files, audit an app folder, or standardize pnpm store usage."),
        h2("2.6 Human Approval Gates"),
        p("Add structured approvals before high-risk actions: deploy, database migration, mass email, credential rotation, domain change, destructive cleanup, billing change, or tenant-wide setting changes. Approval records should be auditable and tied to the task execution timeline."),
        h2("2.7 Artifact Gallery"),
        p("Create an artifact area where task outputs are browsable by product, company, task, type, and date. This can store PDFs, DOCX files, screenshots, deployment logs, test reports, generated images, HTML exports, and data extracts."),
    ]

    story += [
        PageBreak(),
        h1("3. Intelligence and Automation Ideas"),
        h2("3.1 Universal Work Intake"),
        p("Add an intake form where users describe what they want in plain language. MADai classifies the request into category, priority, product, required worker skills, risk level, likely subtasks, and suggested template. It then creates a structured task or multi-task workflow."),
        h2("3.2 AI Planning Layer"),
        p("Before execution, an AI planner can break work into research, design, implementation, test, deploy, and documentation phases. The plan should be editable by a human and saved as task dependencies."),
        h2("3.3 Automated QA and Evidence Packs"),
        p("Every task that changes code or production should produce an evidence pack: build output, test output, health checks, screenshots, deployed URLs, changed files, and rollback notes. Evidence packs become artifacts and can be attached to release notes."),
        h2("3.4 Self-Healing Operations"),
        bullet(
            [
                "Detect repeated failures by endpoint, worker, product, category, or exception type.",
                "Auto-create diagnostic tasks with logs and affected entity links.",
                "Suggest fixes and confidence levels.",
                "Allow an operator to approve self-healing execution.",
                "Close the loop by rechecking health after the fix.",
            ]
        ),
        h2("3.5 Prompt Template Governance"),
        p("Version prompt templates. Track which template produced which task result. Add ratings, success metrics, rollback capability, and environment-specific template variants."),
        h2("3.6 Multi-Agent Execution"),
        p("Introduce specialized worker roles: planner, coder, reviewer, tester, deployer, documenter, researcher, data analyst, and compliance checker. Workflows can route through multiple agents with explicit handoff artifacts."),
        h2("3.7 Cost and Capacity Intelligence"),
        p("Use the billing and usage domains to track estimated and actual effort, worker runtime, token usage where available, artifact storage, and queue wait time. Surface capacity forecasts and recommend when to scale workers."),
    ]

    story += [
        h1("4. MADProspects Universe Integration Map"),
        table(
            ["Application", "What it does", "How MADai can integrate"],
            [
                ["MADLeads", "AI-first lead management, SaaS subscriptions, feature flags, AI providers, quota management.", "Create lead-enrichment, campaign-generation, CRM cleanup, data-quality, and AI-cost-analysis tasks. Receive webhook events for new leads, failed syncs, quota alerts, and campaign milestones."],
                ["MADHub", "Marketing and operations cockpit for entrepreneurs and founders.", "Use MADai as the execution backend for marketing playbooks, content calendars, operational checklists, and cross-app status dashboards."],
                ["MADCreate", "Website and app generation platform.", "Route generation, editing, deployment, DNS checks, site audits, screenshot tests, copywriting, and repair tasks through MADai workers."],
                ["MADAuthor", "AI-native book creation and publishing platform.", "Run research, outline, chapter drafting, editing, formatting, export, cover, KDP metadata, and marketing asset pipelines as task workflows."],
                ["MADRecruiting", "Recruitment platform with local API/frontend targets.", "Create candidate screening, job post generation, interview pack, reference check, CRM follow-up, and hiring pipeline tasks."],
                ["MADPulse", "Venue discovery, ratings, and promotions.", "Run venue data enrichment, scraping, moderation, promotion generation, geospatial checks, review summarization, and venue onboarding tasks."],
                ["MADLearn", "Learning management for South African businesses.", "Generate courses, quizzes, learning paths, certificates, transcript analysis, assessment reports, and compliance reminders."],
                ["MADLove", "Relationship operating system.", "Create guided reflection packs, health summaries, conflict-repair playbooks, reminders, and anonymized pattern analysis with careful privacy controls."],
                ["MADCloud", "Repository and product orchestration dashboard.", "Share repository intelligence, health checks, deployment orchestration, worker fleet status, and cross-product automation with MADai."],
                ["MADHeroes", "Leads and venue inquiry platform for Heroes Advertising.", "Turn venue inquiries, newsletter signups, and lead forms into response, qualification, follow-up, and reporting tasks."],
                ["Multisciple", "Path-based multi-tenant church management SaaS.", "Automate tenant onboarding, member import, event schedules, sermon content, communications, audit checks, and custom-domain verification."],
            ],
            [1.15, 1.75, 3.35],
        ),
    ]

    story += [
        PageBreak(),
        h1("5. Cross-Product Integration Patterns"),
        h2("5.1 Shared Event Bus Through Webhooks"),
        p("Each MAD application can emit events into MADai: lead.created, site.deploy.failed, book.chapter.ready, candidate.shortlisted, venue.promotion.expiring, course.completed, tenant.created, or health.failed. MADai stores the event, creates tasks where appropriate, and delivers signed callbacks when work progresses."),
        h2("5.2 Shared Task API"),
        p("Every product should be able to create a MADai task using a stable contract: product key, tenant/company, category, title, description, priority, queue, due date, input payload, artifact URLs, callback endpoint, and approval policy."),
        h2("5.3 Shared Identity and Role Mapping"),
        p("Longer term, MADai can participate in a MADProspects identity layer. Users should not maintain separate accounts per product. Roles can map to product scopes, while SystemAdmin and support roles remain ecosystem-wide."),
        h2("5.4 Shared Artifact Storage"),
        p("Task artifacts should be reusable across products. A MADAuthor chapter export, MADCreate screenshot, MADLeads CSV, or MADRecruiting interview pack should have metadata, owner, retention policy, and source task links."),
        h2("5.5 Shared Audit Language"),
        p("Standardize audit findings across apps: severity, status, product, tenant, source, evidence, recommendation, owner, due date, related tasks, and resolution notes. MADai then becomes the shared audit-remediation engine."),
        h2("5.6 Shared Deployment Control Plane"),
        p("MADai can coordinate deployments across API/frontend pairs. A deployment task can build, upload, inject environment values, hit health checks, verify browser login, take screenshots, and mark a release as passed or failed."),
    ]

    story += [
        h1("6. Product-Specific Playbook Ideas"),
        h2("6.1 MADLeads Playbooks"),
        bullet(
            [
                "Lead enrichment: append company size, industry, contact role, likely intent, and recommended next action.",
                "Campaign copy generation: produce email, SMS, WhatsApp, and landing-page variants from a lead segment.",
                "Quota alert remediation: analyze heavy AI usage and recommend cheaper provider/model choices.",
                "Payfast lifecycle monitor: create tasks for failed payments, expiring trials, and upgrade opportunities.",
            ]
        ),
        h2("6.2 MADCreate Playbooks"),
        bullet(
            [
                "Generate SME website from brief: sitemap, copy, theme, imagery prompts, pages, SEO metadata, and deploy checklist.",
                "Site repair: crawl production, detect broken pages/assets, create fix tasks, and verify screenshots.",
                "Domain onboarding: DNS checklist, SSL check, tenant routing, and deployment proof.",
                "Conversion audit: review above-the-fold message, CTAs, page speed, accessibility, and lead capture.",
            ]
        ),
        h2("6.3 MADAuthor Playbooks"),
        bullet(
            [
                "Book pipeline: concept, market scan, outline, chapter briefs, drafts, edits, continuity pass, export, and launch copy.",
                "Transcription to book: ingest sermons, voice notes, or interviews and convert to chapters.",
                "KDP preparation: metadata, categories, keywords, back-cover copy, author bio, and launch assets.",
                "Series continuity: maintain canon, terminology, voice, and structure across volumes.",
            ]
        ),
        h2("6.4 MADRecruiting Playbooks"),
        bullet(
            [
                "Job advert generation from role brief and company profile.",
                "Candidate screening summary with skills, risks, questions, and ranking rationale.",
                "Interview pack generation for hiring managers.",
                "Reference-check follow-up automation and status tracking.",
                "Offer-letter drafting and onboarding checklist generation.",
            ]
        ),
        h2("6.5 MADPulse Playbooks"),
        bullet(
            [
                "Venue onboarding: enrich profile, normalize location, validate categories, and suggest promotional copy.",
                "Review summarization: turn ratings and comments into insight cards for venue owners.",
                "Promotion optimizer: recommend time windows, target audiences, and offer wording.",
                "Quality monitor: flag stale venue data, suspicious reviews, and missing media.",
            ]
        ),
        h2("6.6 MADLearn Playbooks"),
        bullet(
            [
                "Course generation from a company SOP, policy, or training need.",
                "Quiz and assessment generation with answer keys and remediation paths.",
                "Learner-risk monitor that creates follow-up tasks for incomplete or failed modules.",
                "Certificate and compliance pack generation for regulated training.",
            ]
        ),
    ]

    story += [
        PageBreak(),
        h1("7. Technical Architecture Enhancements"),
        h2("7.1 Integration SDKs"),
        p("Create TypeScript and .NET SDKs for creating tasks, sending events, reading status, downloading artifacts, and verifying webhook signatures. This keeps each product from hand-writing MADai integration code."),
        h2("7.2 Product Registry"),
        p("Add a Product table for MADLeads, MADHub, MADCreate, MADAuthor, MADRecruiting, MADPulse, MADLearn, MADLove, MADCloud, MADHeroes, Multisciple, and future products. Store URLs, API endpoints, health checks, owners, deployment targets, source paths, environment names, and integration capabilities."),
        h2("7.3 Secure Secret Broker"),
        p("Do not put product secrets in task payloads. Add references to secret names and resolve them only inside approved workers. Log secret access without logging secret values."),
        h2("7.4 Workflow Engine"),
        p("Introduce a workflow layer above tasks. A workflow defines phases, dependencies, approval gates, rollback steps, artifacts, SLAs, and notifications. Tasks remain the executable units."),
        h2("7.5 Standard Health Contract"),
        p("Every product should expose /health and /health/ready with a consistent JSON shape. MADai can poll them, show product status, and create remediation tasks on failure."),
        h2("7.6 Browser Verification Workers"),
        p("For frontend apps, a browser worker can log in, click critical flows, capture screenshots, compare visual regressions, and attach proof. This is especially useful after FTP/Plesk deployments."),
        h2("7.7 Data Connectors"),
        p("Add connectors for SQL Server, PostgreSQL, Payfast, SMTP, GitHub, Google Drive, SharePoint, Slack/Teams, WhatsApp providers, Google Analytics, Search Console, and domain/DNS providers. Each connector should be permissioned and auditable."),
    ]

    story += [
        h1("8. Governance, Security, and Compliance Ideas"),
        h2("8.1 Risk Scoring"),
        p("Every task should have a risk score based on action type, environment, affected product, tenant count, data sensitivity, and required permissions. High-risk tasks should require approval and evidence packs."),
        h2("8.2 Data Privacy Controls"),
        bullet(
            [
                "Classify task payloads as public, internal, confidential, personal, or sensitive.",
                "Redact secrets and personal data from logs by default.",
                "Add retention policies for artifacts, logs, and webhook events.",
                "Support tenant-specific privacy rules for products handling relationships, recruiting, learning, or church data.",
            ]
        ),
        h2("8.3 Audit Trails"),
        p("Every create, update, delete, status transition, approval, deployment, webhook delivery, secret access, and artifact download should be auditable. The existing auditable entity foundation is a strong start."),
        h2("8.4 Human-in-the-Loop Defaults"),
        p("MADai should automate aggressively but deploy conservatively. Risky tasks should stage recommendations, present evidence, and ask a SystemAdmin or product owner to approve execution."),
    ]

    story += [
        PageBreak(),
        h1("9. Suggested Roadmap"),
        table(
            ["Phase", "Theme", "Deliverables"],
            [
                ["Phase 1", "Make current MADai fully usable", "Full frontend: dashboard, task board, worker fleet, Claude board, admin basics, settings, files, notifications, webhooks."],
                ["Phase 2", "Cross-product intake", "Product registry, shared task API, SDKs, webhook event standard, MADProspects product health board."],
                ["Phase 3", "Automation playbooks", "Template library, workflow engine, approval gates, evidence packs, browser verification worker."],
                ["Phase 4", "Self-healing universe", "Shared audit language, product health monitors, auto-created remediation tasks, post-fix verification."],
                ["Phase 5", "Commercial intelligence", "Usage/cost dashboards, tenant billing hooks, capacity forecasts, AI provider optimization, SLA reporting."],
                ["Phase 6", "Autonomous operations network", "Multi-agent orchestration, product-specific specialist agents, cross-app campaign and delivery workflows."],
            ],
            [0.8, 1.55, 3.9],
        ),
        h2("9.1 Immediate Backlog"),
        numbered(
            [
                "Build a real /app/claude board from the existing Claude tasks API.",
                "Rebuild task list/detail/create screens and connect them to /api/v1/tasks.",
                "Add worker fleet screens and live SignalR updates.",
                "Add product registry seeded from Setup.xlsx and local MADProspects folders.",
                "Add a universal task intake form with template suggestions.",
                "Add frontend screens for audit findings, webhooks, notifications, settings, files, and admin.",
                "Create integration SDKs for .NET and TypeScript.",
                "Add a deployment playbook that builds, deploys, health-checks, and browser-verifies each app.",
            ]
        ),
    ]

    story += [
        h1("10. Success Metrics"),
        table(
            ["Metric", "Why it matters"],
            [
                ["Task cycle time", "Shows how quickly requests move from intake to done."],
                ["Queue wait time", "Shows whether worker capacity is sufficient."],
                ["Worker utilization", "Shows idle, busy, and overloaded agents."],
                ["Success rate", "Shows reliability by product, queue, category, and worker."],
                ["Dead-letter count", "Shows unresolved failures requiring operator attention."],
                ["Automation acceptance rate", "Shows whether AI suggestions are useful enough for operators to approve."],
                ["Deployment verification pass rate", "Shows release quality."],
                ["Cross-product tasks created", "Shows whether MADai is becoming the universe control plane."],
                ["Evidence completeness", "Shows whether work is auditable and supportable."],
            ],
            [1.8, 4.45],
        ),
        callout(
            "Recommended first milestone",
            "Make MADai the trusted operator cockpit for its own repository first, then connect one high-value product such as MADRecruiting or MADCreate through the shared task API. Prove the pattern with one complete workflow before rolling it out to the whole universe.",
        ),
    ]

    doc = ManualDocTemplate(str(path), "MADai Enhancement and Integration Ideas")
    doc.build(story)


def main():
    build_user_manual(ROOT / "MADaiUserManual.pdf")
    build_ideas(ROOT / "MADaiIdeas.pdf")


if __name__ == "__main__":
    main()
