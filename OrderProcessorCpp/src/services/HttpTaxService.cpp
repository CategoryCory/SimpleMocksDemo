#include "HttpTaxService.h"
#include "httplib.h"
#include <json.hpp>
#include <utility>

using json = nlohmann::json;

HttpTaxService::HttpTaxService(std::string baseUrl) : m_baseUrl(std::move(baseUrl)) {}

bool HttpTaxService::IsOrderTaxable(const Order& order) {
    try {
        // Normalize baseUrl (expecting http://host[:port][/<base>])
        std::string url = m_baseUrl;
        if (url.rfind("http://", 0) == 0) {
            url = url.substr(7);
        } else if (url.rfind("https://", 0) == 0) {
            // Keep minimal: do not handle HTTPS in this demo
            return false;
        }

        std::string hostport = url;
        std::string basePath;
        auto slashPos = url.find('/');
        if (slashPos != std::string::npos) {
            hostport = url.substr(0, slashPos);
            basePath = url.substr(slashPos); // includes leading '/'
        }

        std::string host = hostport;
        int port = 80;
        auto colonPos = hostport.find(':');
        if (colonPos != std::string::npos) {
            host = hostport.substr(0, colonPos);
            try { port = std::stoi(hostport.substr(colonPos + 1)); } catch (...) { port = 80; }
        }

        httplib::Client cli(host, port);
        cli.set_follow_location(true);

        std::string path = basePath.empty() ? "/calculate-tax" : basePath + "/calculate-tax";

        json body;
        body["order_id"] = order.orderId;
        body["items"] = json::array();
        for (const auto& [name, quantity, price] : order.items) {
            json jitem;
            jitem["name"] = name;
            jitem["quantity"] = quantity;
            jitem["price"] = price;
            body["items"].push_back(jitem);
        }

        std::string payload = body.dump();
        auto res = cli.Post(path, payload, "application/json");

        if (!res || res->status != 200) return false;

        try {
            auto j = json::parse(res->body);
            return j.value("is_taxable", false);
        } catch (...) {
            return false;
        }
    } catch (...) {
        return false;
    }
}
