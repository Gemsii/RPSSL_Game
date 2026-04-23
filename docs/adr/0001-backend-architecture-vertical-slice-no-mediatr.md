# Backend Architecture: Vertical-slice features, explicit handlers (no MediatR)

- Status: proposed
- Deciders: Project Team
- Date: 2026-04-20
- Technical story: Organize API features for clarity, testability, and minimal indirection

## Context and Problem Statement

Determine the appropriate request-handling and feature organization for the API to balance clarity, testability, and support for cross-cutting concerns. This ADR frames that architectural question and records the candidate approaches to evaluate before selecting an implementation.

## Considered Options

- Vertical-slice, explicit handler registration
- Vertical-slice with MediatR (mediator pattern and pipelines)
- Layered controllers/services (traditional controllers delegating to services)

## Proposed decision

Proposal: Adopt a vertical-slice organization with explicit handler classes and direct DI registration. Do not introduce MediatR as part of the initial implementation.

### Why this is proposed

- Simplicity: Explicit handlers and direct DI make the control flow straightforward and easier to follow for new contributors.
- Reduced dependency surface: Avoid adding an external library and the conceptual overhead of mediator patterns and pipeline behaviors for the initial project scope.
- Performance and clarity: Direct invocation reduces indirection, simplifies debugging, and produces clearer stack traces.
- Testability: Handlers remain plain classes that can be constructed or resolved from DI for unit tests; cross-cutting behaviors can later be implemented with decorators if needed.

## Expected consequences

- Positive: Easier onboarding, less runtime indirection, and fewer external dependencies.
- Negative: Lacks the out-of-the-box pipeline behaviors and unified pre/post processing that a mediator like MediatR would provide; if extensive cross-cutting concerns emerge, consider introducing decorators or adopting MediatR incrementally.

## Alternatives considered

- MediatR: Offers a mature mediator and pipeline behaviors for cross-cutting concerns (logging, validation, retry). Rejected initially to keep the stack small and code paths explicit.
- Traditional controllers/services: Rejected in favor of vertical-slice to colocate feature code and minimize coupling.

## Migration Path

If cross-cutting behavior requirements grow (e.g., many features need the same pre/post processing), we can:

1. Introduce decorator patterns or middleware to implement the behavior without a full mediator.
2. Or adopt MediatR incrementally for features that benefit from pipeline behaviors.
