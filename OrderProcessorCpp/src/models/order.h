#pragma once
#include <vector>
#include <string>

struct OrderItem 
{
    std::string name;
    int quantity;
    double price;
};

struct Order
{
    std::string orderId;
    std::vector<OrderItem> items;
};
