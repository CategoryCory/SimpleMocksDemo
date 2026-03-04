#pragma once
#include "../models/order.h"
#include "ITaxService.h"

class PriceCalculator
{
public:
    explicit PriceCalculator(ITaxService* taxService, double taxRatePercent = 10.0);
    [[nodiscard]] double CalculateTotalPrice(const Order& order) const;
private:
    ITaxService* m_taxService;
    double m_taxRatePercent;
};
