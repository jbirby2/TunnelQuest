
TunnelQuest.Web contains a javascript single-page-application (SPA) written in TypeScript and Vue, and compiled using Webpack and Node Package Manager (npm).
At the very least, you'll need to install Node on your development machine to globally install the "npm" command line executable on your machine
for the steps below.

SETUP:

	When you first clone this project to your development machine, run the following command in the TunnelQuest.Web folder to download all the
	javascript dependency libraries defined in package.json.  These will be downloaded to the node_modules folder, which is excluded from
	source control but necessary at webpack buildtime:

		npm install

MAKING SPA CODE CHANGES:

	In order to make changes to the javascript SPA, you'll need to modify the relevant files in the /src folder, then run the following
	command in the TunnelQuest.Web folder:

		npm run release

	This will rebuild the runtime javascript/css/html files in the /wwwroot folder with the latest code from the /src folder.  The
	next time the runtime SPA page (/wwwroot/index.html) is loaded in the web browser, it will be running your code changes from the
	/src folder.
