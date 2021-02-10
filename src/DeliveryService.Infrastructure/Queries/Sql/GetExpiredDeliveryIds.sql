SELECT
	Id
FROM DeliveryService.Delivery
WHERE AccessWindow->>'$.EndTime' < UTC_TIMESTAMP()
AND State <> 'Completed' AND State <> 'Expired'