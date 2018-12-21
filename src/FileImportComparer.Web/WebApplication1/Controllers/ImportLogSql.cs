using System;

namespace WebApplication1.Controllers {
    public static class ImportLogSql {

        public static Func<int, string> TodoSql = count => $@"
SELECT TOP {count}  
	t.TodoID,
	t.TodoName ,
	tf.PartnerID, 
	p.PartnerName, 
	t.StartDate ,
	t.EndDate ,
	t.CreatedBy ,
	t.CreatedDate ,
	tl.TodoLogID ,
	tl.MessageID ,
	tl.TotalNumberOfRows ,
	tl.TotalNumberOfCommands ,
	tl.TotalNumberOfErrors ,
	tl.TotalNumberOfDuplicates, 
	tf.FileID AS FileId,
	ISNULL(tf.RealFileName, t.TodoName) AS 'FileName',
	ISNULL(tld.NumberOfEvents, 0) AS 'TotalNumberOfEvents'
 FROM dbo.tTodoLog tl 
 INNER JOIN dbo.tTodo AS t ON tl.TodoID = t.TodoID 
 LEFT JOIN dbo.tTodoFile AS tf ON t.TodoID = tf.TodoID 
 INNER JOIN dbo.tPartner AS p ON p.PartnerID = tf.PartnerID
 LEFT JOIN (SELECT TodoLogID, SUM(NumberOfRows) AS 'NumberOfEvents' 
				FROM dbo.tTodoLogDetail
				GROUP BY TodoLogID) tld ON tl.TodoLogID = tld.TodoLogID
ORDER BY tl.TodoLogID DESC;
";
        public const string TodoByTodoId = @"
SELECT
	t.TodoID,
	t.TodoName ,
	tf.PartnerID, 
	p.PartnerName, 
	t.StartDate ,
	t.EndDate ,
	t.CreatedBy ,
	t.CreatedDate ,
	tl.TodoLogID ,
	tl.MessageID ,
	tl.TotalNumberOfRows ,
	tl.TotalNumberOfCommands ,
	tl.TotalNumberOfErrors ,
	tl.TotalNumberOfDuplicates, 
	ISNULL(tf.RealFileName, t.TodoName) AS 'FileName' 
 FROM dbo.tTodoLog tl 
 INNER JOIN dbo.tTodo AS t ON tl.TodoID = t.TodoID 
 LEFT JOIN dbo.tTodoFile AS tf ON t.TodoID = tf.TodoID 
 INNER JOIN dbo.tPartner AS p ON p.PartnerID = tf.PartnerID
 WHERE t.TodoId = @todoId
ORDER BY tl.TodoLogID DESC;";

        public const string TodoLogSql = @"
select tld.* 
from dbo.tTodoLog tl
join dbo.tTodoLogDetail tld ON tl.TodoLogID = tld.TodoLogID
where tl.TodoID = @todoId
order by tld.EventTypeID";
    }
}