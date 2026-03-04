#include <gtest/gtest.h>
#include "../src/services/OrderValidator.h"
#include "../src/models/order.h"

TEST(OrderValidatorTests, ValidOrder) {
    Order o;
    o.orderId = "ok";
    o.items = { { "sku", 1, 9.99 } };
    EXPECT_TRUE(OrderValidator::Validate(o));
}

TEST(OrderValidatorTests, InvalidOrderEmptyId) {
    Order o;
    o.orderId = "";
    o.items = { { "sku", 1, 9.99 } };
    EXPECT_FALSE(OrderValidator::Validate(o));
}