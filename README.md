
1. Clone this repo
2. Run `dotnet watch run`
3. Send a Post Request to `https://localhost:5001/api/Watch/start`
4. Save the `runToken` from the returned object in the response
5. Send a Put Request to `https://localhost:5001/api/Watch/lap/{runToken}`, where `{runToken}` is the one returned
3. Send a Post Request to `https://localhost:5001/api/Watch/stop/{runToken}`, where `{runToken}` is the one returned
