<Query Kind="SQL">
  <Connection>
    <ID>c107d803-c54b-47bb-979c-49c55697bc03</ID>
    <Persist>true</Persist>
    <Server>PLDMDEV01LND\PLDMDEVLND</Server>
    <NoPluralization>true</NoPluralization>
    <NoCapitalization>true</NoCapitalization>
    <IncludeSystemObjects>true</IncludeSystemObjects>
    <DisplayName>Landing - DEV01</DisplayName>
    <Database>PLDMSource</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\MAD\Projects\DevTools\DevTools.Core\bin\Release\DevTools.Core.dll</Reference>
  <Reference>C:\MAD\Projects\PLDMTest\packages\FastMember.1.1.0\lib\net40\FastMember.dll</Reference>
  <Reference>C:\MAD\Projects\PLDMTest\packages\HyperDescriptor.1.0.5\lib\net40\Hyper.ComponentModel.dll</Reference>
  <Namespace>DevTools.Core.DataAccess</Namespace>
  <Namespace>FastMember</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Data</Namespace>
  <Namespace>System.Diagnostics</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

USE PLDMSource
GO

-- Get Table names, row counts, and compression status for clustered index or heap

SELECT 
DB_NAME() AS [Database],
OBJECT_NAME(object_id) AS [Table Name], 
SUM(Rows) AS [RowCount], data_compression_desc AS [CompressionType]
FROM sys.partitions WITH (NOLOCK)
WHERE index_id < 2 --ignore the partitions from the non-clustered index if any
AND OBJECT_NAME(object_id) NOT LIKE N'sys%'
AND OBJECT_NAME(object_id) NOT LIKE N'queue_%' 
AND OBJECT_NAME(object_id) NOT LIKE N'filestream_tombstone%' 
AND OBJECT_NAME(object_id) NOT LIKE N'fulltext%'
AND OBJECT_NAME(object_id) NOT LIKE N'ifts_comp_fragment%'
AND OBJECT_NAME(object_id) NOT LIKE N'filetable_updates%'
AND OBJECT_NAME(object_id) NOT LIKE N'xml_index_nodes%'
GROUP BY object_id, data_compression_desc
ORDER BY SUM(Rows) DESC OPTION (RECOMPILE);