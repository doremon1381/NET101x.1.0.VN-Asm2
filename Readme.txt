I have modified some endpoint parameters to use the access token if that token is valid
- For example, IdentityClaims' NameIdentifier as the username of the account.
Or email will be used as identity information of Person table which is used as an identity agent.

Because of that, with some endpoint in which the userId is required inside the URL or the request body,
I'll ignore that requirement and still use the username claim inside the access token,
If the endpoint function only needs to identify the user whose token belongs to.

List of affecting endpoints:
5.2.2. POST /search/doctor 		       --> GET /search/doctor
5.2.3. POST /specialist/{specialistId}/details --> GET /specialist/{specialistId}/details
5.2.4. POST /opd/complete/user/{userId}	       --> GET /opd/complete/user/{userId}
5.3.1. POST /schedule/book/unitId              --> POST /schedule/book - with the parameters are inside RequestBody
/{unitId}/referenceNo/{referenceNo}		        and an email as a claim will come along with accesstoken
5.3.2. GET /booking/unitId/{unitId}	       --> GET /booking/unitId/{id}/details - I ignore referenceNo
/referenceNo/{referenceNo}/details		       , an email as a claim will come along with accesstoken
5.3.3. POST /booking/userId/{userId}/records   --> GET /booking/unitId/{appointmentId}/records - I add 
						        appointmentId to request parameters and 
							leave the email for accesstoken, now this 
							endpoint will need email and appointmentId as parameters

2. If in your developing environment, the missing of libwkhtmltox.dll causes an error while the DinkToPdf library is used
just copy the libwkhtmltox.dll file to Asm2's "..\bin\Debug\net5.0" or "..\bin\Release\net5.0"
, the error that is related to the missing libwkhtmltox.dll will be gone for now.