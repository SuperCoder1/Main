<Query Kind="SQL">
  <Connection>
    <ID>1f54f1de-c270-41b6-b91d-83303a1c9376</ID>
    <Persist>true</Persist>
    <Server>(local)</Server>
    <IncludeSystemObjects>true</IncludeSystemObjects>
    <Database>PLDMSource2016</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference>C:\tfs\Dev1\packages\Dapper.StrongName.1.50.5\lib\net451\Dapper.StrongName.dll</Reference>
  <Reference Relative="..\..\..\Projects\DevTools\DevTools.Core\bin\Release\DevTools.Core.dll">C:\MAD\Projects\DevTools\DevTools.Core\bin\Release\DevTools.Core.dll</Reference>
  <Reference>C:\tfs\Dev1\PLDM.DataAccess\bin\Release\FastMember.Signed.dll</Reference>
  <Reference Relative="..\..\..\Projects\PLDMTest\packages\HyperDescriptor.1.0.5\lib\net40\Hyper.ComponentModel.dll">C:\MAD\Projects\PLDMTest\packages\HyperDescriptor.1.0.5\lib\net40\Hyper.ComponentModel.dll</Reference>
  <Reference>C:\tfs\Dev1\packages\morelinq.2.10.0\lib\net40\MoreLinq.dll</Reference>
  <Reference>C:\Users\M755411\.nuget\packages\Newtonsoft.Json\11.0.2\lib\net45\Newtonsoft.Json.dll</Reference>
  <Reference>C:\tfs\Dev1\PLDM.DataAccess\bin\Release\PLDM.DataAccess.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.DataAnnotations.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Namespace>DevTools.Core.DataAccess</Namespace>
  <Namespace>FastMember</Namespace>
  <Namespace>MoreLinq</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Data</Namespace>
  <Namespace>System.Diagnostics</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>


--source: Jeff Smith Blog
--https://weblogs.sqlteam.com/jeffs/2004/11/10/2737/

/*

SELECT MIN(TableName) as TableName, ID, COL1, COL2, COL3 ...
FROM
(
  SELECT 'Table A' as TableName, A.ID, A.COL1, A.COL2, A.COL3, ...
  FROM A
  UNION ALL
  SELECT 'Table B' as TableName, B.ID, B.COL1, B.COl2, B.COL3, ...
  FROM B
) tmp
GROUP BY ID, COL1, COL2, COL3 ...
HAVING COUNT(*) = 1
ORDER BY ID


-- Enter the name of the 2 tables to compare data

EXEC dbo.usp_CompareTables 
	@table1 = 'dbo.TestTable', 
	@table2 = 'dbo.TestTable1'
	--@T1ColumnList = 'EmpID, FirstName, LastName, Notes, Status'
*/

ALTER PROCEDURE dbo.usp_CompareTables
(
	@table1 varchar(128),
	@table2 Varchar(128),
	@T1ColumnList varchar(8000) = null,
	@T2ColumnList varchar(8000) = null
)
AS
 
-- Table1, Table2 are the tables or views to compare.
-- T1ColumnList is the list of columns to compare, from table1.
-- Just list them comma-separated, like in a GROUP BY clause.
-- If T2ColumnList is not specified, it is assumed to be the same
-- as T1ColumnList.  Otherwise, list the columns of Table2 in
-- the same order as the columns in table1 that you wish to compare.
--
-- The result is all rows from either table that do NOT match
-- the other table in all columns specified, along with which table that
-- row is from.
 
declare @SQL varchar(8000);

IF ISNULL(@t1ColumnList,'') = ''
BEGIN
	--TODO: Get the column list
	DECLARE @columnListSQL varchar(MAX) = ''

    SELECT @columnListSQL = FORMATMESSAGE('DECLARE @columnList varchar(MAX)
	SELECT @columnList = COALESCE(@columnList+'','', '''') + column_name FROM %s.INFORMATION_SCHEMA.Columns'+
	' WHERE TABLE_NAME = ''%s'''+
	' ORDER BY ORDINAL_POSITION;'+
	' SELECT @columnList;',
		DB_NAME(), PARSENAME(@table1, 1))
	
    CREATE TABLE #ColList (list VARCHAR(MAX));
	
	--SELECT @columnListSQL

	INSERT INTO #ColList(list) EXEC (@columnListSQL)
	SELECT TOP 1 @t1ColumnList = list FROM #ColList
END

SELECT @t1ColumnList AS [@t1ColumnList]

IF ISNULL(@t2ColumnList,'') = '' 
	SET @T2ColumnList = @T1ColumnList
 
set @SQL = 'SELECT ''' + @table1 + ''' AS TableName, ' + @t1ColumnList +
 ' FROM ' + @Table1 + ' UNION ALL SELECT ''' + @table2 + ''' As TableName, ' +
 @t2ColumnList + ' FROM ' + @Table2
 
set @SQL = 'SELECT Max(TableName) as TableName, ' + @t1ColumnList +
 ' FROM (' + @SQL + ') A GROUP BY ' + @t1ColumnList +
 ' HAVING COUNT(*) = 1'
 
exec (@SQL)

