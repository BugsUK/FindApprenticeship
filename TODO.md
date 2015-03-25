# NAS Exemplar Tech Debt List #

Dev work that is not covered by backlog stories or TODO comments in the code. 

## Web layer ##



## Service layer ##

- Lock down providers to "internal", web, application and infrastructure to enforce structural pattern, see [http://msdn.microsoft.com/en-us/library/system.runtime.compilerservices.internalsvisibletoattribute%28v=vs.110%29.aspx](http://msdn.microsoft.com/en-us/library/system.runtime.compilerservices.internalsvisibletoattribute%28v=vs.110%29.aspx) for application providers
    - defer until we know more about host processes required in phase 2

## Infrastructure layer ##

- log request/response payloads for nas gateway calls
- change to entity repos (re. Mark). E.g. Consider renaming GenericMongoClient to MongoRepositoryBase; move MongoDB code out of into new MongoClient class; MongoRepositoryBase (and other future repos that may not be based on Domain EntityBase) would consume MongoClient (via IoC).

## Cross cutting ##


## WebOps

- Configure remote powershell from Build Servers to Deployment Server to use Certificates over file system stored encrypted user details.   
- Merge build and management networks (See Simon)

----------

# Done #

- candidate registration should be queued (need to consider applying if not registered)
- replace AD with user auth repo
- some website URLs need to be reviewed to be more "friendly". e.g. vacancy detail should be /vacancy/12345 not /vacancysearch/details/446897
- controller actions should provide caching hints
- refactor azure message queue types
- integrate revised vacancy summary service
- integrate revised vacancy detail service
- integrate application update service
- integrate gateway certificates
- logging levels should be used in accordance with article on wiki
- logging should be called consistently across components (i.e. volume of log entries)
- logging should include an identifier which can be used to correlate a user's activity during a session (NLog MDC)
- remove legacy reference data service
- controller actions should use async where possible
- validation summary links need to be clicked twice
- ensure autosave interval and other settings are set to production values
- need to trim user input data, e.g. http://stackoverflow.com/questions/1718501/asp-net-mvc-best-way-to-trim-strings-after-data-entry-should-i-create-a-custo
- need to consider turning off integration tests against NAS Gateway services once we are hitting the live service
- solution should be executable when disconnected from platform (i.e. standalone)
- demo website should use separate configuration (e.g. databases, settings, etc.)
- refactor: controllers should use providers to avoid containing orchestration logic
- Razor view unit tests (Create example use)
- Multiple PreFetchCount values for each queue. Intetrnal processes should be able to have a much higher value than ones that talk to the gateway- 
- Review and increase heap allocation for elasticsearch. Production is 1Gb and we have 4Gb of free memory available.
- review ApplyWebTrends attribute - use on controller or apply globally (reviewed all attribute usage)
- add a "contact message" collection to the communication repository
- Upgrade TeamCity

# Descoped #

- write bundle orderer for bundle.config
- replace address lookup with public service. No longer available - need to investigate other options with the agency
