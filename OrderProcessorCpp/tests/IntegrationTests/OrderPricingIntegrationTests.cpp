#include <gtest/gtest.h>
#include "../src/services/HttpTaxService.h"
#include "../src/services/PriceCalculator.h"
#include "../src/models/order.h"

// This integration test calls the real tax-service at the URL below.
// Start the Python tax-service locally before running integration tests.
TEST(Integration_OrderPricing, UsesRemoteTaxService) {
    HttpTaxService tax("http://127.0.0.1:8000"); // adjust if needed

    Order order;
    order.orderId = "int-1";
    order.items = { { "sku", 1, 100.0 } };

    const PriceCalculator calc(&tax, 10.0);
    const double total = calc.CalculateTotalPrice(order);

    // Basic sanity: the total should be positive.
    ASSERT_GT(total, 0.0);
}