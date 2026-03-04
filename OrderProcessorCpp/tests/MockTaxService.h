#pragma once

#include <gmock/gmock.h>
#include "../src/services/ITaxService.h"

/**
 * @file MockTaxService.h
 * @brief Google Mock implementation of `ITaxService` for unit tests.
 */

/**
 * @brief Mock implementation of `ITaxService` used in tests.
 *
 * This mock allows test code to set expectations and return values for the
 * `IsOrderTaxable` method without depending on a real external tax service.
 */
class MockTaxService : public ITaxService
{
public:
    /**
     * @brief Mocked method for determining whether an order is taxable.
     *
     * @param order The order instance to evaluate.
     * @return true if the order is considered taxable by the mock; false otherwise.
     */
    MOCK_METHOD(bool, IsOrderTaxable, (const Order& order), (override));
};
