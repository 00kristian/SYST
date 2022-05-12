export const msalConfig = {
    auth: {
      clientId: "18686055-5912-4c57-a1c9-0bb76dde9d96",
      authority: "https://login.microsoftonline.com/55f0e7d6-df83-4287-a2bc-b7c2a11f8243", // This is a URL (e.g. https://login.microsoftonline.com/55f0e7d6-df83-4287-a2bc-b7c2a11f8243)
      redirectUri: "https://localhost:44419/",
    },
    cache: {
      cacheLocation: "sessionStorage", // This configures where your cache will be stored
      storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
    }
  };
  
  // Add scopes here for ID token to be used at Microsoft identity platform endpoints.
  export const loginRequest = {
   scopes: ["User.Read", "openid"] 
  };
  
  // Add the endpoints here for Microsoft Graph API services you'd like to use.
  export const graphConfig = {
      graphMeEndpoint: "https://graph.microsoft.com/"
  };