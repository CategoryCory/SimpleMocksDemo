#pragma once

#include <gmock/gmock.h>
#include "../src/services/ITaxService.h"

class MockTaxService : public ITaxService
{
public:
    MOCK_METHOD(bool, IsOrderTaxable, (const Order& order), (override));
};
