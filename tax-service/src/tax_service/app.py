from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI(title="Tax Service API", version="1.0")


class OrderItemDto(BaseModel):
    """OrderItemDto model.

    Represents a single item within an order.

    :param name: Human-readable name of the item.
    :type name: str
    :param price: Unit price for the item.
    :type price: float
    :param quantity: Number of units ordered.
    :type quantity: int
    """
    
    name: str
    price: float
    quantity: int


class OrderDto(BaseModel):
    """OrderDto model.

    Represents an order comprised of multiple order items.

    :param order_id: Unique identifier for the order.
    :type order_id: str
    :param items: List of `OrderItemDto` objects included in the order.
    :type items: list[OrderItemDto]
    """

    order_id: str
    items: list[OrderItemDto]


class TaxResponseDto(BaseModel):
    """TaxResponseDto model.

    Response returned by the tax calculation endpoint.

    :param order_id: The original order's identifier.
    :type order_id: str
    :param is_taxable: Whether the order is subject to tax.
    :type is_taxable: bool
    :param total_amount: Computed total amount for the order.
    :type total_amount: float
    """
    
    order_id: str
    is_taxable: bool
    total_amount: float
    

@app.get("/health", response_model=dict, status_code=200)
async def health_check() -> dict:
    """Health check endpoint.

    Returns a simple status payload used by uptime and readiness probes.

    :returns: A dictionary containing the service status.
    :rtype: dict
    """

    return {"status": "ok"}

@app.post("/calculate-tax", response_model=TaxResponseDto, status_code=200)
async def calculate_tax(order: OrderDto) -> TaxResponseDto:
    """Calculate tax for an order.

    Computes the total amount for the provided `order` by summing
    `price * quantity` for each item and determines whether the order
    is taxable. Current business rule: an order is taxable when
    `total_amount > 100`.

    :param order: The order to calculate tax for.
    :type order: OrderDto
    :returns: A `TaxResponseDto` containing `order_id`, `is_taxable`, and
        `total_amount`.
    :rtype: TaxResponseDto
    """

    total_amount = sum(item.price * item.quantity for item in order.items)
    is_taxable = total_amount > 100
    return TaxResponseDto(order_id=order.order_id, is_taxable=is_taxable, total_amount=total_amount)
