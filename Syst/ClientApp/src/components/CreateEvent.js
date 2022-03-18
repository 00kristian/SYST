import React, { Component } from 'react';

export class CreateEvent extends Component {
    static displayName = CreateEvent.name;


    render() {
        return (
            <div>
                <h1>Here you can create an event</h1>
                <form>
                    <label>
                        <h3>Name</h3>
                        <input className="input-field"></input>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h3>Date</h3>
                        <input className="input-field"></input>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h3>Location</h3>
                        <input className="input-field"></input>
                    </label>
                </form>
                <br />
                <h3>Quiz</h3>
                <button className="btn btn-primary" >Create quiz</button>
                <br />
                <br />
                <button className="btn btn-primary rightbtn" >Create event</button>
            </div>
        );
    }
}
