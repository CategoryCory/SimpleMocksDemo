#include <gtest/gtest.h>
#include <gmock/gmock.h>

#include "../src/services/PriceCalculator.h"
#include "MockTaxService.h"

using testing::_;
using testing::Return;

TEST(PriceCalculatorTests, AppliesTaxWhenTaxable) {
    MockTaxService mock;
    EXPECT_CALL(mock, IsOrderTaxable(_)).WillOnce(Return(true));

    Order order;
    order.orderId = "1";
    order.items = { { "sku1", 2, 10.0 } }; // subtotal 20.0

    const PriceCalculator calc(&mock, 10.0); // 10% tax
    const double total = calc.CalculateTotalPrice(order);
    EXPECT_DOUBLE_EQ(total, 22.0);
}

TEST(PriceCalculatorTests, NoTaxWhenNotTaxable) {
    MockTaxService mock;
    EXPECT_CALL(mock, IsOrderTaxable(_)).WillOnce(Return(false));

    Order order;
    order.orderId = "2";
    order.items = { { "sku1", 1, 15.0 } };

    const PriceCalculator calc(&mock, 10.0);
    const double total = calc.CalculateTotalPrice(order);
    EXPECT_DOUBLE_EQ(total, 15.0);
}