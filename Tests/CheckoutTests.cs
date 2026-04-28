using System.Text.RegularExpressions;
using AutomationExercise.Playwright.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;

namespace AutomationExercise.Playwright.Tests.Tests;

[TestFixture]
public class CheckoutTests : BaseTest
{
    [Test]
    public async Task TC23_VerifyAddressDetailsInCheckoutPage_Works()
    {
        string email = TestDataFactory.UniqueEmail("address");
        string password = "Password123!";

        await CreateAccountAsync(email, password);

        await Page.GetByRole(AriaRole.Link, new() { Name = "Products" }).ClickAsync();

        await Page.Locator("a[data-product-id='1']").First.ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Continue Shopping" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Cart" }).ClickAsync();
        await Expect(Page.Locator("#product-1")).ToBeVisibleAsync();

        await Page.GetByText("Proceed To Checkout").ClickAsync();

        await Expect(Page.Locator("#address_delivery")).ToContainTextAsync("Intern");
        await Expect(Page.Locator("#address_delivery")).ToContainTextAsync("Candidate");
        await Expect(Page.Locator("#address_delivery")).ToContainTextAsync("Test Street 1");
        await Expect(Page.Locator("#address_delivery")).ToContainTextAsync("Toronto");
        await Expect(Page.Locator("#address_delivery")).ToContainTextAsync("Ontario");
        await Expect(Page.Locator("#address_delivery")).ToContainTextAsync("Canada");

        await Expect(Page.Locator("#address_invoice")).ToContainTextAsync("Intern");
        await Expect(Page.Locator("#address_invoice")).ToContainTextAsync("Candidate");
        await Expect(Page.Locator("#address_invoice")).ToContainTextAsync("Test Street 1");
        await Expect(Page.Locator("#address_invoice")).ToContainTextAsync("Toronto");
        await Expect(Page.Locator("#address_invoice")).ToContainTextAsync("Ontario");
        await Expect(Page.Locator("#address_invoice")).ToContainTextAsync("Canada");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Delete Account" }).ClickAsync();
    }

    [Test]
    public async Task TC24_DownloadInvoiceAfterPurchaseOrder_Works()
    {
        string email = TestDataFactory.UniqueEmail("invoice");
        string password = "Password123!";

        await CreateAccountAsync(email, password);

        await Page.GetByRole(AriaRole.Link, new() { Name = "Products" }).ClickAsync();

        await Page.Locator("a[data-product-id='1']").First.ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Continue Shopping" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Cart" }).ClickAsync();
        await Page.GetByText("Proceed To Checkout").ClickAsync();

        await Page.Locator("textarea[name='message']").FillAsync("Automated order test.");
        await Page.GetByRole(AriaRole.Link, new() { Name = "Place Order" }).ClickAsync();

        await Page.Locator("input[data-qa='name-on-card']").FillAsync("Intern Candidate");
        await Page.Locator("input[data-qa='card-number']").FillAsync("4111111111111111");
        await Page.Locator("input[data-qa='cvc']").FillAsync("123");
        await Page.Locator("input[data-qa='expiry-month']").FillAsync("12");
        await Page.Locator("input[data-qa='expiry-year']").FillAsync("2030");

        await Page.Locator("button[data-qa='pay-button']").ClickAsync();

        await Expect(Page.GetByText("Congratulations! Your order has been confirmed!", new() { Exact = false }))
            .ToBeVisibleAsync();

        var download = await Page.RunAndWaitForDownloadAsync(async () =>
        {
            await Page.GetByRole(AriaRole.Link, new() { Name = "Download Invoice" }).ClickAsync();
        });

        Assert.That(download.SuggestedFilename, Does.Contain("invoice"));

        await Page.GetByRole(AriaRole.Link, new() { Name = "Continue" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Delete Account" }).ClickAsync();
    }

    private async Task CreateAccountAsync(string email, string password)
    {
        await Page.GetByRole(AriaRole.Link, new() { Name = "Signup / Login" }).ClickAsync();

        await Page.Locator("input[data-qa='signup-name']").FillAsync("Intern Candidate");
        await Page.Locator("input[data-qa='signup-email']").FillAsync(email);
        await Page.Locator("button[data-qa='signup-button']").ClickAsync();

        await Page.Locator("#id_gender2").CheckAsync();
        await Page.Locator("input[data-qa='password']").FillAsync(password);

        await Page.Locator("select[data-qa='days']").SelectOptionAsync("10");
        await Page.Locator("select[data-qa='months']").SelectOptionAsync("5");
        await Page.Locator("select[data-qa='years']").SelectOptionAsync("2000");

        await Page.Locator("input[data-qa='first_name']").FillAsync("Intern");
        await Page.Locator("input[data-qa='last_name']").FillAsync("Candidate");
        await Page.Locator("input[data-qa='address']").FillAsync("Test Street 1");
        await Page.Locator("select[data-qa='country']").SelectOptionAsync("Canada");
        await Page.Locator("input[data-qa='state']").FillAsync("Ontario");
        await Page.Locator("input[data-qa='city']").FillAsync("Toronto");
        await Page.Locator("input[data-qa='zipcode']").FillAsync("12345");
        await Page.Locator("input[data-qa='mobile_number']").FillAsync("0712345678");

        await Page.Locator("button[data-qa='create-account']").ClickAsync();

        await Expect(Page.GetByText("Account Created!", new() { Exact = false }))
            .ToBeVisibleAsync();

        await Page.Locator("a[data-qa='continue-button']").ClickAsync();
    }
}
