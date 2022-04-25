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
                            <td><a href={'/eventdetail/' + event.id}> <button className="btn btn-host btn-right">Details</button></a></td>
                            <td onClick={()=> window.open('/CandidateQuiz/' + event.id + '/' + event.quiz.id, "_blank", 'location=yes,height=800,width=1300,scrollbars=yes,status=yes')}><button className="btn btn-primary btn-right">Host</button></td>
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
                            <td><a href={'/eventdetail/' + event.id}> <button className="btn btn-host btn-right">Details</button></a></td>
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
                    <button className="btn btn-primary btn-right" onClick={this.rerouteToEventCreation}>Create</button>
                </h3>
                {UpcomingContents}
                <br/>
                <h3 className="obj-top_padding">Recent Events
                    <button className="btn btn-primary btn-right">View All</button>
                </h3>
                {RecentContents}
            </div>
        );
    }

    rerouteToEventCreation = async () => {
        let event = {
            "name": "new event",
            "date":  new Date().toISOString().split('T')[0],
            "location": "location"
        };

        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(event)
        };
        let id = await fetch('api/events', requestOptions)
          .then(response => response.json())

        const { history } = this.props;
        history.push("/CreateEvent/"+id);
    }

    async populateData() {
        const response1 = await fetch('api/eventsquery/recent');
        const data1 = await response1.json();
        const response2 = await fetch('api/eventsquery/upcoming');
        const data2 = await response2.json();
        this.setState({ recent: data1, upcoming: data2, loading: false });
    }
}
