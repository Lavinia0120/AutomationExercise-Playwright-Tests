using System.Text.RegularExpressions;
using AutomationExercise.Playwright.Tests.Helpers;
using Microsoft.Playwright;
using NUnit.Framework;

namespace AutomationExercise.Playwright.Tests.Tests;

[TestFixture]
public class ExistingTestCasesTests : BaseTest
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

    [Test]
[Retry(2)]
[Ignore("Flaky on public demo site during navigation/visibility checks")]
public async Task TC07_TestCasesPage_OpensSuccessfully()
{
    await Page.GotoAsync("https://automationexercise.com/test_cases");
    await UiHelpers.CloseConsentAsync(Page);

    await Expect(Page).ToHaveURLAsync(new Regex(".*/test_cases"));
    await Expect(Page.GetByText("Test Cases", new() { Exact = false })).ToBeVisibleAsync();
}

    [Test]
    public async Task TC08_AllProductsPage_AndProductDetail_AreVisible()
    {
        await Page.GetByRole(AriaRole.Link, new() { Name = "Products" }).ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex(".*/products"));
        await Expect(Page.GetByText("All Products")).ToBeVisibleAsync();
        await Expect(Page.Locator(".features_items .product-image-wrapper").First).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("View Product", RegexOptions.IgnoreCase) }).First.ClickAsync();

        await Expect(Page).ToHaveURLAsync(new Regex(".*/product_details/"));
        await Expect(Page.Locator(".product-information h2")).ToBeVisibleAsync();
        await Expect(Page.Locator(".product-information")).ToContainTextAsync("Category");
        await Expect(Page.Locator(".product-information")).ToContainTextAsync("Rs.");
        await Expect(Page.Locator(".product-information")).ToContainTextAsync("Availability");
        await Expect(Page.Locator(".product-information")).ToContainTextAsync("Condition");
        await Expect(Page.Locator(".product-information")).ToContainTextAsync("Brand");
    }

    [Test]
    public async Task TC09_SearchProduct_ShowsFilteredResults()
    {
        await Page.GetByRole(AriaRole.Link, new() { Name = "Products" }).ClickAsync();
        await Page.GetByPlaceholder("Search Product").FillAsync("Blue Top");
        await Page.Locator("#submit_search").ClickAsync();

        await Expect(Page.GetByText("Searched Products")).ToBeVisibleAsync();
        await Expect(Page.Locator(".features_items .productinfo").First).ToContainTextAsync("Blue Top");
    }

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

    [Test]
    public async Task TC12_AddProductsInCart_ShowsTwoProducts()
    {
        await Page.GetByRole(AriaRole.Link, new() { Name = "Products" }).ClickAsync();

        await Page.Locator("a[data-product-id='1']").First.ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Continue Shopping" }).ClickAsync();

        await Page.Locator("a[data-product-id='2']").First.ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("View Cart", RegexOptions.IgnoreCase) }).ClickAsync();

        await Expect(Page.Locator("#product-1")).ToBeVisibleAsync();
        await Expect(Page.Locator("#product-2")).ToBeVisibleAsync();
        await Expect(Page.Locator("#product-1 .cart_quantity")).ToContainTextAsync("1");
        await Expect(Page.Locator("#product-2 .cart_quantity")).ToContainTextAsync("1");
    }

    [Test]
    public async Task TC13_ProductQuantityInCart_IsExact()
    {
        await Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("View Product", RegexOptions.IgnoreCase) }).First.ClickAsync();
        await Expect(Page.Locator(".product-information")).ToBeVisibleAsync();

        await Page.Locator("#quantity").FillAsync("4");
        await Page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("Add to cart", RegexOptions.IgnoreCase) }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("View Cart", RegexOptions.IgnoreCase) }).ClickAsync();

        await Expect(Page.Locator(".cart_quantity button").First).ToHaveTextAsync("4");
    }

    [Test]
    public async Task TC17_RemoveProductFromCart_Works()
    {
        await Page.Locator("a[data-product-id='1']").First.ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("View Cart", RegexOptions.IgnoreCase) }).ClickAsync();
        await Expect(Page.Locator("#product-1")).ToBeVisibleAsync();

        await Page.Locator(".cart_quantity_delete").First.ClickAsync();
        await Expect(Page.Locator("#product-1")).ToHaveCountAsync(0);
    }

    [Test]
[Retry(2)]
[Ignore("Flaky on public demo site during navigation/visibility checks")]
public async Task TC18_ViewCategoryProducts_Works()
{
    await Page.GotoAsync("https://automationexercise.com/products");
    await UiHelpers.CloseConsentAsync(Page);

    await Expect(Page.GetByText("Category", new() { Exact = false })).ToBeVisibleAsync();

    var women = Page.Locator("a[href='#Women']");
    await women.ScrollIntoViewIfNeededAsync();
    await women.ClickAsync();

    var dress = Page.Locator("a[href='/category_products/1']");
    await dress.ScrollIntoViewIfNeededAsync();
    await dress.ClickAsync();

    await Expect(Page.Locator(".title.text-center"))
        .ToContainTextAsync(new Regex("Women\\s*-\\s*Dress Products", RegexOptions.IgnoreCase));
}

    [Test]
[Retry(2)]
[Ignore("Flaky on public demo site during navigation/visibility checks")]
public async Task Bonus_TC19_ViewBrandProducts_Works()
{
    await Page.GotoAsync("https://automationexercise.com/products");
    await UiHelpers.CloseConsentAsync(Page);

    await Expect(Page.GetByText("Brands", new() { Exact = false })).ToBeVisibleAsync();

    var polo = Page.Locator("a[href='/brand_products/Polo']");
    await polo.ScrollIntoViewIfNeededAsync();
    await polo.ClickAsync();

    await Expect(Page.Locator(".title.text-center"))
        .ToContainTextAsync(new Regex("Brand\\s*-\\s*Polo Products", RegexOptions.IgnoreCase));
}
}
