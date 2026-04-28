using Microsoft.Playwright;
using NUnit.Framework;
using AutomationExercise.Playwright.Tests.Helpers;

namespace AutomationExercise.Playwright.Tests.Tests;

[TestFixture]
public class AuthenticationTests : BaseTest
{
    [Test]
    public async Task Extra_InvalidLogin_ShowsErrorMessage()
    {
        await Page.GotoAsync("https://automationexercise.com/login");
        await UiHelpers.CloseConsentAsync(Page);

        await Page.Locator("input[data-qa='login-email']").FillAsync("invalid_user@test.com");
        await Page.Locator("input[data-qa='login-password']").FillAsync("wrongPassword123");
        await Page.Locator("button[data-qa='login-button']").ClickAsync();

        await Expect(Page.GetByText("Your email or password is incorrect!", new() { Exact = false }))
            .ToBeVisibleAsync();
    }
}
