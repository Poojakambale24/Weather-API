#!/bin/bash

# Weather Service Test Script
# This script tests the weather.request NATS subject with various cities

# Set NATS_URL if not already set
export NATS_URL="${NATS_URL:-nats://localhost:4222}"

echo "================================"
echo "Weather Service NATS Test Suite"
echo "================================"
echo ""

# Color codes for output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Test cities
cities=(
    "San Francisco"
    "Tokyo"
    "London"
    "Paris"
    "New York"
    "Sydney"
)

test_count=0
success_count=0

# Function to test a city
test_city() {
    local city=$1
    test_count=$((test_count + 1))
    
    echo -e "${YELLOW}Test $test_count: Testing city: '$city'${NC}"
    
    if [ -z "$city" ]; then
        echo "  Request: {\"city\": \"\"}"
    else
        echo "  Request: {\"city\": \"$city\"}"
    fi
    
    # Send NATS request with timeout (using nats built-in timeout)
    response=$(nats request weather.request "{\"city\": \"$city\"}" --timeout=5s 2>&1)
    
    if [ $? -eq 0 ] && echo "$response" | grep -q "weather"; then
        echo -e "  ${GREEN}✓ Response received${NC}"
        # Extract just the JSON response
        echo "$response" | grep -o '{"weather.*}' | jq '.' 2>/dev/null || echo "$response" | tail -1
        success_count=$((success_count + 1))
    else
        echo -e "  ${RED}✗ Request failed or timed out${NC}"
    fi
    
    echo ""
}

# Check if NATS CLI is installed
if ! command -v nats &> /dev/null; then
    echo -e "${RED}Error: NATS CLI is not installed${NC}"
    echo "Install it with: brew install nats-io/nats-tools/nats"
    exit 1
fi

# Check if jq is installed (optional)
if ! command -v jq &> /dev/null; then
    echo -e "${YELLOW}Note: jq is not installed (optional for pretty output)${NC}"
    echo "Install it with: brew install jq"
    echo ""
fi

# Check if NATS server is running by trying a simple connection
echo "Checking NATS server connection..."
if docker ps | grep -q nats-server; then
    echo -e "${GREEN}✓ NATS server is running${NC}"
else
    echo -e "${RED}Error: NATS server is not running${NC}"
    echo "Start it with: docker run -d --name nats-server -p 4222:4222 nats:latest --jetstream"
    exit 1
fi
echo ""

# Run tests
for city in "${cities[@]}"; do
    test_city "$city"
    sleep 1
done

# Summary
echo "================================"
echo "Test Summary"
echo "================================"
echo "Total tests: $test_count"
echo -e "Successful: ${GREEN}$success_count${NC}"
echo -e "Failed: ${RED}$((test_count - success_count))${NC}"
echo ""

if [ $success_count -eq $test_count ]; then
    echo -e "${GREEN}All tests passed!${NC}"
    exit 0
else
    echo -e "${YELLOW}Some tests failed${NC}"
    exit 1
fi
