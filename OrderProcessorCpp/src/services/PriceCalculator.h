#pragma once
#include "../models/order.h"
#include "ITaxService.h"

/**
 * @file PriceCalculator.h
 * @brief Calculates order total including tax when applicable.
 */

/**
 * @brief Computes pricing for orders, applying tax when the provided
 * `ITaxService` reports the order as taxable.
 *
 * The class depends on an `ITaxService` instance to decide whether tax
 * should be applied. The tax rate is provided as a percentage (for example,
 * 10.0 means 10%).
 */
class PriceCalculator
{
public:
    /**
     * @brief Construct a new PriceCalculator
     *
     * @param taxService Pointer to an `ITaxService` used to determine taxability.
     * The pointer is not owned by this class; callers are responsible for
     * managing its lifetime.
     * @param taxRatePercent Tax rate as a percentage (default: 10.0).
     */
    explicit PriceCalculator(ITaxService* taxService, double taxRatePercent = 10.0);

    /**
     * @brief Calculate the total price for an order, including tax if applicable.
     *
     * The method consults the injected `ITaxService` to determine whether
     * tax should be applied. If taxable, tax is computed as
     * `order.subtotal * (taxRatePercent / 100.0)` and added to the subtotal.
     *
     * @param order The order to price.
     * @return The total price including tax when applicable.
     */
    [[nodiscard]] double CalculateTotalPrice(const Order& order) const;

private:
    /**
     * @brief Non-owning pointer to the tax service used to determine taxability.
     */
    ITaxService* m_taxService;

    /**
     * @brief Tax rate expressed as a percent (e.g., 10.0 for 10%).
     */
    double m_taxRatePercent;
};
