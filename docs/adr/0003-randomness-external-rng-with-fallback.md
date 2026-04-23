# Randomness: External RNG with resilient fallback

- Status: accepted
- Deciders: Project Team
- Date: 2026-04-20
- Technical story: Provide random choices for computer player in game logic

## Context and Problem Statement

How will the application source randomness for the computer player? To support auditability and provide a uniform distribution of choices across deployments, the team intends to adopt an external RNG service as the primary source of randomness in production. The implementation will include retry and fallback behavior to ensure availability. This ADR records that planned decision and its implications for testing and operations.

## Considered Options

- Use external RNG service as primary source, fallback to local RNG on failure (current)
- Use local `Random.Shared` only (no external dependency)
- Use seeded deterministic RNG for reproducible test runs

## Decision outcome

Decision: The project will use an external RNG service as the primary source of randomness in production to support auditability and uniform distribution. The implementation will include configurable retries with exponential backoff; if the external service is unavailable after retries, the system will fall back to `Random.Shared`. An `IRandomNumberService` abstraction will be used to allow test doubles and to enable provider replacement.

## Consequences

- Positive: Centralized randomness enables potential auditability, consistent distribution across deployments, and the ability to swap providers without changing consumers.
- Negative: Introduces a runtime dependency on an external service (latency and availability concerns); requires robust retry/fallback behavior and appropriate monitoring; unit and integration tests must use injected deterministic doubles or mocks to remain reliable.

## Alternatives considered

- Local-only `Random.Shared` — simpler and without external dependencies, but less auditable and may produce inconsistencies across environments.
- Seeded deterministic RNG — useful for fully reproducible test runs but not desirable for production randomness.
