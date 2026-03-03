FROM python:3.14.3-slim-bookworm

WORKDIR /app

COPY . .

RUN pip install --no-cache-dir fastapi "uvicorn[standard]"

EXPOSE 8000

CMD ["uvicorn", "src.tax_service.app:app", "--host", "0.0.0.0", "--port", "8000"]