# E2E tests (Playwright)

Browser-driven end-to-end tests for `VumbaSoft.ErmanApp.Web`, using [Microsoft Playwright for .NET](https://playwright.dev/dotnet/) + xunit.

Unlike the other `test/` projects (which spin up an in-memory ABP host), these tests drive a **real browser against a running instance of the app**, because that's the only way to exercise actual client-side behavior (page navigation, form submission, JS).

## Prerequisites

1. Have the app running (in another terminal):
   ```bash
   dotnet run --project src/VumbaSoft.ErmanApp.Web
   ```
   By default tests target `https://localhost:44300` (the `launchSettings.json` URL). Override with the `E2E_BASE_URL` environment variable if you're running against a different host/port.

2. Make sure the target database has been migrated/seeded (`VumbaSoft.ErmanApp.DbMigrator`), so the default admin account exists for the login tests.

The Chromium browser binary is installed automatically the first time you run the tests (via `Microsoft.Playwright.Program.Main(["install", "chromium"])` in `PlaywrightFixture`) — no manual `playwright install` step needed.

## Running

```bash
dotnet test test/VumbaSoft.ErmanApp.Web.E2ETests
```

## Configuration (environment variables)

| Variable | Default | Purpose |
|---|---|---|
| `E2E_BASE_URL` | `https://localhost:44300` | URL of the running app |
| `HEADED` | `false` | Set to `true` to watch the browser while debugging locally |
| `E2E_ADMIN_USERNAME` | `admin` | Username used by login tests |
| `E2E_ADMIN_PASSWORD` | `1q2w3E*` | Password used by login tests |

## Writing new tests

- Inherit from `E2ETestBase` — it gives you a fresh, isolated `Page` (and `IBrowserContext`) per test, sharing one Chromium instance across the whole run via `PlaywrightFixture`.
- Prefer Playwright's role/label locators (`Page.GetByRole(...)`, `Page.GetByLabel(...)`) over CSS selectors — they're more resilient to markup changes.
- Relative URLs in `Page.GotoAsync("/some/path")` resolve against `E2E_BASE_URL` (configured as the context's `BaseURL`).
