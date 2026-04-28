using Microsoft.Playwright;
using NUnit.Framework;
using System.Text.RegularExpressions;
using AutomationExercise.Playwright.Tests.Helpers;

namespace AutomationExercise.Playwright.Tests.Tests;

[TestFixture]
public class ContactFormTests : BaseTest
{
    [Test]
    [Ignore("Flaky on public demo site due to alert/upload/contact form behavior")]
    public async Task TC06_ContactUsForm_SubmitsSuccessfully()
    {
        await Page.GetByRole(AriaRole.Link, new() { Name = "Contact us" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Get In Touch" })).ToBeVisibleAsync();

        await Page.GetByPlaceholder("Name").FillAsync("Intern Candidate");
        await Page.GetByPlaceholder("Email").FillAsync(TestDataFactory.UniqueEmail("contact"));
        await Page.GetByPlaceholder("Subject").FillAsync("Internship automation submission");
        await Page.GetByPlaceholder("Your Message Here").FillAsync("This is an automated Playwright test message.");

        var uploadPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Assets", "sample-upload.txt");
        await Page.Locator("input[type='file']").SetInputFilesAsync(uploadPath);

        Page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();

        await Expect(Page.GetByText("Success! Your details have been submitted successfully.")).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Home" }).ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex("automationexercise\\.com/?$"));
    }

}
