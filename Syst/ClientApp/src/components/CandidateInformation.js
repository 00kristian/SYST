import React, { Component } from 'react';
import Dropdown from 'react-dropdown';
import DatePicker from "react-datepicker";

import "react-datepicker/dist/react-datepicker.css";
import 'react-dropdown/style.css'

export class CandidateInformation extends Component {
    static displayName = CandidateInformation.name;

    constructor(props) {
        super(props);
        this.state = {Answers: props.Answers, Name: "", Email: "", University: "", StudyProgram: "", ShowSpecialUni : false, validateName : false, validateEmail : false, validateStudyProgram : false, validateCheckBox : false, clickedOnSubmit : false, validateUniversity : false, GraduationDate: new Date()};
    }
    

    render() {
        const options = [
            'Aalborg University',
            'Aarhus University',
            'Copenhagen Business School',
            'IT-University of Copenhagen',
            'Roskilde University',
            'Technical University of Denmark',
            'University of Copenhagen',
            'University of Southern Denmark',
            'Other'  
        ];
        
        return (
            <div>
                <div className='div-header'>
                <h5>Please write your contact information to enter the competition</h5>
                </div>
                <br/>
                <div className='div-input_layout'>
                <form>
                    {this.state.validateName ? (
                        <div>
                            <label>
                                <h5 className='txt-red'>*Name</h5>
                                <input className="input-layout" onChange={(candidate) => this.state.Name = candidate.target.value} placeholder="Name"></input>
                            </label>
                        </div>
                    ) : (
                        <div>
                            <label>
                                <h5>Name</h5>
                                <input className="input-layout" onChange={(candidate) => this.state.Name = candidate.target.value} placeholder="Name"></input>
                            </label>
                        </div>
                    )}
                    <br />
                    <br />
                    {this.state.validateEmail ? (
                        <div>
                            <label>
                                <h5 className='txt-red'>*Email</h5>
                                <input className="input-layout" onChange={(candidate) => this.state.Email = candidate.target.value } placeholder="Email"></input>
                                <p>Please enter your email address in format: yourname@example.com</p>
                            </label>
                        </div>
                    ) : (
                        <div>
                            <label>
                                <h5>Email</h5>
                                <input className="input-layout" onChange={(candidate) => this.state.Email = candidate.target.value } placeholder="Email"></input>
                                <p>Please enter your email address in format: yourname@example.com</p>
                            </label>
                    </div>
                    )}
                    <br />
                    <br />

                    {this.state.validateUniversity ? (
                        <label>
                        <h5 className='txt-red'> *University</h5>
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
                                <input className="input-layout" onChange={(uni) => this.state.University= uni.target.value} placeholder="University"></input>
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
                                <h5 className='txt-red'>*Study Program</h5>
                                <input className="input-layout" onChange={(candidate) => this.state.StudyProgram = candidate.target.value} placeholder="Study Program"></input>
                            </label>
                        </div>
                    ) : (
                        <div>
                            <label>
                                <h5>Study Program</h5>
                                <input className="input-layout" onChange={(candidate) => this.state.StudyProgram = candidate.target.value} placeholder="Study Program"></input>
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
                            <p className='txt-red'><input id="checkbox" type="checkbox" onClick={this.checkedBox}/> *Accept that Systematic can store your information <a href='https://systematic.com/da-dk/kontakt/privacy-policyings/'>Read more</a></p>
                        </div>
                    ) : (
                    <div>
                        <p><input type="checkbox" onClick={(event) => this.checkedBox(event)}/> Accept that Systematic can store your information <a href='https://systematic.com/da-dk/kontakt/privacy-policyings/'>Read more</a></p>
                    </div>
                    )}
                    <p><input type="checkbox" id='checkbox'/> Accept Systematics newsletters........</p>
                </form>
                <br />
                    {(this.state.validateName || this.state.validateEmail || this.state.validateStudyProgram) ? (
                        <div>
                            <label>
                                <p className='txt-red'>*Need to be filled out</p>
                            </label>
                        </div>
                    ) : (
                    <div></div>
                    )}
                    <button className="btn btn-primary btn-right" onClick={this.rerouteToCandidateConfirmation}>Submit</button>
                </div>
            </div>
        );
    }

    selectUni = (option) => {
        if (option.value === 'Other') {
            this.setState({ShowSpecialUni: true});
        } else {
            this.setState({ShowSpecialUni: false, University: option.value});
        }
    }

    checkedBox = (e) => {
        this.setState({validateCheckBox : e.target.checked})
    }

    rerouteToCandidateConfirmation = async () => {

        this.setState({clickedOnSubmit : true});

        const NameGood = this.state.Name.length !== 0;
        const EmailGood = this.state.Email.length !== 0;
        const UniversityGood = this.state.University.length !== 0;
        const StudyProgramGood = this.state.StudyProgram.length !== 0;
        const CheckBoxGood = this.state.validateCheckBox;

        this.setState({validateName : !NameGood});
        this.setState({validateEmail : !EmailGood});
        this.setState({validateUniversity : !UniversityGood});
        this.setState({validateStudyProgram : !StudyProgramGood});

        if(NameGood && EmailGood && UniversityGood && StudyProgramGood && CheckBoxGood) {


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
            let candid = await  fetch('api/candidates', requestOptions)
            .then(response => response.json())

            let Answer = {
                quizid: this.props.QuizId,
                eventid: this.props.EventId,
                answers: this.state.Answers
            }

            const requestOptions2 = {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(Answer)
            };

            fetch('api/candidates/answer/' + candid, requestOptions2)
            .then(response => response.json())

            window.location.reload(true);
        }
    }
}