# CRON expression parser
This is a command line tool to parse input cron string and print expanded fields values in table format. For more information about cron see https://en.wikipedia.org/wiki/Cron.

### Examples of cron expressions
Please look at unit tests (CronExprParser.UnitTests/CronExpressionParserTests.cs) for some valid and invalid inputs

### Application build
To build the tool you need to have .NET 5 SDK installed (https://dotnet.microsoft.com/download/dotnet/5.0)

Execute the following command to build the tool for your platform:
1. **Linux**: dotnet publish CronExprParser/CronExprParser.csproj -c Release -o bin/linux-x64 --self-contained true -r linux-x64 -p:PublishSingleFile=true
2. **OS X**: dotnet publish CronExprParser/CronExprParser.csproj -c Release -o bin/osx-x64 --self-contained true -r osx-x64 -p:PublishSingleFile=true

### Examples of usage
#### Input
CronExprParser "*/15 0 1,15 * 1-5 /usr/bin/find" 
#### Output
|||
|---|---|
|minute|0 15 30 45|
|hour|0|
|day of month|1 15|
|month|1 2 3 4 5 6 7 8 9 10 11 12|
|day of week|1 2 3 4 5|
|command|/usr/bin/find|
