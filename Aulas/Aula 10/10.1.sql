/*
DBCC FREEPROCCACHE;
DBCC DROPCLEANBUFFERS;
*/

-- SELECT * FROM Production.WorkOrder
-- SELECT * FROM Production.WorkOrder WHERE WorkOrderID=1234
-- SELECT * FROM Production.WorkOrder WHERE WorkOrderID between 10000 and 10010
-- SELECT * FROM Production.WorkOrder WHERE WorkOrderID between 1 and 72591
-- SELECT * FROM Production.WorkOrder WHERE StartDate = '2007-06-25'

-- CREATE INDEX IX_WordOrder_ProductID ON Production.WorkOrder(ProductID)
-- SELECT * FROM Production.WorkOrder WHERE ProductID = 757
-- DROP INDEX Production.WorkOrder.IX_WordOrder_ProductID

-- CREATE INDEX IX_WordOrder_ProductID_Covering_StartDate ON Production.WorkOrder(ProductID) INCLUDE (StartDate)
-- SELECT WorkOrderID, StartDate FROM Production.WorkOrder WHERE ProductID = 757
-- SELECT WorkOrderID, StartDate FROM Production.WorkOrder WHERE ProductID = 945
-- SELECT WorkOrderID FROM Production.WorkOrder WHERE ProductID = 945 AND StartDate = '2006-01-04'
-- DROP INDEX Production.WorkOrder.IX_WordOrder_ProductID_Covering_StartDate

-- CREATE INDEX IX_WordOrder_ProductID ON Production.WorkOrder(ProductID)
-- CREATE INDEX IX_WordOrder_StartDate ON Production.WorkOrder(StartDate)
-- SELECT WorkOrderID, StartDate FROM Production.WorkOrder WHERE ProductID = 945 AND StartDate = '2006-01-04'
-- DROP INDEX Production.WorkOrder.IX_WordOrder_ProductID
-- DROP INDEX Production.WorkOrder.IX_WordOrder_StartDate

-- CREATE INDEX IX_WordOrder_ProductID_StartDate ON Production.WorkOrder(ProductID, StartDate)
-- SELECT WorkOrderID, StartDate FROM Production.WorkOrder WHERE ProductID = 945 AND StartDate = '2006-01-04'
-- DROP INDEX Production.WorkOrder.IX_WordOrder_ProductID_StartDate