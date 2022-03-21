import React, { Component } from 'react';

export class CreateEvent extends Component {
    static displayName = CreateEvent.name;


    render() {
        return (
            <div>
                <h2>Here you can create an event</h2>
                <br/>
                <form>
                    <label>
                        <h5>Name</h5>
                        <input className="input-field"></input>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5>Date</h5>
                        <input className="input-field"></input>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5>Location</h5>
                        <input className="input-field"></input>
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
        const { history } = this.props;
        history.push("/Confirmation");
    }

    rerouteToEvents = () => {
        const { history } = this.props;
        history.push("/Events");
    }
}
