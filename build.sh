#!/bin/bash

echo " Building Wonga Assessment..."

# Stop and remove existing containers
docker compose down

# Build and start all containers
docker compose up --build -d

echo "✅ Build complete!"
echo "Frontend: http://localhost:3000"
echo "Backend:  http://localhost:5000"