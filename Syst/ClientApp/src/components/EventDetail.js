import React, { Component } from 'react';
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';

export class EventDetail extends Component {
  static displayName = EventDetail.name;

  constructor(props) {
    super(props);
    this.state = { event: Object, loading: true, winnerName: "" };
  }

  componentDidMount() {
    this.populateData();
  }

  static renderEvent(event, editEvent, editRating, deleteEvent, pickAWinner, winnerName) {

    return (
        <div>
            <h1>{event.name}</h1>
            <h2>{event.date}</h2>
            <h2>{event.location}</h2>
            <h2>Winner: { winnerName  }</h2>
            <h2 className='txt-right'>Rating: {event.rating}</h2>
            <br/>
            <br/>
            <button onClick={() => editEvent()} className="btn btn-primary">Edit event</button>
            <button onClick={() => editRating()} className="btn btn-primary btn-right">Edit rating</button>
            <br/>
            <h3>Participants</h3>
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>University</th>
                    <th>Degree</th>
                    <th>Graduation date</th>
                </tr>
                </thead>
                <tbody>
                {event.candidates.map(candidate =>
                    <tr key={candidate.id}>
                        <td>{candidate.id}</td>
                        <td>{candidate.name}</td>
                        <td>{candidate.email}</td>
                        <td>{candidate.university}</td>
                        <td>{candidate.studyProgram}</td>
                        <td>{candidate.graduationDate}</td>
                    </tr>
                )}
                </tbody>
            </table>
            <button className="btn btn-primary" onClick={()=>pickAWinner()}>Generate a winner</button>
            <br></br>
            <a href={'/events'}> <button className="btn btn-primary btn-right">Back</button> </a>
            <Popup trigger = {<button className="btn btn-primary">Delete event</button>} modal nested>
              {close => (
                <div>
                  <p>Are you sure you want to delete this event?</p>
                  <button className="btn btn-primary btn-yes" onClick={()=>deleteEvent()}>Yes</button>
                  <button className="btn btn-primary"onClick={() => {close();}}>No</button>
                  </div>
              )}
            </Popup>
        </div>
        
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : EventDetail.renderEvent(this.state.event,this.editEvent, this.editRating, this.deleteEvent, this.pickAWinner, this.state.winnerName);

    return (
      <div>
        {contents}
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
    console.log(winnerName);
    this.setState({ event: data, loading: false, winnerName: winnerName });
    
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
  

}
