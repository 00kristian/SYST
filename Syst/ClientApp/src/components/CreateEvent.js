import React, { Component } from 'react';

export class CreateEvent extends Component {
    static displayName = CreateEvent.name;


    render() {
        return (
            <div>
                <h1>Here you can create an event</h1>
                <form>
                    <label>
                        <h2>Name</h2>
                        <input></input>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h2>Date</h2>
                        <input></input>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h2>Location</h2>
                        <input></input>
                    </label>
                </form>
                <br />
                <h2>Quiz</h2>
                <button className="btn btn-primary" >Create quiz</button>
                <br />
                <br />
                <button className="btn btn-primary" >Create event</button>

                
            </div>
        );
    }
}
