import React, { Component } from 'react';
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import { InteractiveTable } from './InteractiveTable';
import Icon from "@mdi/react";
import { mdiThumbUp, mdiThumbDown } from '@mdi/js';


export class EventDetail extends Component {
  static displayName = EventDetail.name;

  constructor(props) {
    super(props);
    this.state = { event: Object, loading: true, winnerName: "", show:  true };

  }

  componentDidMount() {
    this.populateData();
  }
  
  static renderEvent(event, editEvent,editRating, deleteEvent, pickAWinner, winnerName, show, upvote, downvote) {

    return (
        <div>
            <h1>{event.name}</h1>
            <h3>{event.location}, {event.date}</h3>
            <h4 className='txt-left'>WINNER: {winnerName}</h4>
            <h4 className='txt-right'>RATING: {event.rating}</h4>
            <br/>
            <br/>
            <button className="btn btn-tertiary">Put winner button here!</button>
            <button onClick={() => editRating()} className="btn btn-tertiary btn-right">Edit Rating</button>
            <div>
              <button className = "btn btn-primary btn-right" onClick={()=> window.open('/CandidateQuiz/' + event.id + '/' + event.quiz.id, "_blank", 'location=yes,height=800,width=1300,scrollbars=yes,status=yes')} >HOST</button>
              <button onClick={() => editEvent()} className="btn btn-tertiary btn-right obj-space">Edit Event</button>
            </div>
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
        
    )
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : EventDetail.renderEvent(this.state.event, this.editEvent,this.editRating, this.deleteEvent, this.pickAWinner, this.state.winnerName, this.state.show, this.clickToUpvoteCandidate, this.clickToDownvoteCandidate);


    return (
      <div>
            {contents}
        {this.state.winnerName != null  ? (
              <div></div>
            ) : (
              <div>
                        <button className="btn btn-primary" onClick={() => this.pickAWinner()}>GENERATE A WINNER</button>   
              </div>
            )}
      </div>
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
    let winnerName = await this.displayWinner(data.winnerId);
    let show = this.state.show;
    this.setState({ event: data, loading: false, winnerName: winnerName});
    console.log(data);
    
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

  pickAWinner = async () => {
    const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
    };
    this.setState({ show: false });
    
    await fetch('api/events/winner'+"/"+this.props.match.params.id, requestOptions);
    this.populateData();
    
  }

  displayWinner = async (id) => {
    const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
    };
  
   
    let candidate = await fetch('api/candidates/'+id, requestOptions).then(response => response.json());
   
    return candidate.name;
    
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
