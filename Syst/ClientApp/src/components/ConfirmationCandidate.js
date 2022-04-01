import React, { Component } from 'react';
import figure from './confirmation-figure.png';


export class ConfirmationCandidate extends Component {

    static displayName = ConfirmationCandidate.name;

    constructor(props) {
        super(props);
        this.state = { candidate: [], loading: true };
    }
    
    render() {
       return (
            <div>
                <div className={"figureConfirmation"}>
                    <img src={figure} alt="figure" width={230}/>
                </div>
                <div className={"confirmationText"}>
                <h1>Thank you for submitting</h1>
                <a href='/CandidateQuiz'><button className={"btn-primary confirmationbtn"}>OK</button></a>
                </div>
            </div>
        );
    }

    rerouteToHomePage = () => {
        const { history } = this.props;
        history.push("/");
    }
}