/**
 * @file main.cpp
 * @brief Entry point for the OrderProcessor application.
 *
 * Demonstrates order validation and price calculation using a live
 * HTTP-based tax service. Constructs a sample order, validates it,
 * and prints the calculated total to standard output.
 */

#include <iomanip>
#include <iostream>

#include "src/services/HttpTaxService.h"
#include "src/services/OrderValidator.h"
#include "src/services/PriceCalculator.h"

/**
 * @brief Application entry point.
 *
 * Creates an @c HttpTaxService connected to the local tax service, builds
 * a sample order, validates it with @c OrderValidator, and uses
 * @c PriceCalculator to compute and display the order total.
 *
 * @return 0 on success, 1 if the order fails validation.
 */
int main()
{
    HttpTaxService taxService("http://127.0.0.1:8000");
    const PriceCalculator priceCalculator(&taxService);

    /// Sample order used to demonstrate the processing pipeline.
    Order order;
    order.orderId = "12345";
    order.items.push_back({ "Item 1", 2, 70.0 });

    if (!OrderValidator::Validate(order))
    {
        std::cerr << "Invalid order\n";
        return 1;
    }

    const double total = priceCalculator.CalculateTotalPrice(order);

    std::cout << "Order total: $" << std::fixed << std::setprecision(2) << total << "\n";
    return 0;
}
