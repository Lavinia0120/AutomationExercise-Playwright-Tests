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
[Test]
public async Task TC25_VerifyScrollUpUsingArrowButton_Works()
{
    await Page.GotoAsync("https://automationexercise.com/");
    await UiHelpers.CloseConsentAsync(Page);

    await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");

    await Expect(Page.GetByText("Subscription", new() { Exact = false }))
        .ToBeVisibleAsync();

    await Page.Locator("#scrollUp").ClickAsync();

    await Page.WaitForTimeoutAsync(1000);

    var scrollY = await Page.EvaluateAsync<int>("window.scrollY");
    Assert.That(scrollY, Is.LessThan(300));
}
[Test]
public async Task TC26_VerifyScrollUpWithoutArrowButton_Works()
{
    await Page.GotoAsync("https://automationexercise.com/");
    await UiHelpers.CloseConsentAsync(Page);

    await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");

    await Expect(Page.GetByText("Subscription", new() { Exact = false }))
        .ToBeVisibleAsync();

    await Page.EvaluateAsync("window.scrollTo(0, 0)");

    await Page.WaitForTimeoutAsync(1000);

    var scrollY = await Page.EvaluateAsync<int>("window.scrollY");
    Assert.That(scrollY, Is.LessThan(300));
}
}
