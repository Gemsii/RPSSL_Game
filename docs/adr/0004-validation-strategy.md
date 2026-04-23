# Validation Strategy: FluentValidation at handler boundary

- Status: proposed
- Deciders: Project Team
- Date: 2026-04-20
- Technical story: Provide a consistent validation approach for API requests

## Context and Problem Statement

How will the application validate incoming requests (commands/queries) in a consistent, testable, and maintainable way? The project needs a strategy that works well with the vertical-slice feature organization and that can be centralized later if cross-cutting validation behaviors are required.

## Considered Options

- Use FluentValidation with explicit validator invocation inside each handler.
- Use FluentValidation with a centralized pipeline or middleware that automatically runs validators for requests.
- Use DataAnnotations (attributes) and rely on model binding for validation.
- Manual validation inside controllers/handlers without a formal validator library.

## Proposed decision

Proposal: Adopt FluentValidation as the primary validation library and invoke validators at the handler boundary (each handler calls the relevant validator before executing domain logic). If a centralized pipeline is later introduced (for example when adopting a mediator or middleware pattern), validators can be executed automatically by that pipeline.

## Rationale

- FluentValidation offers expressive, composable rules and good testability for complex validation scenarios.
- Invoking validators at the handler keeps validation colocated with feature logic in the vertical-slice approach and avoids introducing mediator-specific pipeline dependencies now.
- The approach allows later migration to a centralized validation pipeline (or middleware) without changing validator implementations.

## Expected consequences

- Positive: Clear separation between validation and domain logic; easy unit testing of validation rules; no immediate dependency on mediator pipelines.
- Negative: Some duplication of validator invocation code across handlers until a centralized pipeline is added; requires convention and discipline to ensure all handlers execute validators.

## Alternatives considered

- DataAnnotations: simpler for trivial cases but limited expressiveness for complex rules and less suitable for rich validation scenarios.
- Centralized pipeline from the start: provides uniform behavior but would introduce an architectural dependency (mediator/middleware) that the project intends to postpone.

## Migration Path

1. Implement `IValidator<TRequest>` classes using FluentValidation for each request type.
2. Create a small helper or base handler that standardizes validator invocation to reduce duplication.
3. If a centralized pipeline is introduced later, adapt the helper to be superseded by pipeline-based automatic validation.

## Links

- FluentValidation: https://docs.fluentvalidation.net/
