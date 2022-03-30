import React, { Component } from 'react';

export class Events extends Component {
    static displayName = Events.name;
    
    constructor(props) {
        super(props);
        this.state = { recent: [], upcoming: [], loading: true };
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
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let UpcomingContents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Events.renderUpcomingEventsTable(this.state.upcoming);
        let RecentContents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Events.renderRecentEventsTable(this.state.recent);

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
        const response1 = await fetch('api/eventsquery/recent');
        const data1 = await response1.json();
        const response2 = await fetch('api/eventsquery/upcoming');
        const data2 = await response2.json();
        this.setState({ recent: data1, upcoming: data2, loading: false });
    }
}