
I use TunnelQuest.DatabaseBuilder for two different purposes:

1.	I used TunnelQuest.DatabaseBuilder to scrape all the item data and icons from wiki.project1999.com.  
	The scraped item data is in TunnelQuest.Data\Migrations\Data\wiki.project1999.com_scrapes.json, and the 
	scraped icons are in TunnelQuest.Web\wwwroot\images\wiki.project1999.com_scrapes.  They were scraped 
	from the wiki by running the following command:
			
		dotnet TunnelQuest.DatabaseBuilder.dll Build-Item-Details "..\..\..\..\TunnelQuest.Data\Migrations\Data" "..\..\..\..\TunnelQuest.Web\wwwroot\images" false

			-	This command can be stopped and re-run as many times as necessary.  It will
				never delete anything, or re-download anything that it's already downloaded.
			-	If any kind of error occurs while scraping (for example, if there is missing data
				on the wiki page for a particular item), then the error will be displayed in the
				console and the program will continue scraping the rest of the items.  If you
				want the command to pause after each error and wait for you to press a key to
				continue, then change the final parameter from "false" to "true".
			-	If the command doesn't finish running because you close it early, then you can
				just run it again later and it will resume downloading the last item where it
				left off.  Likewise, if there are any errors pulling specific items, you can
				just let the command finish downloading everything else, then run it again later
				to retry the ones that failed.
			-	This command takes a long time to finish, over an hour.  There are a lot of items,
				after all.  You *could* speed it up by removing the hard-coded Thread.Sleep(250)
				but it's possible that wiki.project1999.com would blacklist you for DDoS.  Also
				it would be rude to spike their server that hard.  Just be patient, you only
				need to run it once.

	Additional commands:

		dotnet TunnelQuest.DatabaseBuilder.dll List-Duplicate-Names "..\..\..\..\TunnelQuest.Data\Migrations\Data"

			-	This command will list any items in the json data file which have identical names (case-insensitive)


2.	I also use TunnelQuest.DatabaseBuilder as my StartupProject instead of TunnelQuest.Web when I run commands in
	Entity Framework tools, because that way I can run EF tools as a different MySQL user than TunnelQuest.Web.
	This is better practice because it means I only have to grant MySQL admin permissions to the TunnelQuestDatabaseBuilder
	user, and I can leave the TunnelQuestWeb user running with only data-reader permissions.

	For example:
		Add-Migration -Name whatever -Project TunnelQuest.Data -StartupProject TunnelQuest.DatabaseBuilder
		Update-Database -Project TunnelQuest.Data -StartupProject TunnelQuest.DatabaseBuilder



