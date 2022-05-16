import React, { useEffect, useState } from "react";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useIsAuthenticated, useMsal } from "@azure/msal-react";
import {FetchOptions} from './FetchOptions';
const { env } = require('process');

//Function that makes the admin able to upload an image to a question 
export function ImageUpload(props) {
    const { instance, accounts } = useMsal();
    const path = window.location.href.replace(window.location.pathname, "");

    const uploadImg = async (e) => {
        const file = e.target.files[0];
        const fd = new FormData();
        fd.append('file', file);
        
        const options = await FetchOptions.Options(instance, accounts, "POST");
        options.body = fd;
        options.credentials = 'same-origin';

        const imgurl = await fetch('api/Image/' + props.QuestionId, options)
            .then(res => res.text());
        document.getElementById("imgdiv").innerHTML = "<img width=350 src=" + path + "/api/Image/" + imgurl + "></img>";
    }
    
    if ((typeof props.Currentimg === 'string' || props.Currentimg instanceof String) && props.Currentimg.length > 0) {
        return (
            <AuthenticatedTemplate>
            <div>
                <input className='txt-small' type="file" onChange={(e) => uploadImg(e)} accept="image/png, image/jpeg"/> 
                <br/>
                <br/>
                <div id="imgdiv">
                    <img width={350} src={path + "/api/Image/" + props.Currentimg}></img>
                </div>
                </div>
            </AuthenticatedTemplate>
        )
    }
    return (
        <AuthenticatedTemplate>
        <div>
            <input className='txt-small' type="file" onChange={(e) => uploadImg(e)} accept="image/png, image/jpeg"/>  
            <br/>  
            <br/>  
            <div id="imgdiv"></div>
            </div>
        </AuthenticatedTemplate>
    );  
}