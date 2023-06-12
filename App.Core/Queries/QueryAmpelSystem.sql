SELECT 
b.PcbId,
b.StorageName,
b.DwellTime,
b.DwellTimeRed,
b.DwellTimeYellow,
b.FailedAt,
b.Finalized as IsFinalized,
b.SerialNumber,
b.TransferCount,
b.PcbPartNumber,
IIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', 
	IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,
	IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,
	1)),
0) AS DwellTimeStatus
FROM(
	SELECT t.*,
	s.StorageName,
	p.SerialNumber,
	p.Finalized,
	p.CreatedDate As FailedAt,
	s.DwellTimeRed,
	s.DwellTimeYellow,
	pt.PcbPartNumber,
	DATEDIFF(DAY, t.LastTransferDate, GETDATE()) as DwellTime 
	FROM (
		SELECT
		PcbId,
		CreatedDate As LastTransferDate,
		StorageLocationId,
		ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn,
		COUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount
		FROM Transfers 
		WHERE CreatedDate > DeletedDate) as t
	INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON t.PcbId=p.Id
	INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
	INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id
	WHERE rn=1) as b 



-- LIKE QUERY --
SELECT 
b.PcbId,
b.StorageName,
b.DwellTime,
b.DwellTimeRed,
b.DwellTimeYellow,
b.FailedAt,
b.Finalized as IsFinalized,
b.SerialNumber,
b.TransferCount,
b.PcbPartNumber,
IIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', 
	IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,
	IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,
	1)),
0) AS DwellTimeStatus
FROM(
	SELECT t.*,
	s.StorageName,
	p.SerialNumber,
	p.Finalized,
	p.CreatedDate As FailedAt,
	s.DwellTimeRed,
	s.DwellTimeYellow,
	pt.PcbPartNumber,
	DATEDIFF(day, t.LastTransferDate, GETDATE()) as DwellTime 
	FROM (
		SELECT
		PcbId,
		CreatedDate As LastTransferDate,
		StorageLocationId,
		ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn,
		COUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount
		FROM Transfers 
		WHERE CreatedDate > DeletedDate) as t
	INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate AND SerialNumber LIKE '%123%') AS p ON t.PcbId=p.Id
	INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
	INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id
	WHERE rn=1) as b 


-- WHERE FILTER AUF PCB --
SELECT 
b.PcbId,
b.StorageName,
b.DwellTime,
b.DwellTimeRed,
b.DwellTimeYellow,
b.FailedAt,
b.Finalized as IsFinalized,
b.SerialNumber,
b.TransferCount,
b.PcbPartNumber,
IIF(NOT b.DwellTimeYellow = '--' AND NOT b.DwellTimeYellow = '--', 
	IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeYellow AS INT) AND CAST(b.DwellTime AS INT) < CAST(b.DwellTimeRed AS INT), 2,
	IIF (CAST(b.DwellTime AS INT) >= CAST(b.DwellTimeRed AS INT), 3,
	1)),
0) AS DwellTimeStatus
FROM(
	SELECT t.*,
	s.StorageName,
	s.Id,
	p.SerialNumber,
	p.Finalized,
	p.CreatedDate As FailedAt,
	s.DwellTimeRed,
	s.DwellTimeYellow,
	pt.PcbPartNumber,
	DATEDIFF(day, t.LastTransferDate, GETDATE()) as DwellTime 
	FROM (
		SELECT
		PcbId,
		CreatedDate As LastTransferDate,
		StorageLocationId,
		ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn,
		COUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount
		FROM Transfers 
		WHERE CreatedDate > DeletedDate) as t
	INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate AND CreatedDate = GetDate) AS p ON t.PcbId=p.Id
	INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
	INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id
	WHERE rn=1) as b 
