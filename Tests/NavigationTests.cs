using Microsoft.Playwright;
using NUnit.Framework;
using AutomationExercise.Playwright.Tests.Helpers;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;

namespace AutomationExercise.Playwright.Tests.Tests;

[TestFixture]
public class NavigationTests : BaseTest
{
    [Test]
[Retry(2)]
[Ignore("Flaky on public demo site during navigation/visibility checks")]
public async Task TC07_TestCasesPage_OpensSuccessfully()
{
    await Page.GotoAsync("https://automationexercise.com/test_cases");
    await UiHelpers.CloseConsentAsync(Page);

    await Expect(Page).ToHaveURLAsync(new Regex(".*/test_cases"));
    await Expect(Page.GetByText("Test Cases", new() { Exact = false }))
        .ToBeVisibleAsync();
}
}
