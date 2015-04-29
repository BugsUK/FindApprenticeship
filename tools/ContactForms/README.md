# Contact Forms #

## Introduction ##

This project is intended to replace the existing employer contact forms with a more readily maintainable, in house solution.

There are three existing forms:

- [Employer Enquiry Form](http://nas.apprenticeships.org.uk/employers/employer-online-enquiry-form.aspx)
- [GLA Employer Enquiry Form](http://nas.apprenticeships.org.uk/employers/GLA-Employer-Enquiry-Form)
- Web Access Request Form

These are replaced by the following (temporary) urls:

- [Employer Enquiry Form](https://sfacontactforms.azurewebsites.net/employer-enquiry)
- [GLA Employer Enquiry Form](https://sfacontactforms.azurewebsites.net/gla-employer-enquiry)
- [Web Access Request Form](https://sfacontactforms.azurewebsites.net/access-request)

## Processes ##
### Existing ###

1. User completes the form and submits
2. Code behind generates an XML file with submitted data & drops it to a directory
3. A chronicle job detects the file and is triggered
4. It picks up the XML file and sends an email to SFA CRM system with XML as an attachment
5. If the GLA web page is used additional identifier data is appended to the XML which is used later in SFA system to identify source web page of enquiry.

### New ###

TODO: Complete

### Testing ###

Complete the form using Zee's contact details so for example: 

- Contact Name: Zeenat Mahadik
- Company Name: SFA
- Contact number: 02476 823 499
- Email: zeenat.mahadik@sfa.bis.gov.uk
- Address: National Office, Coventry CV1 2WT
- Nature of enquiry: TEST please assign to Zeenat Mahadik 

Anyone who picks up the lead before Zee does will know to assign these to Zee based on the notes. Zee can then pick these up and let you know if the lead has come through correctly. She has informed the team to give them a heads up so that they are aware to expect these in and that no action is required.

## Important Contacts ##

- [Zeenat Mahadik](mailto://Zeenat.Mahadik@sfa.bis.gov.uk) (Zee) - Primary contact for diagnosing issues with CRM integration
- [Craig Thompson](mailto://Craig.Thompson@sfa.bis.gov.uk) - Old CRM support contact
- nascminbound@apprenticeships.gov.uk - Email to send XML to to it is picked up in the CRM

## Azure ##

The application is configured as a Web App in the Pre-Production subscription.

There are two slots, production and staging. The UI is slightly different to cloud services in that you have to click the triangle next to sfacontactforms to expand the tree and see the staging slot.

## Deployment ##

A link from github to Azure has been created so that the app is automatically compiled and deployed to the staging slot on any push to the master branch.

## Configuration ##

The send grid credentials are stored in the app settings on the configuration tab in Azure.

## Outstanding Work ##

- Document the CRM integration process
- The Web Access Request Form is generating XML but this has not yet been integrated with the CRM. This needs testing so contact Zee to begin the testing process
- Work out either how to make the location lookup work or remove it
- It's unclear if logging is working and, if so, whether we will be alerted in the event of failure. Logging should follow FAA pattern and all submitted information logged in a verbose manner to enable reconstruction of the requests
- Update send grid credentials
