# Contact Forms #

## Introduction ##

This project is intended to replace the existing employer contact forms with a more readily maintainable, in house solution.

There are three existing forms:

- [Employer Enquiry Form](http://nas.apprenticeships.org.uk/employers/employer-online-enquiry-form.aspx)
- [GLA Employer Enquiry Form](http://nas.apprenticeships.org.uk/employers/GLA-Employer-Enquiry-Form)
- 

## Important Contacts ##

- [Zeenat Mahadik](mailto://Zeenat.Mahadik@sfa.bis.gov.uk) (Zee) - Primary contact for diagnosing issues with CRM integration
- [Craig Thompson](mailto://Craig.Thompson@sfa.bis.gov.uk) - Old CRM support contact 

## Azure ##

The application is configured as a Web App in the Pre-Production subscription.

There are two slots, production and staging. The UI is slightly different to cloud services in that you have to click the triangle next to sfacontactforms to expand the tree and see the staging slot.

## Deployment ##

A link from github to Azure has been created so that the app is automatically compiled and deployed to the staging slot on any push to the master branch.

## Configuration ##

The send grid credentials are stored in the app settings on the configuration tab in Azure.