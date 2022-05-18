import React, { useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import { QuizPicker } from "./QuizPicker";
import Icon from "@mdi/react";
import { mdiTrashCan } from '@mdi/js';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { FetchOptions } from "./FetchOptions";

//Page where the admin can create a new quiz
export default CreateQuiz

function CreateQuiz(props) {

    const { instance, accounts } = useMsal();
    const [name, setName] = useState("");
    const [questions, setQuestions] = useState([]);
    const history = useHistory();
    const [quizes, setQuizes] = useState([]);
    const [cloneId, setCloneId] = useState(-1);

    useEffect(async () => {
        const options = await FetchOptions.Options(instance, accounts, "GET");
        const data = await fetch('api/quiz/'+props.match.params.id, options)
        .then(response => response.json())
        .catch(error => console.log(error));
        
        setName(data.name);
        setQuestions(data.questions);
    }, []);

    useEffect(async () => {
        const options = await FetchOptions.Options(instance, accounts, "GET");
        const data = await fetch('api/quiz', options)
        .then(response => response.json())
        .catch(error => console.log(error));
      

        setQuizes(data.filter(q => q.id != props.match.params.id));
    }, []);

    //Methods
    const updateQuiz = async () => {
        let quiz = {
            "name": name
        };
        const options = await FetchOptions.Options(instance, accounts, "PUT");
        options.headers ={
            ...options.headers,
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        };
        options.body = JSON.stringify(quiz);

        await fetch('api/quiz/' + props.match.params.id, options);
    }

    const _confirm = async () => {
        await updateQuiz();
        history.push("/CreateEvent/"+ props.match.params.event_id);
    }

    const addQuestion = async () => {
        let question = {
            "representation": "New question",
            "answer": "",
            "Options": [],
            "imageUrl": ""
        };
        const options = await FetchOptions.Options(instance, accounts, "PUT");
        options.headers ={
            ...options.headers,
            'Content-Type': 'application/json',
        };
        options.body = JSON.stringify(question);
        
        let index = await fetch('api/QuizQuestion/' + props.match.params.id, options)
        .then(response => response.json());
        
        setQuestions([...questions, {representation : question.representation, id : index}])
    }

    const removeQuestion = async () => {
        if (questions.length === 0) return;
        const options = await FetchOptions.Options(instance, accounts, "DELETE");
        
        
        await fetch('api/Questions/' + questions[questions.length - 1].id, options);

        setQuestions(questions.slice(0, questions.length - 1));
    }

    const modifyQuestion = (id) => {
        updateQuiz();
        history.push("/CreateQuestion/" + props.match.params.event_id + "/" + props.match.params.id + "/" + id);
    }

    const deleteQuiz = async () => {
        const options = await FetchOptions.Options(instance, accounts, "DELETE");
        options.headers ={
            ...options.headers,
            'Content-Type': 'application/json',
        }
        options.body = JSON.stringify(props.match.params.id);
           
            await fetch('api/quiz'+"/"+props.match.params.id, options);
            history.push("/CreateEvent/"+ props.match.params.event_id);
        
    }

    function renderQuiz(question, index) {
        return (
            <AuthenticatedTemplate>
            <div key={index}>
                <h5> Question {index + 1}
                    <button onClick={(event) => modifyQuestion(question.id)} className="btn btn-primary btn-modify"> MODIFY </button>
                </h5>
                <label className="obj-bottom_margin">{question.representation}</label>
                </div>
            </AuthenticatedTemplate>
        );  
    }

    const cloneQuiz = async () => {
        const options = await FetchOptions.Options(instance, accounts, "PUT");
        options.headers ={
            ...options.headers,
            'Content-Type': 'application/json',
        }
        
        await fetch('api/Quiz/' + props.match.params.id + '/clone/' + cloneId, options);
    }

    //User Interface
    return (
        <AuthenticatedTemplate>
        <div className="page-padding">
            <h2>Here you can create or edit a quiz</h2>
            <br/>
            <form>
                <div className="div-flex2">
                    <label className="row-layout">
                        <h5>Quiz name</h5>
                        <input value={name} className="input-layout txt-small" onChange={(event) => setName(event.target.value)}></input>
                    </label>
                    <div style={{ width: 50 }}> </div>
                    <div>
                        <h5>Clone existing quiz</h5>
                        {QuizPicker.Picker(quizes, "", (qId) => setCloneId(qId))}
                        {cloneId > 0 ?
                            <button onClick={() => cloneQuiz()} className="btn btn-primary"> CLONE </button>
                        :
                            <span></span>
                        }
                    </div>
                </div>
                <br /> <hr/> <br />
                <h5>Questions:</h5>
                {questions?.map((question, index) => renderQuiz(question, index))}
            </form>
            <button onClick={addQuestion} className="btn btn-primary" type="button">+</button> 
            <button onClick={removeQuestion} className="btn btn-minus_quiz" type="button"><Icon path={mdiTrashCan} size={1}/></button>        
            <br />
            <button onClick={_confirm} className="btn btn-primary btn-right btn-corner">SAVE QUIZ</button>
            <Popup className="popup-overlay" trigger = {<button className="btn btn-delete btn-right btn-corner">DELETE</button>} modal nested>
              {close => (
                <div>
                  <p className="txt-popup">Are you sure you want to delete this quiz?</p>
                  <div className="div-center">
                    <button className="btn btn-primary btn-yes btn-popup" onClick={()=>deleteQuiz()}>YES</button>
                    <button className="btn btn-primary btn-popup"onClick={() => {close();}}>NO</button>
                  </div>
                  </div>
              )}
            </Popup>
            <button onClick={() => history.push("/CreateEvent/"+ props.match.params.event_id)} className="btn btn-secondary">Cancel</button>
            </div>
        </AuthenticatedTemplate>
    );
}