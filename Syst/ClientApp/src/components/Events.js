import React, { Component } from 'react';
import { withRouter } from "react-router";

export class Events extends Component {
    static displayName = Events.name;
    
    constructor(props) {
        super(props);
        this.state = { events: [], loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    static renderUpcomingEventsTable(events) {
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
                            <td> <button className="btn btn-host rightbtn">Host</button> </td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }
    static renderRecentEventsTable(events) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Date</th>
                        <th>Location</th>
                        <th>Rating</th>
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
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let UpcomingContents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Events.renderUpcomingEventsTable(this.state.events);
        let RecentContents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Events.renderRecentEventsTable(this.state.events);

        return (
            <div>
                <h3 id="tabelLabel" >Upcoming Events
                    <button className="btn btn-primary rightbtn" onClick={this.rerouteToEventCreation}>Create</button>
                </h3>
                {UpcomingContents}
                <br/>
                <h3>Recent Events
                    <button className="btn btn-primary rightbtn" >View All</button>
                </h3>
                {RecentContents}
            </div>
        );
    }

    rerouteToEventCreation = () => {
        const { history } = this.props;
        history.push("/CreateEvent");
    }

    async populateData() {
        const response = await fetch('api/events');
        const data = await response.json();
        this.setState({ events: data, loading: false });
    }
}