How to Run:  
- Open the solution (ExchangeService.sln) in Visual Studio 2022 or newer.  
- Press F5 or click Run to start the application.  
- Once running, you can test the API through the Swagger UI by navigating to:  
    https://localhost:{port}/swagger  

How to Run Tests:  
-In Visual Studio, open the Test Explorer window.  
-Click Run All Tests to execute the unit tests in the ExchangeService.Tests project.  

Caveats:  
- Only AUD to USD conversion is supported.  
- API key for the external service is stored in appsettings.json for simplicity (should be secured).  
- If the external API fails, the request will fail without retrying.  

Possible Improvements:  
- Support multiple currency pairs.  
- Secure the API key using environment variables or a secret manager.  
- Add authentication (e.g., JWT).  
- Implement retry logic or a circuit breaker for external API calls.  
- Enhance global error handling using middleware (already added basic version).  
