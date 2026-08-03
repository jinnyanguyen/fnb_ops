# Gusto Ops Integration Architecture

## Overview

The Gusto Ops Platform supports importing restaurant sales from multiple external sources.

Current supported sources:

- CSV Import
- iPOS API (planned)

Future supported sources:

- Toast POS
- Square POS
- Lightspeed POS
- Manual Entry
- Other third-party integrations

---

## Design Goals

The integration layer is designed to:

- Separate external systems from business logic.
- Allow multiple data sources.
- Reuse existing inventory deduction logic.
- Prevent duplicate imports.
- Support future POS integrations without modifying the business layer.

---

## High-Level Architecture

External Source

↓

Importer

↓

RestaurantSaleEvent

↓

SalesImportService

↓

Inventory

↓

Dashboard

↓

Reports

---

## Components

### Importers

Responsible for retrieving data.

Examples:

- CsvSalesImporter
- IPosApiImporter

---

### Mapping

Responsible for converting external formats into internal events.

Examples:

- CsvMapper
- IPosMapper

---

### Business Layer

Receives RestaurantSaleEvent objects.

Business logic never knows where the sale originated.

---

## Benefits

- Loose coupling
- Testability
- Extensibility
- Vendor independence