#pragma once
#include <string>
#include "ITaxService.h"

/**
 * @file HttpTaxService.h
 * @brief HTTP-based implementation of the `ITaxService` interface.
 */

/**
 * @brief Implementation of `ITaxService` that queries a remote HTTP service
 * to determine whether an order is taxable.
 *
 * This class encapsulates the network endpoint used to fetch taxability
 * information. It is useful in production code and can be replaced with a
 * mocked implementation for unit tests.
 */
class HttpTaxService : public ITaxService
{
public:
    /**
     * @brief Construct a new HttpTaxService object.
     *
     * @param baseUrl Base URL of the remote tax service (for example,
     * "https://tax.example.com/api"). The value is copied into the instance.
     */
    explicit HttpTaxService(std::string baseUrl);

    /**
     * @brief Determine whether an order is taxable by consulting the remote
     * tax service.
     *
     * Implementations typically perform an HTTP request to the configured
     * `m_baseUrl` and interpret the response according to the remote API.
     *
     * @param order The order to evaluate.
     * @return true if the order is taxable; false otherwise.
     */
    bool IsOrderTaxable(const Order& order) override;

private:
    /**
     * @brief Base URL of the remote tax service used for requests.
     */
    std::string m_baseUrl;
};
