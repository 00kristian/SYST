import React, { useEffect, useState } from "react";
import { ImageUpload } from './ImageUpload';
import { Container, Row, Col } from 'react-grid';
import { useHistory } from "react-router-dom";
import Icon from "@mdi/react";
import { mdiTrashCan } from '@mdi/js';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { FetchOptions } from "./FetchOptions";


export default CreateQuestion

function CreateQuestion(props) {
    const [theseOptions, setOptions] = useState([]);
    const [representation, setRepresentation] = useState("");
    const [answer, setAnswer] = useState("");
    const [imageURl, setImageURl] = useState("");
    const { instance, accounts } = useMsal();
    const history = useHistory();

    useEffect(async () => {
        const options = await FetchOptions.Options(instance, accounts, "GET");
        const data = await fetch('api/questions/'+props.match.params.id, options)
        .then(response => response.json())
        .catch(error => console.log(error));

        
        setRepresentation(data.representation);
        setAnswer(data.answer);
        setOptions(data.options);
        setImageURl(data.imageURl);
    }, []);

    const indexToLetter = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Æ", "Ø", "Å"];

    function renderOption(option, index) {
        return (
            <AuthenticatedTemplate>
            <div key={index}>
                <label>
                    <div className="div-option">
                    <h5>Option {indexToLetter[index]}</h5>
                        <div className="div-correct_answer"> 
                            <label className="label-correct_answer txt-small">Correct answer?</label>
                            <input className="txt-small" checked={answer === option} type = "radio" name="correctAnswer" 
                                onClick={(event) => setAnswer(option)}/>
                        </div>
                    </div>
                    <input value={theseOptions[index]} className= "input-layout txt-small" onChange={(event) => {
                            let newOps = [...theseOptions];
                            newOps[index] = event.target.value;
                            setOptions(newOps);
                        }}>
                    </input>
                </label>
                </div>
            </AuthenticatedTemplate>
        )
    }

    function removeOptionFields() {
        let temp = theseOptions;
        if (temp.length === 0) return;
        temp.pop();
        setOptions(temp);
        
    }

    function addOptionFields() {
        setOptions([...theseOptions, ""]);
    }

  

    async function confirm() {
        let question = {
            "representation": representation,
            "answer": answer,
            "Options": theseOptions,
            "imageURl": imageURl
        };
        const options = await FetchOptions.Options(instance, accounts, "PUT")
        options.headers ={
            ...options.headers,
            "Content-Type": "application/json"
        }
        options.body = JSON.stringify(question);
        
        await fetch('api/questions/' + props.match.params.id, options);
        history.push("/CreateQuiz/" + props.match.params.event_id + "/" + props.match.params.quiz_id);
    }

    return (
        <AuthenticatedTemplate>
        <div className="page-padding">
            <Container>
                <Row>
                    <Col>
                        <h2>Here you can create or edit a question </h2>
                        <br/>
                        <label>
                            <h5>Question Text</h5>
                            <input value={representation} className="input-layout input-question representation-text txt-small" onChange={(event) => setRepresentation(event.target.value)} />
                        </label>
                        <br />
                        <hr/>
                        <div> {theseOptions?.map((option, index) => renderOption(option, index))} </div>
                        <button className="btn btn-primary" type="button" onClick={() => addOptionFields()}>+</button>
                            <button className="btn btn-minus_question" type="button" onClick={() => removeOptionFields()}><Icon path={mdiTrashCan} size={1}/></button>
                    </Col>
                    <Col>
                        <h5 className="obj-top_padding">Select an image for the question</h5>
                        <br/>
                        <ImageUpload QuestionId={props.match.params.id} Currentimg={imageURl}></ImageUpload>
                    </Col>
                </Row>
            </Container>
            <br /> <br />
            <button onClick={() => history.push("/CreateQuiz/" + props.match.params.event_id + "/" + props.match.params.quiz_id)} className="btn btn-secondary">Cancel</button>
            <button className="btn btn-primary btn-right" onClick={() => confirm()}>SAVE QUESTION</button>
        </div>
        </AuthenticatedTemplate>       
    );
}