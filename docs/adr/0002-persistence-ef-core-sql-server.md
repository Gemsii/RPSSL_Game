# Persistence: Use EF Core + SQL Server and Migrations

- Status: accepted
- Deciders: Project Team
- Date: 2026-04-20
- Technical story: Manage schema and persistence for the RPSSL API

## Context and Problem Statement

Where and how will the application persist its data? To provide a predictable schema evolution path and preserve relational semantics between development and production, the team intends to adopt Entity Framework Core with SQL Server and code-first migrations for production. This ADR records that decision and its implications for development, testing, and deployment.

## Considered Options

- EF Core InMemory provider (for fast local development and unit tests)
- EF Core with SQL Server and code-first migrations (production)

## Decision outcome

Decision: The project has adopted Entity Framework Core with SQL Server and code-first migrations for production. Implementation includes code-first migrations and a seeding strategy; integration and repository tests will be executed against a transient relational test database or a test SQL instance/container to maintain parity with production.

### Rationale

- Choosing SQL Server with EF Core preserves relational semantics (query translation, relational constraints, transactions) and reduces the risk of provider-specific issues surfacing only in production.
- Code-first migrations provide an explicit, versioned path for schema evolution, improving deployment predictability.
- Favoring a relational provider aligns the testing strategy with production behavior and reduces integration surprises.

## Consequences

- Positive: Tests and production will exercise accurate SQL semantics; schema evolution will be tracked via migrations; provider-specific features will be available.
- Negative: Tests that target real SQL behavior will be slower than pure in-memory tests; the project will need practices for DB provisioning and migration workflows and must manage test DB lifecycle (containers or ephemeral instances).

## Alternatives considered

- EF InMemory provider — fast and simple for unit tests and local experimentation, but differs from SQL Server in several behaviors (constraints, LINQ translation, type mappings) which can lead to undetected bugs in integration.
- Other persistence providers (e.g., SQLite, provider-specific in-memory options) were considered but not chosen because the project requires parity with SQL Server semantics in production.
