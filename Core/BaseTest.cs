using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace AutomationExercise.Playwright.Tests.Core;

public abstract class BaseTest : PageTest
{
    [SetUp]
    public async Task SetUpPageAsync()
    {
        await UiHelpers.GoHomeAsync(Page);
    }
}
