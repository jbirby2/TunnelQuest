
To create the database:

1. Build the solution

2. Update the appSettings.json file in the TunnelQuest.DatabaseBuilder project with your own connection string.  You don't need to 
   create the empty schema beforehand; the next step will create the schema from scratch.

2. Run the following line in Package Manager Console (i.e. PowerShell) to create the new schema and build the tables:
	Update-Database -Project TunnelQuest.Data -StartupProject TunnelQuest.DatabaseBuilder
