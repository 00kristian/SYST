import React, { useEffect, useState } from "react";
import Icon from "@mdi/react";
import {mdiThumbUp, mdiThumbDown} from '@mdi/js';
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import ReactDropdown from 'react-dropdown';
import { InteractiveTable } from './InteractiveTable';
import {FetchOptions} from './FetchOptions';
import { loginRequest } from "../authConfig";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";


export default Candidates

function Candidates() {
    const [candidates, setCandidates] = useState([]);
    const { instance, accounts } = useMsal();


    useEffect(async () => {
        const options = await FetchOptions.Options(instance, accounts, "GET");
        const data = await fetch('api/candidates', options)
        .then(response => response.json())
        .catch(error => console.log(error));
        setCandidates(data);
    }, []);

   

    const clickToDownvoteCandidate = async (id) => {
        const options = await FetchOptions.Options(instance, accounts, "DELETE");

        await fetch('api/candidates/' + id, options);
    }

/*     const getCandidateById = async (id) =>{
        await fetch('api/candidates/' + id, {
            method: 'GET'
        })
    }
     */
    const clickToUpvoteCandidate =  async (id) => {
        const options = await FetchOptions.Options(instance, accounts, "PUT");
        console.log (options);
        
        await fetch("api/candidates"+"/upvote/"+ id, options)
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
                                    <button className="btn btn-primary btn-yes" onClick={()=>{clickToDownvoteCandidate(candidate.id); close();}}>Yes</button>
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
                                        <button className="btn btn-primary btn-yes btn-popup" onClick={()=>{ clickToDownvoteCandidate(candidate.id); close();}}>Yes</button>
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
 