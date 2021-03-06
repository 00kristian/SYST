import React, { useEffect, useState, useCallback } from "react";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useIsAuthenticated, useMsal } from "@azure/msal-react";
import {FetchOptions} from './FetchOptions';
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import { InteractiveTable } from './InteractiveTable';
import Icon from "@mdi/react";
import { mdiThumbUp, mdiThumbDown } from '@mdi/js';
import { useHistory } from "react-router-dom";

//Page that shows a specific events details 
export function EventDetail(props) {

  const [event, setEvent] = useState(null);
  const [winnerNames, setWinnerNames] = useState("");
  const [numWinners, setNumWinners] = useState(0);
  const history = useHistory();
  const { instance, accounts } = useMsal();
  const [calls, setCalls] = useState(0);
  
  const displayWinners = async (winnersId) => {
    
	const options = await FetchOptions.Options(instance, accounts, "GET");
    
    let returnString = "";

      if (winnersId !== undefined) {
          for (let i = 0; i < winnersId.length; i++) {
              if (i != 0) returnString = returnString + ", ";
              let id = winnersId[i];
              let candidate = await fetch('api/candidates/' + id, options).then(response => response.json());
              returnString = returnString + candidate.name
          }
      }
    return returnString;
  }

  const pickWinners = async () => {
	const options = await FetchOptions.Options(instance, accounts, "GET");
    await fetch('api/events/winners'+"/"+props.match.params.id+"/" + numWinners, options);
    setCalls((c) => c + 1)
  }
  
  const clickToUpvoteCandidate = async (id) => {
    const options = await FetchOptions.Options(instance, accounts, "PUT");
    await fetch("api/candidates"+"/upvote/"+ id, options)
    setCalls((c) => c + 1)
  }

  const clickToDownvoteCandidate = async (id) => {
	const options = await FetchOptions.Options(instance, accounts, "DELETE");
    await fetch('api/candidates/' + id, options);
    setCalls((c) => c + 1)
  }

  useEffect(async () => {
	const options = await FetchOptions.Options(instance, accounts, "GET");
    const response = await fetch('api/events/' + props.match.params.id, options);
    const data = await response.json();
    let winnerNames = await displayWinners(data.winnersId);
    setEvent(data);
    setWinnerNames(winnerNames);
  }, [calls])

  const editEvent = () => {
    history.push("/CreateEvent/" + props.match.params.id);
  }

  const editRating = () => {
    history.push("/EventRating/" + props.match.params.id);
  }

  const deleteEvent = async () => {
	const options = await FetchOptions.Options(instance, accounts, "DELETE");
	options.headers = {
		...options.headers,
		'Content-Type': 'application/json'
	}
	options.body = JSON.stringify(props.match.params.id);
    await fetch('api/events'+"/"+props.match.params.id, options);

    history.push("/events");
}
  
  const renderEvent = () => {
      return (
        <AuthenticatedTemplate>
        <div className="page-padding">
            <h1>{event.name}</h1>
            <h3>{event.location}, {event.date}</h3>
            <button onClick={() => editEvent()} className="btn btn-tertiary obj-space btn-right">Edit Event</button>
                <button style={{
                  marginRight: 10
                }} onClick={() => editRating()} className="btn btn-tertiary btn-right">Edit Rating</button>
            <h4>Rating: {event.rating}</h4>
            <h4>Winners: {winnerNames}</h4>
            <br/>
            <div>
            {winnerNames == "" ? (
              <div>
                  <Popup className="popup-overlay" trigger = {<button className="btn btn-tertiary">Generate winners</button>
                  } modal nested>
                      {close => (
                          <div>
                              <p className="txt-popup">How many winners would you like to generate?</p>
                              <div className="div-center">
    
                                  <input value={numWinners} onChange={(e) => setNumWinners(e.target.value) } type="number" min="1" max={event.candidates.length} step="1" />
                                  <br/>
                                  <br/>
                                  <button className="btn btn-primary" onClick={() => pickWinners()}>OK</button>
                              </div>
                          </div>
                      )}
                  </Popup>
              </div>
              ) :
              (
                <div></div>
              ) 
            }
            </div>
            <button className = "btn btn-primary btn-right" onClick={()=> window.open('/CandidateQuiz/' + event.id + '/' + event.quiz.id, "_blank", 'location=yes,height=800,width=1300,scrollbars=yes,status=yes')} >HOST</button>
            <br/>
            <h4>PARTICIPANTS</h4>
            <InteractiveTable ExportName={event.name + ".csv"} SearchBar={true} Columns={[["Id", "id"], ["Name", "name"], ["Email", "email"], ["University", "university"], ["Degree", "currentDegree"], ["Study Program", "studyProgram"], ["Graduation Date", "graduationDate"], ["Answer Rate in %", "percentageOfCorrectAnswers" ]]} Content={event.candidates}>
            {candidate =>
                    <div>
                        {candidate.isUpvoted ? (
                        <div className='div-right'>
                            <td><button className="btn btn-right btn-green" onClick={() => clickToUpvoteCandidate(candidate.id)} ><Icon path={mdiThumbUp} size={1}/></button></td>
                            <td>
                                <Popup className="popup-overlay" trigger = {<button className="btn btn-primary btn-right"><Icon path={mdiThumbDown} size={1}/></button>} modal nested>
                                {close => (
                                    <div className="div-center">
                                        <p>Are you sure you want to delete this candidate?</p>
                                        <button className="btn btn-primary btn-yes" onClick={()=> clickToDownvoteCandidate(candidate.id)}>YES</button>
                                        <button className="btn btn-primary"onClick={() => {close();}}>NO</button>
                                    </div>
                                )}
                                </Popup>
                            </td>
                        </div>
                        ) : (
                        <div className='div-right'>
                             <td><button className="btn btn-primary btn-right" onClick={() => clickToUpvoteCandidate(candidate.id)} ><Icon path={mdiThumbUp} size={1}/></button></td>
                            <td>
                                <Popup className="popup-overlay" trigger = {<button className="btn btn-primary btn-right"><Icon path={mdiThumbDown} size={1}/></button>} modal nested>
                                {close => (
                                    <div>
                                        <p className="txt-popup">Are you sure you want to delete this candidate?</p>
                                        <div className="div-center">
                                            <button className="btn btn-primary btn-yes btn-popup" onClick={()=> clickToDownvoteCandidate(candidate.id)}>YES</button>
                                            <button className="btn btn-primary btn-popup"onClick={() => {close();}}>NO</button>
                                        </div>
                                    </div>
                                )}
                                </Popup>
                            </td>
                        </div>
                        )}
                    </div>
                }
            </InteractiveTable>
            <br></br>
            <Popup className="popup-overlay" trigger={<button className="btn btn-secondary btn-cancel">DELETE EVENT</button>} modal nested>
              {close => (
                <div>
                  <p className="txt-popup">Are you sure you want to delete this event?</p>
                  <div className="div-center">
                    <button className="btn btn-primary btn-yes btn-popup" onClick={()=>deleteEvent()}>YES</button>
                    <button className="btn btn-primary btn-popup"onClick={() => {close();}}>NO</button>
                  </div>
                  </div>
              )}
            </Popup>
        </div>
        </AuthenticatedTemplate>
    )
  }
  
  let contents = event == null ? <></> : renderEvent();
    
    return (
      <AuthenticatedTemplate>
          {contents}
      </AuthenticatedTemplate>
  );
}
