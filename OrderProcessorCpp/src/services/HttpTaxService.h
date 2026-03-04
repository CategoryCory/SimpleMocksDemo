#pragma once
#include <string>
#include "ITaxService.h"

class HttpTaxService : public ITaxService
{
public:
    explicit HttpTaxService(std::string baseUrl);
    bool IsOrderTaxable(const Order& order) override;
private:
    std::string m_baseUrl;
};
