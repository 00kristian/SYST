import React, { Component } from 'react';

export class EventDetail extends Component {
  static displayName = EventDetail.name;

  constructor(props) {
    super(props);
    this.state = { event: Object, loading: true, winnerName: "", show:  true };
  }

  componentDidMount() {
    this.populateData();
  }

  

  static renderEvent(event, edit, deleteEvent, pickAWinner, winnerName, show) {
   
    return (
        <div>
            <h1>{event.name}</h1>
            <h2>{event.date}</h2>
            <h2>{event.location}</h2>
            <h2>Winner: { winnerName  }</h2>
         {/*    <h3 className='txt-right'>Winner ={displayWinner(event.winner)} </h3> */}
            <button onClick={() => edit()} className="btn btn-primary btn-right">Edit event</button>
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
            {show  ? (
              <div>
              <button className="btn btn-primary" onClick={()=>pickAWinner()}>Generate a winner</button>
              </div>
            ) : (
              <div></div>
            ) }
            <br></br>
            <a href={'/events'}> <button className="btn btn-primary btn-right">Back</button> </a>
            <button className="btn btn-primary" onClick={()=>deleteEvent()}>Delete event</button>
           
        </div>
        
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : EventDetail.renderEvent(this.state.event, this.edit, this.deleteEvent, this.pickAWinner, this.state.winnerName, this.state.show);

    return (
      <div>
        {contents}
      </div>
    );
  }

  edit = () => {
    const { history } = this.props;
    history.push("/CreateEvent/" + this.props.match.params.id);
  }

  async populateData() {
    const response = await fetch('api/events/' + this.props.match.params.id);
    const data = await response.json();
    let winnerName = await this.displayWinner(data.winnerId);
    let show = this.state.show;
    this.setState({ event: data, loading: false, winnerName: winnerName});
    
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
  
}
