import React, { Component } from 'react';
import logo from './Systematic_Logo.png';
import Dropdown from 'react-dropdown';
import DatePicker from "react-datepicker";
import Popup from './Popup';

import "react-datepicker/dist/react-datepicker.css";
import 'react-dropdown/style.css'
import 'reactjs-popup/dist/index.css';

export class CandidateInformation extends Component {
    static displayName = CandidateInformation.name;

    constructor(props) {
        super(props);
        this.state = { Name: "", Email: "", University: "", StudyProgram: "", ShowSpecialUni : false, validateName : false, validateEmail : false, validateStudyProgram : false, validateCheckBox : false, clickedOnSubmit : false, validateUniversity : false, GraduationDate: new Date()};
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
                    {this.state.validateName ? (
                        <div>
                            <label>
                                <h5 id='red-text'>*Name</h5>
                                <input className="input-field" onChange={(candidate) => this.state.Name = candidate.target.value} placeholder="Name"></input>
                            </label>
                        </div>
                    ) : (
                        <div>
                            <label>
                                <h5>Name</h5>
                                <input className="input-field" onChange={(candidate) => this.state.Name = candidate.target.value} placeholder="Name"></input>
                            </label>
                        </div>
                    )}
                    <br />
                    <br />
                    {this.state.validateEmail ? (
                        <div>
                            <label>
                                <h5 id='red-text'>*Email</h5>
                                <input className="input-field" onChange={(candidate) => this.state.Email = candidate.target.value } placeholder="Email"></input>
                                <p>Please enter your email address in format: yourname@example.com</p>
                            </label>
                        </div>
                    ) : (
                        <div>
                            <label>
                                <h5>Email</h5>
                                <input className="input-field" onChange={(candidate) => this.state.Email = candidate.target.value } placeholder="Email"></input>
                                <p>Please enter your email address in format: yourname@example.com</p>
                            </label>
                    </div>
                    )}
                    <br />
                    <br />

                    {this.state.validateUniversity ? (
                        <label>
                        <h5 id='red-text'> *University</h5>
                        <Dropdown options={options} onChange={this.selectUni} value="Select your University"/>
                    </label>
                    ) : (
                        <label>
                        <h5>University</h5>
                        <Dropdown options={options} onChange={this.selectUni} value="Select your University"/>
                    </label>
                    )}

                    <br />
                    <br />
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






                    {this.state.validateStudyProgram ? (
                        <div>
                            <label>
                                <h5 id='red-text'>*Study Program</h5>
                                <input className="input-field" onChange={(candidate) => this.state.StudyProgram = candidate.target.value} placeholder="Study Program"></input>
                            </label>
                        </div>
                    ) : (
                        <div>
                            <label>
                                <h5>Study Program</h5>
                                <input className="input-field" onChange={(candidate) => this.state.StudyProgram = candidate.target.value} placeholder="Study Program"></input>
                            </label>
                        </div>
                    )}
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
                    {(!this.state.validateCheckBox && this.state.clickedOnSubmit) ? (
                        <div>
                            <p id='red-text'><input id="checkbox" type="checkbox" onClick={this.checkedBox}/> *Accept that Systematic can store your information <a href='https://systematic.com/da-dk/kontakt/privacy-policyings/'>Read more</a></p>
                        </div>
                    ) : (
                    <div>
                        <p><input type="checkbox" onClick={this.checkedBox}/> Accept that Systematic can store your information <a href='https://systematic.com/da-dk/kontakt/privacy-policyings/'>Read more</a></p>
                    </div>
                    )}
                    <p><input type="checkbox" id='checkbox'/> Accept Systematics newsletters........</p>
                </form>
                <br />
                    {(this.state.validateName || this.state.validateEmail || this.state.validateStudyProgram) ? (
                        <div>
                            <label>
                                <p id='red-text'>*Need to be filled out</p>
                            </label>
                        </div>
                    ) : (
                    <div></div>
                    )}
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

    checkedBox = () => {
        if(this.state.validateCheckBox == false){
            this.setState({validateCheckBox : true})
        }else {
            this.setState({validateCheckBox : false})
        }
    }

    rerouteToCandidateConfirmation = () => {

        this.setState({clickedOnSubmit : true});

        if(this.state.University.length == 0){
            this.setState({validateUniversity : true});
        }

        if(this.state.Name.length == 0){
            this.setState({validateName : true});
        }

        if(this.state.Email.length == 0){
            this.setState({validateEmail : true});
        }

        if(this.state.StudyProgram.length == 0){
            this.setState({validateStudyProgram : true});
        }

        if(this.state.Name.length != 0){
            this.setState({validateName : false});
        }

        if(this.state.Email.length != 0){
            this.setState({validateEmail : false});
        }

        if(this.state.StudyProgram.length != 0){
            this.setState({validateStudyProgram : false});
        }

        if(this.state.validateUniversity != 0){
            this.setState({validateUniversity : false})
        }
        
        if(!this.state.Name.length == 0 && !this.state.Email.length == 0 && !this.state.StudyProgram == 0 && !this.state.validateUniversity ==0 && this.state.validateCheckBox == true) {
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
}