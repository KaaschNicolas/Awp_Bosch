SELECT 
c.DwellTimeStatus,
COUNT(c.DwellTimeStatus) AS CountDwellTimeStatus
FROM (
	SELECT 
	IIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', 
		IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,
		IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,
		1)),
	0) AS DwellTimeStatus
	FROM(
		SELECT 
		s.DwellTimeRed,
		s.DwellTimeYellow,
		DATEDIFF(DAY, t.LastTransferDate, GETDATE()) as DwellTime
		FROM (
			SELECT
			PcbId,
			StorageLocationId,
			CreatedDate As LastTransferDate,
			ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn
			FROM Transfers 
			WHERE CreatedDate > DeletedDate) as t
		INNER JOIN  (SELECT  Id FROM Pcbs WHERE CreatedDate > DeletedDate AND Finalized = 0) AS p ON t.PcbId=p.Id
		INNER JOIN 	(SELECT Id, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
		WHERE rn=1)
		as b )
	as c
GROUP BY c.DwellTimeStatus


SELECT TOP(3)
c.StorageName,
COUNT(c.PcbId) AS CountPcbs,
SUM(CASE WHEN c.DwellTimeStatus = 1 THEN 1 ELSE 0 END) AS CountGreen,
SUM(CASE WHEN c.DwellTimeStatus = 2 THEN 1 ELSE 0 END) AS CountYellow,
SUM(CASE WHEN c.DwellTimeStatus = 3 THEN 1 ELSE 0 END) AS CountRed
FROM (
	SELECT 
	b.PcbId,
	b.StorageName,
	IIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', 
		IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,
		IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,
		1)),
	0) AS DwellTimeStatus
	FROM(
		SELECT
		t.PcbId,
		s.StorageName,
		p.CreatedDate As FailedAt,
		s.DwellTimeRed,
		s.DwellTimeYellow,
		DATEDIFF(DAY, t.LastTransferDate, GETDATE()) as DwellTime
		FROM (
			SELECT
			PcbId,
			CreatedDate As LastTransferDate,
			StorageLocationId,
			ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn
			FROM Transfers 
			WHERE CreatedDate > DeletedDate) as t
		INNER JOIN  (SELECT CreatedDate, Finalized, Id FROM Pcbs WHERE CreatedDate > DeletedDate AND Finalized = 0) AS p ON t.PcbId=p.Id
		INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
		WHERE rn=1) as b )
	as c
GROUP BY c.StorageName
ORDER BY CountPcbs DESC 
