# SFA Find an Apprenticeship #

**FAA-Candidate** is the web front-end used to search and apply for Apprenticeships and Traineeships in England. Users of the service can register to receive search alerts, status updates and reminders.

This repository contains the code for the service which is hosted on [gov.uk](https://www.findapprenticeship.service.gov.uk/apprenticeshipsearch) and is operated by the [Skills Funding Agency](https://www.gov.uk/government/organisations/skills-funding-agency).

## Overview ##
The code follows a [clean code architecture](https://www.google.co.uk/#q=clean%20code%20architecture) so the code is organised into **Domain**, **Application** and **Infrastructure** service projects.

The live service runs on the [Azure](http://azure.microsoft.com) platform and integrates with:

## Web sites ##

- Find An Apprenticeship
	- https://www.findapprenticeship.service.gov.uk/
- Recruit An Apprenticeship
	- https://recruit.findapprenticeship.service.gov.uk
- Manage An Apprenticeship
	- https://manage.findapprenticeship.service.gov.uk
- Web Proxy
	- Azure Web Application that deploys directly from the Github master

## Azure worker roles ##

- Migrate
- Migrate FAA
- Log Event Indexer
- Processes
- Scheduled Jobs
	- Application Status Pages
	- Send Daily Digests
	- Send Saved Search Alerts
	- Queue Candidates
	- Application House Keeper
	- Communication House Keeper
	- Candidate Saved Searches
	- Apprenticeship Vacancy Indexer
	- Traneeship Vacancy Indexer

## Dependencies ##

- [PCA Predict](http://www.pcapredict.com/en-gb/index/) for address lookups
- [SendGrid](https://sendgrid.com/) for authoring and dispatching emails
- [Reach](https://www.reach-interactive.com/) for dispatching SMS messages

The web layer is built predominantly in C# and [ASP.NET MVC 5.x](http://www.asp.net/mvc)

Infrastructure components include:

- [MongoDB](https://www.mongodb.org/)
- [RabbitMq](https://www.rabbitmq.com/)
- [ElasticSearch](https://www.elastic.co/products/elasticsearch)
- [Logstash](https://www.elastic.co/products/logstash)
- [Kibana](https://www.elastic.co/products/kibana)


## Running the application ##

**FAA-Candidate** requires certain private configuration settings to be able to run. If you don't have access to this info then you won't be able to run the application.

## License

Licensed under the [MIT license](LICENSE)