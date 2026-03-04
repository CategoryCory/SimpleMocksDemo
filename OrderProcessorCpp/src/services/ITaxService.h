#pragma once
#include "../models/order.h"

/**
 * @file ITaxService.h
 * @brief Interface for tax-related operations used by the order processor.
 */

/**
 * @brief Abstract interface for determining taxability of orders.
 *
 * Implementations of this interface encapsulate tax rules and external
 * tax lookups. The order-processing code depends only on this interface so
 * that different tax strategies (including mocks in tests) may be injected.
 */
struct ITaxService
{
    /**
     * @brief Virtual destructor to allow proper cleanup in derived types.
     */
    virtual ~ITaxService() = default;

    /**
     * @brief Determine whether an order is subject to tax.
     *
     * Implementations should examine the provided @p order and decide whether
     * tax should be applied according to the applicable rules or external
     * service responses.
     *
     * @param order The order to evaluate.
     * @return true if the order is taxable; false otherwise.
     */
    virtual bool IsOrderTaxable(const Order& order) = 0;
};