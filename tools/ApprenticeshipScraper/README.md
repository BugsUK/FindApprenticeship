# FAA Apprenticeship Scraper #

## Context ##

When we migrated the AVMS database to AVMS+ we wanted the make sure the vacancies displayed on the Find an apprenticeship matches vacancies from before the migration.

## Usage ##

```
FAAScraper.exe -s PRE -d "C:\some\folder" --force
```

This scrapes pages from https://pre.findapprenticeship.service.gov.uk into c:\some\folder and force doesn't prompt if you want to scrape each page type.

* Apprenticeship Results
* Apprenticeship Details
* Traineeship Results
* Traineeship Details