#pragma once
#include <vector>
#include <string>

/**
 * @file order.h
 * @brief Models for orders used by the OrderProcessorCpp demo.
 *
 * This header defines simple POD structs used to represent an
 * individual order line (`OrderItem`) and a complete `Order`.
 */

/**
 * @brief Represents a single line item in an order.
 *
 * Contains the product name, the quantity ordered, and the unit
 * price for the item. Quantities are expected to be non-negative.
 */
struct OrderItem
{
    /** Product name or SKU. */
    std::string name;

    /** Quantity ordered (integer, typically >= 0). */
    int quantity;

    /** Unit price for the item in the order's currency. */
    double price;
};

/**
 * @brief Represents a customer's order.
 *
 * Holds a unique identifier and a list of the order's line items.
 */
struct Order
{
    /** Unique order identifier. */
    std::string orderId;

    /** Collection of items included in the order. */
    std::vector<OrderItem> items;
};
