import React, { Component } from 'react';
const { env } = require('process');

export class ImageUpload extends Component {
    static Uploader(questionid, currentimg) {
        const path = window.location.href.replace(window.location.pathname, "");

        const uploadImg = async (e) => {
            const file = e.target.files[0];
            const fd = new FormData();
            fd.append('file', file);

            const requestOptions = {
                method: 'POST',
                body: fd,
                credentials: 'same-origin',
            };

            const imgurl = await fetch('api/Image/' + questionid, requestOptions)
                .then(res => res.text());
            document.getElementById("imgdiv").innerHTML = "<img width=350 src=" + path + "/api/Image/" + imgurl + "></img>";
        }
        
        if ((typeof currentimg === 'string' || currentimg instanceof String) && currentimg.length > 0) {
            return (
                <div>
                    <input type="file" onChange={(e) => uploadImg(e)} accept="image/png, image/jpeg"/> 
                    <br/>
                    <br/>
                    <div id="imgdiv">
                        <img width={350} src={path + "/api/Image/" + currentimg}></img>
                    </div>
                </div>
            )
        }
        return (
            <div>
                <input type="file" onChange={(e) => uploadImg(e)} accept="image/png, image/jpeg"/>  
                <br/>  
                <br/>  
                <div id="imgdiv"></div>
            </div>
        );  
    }
}