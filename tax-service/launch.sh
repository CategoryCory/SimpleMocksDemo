#!/bin/bash

set -e

docker build -t tax-service:latest .
docker run -d -p 8000:8000 --name tax-service tax-service:latest