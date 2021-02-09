UPDATE DeliveryService.Delivery
    SET
    UpdatedOn = @UpdatedOn,
    State = @State,
    AccessWindow = @AccessWindow,
    Recipient = @Recipient,
    `Order` = @Order
WHERE Id = @Id