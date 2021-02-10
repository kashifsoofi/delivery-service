# Delivery Service

## Usage
1. Clone repository locally
2. Open Powershell command at the root of Directory and issue following command to start MySql database in a docker container  
`.\dev-env.ps1 start`
3. Open `DeliveryService.sln` in Visual Studio  
4. Set `DeliveryService.Api` and `DeliveryService.Host` as startup projects  
5. Debug would start web browser with Api Url and Host would start a command window  
6. After finishing issue following command to stop and close database container  
`.\dev-env.ps1 stop`

## Project Structure
### Contracts
This project contains commands, events and common classes to perform operations expected by this service.  

### Domain
This project contains aggregate that this service is responsible for. Aggregate implements data and business checks and for each change creates an event.  

### Infrastructure
This project contains code that is not part of domain and mainly interacts with external systems e.g. database.  

### Api
This is WebApi project that exposes rest endpoints to perform CRUD operations on Delivery data. Api directly reads data from database with help of Queries and for updates (create/update) sends commands to Host (Message Processor).  

### Host
This is a commandline application. This is message processor of the system. It implements Handlers that listen for commands sent by Api and perform those opertions on Delivery aggregate.  

### Database
MySql container is used for storage, to use a differnt storage layer, main changes would be in sql, Query classes and aggregate repository.  

## Assumptions & Observations
* Only state is updateable through Update opertion  
* Create endpoint is expecting `Recipient` and `Order` as part of request for simplicity (and completion of this task). This maybe the case depending on the design or these can be retrieved from appropriate services requiring only `ids` of recipient & order be passed in create request  
* Expire method on aggregate is implented, this is triggered by a background job, an improvement can be to use Hangfire  
* Separate endpoints are implemented to change state to make it easy to check for access (user/partner)  

## Further
* Api & Host are containerised, but in order to make service work in containers NServiceBus would need to be configured to use a shared transport (or mount .learning folder on both containers)  