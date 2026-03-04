#include "PriceCalculator.h"

PriceCalculator::PriceCalculator(ITaxService* taxService, const double taxRatePercent)
    : m_taxService(taxService), m_taxRatePercent(taxRatePercent / 100.0) {}

double PriceCalculator::CalculateTotalPrice(const Order& order) const {
    double subTotal = 0.0;
    for (auto const& item : order.items) {
        subTotal += item.price * item.quantity;
    }
    
    if (m_taxService && m_taxService->IsOrderTaxable(order))
    {
        return subTotal * (1.0 + m_taxRatePercent);
    }
    
    return subTotal;
}
