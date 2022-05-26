import { useMsal, AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";

function signOutClickHandler(instance, accounts) {
    const logoutRequest = {
        account: accounts[0], 
        postLogoutRedirectUri: "https://localhost:44419/"
    }
    instance.logoutRedirect(logoutRequest);
}

// SignOutButton Component returns a button that invokes a redirect logout when clicked
export function SignOutButton() {
    // useMsal hook will return the PublicClientApplication instance you provided to MsalProvider
    const { instance, accounts } = useMsal();

    return <button variant="secondary" className="btn txt-navbar" onClick={() => signOutClickHandler(instance, accounts)}>Sign Out</button>
};

