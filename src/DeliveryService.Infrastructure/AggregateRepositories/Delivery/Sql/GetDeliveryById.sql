SELECT
    Id,
    CreatedOn,
    UpdatedOn,
    State,
    AccessWindow,
    Recipient,
    `Order`
FROM DeliveryService.Delivery
WHERE Id = @Id