
Here's the plan for authentication:
		
	1. At startup, this app will make a HTTP GET call to https://{web server}/api/auth_token with an HTTP header of
	   "Authorization : Bearer this_machines_token", where this_machines_token is the string returned by 
	   TokenGenerator.GetAuthToken().  (Feel free to implement the stub inside that function to hide the processor ID.)
		
		a. If the HTTP response code from the server is 401 (Unauthorized), then that means that the user has not 
		   yet requested permission to run a TunnelWatcher on this machine.

		b. Else, if the HTTP response code from the server is 200 (OK), then the content returned by the server will 
		   be a JSON string in the following format:
		   {
			"name": "whatever_the_user_typed_in_the_box_when_requesting_permission",
		 	"status": "Pending" or "Approved" or "Declined"
		   }

	2. The UI will be slightly different depending on the result of the initial HTTP call described above:
			
		a. If the HTTP response code was 401 (Unauthorized), then the UI should display a section titled
		   "Request Permission to Submit".  This section should contain a textbox titled "Who are you and
		   what is this device?" and a button titled "Send Request".

			i. When the button is clicked, it should make a HTTP POST call to https://{web server}/api/auth_token with
			   an HTTP header of "Authorization : Bearer this_machines_token" (see above).  The body of the POST
			   should contain a JSON string in the following format:
				{
					"name": "whatever_the_user_typed_in_the_box"
				}

				- The response from this server for this request should always be HTTP status 200 (OK), and
				  the content returned should be the same JSON string returned by 1.b.  The UI should be
				  updated with the rules defined in 2 based on this latest status response from the server.

		b. Else if the HTTP response code was 200 (OK), then display a simple text label telling the user
		   what the returned status was ("Awaiting Approval", "Approved", "Politely Declined")

			i. If the status was anything besides "Approved", then don't even allow the exe to send
			   lines to the server, because they'll all be rejected anyway and just waste bandwidth.

	3. Whenever a new line is added to the EQ log file, if it is an /auction line, make an HTTP POST call to
	   https://{web server}/api/chat_line with an HTTP header of "Authorization : Bearer this_machines_token"
	   (see above).  The body of the POST should contain a JSON string in the following format:
		{
			"lines": [
				"new raw eq log line #1",
				"new raw eq log line #2",
				"new raw eq log line #3",
				etc.
			]
		}

		i. The response from the server will either be HTTP response 401 (Unauthorized) if for whatever reason
		   the auth token you passed in the header is invalid, or else HTTP response 200 (OK).  Assuming it's
		   HTTP response 200 (OK), the content returned by the server will be a JSON string in the following format:
			{
				"errors": [
					"error message 1",
					"error message 2",
					etc.
				]
			}

			- There's really not much you can do with these returned error messages.  Maybe write them to a log file
			  or just display them in a scrolling log textbox in the UI, in case the user actually looks
			  at it.  Either way, logging errors returned by the API shouldn't affect the application's
			  behavior; it should ultimately ignore errors and just keep on sending the next new lines.