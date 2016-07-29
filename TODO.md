# NAS Exemplar Tech Debt List #

Dev work that is not covered by backlog stories or TODO comments in the code. 

## Service layer ##

- Lock down providers to "internal", web, application and infrastructure to enforce structural pattern, see [http://msdn.microsoft.com/en-us/library/system.runtime.compilerservices.internalsvisibletoattribute%28v=vs.110%29.aspx](http://msdn.microsoft.com/en-us/library/system.runtime.compilerservices.internalsvisibletoattribute%28v=vs.110%29.aspx) for application providers
    - defer until we know more about host processes required in phase 2

## Infrastructure layer ##

- log request/response payloads for nas gateway calls
- change to entity repos (re. Mark). E.g. Consider renaming GenericMongoClient to MongoRepositoryBase; move MongoDB code out of into new MongoClient class; MongoRepositoryBase (and other future repos that may not be based on Domain EntityBase) would consume MongoClient (via IoC).


## WebOps ##

- Configure remote powershell from Build Servers to Deployment Server to use Certificates over file system stored encrypted user details.   
- Merge build and management networks (See Simon)


## Descoped ##

- write bundle orderer for bundle.config
- replace address lookup with public service. No longer available - need to investigate other options with the agency
