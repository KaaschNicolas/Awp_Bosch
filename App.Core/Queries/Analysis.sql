-- Analyse Verweildauer pro Lagerort 
SELECT b.StorageName,
       ROUND(AVG(CAST(DwellTime AS FLOAT)), 2) AS AvgDwellTime
FROM
  (SELECT PcbId,
          t.Id,
          CreatedDate AS TransferDate,
          StorageLocationId,
          s.StorageName,
          DATEDIFF(DAY, CreatedDate, lag(CreatedDate, 1, GETDATE()) OVER(PARTITION BY PcbId
                                                                         ORDER BY PcbId DESC, CreatedDate DESC)) AS DwellTime
   FROM Transfers AS t
   INNER JOIN
     (SELECT Id,
             StorageName
      FROM StorageLocations) AS s ON s.Id = t.StorageLocationId
   WHERE CreatedDate > DeletedDate) AS b
GROUP BY b.StorageName

