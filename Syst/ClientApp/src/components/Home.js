import React, { Component } from 'react';
import { withRouter } from "react-router";

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { events: [], loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    static renderEventsTable(events) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Date</th>
                    <th>Location</th>
                    <th>Rating</th>
                    <th></th>
                </tr>
                </thead>
                <tbody>
                {events.map(event =>
                    <tr key={event.id}>
                        <td>{event.id}</td>
                        <td>{event.name}</td>
                        <td>{event.date}</td>
                        <td>{event.location}</td>
                        <td>{event.rating}</td>
                        <td><a href={'/eventdetail/' + event.id}> <button className="btn btn-host rightbtn">Details</button></a></td>
                        <td onClick={()=> window.open('/CandidateQuiz', "_blank", 'location=yes,height=800,width=1300,scrollbars=yes,status=yes')}><button className="btn btn-primary rightbtn">Host</button></td>
                    </tr>
                )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderEventsTable(this.state.events);

        return (
            <div>
                <h1>Welcome to Systematic Event Tool!</h1>
                <p>From this home page you'll be able to create, host and see and overview over events! Check it out!</p>

                <h1 id="tabelLabel" >Upcoming Events
                    <button className="btn btn-primary rightbtn" onClick={this.rerouteToEventCreation}>Create</button>
                </h1>
                {contents}
            </div>
    );
    }

    rerouteToEventCreation = () => {
        const { history } = this.props;
        history.push("/CreateEvent");
    }

    async populateData() {
        const response = await fetch('api/eventsquery/upcoming');
        const data = await response.json();
        this.setState({ events: data, loading: false });
    }

    handleHostClick = () => {
        window.open('/CandidateQuiz');
      };
}
