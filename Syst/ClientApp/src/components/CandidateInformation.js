import React, { Component } from 'react';
import Dropdown from 'react-dropdown';


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
        
        const educations = [
            'BSc',
            'MSc',
            'PhD'
        ];
        
        return (
            <div>
                <div className='div-header'>
                <h5>PLEASE WRITE YOUR CONTACT INFORMATION TO ENTER THE COMPETITION</h5>
                </div>
                <br/>
                <div className='div-input_layout'>
                <form>
                    {this.state.validateName ? (
                        <div>
                            <label>
                                <h5 className='txt-red'>* Name</h5>
                                <input className="input-layout input-candInfoPage txt-small" onChange={(candidate) => this.state.Name = candidate.target.value} placeholder="Name"></input>
                            </label>
                        </div>
                    ) : (
                        <div>
                            <label>
                                <h5>Name</h5>
                                <input className="input-layout input-candInfoPage txt-small" onChange={(candidate) => this.state.Name = candidate.target.value} placeholder="Name"></input>
                            </label>
                        </div>
                    )}
                    <br />
                    {this.state.validateEmail ? (
                        <div>
                            <label>
                                <h5 className='txt-red'>* E-mail</h5>
                                <input className="input-layout input-candInfoPage txt-small" onChange={(candidate) => this.state.Email = candidate.target.value } placeholder="E-mail"></input>
                                <p className="txt-example">e.g. <i>example@mail.com</i></p>
                            </label>
                        </div>
                    ) : (
                        <div>
                            <label>
                                <h5>E-mail</h5>
                                <input className="input-layout input-candInfoPage txt-small" onChange={(candidate) => this.state.Email = candidate.target.value } placeholder="E-mail"></input>
                                <p className="txt-example">e.g. <i>example@mail.com</i></p>
                            </label>
                    </div>
                    )}
                    <br />

                    {this.state.validateUniversity ? (
                        <label>
                        <h5 className='txt-red'> * University</h5>
                        <Dropdown className="dropdown-length txt-small" options={options} onChange={this.selectUni} value="Select your university"/>
                    </label>
                    ) : (
                        <label>
                        <h5>University</h5>
                        <Dropdown className="dropdown-length txt-small" options={options} onChange={this.selectUni} value="Select your university"/>
                    </label>
                    )}

                    <br />
                    {this.state.ShowSpecialUni ? (
                        <div>
                            <label>
                                <input className="input-layout input-candInfoPage txt-small" onChange={(uni) => this.state.University= uni.target.value} placeholder="University"></input>
                            </label>
                        </div>
                    ) : (
                    <div></div>
                    )}
                    <br />

                    {this.state.validateStudyProgram ? (
                        <div>
                            <label>
                                <h5 className='txt-red'>* Degree</h5>
                                <Dropdown className="dropdown-length txt-small"  options={educations} onChange={this.selectDegree} value="Select your program"/>                            
                            </label>
                            <br />
                            <br />
                            <label>
                                <h5 className='txt-red'>* Study Program</h5>
                                <input className="input-layout input-candInfoPage txt-small" onChange={(candidate) => this.state.StudyProgram = candidate.target.value} placeholder="Study Program"></input>
                                <p className="txt-example">e.g. <i>Computer Science</i> or <i>Law</i></p>
                            </label>
                        </div>
                    ) : (
                        <div>
                            <label>
                                <h5>Degree</h5>
                                <Dropdown className="dropdown-length txt-small" options={educations} onChange={this.selectDegree} value="Select your program"/>
                            </label>
                            <br />
                            <br />
                            <label>
                                <h5>Study Program</h5>
                                <input className="input-layout input-candInfoPage txt-small" onChange={(candidate) => this.state.StudyProgram = candidate.target.value} placeholder="Study Program"></input>
                                <p className="txt-example">e.g. <i>Computer Science</i> or <i>Law</i></p>
                            </label>
                        </div>
                    )}
                    <br />
                    <label>
                        <h5>Graduation Month</h5>
                        <input onInput={(v) => {
                            this.setState({GraduationDate: new Date(v.target.value + "-01")});
                        }} 
                        defaultValue={Date.now}
                        type="month"
                        min="2018-01" max="2030-12"></input>
                    </label>
                    <br />
                    <br />
                    {(!this.state.validateCheckBox && this.state.clickedOnSubmit) ? (
                        <div>
                            <p className='txt-red'><input id="checkbox" type="checkbox" onClick={this.checkedBox}/> * Accept that Systematic can store your information <a href='https://systematic.com/da-dk/kontakt/privacy-policyings/'>Read more</a></p>
                        </div>
                    ) : (
                    <div>
                        <p><input type="checkbox" onClick={(event) => this.checkedBox(event)}/> Accept that Systematic can store your information <a href='https://systematic.com/da-dk/kontakt/privacy-policyings/'>Read more</a></p>
                    </div>
                    )}
                </form>
                <br />
                    {(this.state.validateName || this.state.validateEmail || this.state.validateStudyProgram) ? (
                        <div>
                            <label>
                                <p className='txt-red'>* Need to be filled out</p>
                            </label>
                        </div>
                    ) : (
                    <div></div>
                    )}
                    <button className="btn btn-primary btn-right" onClick={this.rerouteToCandidateConfirmation}>SUBMIT</button>
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
    
    selectDegree = (education) => {this.setState({CurrentDegree: education.value});}

    checkedBox = (e) => {
        this.setState({validateCheckBox : e.target.checked})
    }

    rerouteToCandidateConfirmation = async () => {

        this.setState({clickedOnSubmit : true});

        const NameGood = this.state.Name.length !== 0;
        const EmailGood = this.state.Email.length !== 0;
        const UniversityGood = this.state.University.length !== 0;
        const DegreeGood = this.state.CurrentDegree.length !== 0;
        const StudyProgramGood = this.state.StudyProgram.length !== 0;
        const CheckBoxGood = this.state.validateCheckBox;

        this.setState({validateName : !NameGood});
        this.setState({validateEmail : !EmailGood});
        this.setState({validateUniversity : !UniversityGood});
        this.setState({validateDegree : !DegreeGood})
        this.setState({validateStudyProgram : !StudyProgramGood});

        if(NameGood && EmailGood && UniversityGood && DegreeGood && StudyProgramGood && CheckBoxGood) {

            console.log("Den er god")
            
            let candidate = {
                "name": this.state.Name,
                "email": this.state.Email,
                "university": this.state.University,
                "currentDegree": this.state.CurrentDegree,
                "studyProgram": this.state.StudyProgram,
                "upVote": this.state.upVote,
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

            console.log(candidate);
            console.log(Answer);

            fetch('api/candidates/answer/' + candid, requestOptions2)
            .then(response => response.json())

            window.location.reload(true);
        }
    }
}