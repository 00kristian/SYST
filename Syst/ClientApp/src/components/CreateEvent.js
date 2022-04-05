import React, { Component } from 'react';

import {DatePicker} from './DatePicker';

import "react-datepicker/dist/react-datepicker.css";

export class CreateEvent extends Component {
    static displayName = CreateEvent.name;

    constructor(props) {
        super(props);
        this.state = { event : {name: "", date: new Date(), location: ""}, loading: true};
    }

    componentDidMount() {  
        this.populateData();
    }

    static renderEvent(e) {
        return (
            <form>
                <label>
                    <h5>Name</h5>
                    <input placeholder={e.name} className="input-field" onChange={(event) => e.name = event.target.value}></input>
                </label>
                <br />
                <br />
                <label>
                    {DatePicker.Picker(e.date.toISOString().split('T')[0], ((date) => e.date = new Date(date)))}
                </label>
                <br />
                <br />
                <label>
                    <h5>Location</h5>
                    <input placeholder={e.location} className="input-field" onChange={(event) => e.location = event.target.value}></input>
                </label>
            </form>
        );
    }

    render() {
        let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : CreateEvent.renderEvent(this.state.event);
        return (
            <div>
                <h2>Here you can create an event</h2>
                <br/>
                {contents}
                <br />
                <h5>Quiz</h5>
                <button className="btn btn-primary" onClick={this.rerouteToCreateQuiz}>Create quiz</button>
                <br />
                <br />
                <button className="btn btn-primary rightbtn" onClick={this.rerouteToConfirmation}>Save event</button>
                <br />
                <br />
                <button className="btn btn-primary rightbtn" onClick={this.rerouteToEvents}>Cancel</button>
            </div>
        );
    }

    rerouteToConfirmation = () => {
        this.updateEvent();
        const { history } = this.props;
        history.push("/eventdetail/" + this.props.match.params.id);
    }

    updateEvent = () => {
        let event = {
            "name": this.state.event.name,
            "date": this.state.event.date.toISOString(),
            "location": this.state.event.location
        };
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(event)
        };
        fetch('api/events/' + this.props.match.params.id, requestOptions);
    }

    rerouteToEvents = () => {
        const { history } = this.props;
        history.push("/Events");
    }

    rerouteToCreateQuiz = async () => {
        let quiz = {
            "name": "New quiz"
        };
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(quiz)
        };
        let quizid = await fetch('api/quiz', requestOptions)
        .then(response => response.json())

        this.updateEvent();

        const requestOptions2 = {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        };

        fetch('api/events/' + this.props.match.params.id + '/' + quizid, requestOptions2);

        const { history } = this.props;
        history.push("/CreateQuiz/" + this.props.match.params.id +"/"+ quizid);
    }

    async populateData() {
        const response = await fetch('api/events/' + this.props.match.params.id);
        const data = await response.json();
        //TODO: fix that date isnt loaded properly
        this.setState({  event : {name: data.name, date: new Date(data.date.split('T')[0]), location: data.location}, loading: false });
    }

}
