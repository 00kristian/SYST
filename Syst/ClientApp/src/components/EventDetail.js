import React, { Component } from 'react';
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import { InteractiveTable } from './InteractiveTable';
import Icon from "@mdi/react";
import { mdiThumbUp, mdiThumbDown } from '@mdi/js';
import { AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";

export class EventDetail extends Component {
  static displayName = EventDetail.name;

  constructor(props) {
    super(props);
    this.state = { event: Object, loading: true, winnerNames: "", show:  true, numWinners : 1 };

  }

  componentDidMount() {
    this.populateData();
  }
  
  static renderEvent(event, editEvent,editRating, deleteEvent, pickWinners, winnerNames, upvote, downvote, setNumWinners) {

      return (
        <AuthenticatedTemplate>
        <div>
            <h1>{event.name}</h1>
            <h3>{event.location}, {event.date}</h3>
            <h4>Rating: {event.rating}</h4>
            <h4>Winners: {winnerNames}</h4>
            <br/>
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
    
                                  <input onChange={(e) => setNumWinners(e.target.value) } type="number" min="1" max={event.candidates.length} step="1" />
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
            <button onClick={() => editEvent()} className="btn btn-tertiary btn-right obj-space">Edit Event</button>
            <button onClick={() => editRating()} className="btn btn-tertiary btn-right">Edit Rating</button>
            <br/>
            <h4>PARTICIPANTS</h4>
            <InteractiveTable ExportName={event.name + ".csv"} SearchBar={true} Columns={[["Id", "id"], ["Name", "name"], ["Email", "email"], ["University", "university"], ["Degree", "currentDegree"], ["Study Program", "studyProgram"], ["Graduation Date", "graduationDate"]]} Content={event.candidates}>
            {candidate =>
                    <div>
                        {candidate.isUpvoted ? (
                        <td>
                            <td><button className="btn btn-right btn-green" onClick={() => upvote(candidate.id)} ><Icon path={mdiThumbUp} size={1}/></button></td>
                            <td>
                                <Popup className="popup-overlay" trigger = {<button className="btn btn-primary btn-right"><Icon path={mdiThumbDown} size={1}/></button>} modal nested>
                                {close => (
                                    <div className="div-center">
                                        <p>Are you sure you want to delete this candidate?</p>
                                        <button className="btn btn-primary btn-yes" onClick={()=> downvote(candidate.id)}>YES</button>
                                        <button className="btn btn-primary"onClick={() => {close();}}>NO</button>
                                    </div>
                                )}
                                </Popup>
                            </td>
                        </td>
                        ) : (
                        <td>
                             <td><button className="btn btn-primary btn-right" onClick={() => upvote(candidate.id)} ><Icon path={mdiThumbUp} size={1}/></button></td>
                            <td>
                                <Popup className="popup-overlay" trigger = {<button className="btn btn-primary btn-right"><Icon path={mdiThumbDown} size={1}/></button>} modal nested>
                                {close => (
                                    <div>
                                        <p className="txt-popup">Are you sure you want to delete this candidate?</p>
                                        <div className="div-center">
                                            <button className="btn btn-primary btn-yes btn-popup" onClick={()=> downvote(candidate.id)}>YES</button>
                                            <button className="btn btn-primary btn-popup"onClick={() => {close();}}>NO</button>
                                        </div>
                                    </div>
                                )}
                                </Popup>
                            </td>
                        </td>
                        )}
                    </div>
                }
            </InteractiveTable>
            <br></br>
            <a href={'/events'}> <button className="btn btn-secondary">Back</button> </a>
            <Popup className="popup-overlay" trigger = {<button className="btn btn-delete btn-right">DELETE</button>} modal nested>
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

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : EventDetail.renderEvent(this.state.event, this.editEvent,this.editRating, this.deleteEvent, this.pickWinners, this.state.winnerNames, this.clickToUpvoteCandidate, this.clickToDownvoteCandidate, ((n) => this.setState({numWinners : n})));
      return (
        <AuthenticatedTemplate>
        <div>
            {contents}
      </div>
        </AuthenticatedTemplate>
    );
  }

  editEvent = () => {
    const { history } = this.props;
    history.push("/CreateEvent/" + this.props.match.params.id);
  }

  editRating = () => {
    const { history } = this.props;
    history.push("/EventRating/" + this.props.match.params.id);
  }

  async populateData() {
    const response = await fetch('api/events/' + this.props.match.params.id);
    const data = await response.json();
    let winnerNames = await this.displayWinners(data.winnersId);
    this.setState({ event: data, loading: false, winnerNames: winnerNames});
    
  }


  deleteEvent = async () => {
        
    const requestOptions = {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(this.props.match.params.id)
    };
    await fetch('api/events'+"/"+this.props.match.params.id, requestOptions);

    const { history } = this.props;
    history.push("/events");

}

  pickWinners = async () => {
    const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
    };
    this.setState({ show: false });
    await fetch('api/events/winners'+"/"+this.props.match.params.id+"/" + this.state.numWinners, requestOptions);
    //den skal have hvor mange vindere den skla have i 
    await this.populateData();
  }

  displayWinners = async (winnersId) => {
    const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
    };
    
    let returnString = "";

      for (let i = 0; i < winnersId.length; i++) {
          if (i != 0) returnString = returnString + ", ";
          let id = winnersId[i];
          let candidate = await fetch('api/candidates/'+id, requestOptions).then(response => response.json());
          returnString = returnString + candidate.name
      }
    
    return returnString;
    
  }
  
  clickToUpvoteCandidate =  async (id) => {
        
    const requestOptions = {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
       
    };
    await fetch("api/candidates"+"/upvote/"+ id, requestOptions)
    
    this.populateData();
  }

  clickToDownvoteCandidate = async (id) => {
    await fetch('api/candidates/' + id, {
        method: 'DELETE'
    });
    this.populateData();
  }

}
