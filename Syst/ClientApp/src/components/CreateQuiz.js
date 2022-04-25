import React, { useEffect, useState } from "react";
import { useHistory } from "react-router-dom";

export default CreateQuiz

function CreateQuiz(props) {

    const [name, setName] = useState("");
    const [questions, setQuestions] = useState([]);
    const history = useHistory();

    useEffect(async () => {
        const response = await fetch('api/quiz/' + props.match.params.id);
        const data = await response.json();
        
        setName(data.name);
        setQuestions(data.questions);
    }, []);

    const updateQuiz = async () => {
        let quiz = {
            "name": name
        };
        const requestOptions = {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(quiz)
        };
        await fetch('api/quiz/' + props.match.params.id, requestOptions);
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
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(question)
        };
        let index = await fetch('api/QuizQuestion/' + props.match.params.id, requestOptions)
        .then(response => response.json());
        
        setQuestions([...questions, {representation : question.representation, id : index}])
    }

    const removeQuestion = async () => {
        if (questions.length === 0) return;
        await fetch('api/Questions/' + questions[questions.length - 1].id, {method: 'DELETE'});

        setQuestions(questions.slice(0, questions.length - 1));
    }

    const modifyQuestion = (id) => {
        updateQuiz();
        history.push("/CreateQuestion/" + props.match.params.event_id + "/" + props.match.params.id + "/" + id);
    }

    const deleteQuiz = async () => {
        let fr = window.confirm('Are you sure you want to delete the quiz: ' + name + ', permanently?');
        if (fr) {
            const requestOptions = {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(props.match.params.id)
            };
            await fetch('api/quiz'+"/"+props.match.params.id, requestOptions);
            history.push("/CreateEvent/"+ props.match.params.event_id);
        }
    }

    function renderQuiz(question, index) {
        return (
            <div key={index}>
                <h5> Question {index + 1}
                    <button onClick={(event) => modifyQuestion(question.id)} className="btn btn-primary btn-modify"> Modify </button>
                </h5>
                <label className="obj-bottom_margin">{question.representation}</label>
            </div>  
        );  
    }

    return (
        <div>
            <h2>Here you can create or edit a quiz</h2>
            <br/>
            <form>
                <label>
                    <h5>Quiz name</h5>
                    <input value={name} className="input-layout" onChange={(event) => setName(event.target.value)}>
                    </input>
                </label>
                <br />
                <hr/>
                <br />
                <h5>Questions:</h5>
                {questions?.map((question, index) => renderQuiz(question, index))}
            </form>
            <button onClick={addQuestion} className="btn btn-primary" type="button">+</button> 
            <button onClick={removeQuestion} className="btn btn-minus_quiz" type="button">-</button>        
            <br />
            <button onClick={_confirm} className="btn btn-primary btn-right">Confirm</button>
            <button onClick={deleteQuiz} className="btn btn-primary btn-right">DELETE</button>
            <button onClick={() => history.push("/CreateEvent/"+ props.match.params.event_id)} className="btn btn-primary">Cancel</button>
        </div>
    );
}