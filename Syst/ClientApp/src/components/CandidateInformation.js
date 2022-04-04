import React, { Component } from 'react';
import logo from './Systematic_Logo.png';
import Dropdown from 'react-dropdown';
import DatePicker from "react-datepicker";

import "react-datepicker/dist/react-datepicker.css";
import 'react-dropdown/style.css'

export class CandidateInformation extends Component {
    static displayName = CandidateInformation.name;

    constructor(props) {
        super(props);
        this.state = { Name: "", Email: "", University: "", StudyProgram: "", ShowSpecialUni : false, GraduationDate: new Date()};
    }

    render() {
        const options = [
            'IT-University of Copenhagen',
            'University of Copenhagen',
            'Technical University of Denmark',
            'Aarhus University',
            'University of Southern Denmark',
            'Roskilde University',
            'Aalborg University',
            'Copenhagen Business School',
            'Other'
        ];
        const defaultOption = options[0];
        return (
            <div>
                <div id="header">
                <img src={logo} alt="Logo" width={500}/>
                <h5>Please write your contact information to enter the competition</h5>
                </div>
                <br/>
                <div id="input">
                <form>

                    <label>
                        <h5>Name</h5>
                        <input className="input-field" onChange={(candidate) => this.state.Name = candidate.target.value} placeholder="Name"></input>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5>Email</h5>
                        <input className="input-field" onChange={(candidate) => this.state.Email = candidate.target.value } placeholder="Email"></input>
                        <p>Please enter your email address in format: yourname@example.com</p>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5>University</h5>
                        <Dropdown options={options} onChange={this.selectUni} value={defaultOption} placeholder="Select Your University" />
                    </label>
                    {this.state.ShowSpecialUni ? (
                        <div>
                            <label>
                                <input className="input-field" onChange={(uni) => this.state.University= uni.target.value} placeholder="University"></input>
                            </label>
                        </div>
                    ) : (
                    <div></div>
                    )}
                    <br />
                    <br />
                    <label>
                        <h5>Study Program</h5>
                        <input className="input-field" onChange={(candidate) => this.state.StudyProgram= candidate.target.value} placeholder="Study Program"></input>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5>Graduation Date</h5><DatePicker selected={this.state.GraduationDate} onChange={(graduationDate) => {
                            this.state.GraduationDate.setDate(graduationDate.getDate());
                            this.state.GraduationDate.setMonth(graduationDate.getMonth());
                            this.state.GraduationDate.setFullYear(graduationDate.getFullYear());
                        }} />
                    </label>
                    <br />
                    <br />

                    <p><input type="checkbox"/> Accept that Systematic can store your information <a href='https://systematic.com/da-dk/kontakt/privacy-policyings/'>Read more</a></p>
                    <p><input type="checkbox"/> Accept Systematics newsletters........</p>
                </form>
                <br />
                    <button className="btn btn-primary rightbtn" onClick={this.rerouteToCandidateConfirmation}>Submit</button>
                </div>
            </div>
        );
    }

    selectUni = (option) => {
        if (option.value == 'Other') {
            this.setState({ShowSpecialUni: true});
        } else {
            this.setState({ShowSpecialUni: false, University: option.value});
        }
    }

    rerouteToCandidateConfirmation = () => {
        let candidate = {
            "name": this.state.Name,
            "email": this.state.Email,
            "university": this.state.University,
            "studyProgram": this.state.StudyProgram,
            "graduationDate": this.state.GraduationDate.toDateString()
        };
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(candidate)
        };
        fetch('api/candidates', requestOptions)
        .then(response => response.json())
        const { history } = this.props;
        history.push('/ConformationCandidate');
    }
}