#include <iomanip>
#include <iostream>

#include "src/services/HttpTaxService.h"
#include "src/services/OrderValidator.h"
#include "src/services/PriceCalculator.h"

int main()
{
    HttpTaxService taxService("http://127.0.0.1:8000");
    const PriceCalculator priceCalculator(&taxService);
    
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
