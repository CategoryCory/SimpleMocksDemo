#pragma once
#include "../models/order.h"

class OrderValidator
{
public:
    static bool Validate(const Order& order);
};
