* Q: If we are going to deploy this on production, what do you think is the next improvement that you will prioritize next? This can be a feature, a tech debt, or an architectural design.
* A: I have many ideas regarding the next improvements but I would like to prioritize setting up the EntityFramework for the Employees Table. So that we can easily modify its columns whenever necessary.
I've listed below some incremental improvements along the way (starting from top to bottom).

TechDebt
* Fully incorporate the EmployeeDto to the EntityFramework. This will allow the use of EF functions instead of sql scripting.
* Add a salary column to the Employee table so HR/Payroll personnel can set the salary per employee and employee type.
* Add a DbBackup/Clean policy and function to clean the deleted employee records based on retention policy.
* Add reflection to Factory Class to remove the need of switch cases for new employee types.

Features
* Add month picker to specify the month the salary is being calculated.
* Add filtering options to allow easier navigation
* Propose to add a modal/pop-up window for crud methods. This way, the flow of the screen will be on a single page.
* Include OT if applicable to client.
* Include tax policies based on Salary amount.
* Add Calendar Table to allow HR/Payroll personnel set the number of work days in a month. 
* Handler for expired tokens. 
* Update Net Income to allow accepting of commas in thousands and above.
