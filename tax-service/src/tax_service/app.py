from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI(title="Tax Service API", version="1.0")


class OrderItemDto(BaseModel):
    name: str
    price: float
    quantity: int


class OrderDto(BaseModel):
    order_id: str
    items: list[OrderItemDto]


class TaxResponseDto(BaseModel):
    order_id: str
    is_taxable: bool
    total_amount: float
    

@app.get("/health")
async def health_check() -> dict:
    return {"status": "ok"}

@app.post("/calculate-tax", response_model=TaxResponseDto)
async def calculate_tax(order: OrderDto) -> TaxResponseDto:
    total_amount = sum(item.price * item.quantity for item in order.items)
    is_taxable = total_amount > 100
    return TaxResponseDto(order_id=order.order_id, is_taxable=is_taxable, total_amount=total_amount)
