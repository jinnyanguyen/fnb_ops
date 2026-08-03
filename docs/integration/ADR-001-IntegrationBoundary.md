# ADR-001

## Title

Use a Vendor-Neutral Integration Model

## Status

Accepted

## Context

Gusto Ops must support importing sales from multiple POS systems.

Each POS exposes different APIs and data formats.

## Decision

All external data will first be mapped into a common RestaurantSaleEvent model before entering the business layer.

The business layer must never depend directly on iPOS DTOs or CSV models.

## Consequences

Positive

- Easier testing
- Easier maintenance
- Future integrations require minimal changes
- Reusable business logic

Negative

- Additional mapping layer