import React, { Component } from 'react';

import DatePicker from "react-datepicker";

import "react-datepicker/dist/react-datepicker.css";

export class CreateEvent extends Component {
    static displayName = CreateEvent.name;

    constructor(props) {
        super(props);
        this.state = { name: "", date: new Date(), location: ""};
    }

    render() {
        return (
            <div>
                <h2>Here you can create an event</h2>
                <br/>
                <form>
                    <label>
                        <h5>Name</h5>
                        <input className="input-field" onChange={(event) => this.state.name = event.target.value}></input>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5>Date</h5><DatePicker selected={this.state.date} onChange={(date) => {
                            this.state.date.setDate(date.getDate());
                            this.state.date.setMonth(date.getMonth());
                            this.state.date.setFullYear(date.getFullYear());
                        }} />
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5>Location</h5>
                        <input className="input-field" onChange={(event) => this.state.location = event.target.value}></input>
                    </label>
                </form>
                <br />
                <h5>Quiz</h5>
                <button className="btn btn-primary" >Create quiz</button>
                <br />
                <br />
                <button className="btn btn-primary rightbtn" onClick={this.rerouteToConfirmation}>Create event</button>
                <br />
                <br />
                <button className="btn btn-primary rightbtn" onClick={this.rerouteToEvents}>Cancel</button>
            </div>
        );
    }

    rerouteToConfirmation = () => {
        let event = {
            "name": this.state.name,
            "date": this.state.date.toDateString(),
            "location": this.state.location
        };
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(event)
        };
        fetch('api/events', requestOptions)
        .then(response => response.json())
        const { history } = this.props;
        history.push("/Confirmation");
    }

    rerouteToEvents = () => {
        const { history } = this.props;
        history.push("/Events");
    }
}
