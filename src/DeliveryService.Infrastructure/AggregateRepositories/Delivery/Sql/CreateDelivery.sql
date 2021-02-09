INSERT INTO DeliveryService.Delivery (
    Id,
    CreatedOn,
    UpdatedOn,
    State,
    AccessWindow,
    Recipient,
    `Order`
    )
VALUES (
    @Id,
    @CreatedOn,
    @UpdatedOn,
    @State,
    @AccessWindow,
    @Recipient,
    @Order
    )
