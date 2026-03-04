#pragma once
#include "../models/order.h"

struct ITaxService
{
    virtual ~ITaxService() = default;
    virtual bool IsOrderTaxable(const Order& order) = 0;
};