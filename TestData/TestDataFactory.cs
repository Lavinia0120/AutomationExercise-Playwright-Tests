namespace AutomationExercise.Playwright.Tests.Helpers;

public static class TestDataFactory
{
    public static string UniqueEmail(string prefix = "qa.intern") =>
        $"{prefix}.{DateTime.UtcNow:yyyyMMddHHmmssfff}@example.com";

    public static string SubscriptionEmail() => UniqueEmail("subscribe");
}
