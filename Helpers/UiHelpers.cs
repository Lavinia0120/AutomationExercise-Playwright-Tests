using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace AutomationExercise.Playwright.Tests.Helpers;

public static class UiHelpers
{
    public static async Task CloseConsentAsync(IPage page)
    {
        var acceptButton = page.GetByRole(
            AriaRole.Button,
            new() { NameRegex = new Regex("Accept|Consent|Agree|I Agree|OK", RegexOptions.IgnoreCase) });

        if (await acceptButton.CountAsync() > 0)
        {
            try
            {
                await acceptButton.First.ClickAsync(new() { Timeout = 2000 });
            }
            catch
            {
            }
        }

        await page.EvaluateAsync(@"() => {
            document.querySelectorAll(
                '.fc-consent-root, .fc-dialog-overlay, .fc-dialog-container, .fc-footer, .fc-button-background, .fc-choice-dialog'
            ).forEach(el => el.remove());
        }");
    }

    public static async Task GoHomeAsync(IPage page)
    {
        await page.GotoAsync("https://automationexercise.com/");
        await CloseConsentAsync(page);
    }

    public static async Task ScrollToFooterAsync(IPage page)
    {
        await page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
    }
}
