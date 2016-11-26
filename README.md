# CoreWebApp
This demonstrates an issue with dotnetcore where a PreFlight (OPTIONS) request returns a 401 from dotnetcore if using Windows Authentication
<br>
<br>Issue described here:
<br>http://stackoverflow.com/questions/15734031/why-does-the-preflight-options-request-of-an-authenticated-cors-request-work-in
<br>
<br>To replicate the issue:
<br>1) Change the launchSettings.json to use your servername.
<br>2) Build and run this solution. 
<br>3) Do a put or post ajax call withcredentials=true from a different domain. 
<br>The above causes a preflight (OPTIONS) request which does not send credentials. As it does not send credentials (and it should not send credentials for an OPTIONS request) the dotnetcore authentication responds with a 401. It should be a 200 OK with the Access-Control-Allow-Origin and Access-Control-Allow-Method headers set.
<br>
<br>The call works in IE, but fails in Chrome as Chrome treats the 401 as a failure.
<br>
<br>You can use this jsFiddle for the ajax calls:
<br>http://jsfiddle.net/enricosaunders/o2u3yete/
<br>
<br>Note that the pipeline does not reach any custom middleware for an OPTIONS Method call.
<br>
<br><b>This issue can be worked around by using AnonymousAuthentication see the working example in the <i>Fix CORS issue</i> commit:</b>
<br>Allow both WindowsAuthentication and AnonymousAuthentication.
<br>In the 1st middleware check for authentication for all methods except OPTIONS (Preflight) and return a 401 where appropriate.

