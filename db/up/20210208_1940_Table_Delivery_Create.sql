USE DeliveryService;

CREATE TABLE IF NOT EXISTS Delivery (
    ClusteredId BIGINT AUTO_INCREMENT NOT NULL,
    Id CHAR(36) NOT NULL UNIQUE,
    CreatedOn DATETIME NOT NULL,
    UpdatedOn DATETIME NOT NULL,
    State CHAR(50) NOT NULL,
    AccessWindow JSON NOT NULL,
    Recipient JSON NOT NULL,
    `Order` JSON NOT NULL,
    PRIMARY KEY (ClusteredId)
) ENGINE=INNODB;