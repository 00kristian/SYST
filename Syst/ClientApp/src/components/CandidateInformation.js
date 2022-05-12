import React, { useEffect, useState } from "react";
import Dropdown from 'react-dropdown';
import {FetchOptions} from './FetchOptions';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";

import "react-datepicker/dist/react-datepicker.css";
import 'react-dropdown/style.css'


export function CandidateInformation(props) {

    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [university, setUniversity] = useState("");
    const [studyProgram, setStudyProgram] = useState("");
    const [currentDegree, setCurrentDegree] = useState("");
    const [showSpecialUni, setShowSpecialUni] = useState(false);
    const [graduationDate, setGraduationDate] = useState(new Date());
    const [validateName, setValidateName] = useState(true);
    const [validateEmail, setValidateEmail] = useState(true);
    const [validateStudyProgram, setValidateStudyProgram] = useState(true);
    const [validateCheckBox, setValidateCheckBox] = useState(true);
    const [validateUniversity, setValidateUniversity] = useState(true);
    const [validateDegree, setValidateDegree] = useState(true);
    const [clickedOnSubmit, setClickedOnSubmit] = useState(false);
    const { instance, accounts } = useMsal();
    
    const universities = [
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

    const selectUni = (option) => {
        if (option.value === 'Other') {
            setShowSpecialUni(true);
        } else {
            setShowSpecialUni(false);
            setUniversity(option.value);
        }
    }
    
    const selectDegree = (education) => {setCurrentDegree(education.value);}

    const checkedBox = (e) => {
        setValidateCheckBox(e.target.checked);
    }

    const rerouteToCandidateConfirmation = async () => {

        setClickedOnSubmit(true);

        const NameGood = name.length !== 0;
        const EmailGood = email.length !== 0;
        const UniversityGood = university.length !== 0;
        const DegreeGood = currentDegree.length !== 0;
        const StudyProgramGood = studyProgram.length !== 0;
        const CheckBoxGood = validateCheckBox;

        setValidateName(name.length !== 0);
        setValidateEmail(email.length !== 0);
        setValidateUniversity(university.length !== 0);
        setValidateDegree(currentDegree.length !== 0);
        setValidateStudyProgram(studyProgram.length !== 0);
        setValidateCheckBox(validateCheckBox)

        if(NameGood && EmailGood && UniversityGood && DegreeGood && StudyProgramGood && CheckBoxGood) {
            
            let candidate = {
                "name": name,
                "email": email,
                "university": university,
                "currentDegree": currentDegree,
                "studyProgram": studyProgram,
                "upVote": false,
                "graduationDate": graduationDate.toDateString()
            };
            const options = await FetchOptions.Options(instance, accounts, "POST");
            options.headers = {
                ...options.headers,
                'Content-Type': 'application/json' 
            }
            options.body = JSON.stringify(candidate);

            let candid = await  fetch('api/candidates', options)
            .then(response => response.json())

            let answer = {
                quizid: props.QuizId,
                eventid: props.EventId,
                answers: props.Answers
            }

            const options2 = await FetchOptions.Options(instance, accounts, "POST");
            options2.headers = {
                ...options.headers,
                'Content-Type': 'application/json' 
            }
            options2.body = JSON.stringify(answer);

            fetch('api/candidates/answer/' + candid, options2)
            .then(response => response.json())

            window.location.reload(true);
        }
    }
        
    return (
        
        <div>
            <div className='div-header'>
            <h5>PLEASE WRITE YOUR CONTACT INFORMATION TO ENTER THE COMPETITION</h5>
            </div>
            <br/>
            <div className='div-input_layout'>
            <form>
                {!validateName ? (
                    <div>
                        <label>
                            <h5 className='txt-red'>* Name</h5>
                            <input className="input-layout input-candInfoPage txt-small" onChange={(event) => setName(event.target.value)} placeholder="Name"></input>
                        </label>
                    </div>
                ) : (
                    <div>
                        <label>
                            <h5>Name</h5>
                            <input className="input-layout input-candInfoPage txt-small" onChange={(event) => setName(event.target.value)} placeholder="Name"></input>
                        </label>
                    </div>
                )}
                <br />
                {!validateEmail ? (
                    <div>
                        <label>
                            <h5 className='txt-red'>* E-mail</h5>
                            <input className="input-layout input-candInfoPage txt-small" onChange={(event) => setEmail(event.target.value) } placeholder="E-mail"></input>
                            <p className="txt-example">e.g. <i>example@mail.com</i></p>
                        </label>
                    </div>
                ) : (
                    <div>
                        <label>
                            <h5>E-mail</h5>
                            <input className="input-layout input-candInfoPage txt-small" onChange={(event) => setEmail(event.target.value) } placeholder="E-mail"></input>
                            <p className="txt-example">e.g. <i>example@mail.com</i></p>
                        </label>
                </div>
                )}
                <br />

                {!validateUniversity ? (
                    <label>
                    <h5 className='txt-red'> * University</h5>
                    <Dropdown className="dropdown-length txt-small" options={universities} onChange={selectUni} value="Select your university"/>
                </label>
                ) : (
                    <label>
                    <h5>University</h5>
                    <Dropdown className="dropdown-length txt-small" options={universities} onChange={selectUni} value="Select your university"/>
                </label>
                )}

                <br />
                {showSpecialUni ? (
                    <div>
                        <label>
                            <input className="input-layout input-candInfoPage txt-small" onChange={(event) => setUniversity(event.target.value)} placeholder="University"></input>
                        </label>
                    </div>
                ) : (
                <div></div>
                )}
                <br />

                {!validateStudyProgram ? (
                    <div>
                        <label>
                            <h5 className='txt-red'>* Degree</h5>
                            <Dropdown className="dropdown-length txt-small"  options={educations} onChange={selectDegree} value="Select your program"/>                            
                        </label>
                        <br />
                        <br />
                        <label>
                            <h5 className='txt-red'>* Study Program</h5>
                            <input className="input-layout input-candInfoPage txt-small" onChange={(event) => setStudyProgram(event.target.value)} placeholder="Study Program"></input>
                            <p className="txt-example">e.g. <i>Computer Science</i> or <i>Law</i></p>
                        </label>
                    </div>
                ) : (
                    <div>
                        <label>
                            <h5>Degree</h5>
                            <Dropdown className="dropdown-length txt-small" options={educations} onChange={selectDegree} value="Select your program"/>
                        </label>
                        <br />
                        <br />
                        <label>
                            <h5>Study Program</h5>
                            <input className="input-layout input-candInfoPage txt-small" onChange={(event) => setStudyProgram(event.target.value)} placeholder="Study Program"></input>
                            <p className="txt-example">e.g. <i>Computer Science</i> or <i>Law</i></p>
                        </label>
                    </div>
                )}
                <br />
                <label>
                    <h5>Graduation Month</h5>
                    <input onInput={(v) => {
                        setGraduationDate(new Date(v.target.value + "-01"));
                    }} 
                    defaultValue={Date.now}
                    type="month"
                    min="2018-01" max="2030-12"></input>
                </label>
                <br />
                <br />
                {(!validateCheckBox && clickedOnSubmit) ? (
                    <div>
                        <p className='txt-red'><input id="checkbox" type="checkbox" onClick={checkedBox}/> * Accept that Systematic can store your information <a href='https://systematic.com/da-dk/kontakt/privacy-policyings/'>Read more</a></p>
                    </div>
                ) : (
                <div>
                    <p><input type="checkbox" onClick={(event) => checkedBox(event)}/> Accept that Systematic can store your information <a href='https://systematic.com/da-dk/kontakt/privacy-policyings/'>Read more</a></p>
                </div>
                )}
            </form>
            <br />
                {(!validateName || !validateEmail || !validateStudyProgram) ? (
                    <div>
                        <label>
                            <p className='txt-red'>* Need to be filled out</p>
                        </label>
                    </div>
                ) : (
                <div></div>
                )}
                <button className="btn btn-primary btn-right" onClick={rerouteToCandidateConfirmation}>SUBMIT</button>
            </div>
        </div>
            
    );
}