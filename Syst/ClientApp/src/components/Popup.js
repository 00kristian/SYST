import React from "react";
import { AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";
 
const Popup = props => {
    return (
      <AuthenticatedTemplate>
        <div className="popup-box">
            <div className="box">
                <span className="close-icon">x</span>
                    {props.content}
            </div>
        </div>
      </AuthenticatedTemplate>
  );
};
 
export default Popup;