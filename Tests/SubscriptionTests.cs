using Microsoft.Playwright;
using NUnit.Framework;
using System.Text.RegularExpressions;
using AutomationExercise.Playwright.Tests.Helpers;

namespace AutomationExercise.Playwright.Tests.Tests;

[TestFixture]
public class SubscriptionTests : BaseTest
{
    [Test]
    public async Task TC10_SubscriptionOnHomePage_Works()
    {
        await UiHelpers.ScrollToFooterAsync(Page);
        await Expect(Page.GetByText("Subscription", new() { Exact = false })).ToBeVisibleAsync();

        await Page.Locator("#susbscribe_email").FillAsync(TestDataFactory.SubscriptionEmail());
        await Page.Locator("#subscribe").ClickAsync();

        await Expect(Page.GetByText("You have been successfully subscribed!")).ToBeVisibleAsync();
    }
     [Test]
    public async Task TC11_SubscriptionOnCartPage_Works()
    {
        await Page.GetByRole(AriaRole.Link, new() { Name = "Cart" }).ClickAsync();
        await UiHelpers.ScrollToFooterAsync(Page);
        await Expect(Page.GetByText("Subscription", new() { Exact = false })).ToBeVisibleAsync();

        await Page.Locator("#susbscribe_email").FillAsync(TestDataFactory.SubscriptionEmail());
        await Page.Locator("#subscribe").ClickAsync();

        await Expect(Page.GetByText("You have been successfully subscribed!")).ToBeVisibleAsync();
    }
}
