# OpenFIS

This is the project that attempts to create API around [FIS ski jumping results database](https://www.fis-ski.com/DB/general/?eventselection=&place=&sectorcode=JP&seasoncode=2022&categorycode=&disciplinecode=&gendercode=&racedate=&racecodex=&nationcode=&seasonmonth=07-2021&saveselection=-1&seasonselection=). Under the hood, the code uses Playwright as a scraper and PostgreSQL to supply a database.

## Requirements

* .NET 5
* PostgreSQL

## Using on your own

1. Clone the repository
2. Set valid PostgreSQL connection string using the ```dotnet user-secrets set "FisDb:ConnectionString" "<connection_string>"```
3. dotnet run


## Endpoints

* GET ```/athlete/{fisCode}``` - gets athlete by given FIS code
* GET ```/athlete/{fisCode}/competitions``` - gets athlete results by given FIS Code
* GET ```/competition/{id}``` - gets CompetitionResult by given id, if not found, the Playwright will be ran, scrape the table, push the result into the database and return it

## TODO

* add unit tests
* add support for DNS/DSQ athletes
* add support for team competitions

## Disclaimer

All the parsed data belongs to FIS.

##  License

MIT