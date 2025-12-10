#!/bin/bash

# Generate C# API client from OpenAPI schema using Kiota

set -e

SWAGGER_URL="http://localhost:5053/swagger/v1/swagger.json"
OUTPUT_DIR="Services/Generated"
CLIENT_NAMESPACE="MedizID.UI.Services.Generated"
CLIENT_CLASS_NAME="MedizIdApiClient"

echo "🔄 Generating C# API client from Kiota..."
echo "📍 Source: $SWAGGER_URL"
echo "📁 Output: $OUTPUT_DIR"
echo ""

# Check if kiota is installed
if ! command -v kiota &> /dev/null; then
    echo "❌ Kiota CLI not found. Installing..."
    dotnet tool install -g microsoft.kiota.cli
fi

# Generate the client with enum support
kiota generate \
    --openapi "$SWAGGER_URL" \
    --language CSharp \
    --namespace-name "$CLIENT_NAMESPACE" \
    --class-name "$CLIENT_CLASS_NAME" \
    --output "$OUTPUT_DIR" \
    --clean-output \
    --structured-mime-types "application/json" \
    --backing-store false \
    --additional-data true

echo ""
echo "✅ API client generated successfully!"
echo "📦 Generated files:"
ls -la "$OUTPUT_DIR" | grep -v "^total" | grep -v "^d" | awk '{print "   " $NF}'

echo ""
echo "🔄 Generating enum classes from OpenAPI x-ms-enum extension..."
python3 generate_enums.py

echo ""
echo "✨ Generation complete! You can now use the generated API client."
