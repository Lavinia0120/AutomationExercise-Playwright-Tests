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
}
