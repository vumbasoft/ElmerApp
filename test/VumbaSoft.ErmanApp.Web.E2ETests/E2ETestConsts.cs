using System;

namespace VumbaSoft.ErmanApp;

public static class E2ETestConsts
{
    /// <summary>
    /// The URL of the already-running VumbaSoft.ErmanApp.Web application.
    /// Override with the E2E_BASE_URL environment variable (defaults to the
    /// project's Kestrel launch profile URL).
    /// </summary>
    public static string BaseUrl { get; } =
        Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "https://localhost:44300";

    /// <summary>
    /// Set the HEADED environment variable to "true" to watch the browser while debugging.
    /// </summary>
    public static bool Headless { get; } =
        !string.Equals(Environment.GetEnvironmentVariable("HEADED"), "true", StringComparison.OrdinalIgnoreCase);

    public static string AdminUserName { get; } =
        Environment.GetEnvironmentVariable("E2E_ADMIN_USERNAME") ?? "admin";

    public static string AdminPassword { get; } =
        Environment.GetEnvironmentVariable("E2E_ADMIN_PASSWORD") ?? "1q2w3E*";
}
