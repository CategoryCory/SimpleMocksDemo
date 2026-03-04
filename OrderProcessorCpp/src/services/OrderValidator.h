#pragma once
#include "../models/order.h"

/**
 * @file OrderValidator.h
 * @brief Validation utilities for `Order` objects.
 */

/**
 * @brief Provides static validation logic for orders.
 *
 * `OrderValidator` exposes utility functions to check that an `Order` has
 * required, well-formed values before it is processed (for example, ensuring
 * non-empty customer information and positive item quantities/prices).
 */
class OrderValidator
{
public:
    /**
     * @brief Validate an order for basic correctness.
     *
     * Implementations should verify that the `order` contains all required
     * fields and that numeric values are in acceptable ranges.
     *
     * @param order The order to validate.
     * @return true if the order passes validation; false otherwise.
     */
    static bool Validate(const Order& order);
};
