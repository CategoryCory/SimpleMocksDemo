#include "OrderValidator.h"

bool OrderValidator::Validate(const Order& order)
{
    if (order.orderId.empty()) return false;
    if (order.items.empty()) return false;
    
    for (auto const& item : order.items)
    {
        if (item.quantity <= 0 || item.price < 0.0) return false;
    }
    
    return true;
}
