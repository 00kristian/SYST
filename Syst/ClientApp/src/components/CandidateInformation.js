import React, { Component } from 'react';
import { Dropdown } from 'rsuite';
//import 'rsuite/dist/styles/rsuite-default.css';

import DatePicker from "react-datepicker";

import "react-datepicker/dist/react-datepicker.css";
import DropdownItem from 'rsuite/esm/Dropdown/DropdownItem';
import { DropdownMenu, DropdownMenuItem } from 'rsuite/esm/Picker';

export class CandidateInformation extends Component {
    static displayName = CandidateInformation.name;

    constructor(props) {
        super(props);
        this.state = { name: "", Email: "", University: "", StudyProgram: "", GraduationDate: new Date()};
    }

    render() {
        return (
            <div className= "CandidateInformation">
                <h2>Please write your contact information to enter the competition</h2>
                <br/>
                <form>
                    <label>
                        <h5>Name</h5>
                        <input className="input-field" onChange={(candidate) => this.state.name = candidate.target.value}></input>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5>Email</h5>
                        <input className="input-field" onChange={(candidate) => this.state.Email = candidate.target.value}></input>
                        <p>Please enter your email address in format: yourname@example.com</p>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5>University</h5>
                        <Dropdown title="Select University">
                            <Dropdown.Item>
                                <button> ITU </button>
                            </Dropdown.Item>
                            <Dropdown.Item>KU</Dropdown.Item>
                            <Dropdown.Item>DTU</Dropdown.Item>
                            <Dropdown.Item>AU</Dropdown.Item>
                            <Dropdown.Item>SDU</Dropdown.Item>
                            <Dropdown.Item>RUC</Dropdown.Item>
                            <Dropdown.Item>AAU</Dropdown.Item>
                            <Dropdown.Item>CBS</Dropdown.Item>
                            <Dropdown.Item>Other</Dropdown.Item>
                        </Dropdown>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5>Study Program</h5>
                        <input className="input-field" onChange={(candidate) => this.state.StudyProgram= candidate.target.value}></input>
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
                <button className="btn btn-primary rightbtn" onClick={this.rerouteToConfirmation}>Submit</button>
            </div>
        );
    }

    rerouteToConfirmation = () => {
        let candidate = {
            "name": this.state.name,
            "email": this.state.Email,
            "university": this.state.University,
            "studyProgram": this.state.StudyProgram,
            "graduationDate": this.state.graduationDate.toDateString()
        };
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(candidate)
        };
        fetch('api/candidates', requestOptions)
        .then(response => response.json())
        const { history } = this.props;
        history.push("/ConfirmationCandidate");
    }
}
