import React, { useEffect, useState } from "react";
import Icon from "@mdi/react";
import {mdiThumbUp, mdiThumbDown} from '@mdi/js';
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import ReactDropdown from 'react-dropdown';
import { InteractiveTable } from './InteractiveTable';

import { loginRequest } from "../authConfig";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";

export default Candidates

function Candidates(props) {
    const [candidates, setCandidates] = useState([]);

    const { instance, accounts } = useMsal();
    const request = {
        ...loginRequest,
        account: accounts[0]
    };
    

    useEffect(async () => {
        const accessToken = await instance.acquireTokenSilent(request);
        const headers = new Headers();
        const bearer = `Bearer ${accessToken.accessToken}`;
        headers.append("Authorization", bearer);

        const options = {
            method: "GET",
            headers: {
                Authorization : bearer
            }
        };

        console.log(await fetch('api/candidates', options));
        const data = await fetch('api/candidates', options)
        .then(response => response.json())
        .catch(error => console.log(error));
        console.log(options);
        console.log(accessToken);
        console.log(bearer);
        console.log(data);
        console.log("swag");
        setCandidates(data);
    }, []);

    const clickToDownvoteCandidate = async (id) => {
        await fetch('api/candidates/' + id, {
            method: 'DELETE'
        });
    }

    const getCandidateById = async (id) =>{
        await fetch('api/candidates/' + id, {
            method: 'GET'
        })
    }
    
    const clickToUpvoteCandidate =  async (id) => {
        
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
           
        };
        await fetch("api/candidates"+"/upvote/"+ id, requestOptions)
    } 

    let contents = <InteractiveTable ExportName="All_Candidates.csv" SearchBar={true} PageSize={8} Columns={[["Id", "id"], ["Name", "name"], ["Email", "email"], ["University", "university"], ["Degree", "currentDegree"], ["Study Program", "studyProgram"], ["Graduation Date", "graduationDate"]]} Content={candidates}>
            {candidate =>
                <div>
                    {candidate.isUpvoted ?(
                    <td>
                        <td><button className="btn btn-right btn-green" onClick={() => clickToUpvoteCandidate(candidate.id)} ><Icon path={mdiThumbUp} size={1}/></button></td>
                        <td>
                            <Popup className="popup-overlay" trigger = {<button className="btn btn-primary btn-right"><Icon path={mdiThumbDown} size={1}/></button>} modal nested>
                            {close => (
                                <div className="div-center">
                                    <p>Are you sure you want to delete this candidate?</p>
                                    <button className="btn btn-primary btn-yes" onClick={()=> clickToDownvoteCandidate(candidate.id)}>Yes</button>
                                    <button className="btn btn-primary"onClick={() => {close();}}>No</button>
                                </div>
                            )}
                            </Popup>
                        </td>
                    </td>
                    ) : (
                    <td>
                            <td><button className="btn btn-primary btn-right" onClick={() => clickToUpvoteCandidate(candidate.id)} ><Icon path={mdiThumbUp} size={1}/></button></td>
                        <td>
                            <Popup className="popup-overlay" trigger = {<button className="btn btn-primary btn-right"><Icon path={mdiThumbDown} size={1}/></button>} modal nested>
                            {close => (
                                <div>
                                    <p className="txt-popup">Are you sure you want to delete this candidate?</p>
                                    <div className="div-center">
                                        <button className="btn btn-primary btn-yes btn-popup" onClick={()=> clickToDownvoteCandidate(candidate.id)}>Yes</button>
                                        <button className="btn btn-primary btn-popup"onClick={() => {close();}}>No</button>
                                    </div>
                                </div>
                            )}
                            </Popup>
                        </td>
                    </td>
                    )}
                </div>
            }
        </InteractiveTable>;
    return (
        <div>
            <h1 id="tabelLabel" >Candidates</h1>
            {contents}
        </div>
    );
} 
 