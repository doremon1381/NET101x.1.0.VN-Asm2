I have modified some endpoint parameters to use the access token if that token is valid
- For example, IdentityClaims' NameIdentifier as the username of the account.

Because of that, with some endpoint in which the userId is required inside the URL or the request body,
I'll ignore that requirement and still use the username claim inside the access token,
If the endpoint function only needs to identify the user whose token belongs to.