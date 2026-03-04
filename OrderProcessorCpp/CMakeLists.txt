#cmake_minimum_required(VERSION 4.1)
#project(OrderProcessorCpp VERSION 0.1 LANGUAGES CXX)
#
#set(CMAKE_CXX_STANDARD 20)
#set(CMAKE_CXX_STANDARD_REQUIRED ON)
#
## Built binaries output directory
#set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)
#
## Project layout variables
#set(SRC_DIR ${CMAKE_CURRENT_SOURCE_DIR}/src)
#set(TEST_DIR ${CMAKE_CURRENT_SOURCE_DIR}/tests)
#set(VENDOR_DIR ${CMAKE_CURRENT_SOURCE_DIR}/vendor)
#
#set(ORDER_CORE_SOURCES
#    ${SRC_DIR}/models/order.h
#    ${SRC_DIR}/services/ITaxService.h
#    ${SRC_DIR}/services/HttpTaxService.h
cmake_minimum_required(VERSION 3.28)
project(OrderProcessorCpp VERSION 0.1 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Built binaries output directory
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)

# Project layout variables
set(SRC_DIR ${CMAKE_CURRENT_SOURCE_DIR}/src)
set(TEST_DIR ${CMAKE_CURRENT_SOURCE_DIR}/tests)
set(VENDOR_DIR ${CMAKE_CURRENT_SOURCE_DIR}/vendor)

set(ORDER_CORE_SOURCES
    ${SRC_DIR}/services/OrderValidator.cpp
    ${SRC_DIR}/services/HttpTaxService.cpp
    ${SRC_DIR}/services/PriceCalculator.cpp
)

add_library(order_core STATIC ${ORDER_CORE_SOURCES})
target_include_directories(order_core PUBLIC
        ${CMAKE_CURRENT_SOURCE_DIR}
        ${SRC_DIR}
        ${VENDOR_DIR}
        ${VENDOR_DIR}/googletest-1.17.0/googletest/include
        ${VENDOR_DIR}/googletest-1.17.0/googlemock/include
)

# Use vendored nlohmann/json if present
if (EXISTS "${VENDOR_DIR}/json.hpp")
    message(STATUS "Using vendored nlohmann/json from ${VENDOR_DIR}")
    target_include_directories(order_core PUBLIC ${VENDOR_DIR})
else()
    message(FATAL_ERROR "vendor/json.hpp not found. Add nlohmann/json single-header to ${VENDOR_DIR} or restore FetchContent.")
endif()

# Use vendored cpp-httplib if present
if (EXISTS "${VENDOR_DIR}/httplib.h")
    message(STATUS "Using vendored cpp-httplib from ${VENDOR_DIR}")
    target_include_directories(order_core PUBLIC ${VENDOR_DIR})
else()
    message(FATAL_ERROR "vendor/httplib.h not found. Add cpp-httplib single-header to ${VENDOR_DIR} or restore FetchContent.")
endif()

add_executable(OrderProcessorCpp main.cpp)
target_link_libraries(OrderProcessorCpp PRIVATE order_core)

# ---- Tests (use vendored googletest) ----
if (EXISTS "${VENDOR_DIR}/googletest-1.17.0/CMakeLists.txt")
    message(STATUS "Adding vendored googletest from ${VENDOR_DIR}/googletest-1.17.0")
    add_subdirectory(${VENDOR_DIR}/googletest-1.17.0 ${CMAKE_BINARY_DIR}/vendor/googletest-build EXCLUDE_FROM_ALL)
else()
    message(FATAL_ERROR "vendor/googletest-1.17.0 not found. Add googletest source tree to ${VENDOR_DIR}/googletest-1.17.0.")
endif()

enable_testing()

set(UNIT_TEST_SOURCES
        ${TEST_DIR}/MockTaxService.h
        ${TEST_DIR}/UnitTests/PriceCalculatorTests.cpp
        ${TEST_DIR}/UnitTests/OrderValidatorTests.cpp
)

add_executable(unit_tests ${UNIT_TEST_SOURCES})
target_include_directories(unit_tests PRIVATE 
        ${SRC_DIR}
        ${TEST_DIR}
        ${VENDOR_DIR}
        ${VENDOR_DIR}/googletest-1.17.0/googletest/include
        ${VENDOR_DIR}/googletest-1.17.0/googlemock/include
)
target_link_libraries(unit_tests PRIVATE order_core gtest_main gmock_main)
include(GoogleTest)
gtest_discover_tests(unit_tests
        WORKING_DIRECTORY ${CMAKE_BINARY_DIR}
        PROPERTIES DISCOVERY_TIMEOUT 60
)

option(BUILD_INTEGRATION_TESTS "Build integration tests" ON)
if (BUILD_INTEGRATION_TESTS)
    set(INTEGRATION_TEST_SOURCES
            ${TEST_DIR}/IntegrationTests/OrderPricingIntegrationTests.cpp
    )

    add_executable(integration_tests ${INTEGRATION_TEST_SOURCES})
    target_include_directories(integration_tests PRIVATE
            ${SRC_DIR}
            ${TEST_DIR}
            ${VENDOR_DIR}
            ${VENDOR_DIR}/googletest-1.17.0/googletest/include
            ${VENDOR_DIR}/googletest-1.17.0/googlemock/include
    )
    target_link_libraries(integration_tests PRIVATE order_core gtest_main)
    if (TARGET integration_tests)
        gtest_discover_tests(integration_tests
                WORKING_DIRECTORY ${CMAKE_BINARY_DIR}
                PROPERTIES DISCOVERY_TIMEOUT 60
        )
    endif()
endif()

# Summary
message(STATUS "OrderProcessorCpp: src=${SRC_DIR}, tests=${TEST_DIR}, vendor=${VENDOR_DIR}")
message(STATUS "Build integration tests: ${BUILD_INTEGRATION_TESTS}")
