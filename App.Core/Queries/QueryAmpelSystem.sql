
SELECT *, IIF(NOT pcb_dwell.DwellTimeYellow = '--' AND NOT pcb_dwell.DwellTimeYellow = '--', 
			IIF (CAST(pcb_dwell.DwellTime AS INT) >= CAST(pcb_dwell.DwellTimeYellow AS INT) AND CAST(pcb_dwell.DwellTime AS INT) < CAST(pcb_dwell.DwellTimeRed AS INT), 2,
			IIF (CAST(pcb_dwell.DwellTime AS INT) >= CAST(pcb_dwell.DwellTimeRed AS INT),3,1)),
		   0) AS Color
FROM
(SELECT *, DATEDIFF(DAY, p_t.LastTransferAt, GETDATE()) AS DwellTime
	FROM 
		(SELECT
			MAX(t.CreatedDate) As LastTransferAt,
			p.CreatedDate AS PcbCreatedAt,
			s.StorageName,
			s.DwellTimeYellow,
			s.DwellTimeRed,
			p.SerialNumber
			FROM Transfers t
				INNER JOIN  (SELECT * FROM Pcbs WHERE CreatedDate > DeletedDate) AS p ON t.PcbId=p.Id
				INNER JOIN 	(SELECT * FROM StorageLocations) AS s ON t.StorageLocationId=s.Id
			GROUP BY t.PcbId, p.[CreatedDate], s.StorageName, s.DwellTimeYellow,s.DwellTimeRed,p.SerialNumber) AS p_t) AS pcb_dwell
ORDER BY Color DESC