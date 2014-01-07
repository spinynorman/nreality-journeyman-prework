DECLARE @i INT
SET @i = 1

WHILE @i <= 100
BEGIN
	IF ((@i % 3 = 0) AND (@i % 5 = 0)) 
		print 'FizzBuzz'
	ELSE IF (@i % 3 = 0)
		print 'Fizz'
	ELSE IF (@i % 5 = 0)
		print 'Buzz'
	ELSE
		print @i
	
	SET @i = @i + 1
END