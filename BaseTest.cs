using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace AutomationExercise.Playwright.Tests.Helpers;

public abstract class BaseTest : PageTest
{
    [SetUp]
    public async Task SetUpPageAsync()
    {
        await UiHelpers.GoHomeAsync(Page);
    }
}
