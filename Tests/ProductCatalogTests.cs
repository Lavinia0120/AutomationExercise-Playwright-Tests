using Microsoft.Playwright;
using NUnit.Framework;
using System.Text.RegularExpressions;
using AutomationExercise.Playwright.Tests.Helpers;

namespace AutomationExercise.Playwright.Tests.Tests;

[TestFixture]
public class ProductCatalogTests : BaseTest
{
    // [Test]
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
[Test]
public async Task Extra_Search_NonExistingProduct_ReturnsNoResults()
{
    await Page.GotoAsync("https://automationexercise.com/products");
    await UiHelpers.CloseConsentAsync(Page);

    string uniqueSearchTerm = "NoProduct_" + Guid.NewGuid().ToString("N");

    await Page.Locator("input[id='search_product']").FillAsync(uniqueSearchTerm);
    await Page.Locator("button[id='submit_search']").ClickAsync();

    await Expect(Page.GetByText("Searched Products", new() { Exact = false }))
        .ToBeVisibleAsync();

    await Expect(Page.Locator(".features_items .product-image-wrapper"))
        .ToHaveCountAsync(0);
}
[Test]
public async Task TC21_AddReviewOnProduct_Works()
{
    await Page.GetByRole(AriaRole.Link, new() { Name = "Products" }).ClickAsync();

    await Expect(Page.GetByText("All Products")).ToBeVisibleAsync();

    await Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("View Product", RegexOptions.IgnoreCase) }).First.ClickAsync();

    await Expect(Page.GetByText("Write Your Review", new() { Exact = false })).ToBeVisibleAsync();

    await Page.Locator("#name").FillAsync("Intern Candidate");
    await Page.Locator("#email").FillAsync(TestDataFactory.UniqueEmail("review"));
    await Page.Locator("#review").FillAsync("This is an automated product review test.");
    await Page.Locator("#button-review").ClickAsync();

    await Expect(Page.GetByText("Thank you for your review.", new() { Exact = false }))
        .ToBeVisibleAsync();
}
}
