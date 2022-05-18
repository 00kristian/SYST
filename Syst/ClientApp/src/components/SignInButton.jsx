import React from "react";
import { useMsal } from "@azure/msal-react";
import { loginRequest } from "../authConfig";
import Icon from '@mdi/react'
import { mdiArrowRight } from '@mdi/js';

function handleLogin(instance) {
    instance.loginRedirect(loginRequest).catch(e => {
        console.error(e);
    });
}


//Renders a button which, when selected, will redirect the page to the login prompt
 
export const SignInButton = () => {
    const { instance } = useMsal();

    return (
        <button variant="secondary" className="btn btn-primary ml-auto btn-center" onClick={() => handleLogin(instance)}><div className="div-signIn">SIGN IN </div><Icon path={mdiArrowRight} size={2}/></button>
    );
}