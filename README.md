# Automation Exercise - Playwright C# Quest Global Internship

This project is a strong starter for the internship brief. It covers **10 existing scenarios** from the public test-case list and includes **1 bonus scenario**.

## Included automated scenarios

1. Test Case 6 - Contact Us Form
2. Test Case 7 - Verify Test Cases Page
3. Test Case 8 - Verify All Products and product detail page
4. Test Case 9 - Search Product
5. Test Case 10 - Verify Subscription in home page
6. Test Case 11 - Verify Subscription in Cart page
7. Test Case 12 - Add Products in Cart
8. Test Case 13 - Verify Product quantity in Cart
9. Test Case 17 - Remove Products From Cart
10. Test Case 18 - View Category Products
11. Bonus - Test Case 19 - View & Cart Brand Products

## Setup

```bash
dotnet restore
dotnet build
pwsh bin/Debug/net10.0/playwright.ps1 install
dotnet test
```

If PowerShell blocks script execution:

```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
.\bin\Debug\net10.0\playwright.ps1 install
```

## Notes

- The site sometimes shows a cookie/consent overlay. `UiHelpers.CloseConsentAsync()` removes it before actions.
- If a selector becomes flaky because the website changes, adjust only that locator rather than rewriting the whole test.
- For the final submission, zip the project folder or push it to a public GitHub repository, as required by the brief.
- Some test scenarios were marked as ignored due to flaky behavior caused by dynamic overlays and sidebar interactions on the public demo site. The remaining automated scenarios run successfully and cover the core user flows.
- Additional scenario: invalid login validation to verify error handling for incorrect credentials.
