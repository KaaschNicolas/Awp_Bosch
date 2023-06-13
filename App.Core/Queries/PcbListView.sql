-- Basis Abfrage für Leiterplatten-Listenansicht
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
b.MainErrorCode,
b.SubErrorCode,
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
	DATEDIFF(DAY, t.LastTransferDate, GETDATE()) as DwellTime,
	e.MainErrorCode,
	e.SubErrorCode
	FROM (
		SELECT
		PcbId,
		CreatedDate As LastTransferDate,
		StorageLocationId,
		ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn,
		COUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount,
		ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY PcbId) AS ErrorCount
		FROM Transfers 
		WHERE CreatedDate > DeletedDate) as t
	INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON t.PcbId=p.Id
	INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
	INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id
	INNER JOIN (SELECT * FROM (
					SELECT
					PcbId,
					FIRST_VALUE(Code) OVER (PARTITION BY PcbId ORDER BY CreatedDate) AS MainErrorCode,
					FIRST_VALUE(Code) OVER (PARTITION BY PcbId ORDER BY CreatedDate DESC) AS SubErrorCode,
					ROW_NUMBER() OVER (PARTITION BY PcbId ORDER BY CreatedDate) AS ErrorRowNumber
					FROM ErrorTypes
					WHERE CreatedDate > DeletedDate AND PcbId IS NOT NULL) AS err
				WHERE err.ErrorRowNumber = 1
				) AS e on p.Id = e.PcbId
	WHERE rn=1) as b 


SELECT * FROM (
	SELECT
	PcbId,
	FIRST_VALUE(Code) OVER (PARTITION BY PcbId ORDER BY CreatedDate) AS MainErrorCode,
	FIRST_VALUE(Code) OVER (PARTITION BY PcbId ORDER BY CreatedDate DESC) AS SubErrorCode,
	ROW_NUMBER() OVER (PARTITION BY PcbId ORDER BY CreatedDate) AS ErrorRowNumber
	FROM ErrorTypes
	WHERE CreatedDate > DeletedDate AND PcbId IS NOT NULL) AS err


SELECT * FROM ErrorTypes WHERE CreatedDate > DeletedDate AND PcbId IS NOT NULL 

-- LIKE QUERY
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
b.MainErrorCode,
b.SubErrorCode,
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
	DATEDIFF(DAY, t.LastTransferDate, GETDATE()) as DwellTime,
	e.MainErrorCode,
	e.SubErrorCode
	FROM (
		SELECT
		PcbId,
		CreatedDate As LastTransferDate,
		StorageLocationId,
		ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn,
		COUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount,
		ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY PcbId) AS ErrorCount
		FROM Transfers 
		WHERE CreatedDate > DeletedDate) as t
	INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate AND SerialNumber LIKE '%12%') AS p ON t.PcbId=p.Id
	INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
	INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id
	INNER JOIN (SELECT * FROM (
					SELECT
					PcbId,
					FIRST_VALUE(Code) OVER (PARTITION BY PcbId ORDER BY CreatedDate) AS MainErrorCode,
					FIRST_VALUE(Code) OVER (PARTITION BY PcbId ORDER BY CreatedDate DESC) AS SubErrorCode,
					ROW_NUMBER() OVER (PARTITION BY PcbId ORDER BY CreatedDate) AS ErrorRowNumber
					FROM ErrorTypes
					WHERE CreatedDate > DeletedDate AND PcbId IS NOT NULL) AS err
				WHERE err.ErrorRowNumber = 1
				) AS e on p.Id = e.PcbId
	WHERE rn=1) as b 


-- WHERE Filter auf Leiterplatte
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
b.MainErrorCode,
b.SubErrorCode,
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
	DATEDIFF(DAY, t.LastTransferDate, GETDATE()) as DwellTime,
	e.MainErrorCode,
	e.SubErrorCode
	FROM (
		SELECT
		PcbId,
		CreatedDate As LastTransferDate,
		StorageLocationId,
		ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY CreatedDate DESC) AS rn,
		COUNT(PcbId) OVER(PARTITION BY PcbId) AS TransferCount,
		ROW_NUMBER() OVER(PARTITION BY PcbId ORDER BY PcbId) AS ErrorCount
		FROM Transfers 
		WHERE CreatedDate > DeletedDate) as t
	INNER JOIN  (SELECT SerialNumber, CreatedDate, Finalized, Id, PcbTypeId FROM Pcbs WHERE CreatedDate > DeletedDate AND CreatedDate = GETDATE()) AS p ON t.PcbId=p.Id
	INNER JOIN 	(SELECT Id, StorageName, DwellTimeRed, DwellTimeYellow FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
	INNER JOIN (SELECT Id, PcbPartNumber FROM PcbTypes) AS pt ON p.PcbTypeId = pt.Id
	INNER JOIN (SELECT * FROM (
					SELECT
					PcbId,
					FIRST_VALUE(Code) OVER (PARTITION BY PcbId ORDER BY CreatedDate) AS MainErrorCode,
					FIRST_VALUE(Code) OVER (PARTITION BY PcbId ORDER BY CreatedDate DESC) AS SubErrorCode,
					ROW_NUMBER() OVER (PARTITION BY PcbId ORDER BY CreatedDate) AS ErrorRowNumber
					FROM ErrorTypes
					WHERE CreatedDate > DeletedDate AND PcbId IS NOT NULL) AS err
				WHERE err.ErrorRowNumber = 1
				) AS e on p.Id = e.PcbId
	WHERE rn=1) as b 
