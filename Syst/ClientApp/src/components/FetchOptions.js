import React, { Component } from 'react';
import { loginRequest } from "../authConfig";

//Finds the acccess Token to the API
export class FetchOptions extends Component { 

    static async Options(instance, accounts, type) {
        const request = {
            ...loginRequest,
            scopes: [ "api://18686055-5912-4c57-a1c9-0bb76dde9d96/API.Access"],
            account: accounts[0]
        };
        const accessToken = await instance.acquireTokenSilent(request);
        const headers = new Headers();
        const bearer = `Bearer ${accessToken.accessToken}`;
        headers.append("Authorization", bearer);
        
        const options = {
            method: type,
            headers: {
                Authorization : bearer
            }
        };
        return options;
    }
}