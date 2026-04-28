# Automation Exercise - Playwright C# Quest Global Internship

## Overview

This project contains automated UI tests for the AutomationExercise website using Playwright and NUnit.  
The test suite covers key functionalities such as authentication, product interactions, cart operations, checkout processes, and UI behavior like scrolling and dynamic elements.  
It implements a selected set of test cases from the public test-case list, along with additional advanced scenarios to improve overall test coverage for the internship assignment.

## Tech Stack
- C#
- .NET 10
- Microsoft Playwright
- NUnit

## Project Structure
- Core – Base test setup (BaseTest)
- Helpers – Utility methods (e.g. handling cookie consent)
- TestData – Dynamic test data generation (e.g. GUID emails)
- Tests – Test classes grouped by functionality

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
12. Test case 20 – Search products & verify cart after login
13. Test case 21 – Add review on product
14. Test case 22 – Add to cart from recommended items
15. Teste case 23 – Verify address details in checkout page
16. Test case 24 – Download invoice after purchase order
17. Test case 25 – Scroll up using arrow button
18. Test case 26 – Scroll up without arrow button
Additionally, the following extra automated scenarios were implemented as part of the optional requirements:

20. Extra - Invalid Login (negative scenario)
21. Extra - Search for non-existing product (dynamic data using GUID)

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
- Some test scenarios were marked as ignored due to flaky behavior caused by dynamic overlays and sidebar interactions on the demo site.
## Additional Notes
- Invalid login validation (negative test case)
- Search for non-existing product using dynamic test data (GUID)
