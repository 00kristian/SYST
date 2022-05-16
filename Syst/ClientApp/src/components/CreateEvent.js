import React, { useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import {DatePicker} from './DatePicker';
import { QuizPicker } from './QuizPicker';
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { FetchOptions } from "./FetchOptions";

export default CreateEvent

function CreateEvent(props) {
    const [quizes, setQuizes] = useState([]);
    const [quizId, setQuizId] = useState(-1);
    const quiz = quizes.find(quiz => quiz.id === quizId);
    const [name, setName] = useState("");
    const [location, setLocation] = useState("");
    const [date, setDate] = useState(new Date());
    const history = useHistory();
    const { instance, accounts } = useMsal();

    useEffect(async () => {
        const options = await FetchOptions.Options(instance, accounts, "GET");
        const data = await fetch('api/quiz', options)
        .then(response => response.json())
        .catch(error => console.log(error));
        
        setQuizes(data);
    }, []);

    useEffect(async () => {
        const options = await FetchOptions.Options(instance, accounts, "GET");
        const data = await fetch('api/events/'+props.match.params.id, options)
        .then(response => response.json())
        .catch(error => console.log(error));
        setName(data.name);
        setLocation(data.location);
        setDate(new Date(data.date.split('T')[0]));
        setQuizId(data.quiz.id)
    }, []);

    async function updateQuizId(qId) {
        const options = await FetchOptions.Options(instance, accounts, "PUT");
        options.headers ={
            ...options.headers,
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        };


        await fetch('api/events/' + props.match.params.id + '/' + qId, options);
    };

    const updateEvent = async () => {

        let event = {
            "name": name,
            "date": date.toISOString(),
            "location": location
        };
        const options = await FetchOptions.Options(instance, accounts, "PUT");
        options.headers ={
            ...options.headers,
            'Content-Type': 'application/json',
        }
        options.body = JSON.stringify(event);
        
        await fetch('api/events/' + props.match.params.id, options);
    }

    const _confirm = async () => {
        await updateEvent();
        if (quizId > 0) await updateQuizId(quizId);
        history.push("/eventdetail/" + props.match.params.id);
    }

    const editQuiz = async (qId) => {
        await updateEvent();
        await updateQuizId(qId);
        history.push("/CreateQuiz/" + props.match.params.id +"/"+ qId);
    }

    const createQuiz = async () => {
        let quiz = {
            "name": "New quiz"
        };
        const options = await FetchOptions.Options(instance, accounts, "POST");
        options.headers ={
            ...options.headers,
            'Content-Type': 'application/json',
        }
        options.body = JSON.stringify(quiz);
       
        const qId = await fetch('api/quiz', options)
        .then(response => response.json());
        console.log(qId);

        setQuizId(qId);
        await editQuiz(qId);
    }

    const deleteEvent = async () => {
        const options = await FetchOptions.Options(instance, accounts, "DELETE");
        options.headers ={
            ...options.headers,
            'Content-Type': 'application/json',
        }
        options.body = JSON.stringify(props.match.params.id);
         
            await fetch('api/events'+"/"+props.match.params.id, options);

            history.push("/events");
    }

    return (
       <AuthenticatedTemplate>
        <div className="page-padding">
            <h2>Here you can create or edit an event</h2>
            <br/>
            <form>
                <label>
                    <h5>Name</h5>
                    <input value={name} className="input-layout txt-small" onChange={(event) => setName(event.target.value)}></input>
                </label>
                <br /> <br />
                <label>
                    <h5>Date</h5>
                    {DatePicker.Picker(date, ((date) => setDate(new Date(date))))}
                </label>
                <br /> <br />
                <label>
                    <h5>Location</h5>
                    <input value={location} className="input-layout txt-small" onChange={(event) => setLocation(event.target.value)}></input>
                </label>
            </form>
            <br />
            <h5>Quiz</h5>
            <button onClick={createQuiz} className="btn btn-primary">CREATE NEW QUIZ</button>
            {(quizId > 0) ? (
                <div> 
                    <button onClick={() => editQuiz(quizId)} className="btn btn-tertiary">Edit quiz</button>
                </div>
                ) : <span/>}
            {QuizPicker.Picker(quizes, quizId, (qId) => setQuizId(qId))}
            <br />
            <button onClick={_confirm} className="btn btn-primary btn-right btn-corner">SAVE EVENT</button>
            <Popup className="popup-overlay" trigger = {<button  className="btn btn-delete btn-right btn-corner">DELETE</button>} modal nested>
              {close => (
                <div>
                  <p className="txt-popup">Are you sure you want to delete this event?</p>
                  <div className="div-center">
                    <button className="btn btn-primary btn-yes btn-popup" onClick={()=>deleteEvent()}>YES</button>
                    <button className="btn btn-primary btn-popup"onClick={() => {close();}}>NO</button>
                  </div>
                  </div>
              )}
            </Popup>
            <button onClick={() => history.push("/eventdetail/" + props.match.params.id)} className="btn btn-secondary">Cancel</button>
            
        </div>
       </AuthenticatedTemplate>   
            );
}