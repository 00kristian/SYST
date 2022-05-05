import React, { Component } from 'react';
import { InteractiveTable } from './InteractiveTable';

export class Events extends Component {
    static displayName = Events.name;
    
    constructor(props) {
        super(props);
        this.state = { recent: [], upcoming: [], loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    render() {
        let UpcomingContents = this.state.loading
            ? <p><em>Loading...</em></p>
            : <InteractiveTable Columns={[["Id", "id"], ["Name", "name"], ["Date", "date"], ["Location", "location"], ["Rating", "rating"]]} Content={this.state.upcoming}>
                {event =>
                    <div>
                        <td><a href={'/eventdetail/' + event.id}> <button className="btn btn-host btn-right">Details</button></a></td>
                        <td onClick={()=> window.open('/CandidateQuiz/' + event.id + '/' + event.quiz.id, "_blank", 'location=yes,height=800,width=1300,scrollbars=yes,status=yes')}><button className="btn btn-primary btn-right">Host</button></td>
                    </div>
                }
            </InteractiveTable>;
        let RecentContents = this.state.loading
            ? <p><em>Loading...</em></p>
            : <InteractiveTable SearchBar={true} PageSize={7} Columns={[["Id", "id"], ["Name", "name"], ["Date", "date"], ["Location", "location"], ["Rating", "rating"]]} Content={this.state.recent}>
            {event =>
                <div>
                    <td><a href={'/eventdetail/' + event.id}> <button className="btn btn-host btn-right">Details</button></a></td>
                </div>
            }
        </InteractiveTable>;

        return (
            <div>
                <h3 id="tabelLabel" >Upcoming Events
                    <button className="btn btn-primary btn-right" onClick={this.rerouteToEventCreation}>Create</button>
                </h3>
                {UpcomingContents}
                <br/>
                <h3 className="obj-top_padding">All Events
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
        const response1 = await fetch('api/events');
        const data1 = await response1.json();
        const response2 = await fetch('api/eventsquery/upcoming');
        const data2 = await response2.json();
        this.setState({ recent: data1, upcoming: data2, loading: false });
    }
}
