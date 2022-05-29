# SYST
This is an event tool developed for Systematic by cgon@itu.dk, luhj@itu.dk, imag@itu.dk, krbj@itu.dk, mfje@itu.dk, sacc@itu.dk in the course Second Year Project: Software Development in Large Teams at the IT University of Copenhagen, 2022.

The project is primarily coded in C# for the backend paired with React.js for the frontend.
For development, a sql server docker image has been used for the database, however this should be changed for production.
Azure active directory is used for authorization, this has to be configured by Systematic.

# Starting the program
Required installations:
  - .Net 6
  - Node.js
  - Docker
  - Powershell
 
 <h4> To run </h4>

- Have Docker desktop running.<br/>
- In a powershell terminal go to the root map and run the "npmDeps.ps1" script, this will install all required node dependencies. (Only first time). <br/>
- Then navigate back to the root map and run the "start.ps1" script and after some time you can open the application in your browser. (Only if database container is not running). <br/>
- Else you use "dotnet run" from the "Syst" directory. <br/>

# Things to note
<h3> Bug when deleting event with seedings </h3>
One specific bug was encountered prior to the release. The team believes it only exists while there's seedings in the system. <br/>
The code for seeding is commented out as a default, but beware that if seedings are present then a bug can be encountered while deleting events. This bug leads to an error page, but only on the first attempt at deleting, afterwards the bug should no longer cause problems.<br/>

<h3> Candidates set to delete after 2 years </h3>
Another thing to note is that the database is set up to delete candidates after 2 years.
