DECLARE 
	@columns NVARCHAR(MAX) = '',
	@sql     NVARCHAR(MAX) = '',
	@start	 DATETIME = '',
	@end	 DATETIME = '';

SELECT
	@columns+=QUOTENAME(PcbPartNumber) + ','
FROM PcbTypes
ORDER BY PcbPartNumber;

SET @columns = LEFT(@columns, LEN(@columns) - 1);
SET @start = '2023-06-24 00:00:00';
SET @end = '2023-06-25 00:00:00';


SET @sql = '
SELECT
*
FROM (
SELECT 
StorageName,
c.PcbPartNumber,
COUNT(StorageName) OVER(PARTITION BY c.PcbPartNumber) AS CountPcbs
FROM (
	SELECT 
	StorageName,
	PcbPartNumber,
	rn
	FROM (
		SELECT
		PcbId,
		t.CreatedDate AS LastTransferDate,
		StorageName,
		pt.PcbPartNumber,
		ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY t.CreatedDate DESC) AS rn,
		COUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount
		FROM Transfers  AS t
		INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON t.PcbId=p.Id
		INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
		INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id
		WHERE t.CreatedDate > t.DeletedDate AND t.CreatedDate > convert(date,''' + Cast(@start AS VARCHAR(50)) + ''' ,103)  AND t.CreatedDate <= convert(date,''' + Cast(@end AS VARCHAR(50)) + ''' ,103) ) AS b
	WHERE rn = 1 ) AS c ) AS a
PIVOT (
COUNT([CountPcbs])
FOR [PcbPartNumber]
IN ('+@columns+')

) AS pivottable;';


PRINT @sql


EXEC(@sql)




SELECT 
*  
FROM (
SELECT 
StorageName,
PcbPartNumber,
COUNT(StorageName) OVER(PARTITION BY c.PcbPartNumber) AS CountPcbs
FROM (
	SELECT 
	StorageName,
	PcbPartNumber,
	rn
	FROM (
		SELECT
		PcbId,
		t.CreatedDate AS LastTransferDate,
		StorageName,
		pt.PcbPartNumber,
		ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY t.CreatedDate DESC) AS rn,
		COUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount
		FROM Transfers  AS t
		INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON t.PcbId=p.Id
		INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
		INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id
		WHERE t.CreatedDate > t.DeletedDate AND t.CreatedDate > '2023-06-24 00:00:00'  AND t.CreatedDate <= '2023-06-25 00:00:00') AS b
	WHERE rn = 1 ) as c )  as a
PIVOT (
  Count([CountPcbs])
  FOR [PcbPartNumber]
  IN (
    [1688400308],
    [1688400333],
    [1688400468]
  )
) AS PivotTables


-- query für eingehende und ausgehenden leiterplatten pro storagelocation
SELECT
StorageName,
PcbPartNumber,
TotalIncoming,
TotalCurrent,
TotalIncoming - TotalCurrent AS TotalOutgoing
FROM(
	SELECT
	PcbId,
	StorageName,
	PcbPartNumber,
	TotalIncoming,
	COUNT(PcbId) OVER(PARTITION BY StorageName) AS TotalCurrent
	FROM(
		SELECT
		PcbId,
		t.CreatedDate AS LastTransferDate,
		StorageName,
		pt.PcbPartNumber,
		ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY t.CreatedDate DESC) AS rn,
		COUNT(StorageName) OVER(PARTITION BY StorageName) AS TotalIncoming
		FROM Transfers  AS t
		INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON t.PcbId=p.Id
		INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
		INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id
	WHERE t.CreatedDate > t.DeletedDate AND t.CreatedDate > '2023-06-24 00:00:00'  AND t.CreatedDate <= '2023-06-25 00:00:00' ) AS b
WHERE rn = 1 ) as c




-- Query für Analyse von Eingang/Ausgang/Aktuell pro PVB und Sachnummer

SELECT
*
FROM (
	SELECT
	StorageName  + ' ' + TotalName AS PVBMovement ,
	PcbPartNumber,
	TotalValue
	FROM (
		SELECT
		StorageName,
		PcbPartNumber,
		TotalName,
		TotalValue
		FROM ( 
			SELECT
			StorageName,
			PcbPartNumber,
			TotalIncoming AS Eingang,
			TotalCurrent AS Aktuell,
			TotalIncoming - TotalCurrent AS Ausgang
			FROM(
				SELECT
				StorageName,
				PcbPartNumber,
				COUNT(rn) AS TotalIncoming,
				SUM(CASE WHEN rn = 1 THEN 1 ELSE 0 END) AS TotalCurrent
				FROM(
						SELECT
						PcbId,
						t.CreatedDate AS LastTransferDate,
						StorageName,
						pt.PcbPartNumber,
						ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY t.CreatedDate DESC) AS rn
		
						FROM Transfers  AS t
						INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON t.PcbId=p.Id
						INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
						INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id -- hier die sachnummern filtern
					WHERE t.CreatedDate > t.DeletedDate AND t.CreatedDate > '2023-06-24 00:00:00'  AND t.CreatedDate <= '2023-06-25 00:00:00') AS c -- hier das start und enddatum dynamisch machen
				GROUP BY StorageName, PcbPartNumber ) AS g 
			 )
		AS s 
		unpivot
		(
		  TotalValue
		  for TotalName in (Aktuell, Eingang, Ausgang)
		) AS unpiv 
	) AS j
) AS f
PIVOT (
  MAX([TotalValue])
  FOR [PcbPartNumber]
  IN ( -- das muss dynamisch generiert werden je nach auswahl der sachnummern
    [1688400308],
    [1688400333],
    [1688400468]
  )
) AS PivotTables