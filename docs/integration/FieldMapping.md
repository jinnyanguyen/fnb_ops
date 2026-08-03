# iPOS Field Mapping

| iPOS Field | Description | Gusto Ops Property | Required | Notes |
|------------|-------------|--------------------|----------|-------|
| sale_id | External sale ID | ExternalSaleId | Yes | Unique identifier |
| brand_id | Brand | BrandId | Yes | Required for uniqueness |
| store_id | Store | StoreId | Yes | Branch mapping |
| tran_date | Sale date | SaleDate | Yes | Unix timestamp |
| datastate | New/Edit/Delete | SyncAction | Yes | Controls synchronization |
| item_id | Menu Item | ProductCode | Yes | Recipe lookup |
| quantity | Quantity Sold | Quantity | Yes | Inventory deduction |
| price_sale | Selling Price | UnitPrice | Yes | Financial reporting |
| payment_method_id | Payment Type | PaymentMethod | Optional | Reporting |