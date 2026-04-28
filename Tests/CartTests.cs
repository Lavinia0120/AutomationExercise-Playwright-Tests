using Microsoft.Playwright;
using NUnit.Framework;
using System.Text.RegularExpressions;
using AutomationExercise.Playwright.Tests.Helpers;

namespace AutomationExercise.Playwright.Tests.Tests;

[TestFixture]
public class CartTests : BaseTest
{
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
public async Task TC20_SearchProductsAndVerifyCartAfterLogin_Works()
{
    string email = TestDataFactory.UniqueEmail("cartlogin");
    string password = "Password123!";

    await CreateAccountAsync(email, password);
    await Page.GetByRole(AriaRole.Link, new() { Name = "Logout" }).ClickAsync();

    await Page.GetByRole(AriaRole.Link, new() { Name = "Products" }).ClickAsync();
    await Page.Locator("#search_product").FillAsync("Blue Top");
    await Page.Locator("#submit_search").ClickAsync();

    await Expect(Page.GetByText("Searched Products")).ToBeVisibleAsync();

    await Page.Locator("a[data-product-id='1']").First.ClickAsync();
    await Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("View Cart", RegexOptions.IgnoreCase) }).ClickAsync();

    await Expect(Page.Locator("#product-1")).ToBeVisibleAsync();

    await Page.GetByRole(AriaRole.Link, new() { Name = "Signup / Login" }).ClickAsync();
    await Page.Locator("input[data-qa='login-email']").FillAsync(email);
    await Page.Locator("input[data-qa='login-password']").FillAsync(password);
    await Page.Locator("button[data-qa='login-button']").ClickAsync();

    await Page.GetByRole(AriaRole.Link, new() { Name = "Cart" }).ClickAsync();

    await Expect(Page.Locator("#product-1")).ToBeVisibleAsync();

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

    await Expect(Page.GetByText("Account Created!", new() { Exact = false })).ToBeVisibleAsync();

    await Page.Locator("a[data-qa='continue-button']").ClickAsync();
}
[Test]
public async Task TC22_AddToCartFromRecommendedItems_Works()
{
    await UiHelpers.ScrollToFooterAsync(Page);

    await Expect(Page.GetByText("Recommended Items", new() { Exact = false }))
        .ToBeVisibleAsync();

    await Page.Locator(".recommended_items a[data-product-id]").First.ClickAsync();

    await Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("View Cart", RegexOptions.IgnoreCase) }).ClickAsync();

    await Expect(Page.Locator(".cart_info")).ToBeVisibleAsync();
}
}
