.PHONY: help build clean restore test run run-api run-web docker-up docker-down docker-logs db-migrate db-reset db-create db-drop db-spawn db-setup install watch lint format setup dev teardown

# Load .env file
-include .env
export

# Variables
SOLUTION := MedizID.sln
API_PROJECT := MedizID.API/MedizID.API.csproj
WEB_PROJECT := MedizID.Web/MedizID.Web.csproj
BUILD_CONFIGURATION := Debug
DB_HOST ?= localhost
DB_PORT ?= 5432
DB_NAME ?= mediz_db
DB_USER ?= mediz_user
DB_PASSWORD ?= mediz_password

# Default target
help:
	@echo "MedizID Project - Available Commands"
	@echo "===================================="
	@echo ""
	@echo "Build & Compilation:"
	@echo "  make build              - Build entire solution"
	@echo "  make build-api          - Build API project only"
	@echo "  make build-web          - Build Web project only"
	@echo "  make rebuild            - Clean and rebuild everything"
	@echo "  make clean              - Clean build artifacts"
	@echo ""
	@echo "Package Management:"
	@echo "  make restore            - Restore NuGet packages"
	@echo "  make install            - Restore packages (alias)"
	@echo ""
	@echo "Testing:"
	@echo "  make test               - Run unit tests"
	@echo ""
	@echo "Running Application:"
	@echo "  make run                - Run API and Web projects"
	@echo "  make run-api            - Run API only"
	@echo "  make run-web            - Run Web only"
	@echo "  make watch              - Run with file watcher (API)"
	@echo ""
	@echo "Database:"
	@echo "  make db-spawn           - Start database container"
	@echo "  make db-create          - Create new migration"
	@echo "  make db-migrate         - Apply pending migrations"
	@echo "  make db-reset           - Reset database (WARNING: deletes data)"
	@echo "  make db-drop            - Drop database entirely"
	@echo "  make db-seed            - Seed database with initial data"
	@echo "  make db-setup           - Full database setup (spawn, migrate, seed)"
	@echo ""
	@echo "Docker:"
	@echo "  make docker-up          - Start containers (PostgreSQL, Redis)"
	@echo "  make docker-down        - Stop containers"
	@echo "  make docker-logs        - View container logs"
	@echo "  make docker-build       - Build Docker image"
	@echo ""
	@echo "Code Quality:"
	@echo "  make lint               - Run code analysis"
	@echo "  make format             - Format code"
	@echo ""
	@echo "Utility:"
	@echo "  make health-check       - Check API health"
	@echo "  make swagger            - Open Swagger UI"
	@echo "  make info               - Display project info"
	@echo ""
	@echo "Workflow:"
	@echo "  make setup              - Complete setup (restore, docker, db-migrate)"
	@echo "  make dev                - Start development mode (docker, watch)"
	@echo "  make teardown           - Cleanup (docker-down, clean)"
	@echo ""

# ==================== Build Commands ====================

build:
	@echo "Building solution..."
	dotnet build $(SOLUTION) -c $(BUILD_CONFIGURATION)

build-api:
	@echo "Building API project..."
	dotnet build $(API_PROJECT) -c $(BUILD_CONFIGURATION)

build-web:
	@echo "Building Web project..."
	dotnet build $(WEB_PROJECT) -c $(BUILD_CONFIGURATION)

rebuild: clean build
	@echo "Rebuild complete"

clean:
	@echo "Cleaning build artifacts..."
	dotnet clean $(SOLUTION)
	find . -type d -name "bin" -exec rm -rf {} + 2>/dev/null || true
	find . -type d -name "obj" -exec rm -rf {} + 2>/dev/null || true

# ==================== Package Management ====================

restore:
	@echo "Restoring NuGet packages..."
	dotnet restore $(SOLUTION)

install: restore

# ==================== Testing ====================

test:
	@echo "Running tests..."
	dotnet test $(SOLUTION) --no-build --verbosity normal

# ==================== Running Application ====================

run: run-api

run-api:
	@echo "Starting API (MedizID.API)..."
	dotnet run --project $(API_PROJECT) --no-build

run-web:
	@echo "Starting Web (MedizID.Web)..."
	dotnet run --project $(WEB_PROJECT) --no-build

watch:
	@echo "Running API with file watcher..."
	dotnet watch --project $(API_PROJECT) run

# ==================== Database Commands ====================

db-spawn:
	@echo "Starting PostgreSQL container..."
	docker-compose up -d postgres
	@echo "Waiting for PostgreSQL to be ready..."
	@sleep 3
	@docker-compose exec -T postgres pg_isready -U $(DB_USER) || (sleep 2 && docker-compose exec -T postgres pg_isready -U $(DB_USER))
	@echo "PostgreSQL is ready!"

db-create:
	@echo "Creating new migration..."
	@read -p "Migration name: " migration_name; \
	dotnet ef migrations add $$migration_name --project $(API_PROJECT)

db-migrate:
	@echo "Applying database migrations..."
	dotnet ef database update --project $(API_PROJECT)

db-reset:
	@echo "WARNING: This will drop and recreate the database!"
	@echo "Press Ctrl+C to cancel, or wait 5 seconds..."
	@sleep 5
	dotnet ef database drop --project $(API_PROJECT) --force
	dotnet ef database update --project $(API_PROJECT)

db-drop:
	@echo "WARNING: This will drop the database entirely!"
	@echo "Press Ctrl+C to cancel, or wait 5 seconds..."
	@sleep 5
	dotnet ef database drop --project $(API_PROJECT) --force
	@echo "Database dropped."

db-seed:
	@echo "Seeding database with initial data..."
	dotnet ef database update --project $(API_PROJECT)

db-setup: docker-up db-migrate
	@echo "Database setup complete!"

# ==================== Docker Commands ====================

docker-up:
	@echo "Starting Docker containers (PostgreSQL, Redis)..."
	docker-compose up -d

docker-down:
	@echo "Stopping Docker containers..."
	docker-compose down

docker-logs:
	@echo "Displaying Docker logs..."
	docker-compose logs -f

docker-build:
	@echo "Building Docker image..."
	docker build -t mediz-id-api:latest .

docker-run:
	@echo "Running application in Docker..."
	docker run -p 5000:5000 --network medizid_default mediz-id-api:latest

# ==================== Code Quality ====================

lint:
	@echo "Running code analysis..."
	dotnet format $(SOLUTION) --verify-no-changes --verbosity diagnostic || true

format:
	@echo "Formatting code..."
	dotnet format $(SOLUTION)

# ==================== Utility Commands ====================

health-check:
	@echo "Checking API health..."
	@curl -s http://localhost:5000/health | jq . || echo "API not running or not responding"

swagger:
	@echo "Opening Swagger UI..."
	@open http://localhost:5000/swagger/ui || xdg-open http://localhost:5000/swagger/ui 2>/dev/null || echo "Visit http://localhost:5000/swagger/ui"

info:
	@echo "Project Information:"
	@echo "==================="
	@dotnet --version
	@echo "Solution: $(SOLUTION)"
	@echo "API Project: $(API_PROJECT)"
	@echo "Web Project: $(WEB_PROJECT)"

# ==================== Development Workflow ====================

setup: restore docker-up db-migrate
	@echo "Development environment setup complete!"
	@echo "Run 'make run' to start the application"

dev: docker-up
	@echo "Starting development environment..."
	$(MAKE) watch

teardown: docker-down clean
	@echo "Development environment cleaned up"
