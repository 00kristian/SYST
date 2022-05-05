import React, { Component } from 'react';
import { InteractiveTable } from './InteractiveTable';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { events: [], loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : <InteractiveTable Columns={[["Id", "id"], ["Name", "name"], ["Date", "date"], ["Location", "location"], ["Rating", "rating"]]} Content={this.state.events}>
                {event =>
                    <div>
                        <td><a href={'/eventdetail/' + event.id}> <button className="btn btn-secondary btn-right obj-left_margin">Details</button></a></td>
                        <td onClick={()=> window.open('/CandidateQuiz/' + event.id + '/' + event.quiz.id, "_blank", 'location=yes,height=800,width=1300,scrollbars=yes,status=yes')}><button className="btn btn-primary btn-right">Host</button></td>
                    </div>
                }
            </InteractiveTable>

        return (
            <div>
                <h1>Welcome to Systematic Event Tool!</h1>
                <p>From this home page you'll be able to create, host and see and overview over events! Check it out!</p>

                <h1 id="tabelLabel" >Upcoming Events
                    <button className="btn btn-primary btn-right" onClick={this.rerouteToEventCreation}>Create</button>
                </h1>
                {contents}
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
        const response = await fetch('api/eventsquery/upcoming');
        const data = await response.json();
        this.setState({ events: data, loading: false });
    }

    handleHostClick = () => {
        window.open('/CandidateQuiz');
      };
}
