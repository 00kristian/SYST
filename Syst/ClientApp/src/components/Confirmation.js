import React, { Component } from 'react';
import figure from './confirmation-figure.png';


export class Confirmation extends Component {

    static displayName = Confirmation.name;

    constructor(props) {
        super(props);
        this.state = { events: [], loading: true };
    }
    
    render() {
       return (
            <div>
                <div className={"figureConfirmation"}>
                    <img src={figure} alt="figure" width={230}/>
                </div>
                <div className={"confirmationText"}>
                <h1>Your event has successfully been created</h1>
                <h3>Event details</h3>
                <h5>Name</h5>
                <h5>Date</h5>
                <h5>Location</h5>
                <button className={"btn-primary confirmationbtn"} onClick={this.rerouteToHomePage}>OK</button>
                </div>
            </div>
        );
    }

    rerouteToHomePage = () => {
        const { history } = this.props;
        history.push("/");
    }
}