# Tax Management API
API is used for applying taxes on a day based on municipality tax policy

# Application Setup
   Install Below software’s to run the application in any machine
     1) .Net Core 3.1
     2) Visual Studio 2019
     3) Mongo DB 4.4.0
   
   after installing the mongo DB open command prompt and run below scripts from location "C:\Program Files\MongoDB\Server\4.4\bin\mongo.exe"
    
    
    
    
     1) mongod --dbPath C:/Temp/TaxDB (Path can of your choice)
     2) Create DB using the Command db.createCollection('MunicipalityTax')
     3) Insert default Tax data in the collection using the below command
          db.MuncipalityTax.insertMany([
	  {'Muncipality':'Copenhagen','Duration':'Yearly','TaxPriority':'3','StartDate':new Date("2016-01-01"),'EndDate':new Date('2016-12-31'),'TaxRate':'0.2'},
	  {'Muncipality':'Copenhagen','Duration':'Monthly','TaxPriority':'2','StartDate':new Date("2016-05-01"),'EndDate':new Date('2016-05-31'),'TaxRate':'0.4'},
	  {'Muncipality':'Copenhagen','Duration':'Daily','TaxPriority':'1','StartDate':new Date("2016-01-01"),'EndDate':new Date('2016-01-01'),'TaxRate':'0.1'},
	  {'Muncipality':'Copenhagen','Duration':'Daily','TaxPriority':'1','StartDate':new Date("2016-12-25"),'EndDate':new Date('2016-12-25'),'TaxRate':'0.1'}])
					
        *** You can import the tax details using excel after setting up the API successfully. Import process will be explained shortly when we go through API testing
     4) change the mongodb connection string in appsettings.json if you try host the mongDB in cloud or on premises server


# Tools
       1) AutoMapper for object to object mapping
       2) Swagger of API Testing
       3) Nunit for Unit Testing
       4) ExcelDataReader for reading data from uploaded file

# Application Architecture
       1) This API is developed in Clean/Onion Architecture considering microservices way deploying the application 
       2) Domains has been placed in Core layer and we are using Domain terminology as application is designed in DDD (Domain Driven Design) way 
        
#  Application Folder Structure       
       1) Application has 4 projects 
           a) API project
           b) Core Project
           c) Infrastructure Project
           d) Unit Test Project
        2) Core project will be referenced by other projects, but Core will not have any dependencies apart from framework libraries
        3) Core is .Net Standard 2.0 project since it can be reused if any other domains depend on this Tax Domain. Rest of the project are .Net Core Projects
 
# Assumptions
        1) Introduced a column called as TaxPriority to fetch the correct tax rate if duration is overlapping with daily ,weekly, monthly and yearly duration.
        2) supported formats for file upload is .xlsx,.xlx,.csv which are configured in appsettings.json
        3) Error messages are configured in appsettings.json to eliminate the hardcodings in application code 

#  API Testing 
        1) Swagger is used for API testing instead of consumer service
        2) API has 5 Action methods 
              a) Get all municipal taxes
              b) Get tax rate based on municipality name and date
              c) Insert new record
              d) Update record
              e) Upload the record using excel file
	3) Exception are handled globally by building the middleware for exception handling and registering it in Configure method in startUp.cs file

# Unit Testing
	1) unit test are available for the method GetTaxRateByMunicipalityDate in controller and service layer
	2) upload , insert and update methods does not have unit tests as those methods are primary dependent on third party tools like mongoDb, ExcelDataReader etc.

# Improvements/Enhancements that can made based on business needs
	 1) Createing application flow and architecture diagrams in http://draw.io
         2) Introduce security layer like identity server and place it in infrastructure layer
	 3) Logging in application by creating extensions to logging frameworks like serilog,log4net etc. and logging the logs in a desired format by creating a model class 	          for logging alone such that logs can be easily queried from log reporting tools like Splunk, elk etc.	 
	 4) Abstracting the validations in controller by introducing model validation frameworks like fluent Validation.
         5) Leverage the resilient frameworks like Polly to maintain the application resilience and transient fault handling for external world Http based communications.
	 6) Dockerize the application to deploy it as a container based on the resources consumption required based on the business need   
	 7) A new installer project like wix project to generate msi and maintain the artifacts in artifact repository like nexus
	 8) Create CI/CD pipe line 
              
        
          
