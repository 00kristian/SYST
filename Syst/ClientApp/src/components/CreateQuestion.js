import React, { useEffect, useState } from "react";
import { ImageUpload } from './ImageUpload';
import { Container, Row, Col } from 'react-grid';
import { useHistory } from "react-router-dom";
import Icon from "@mdi/react";
import { mdiTrashCan } from '@mdi/js';

export default CreateQuestion

function CreateQuestion(props) {
    const [options, setOptions] = useState([]);
    const [representation, setRepresentation] = useState("");
    const [answer, setAnswer] = useState("");
    const [imageURl, setImageURl] = useState("");
    const history = useHistory();

    useEffect(async () => {
        const response = await fetch('api/questions/' + props.match.params.id);
        const data = await response.json();
        
        setRepresentation(data.representation);
        setAnswer(data.answer);
        setOptions(data.options);
        setImageURl(data.imageURl);
    }, []);

    const indexToLetter = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Æ", "Ø", "Å"];

    function renderOption(option, index) {
        return (
            <div key={index}>
                <label>
                    <div className="div-option">
                    <h5>Option {indexToLetter[index]}</h5>
                        <div className="div-correct_answer"> 
                            <label className="label-correct_answer">Correct answer?</label>
                            <input checked={answer === option} type = "radio" name="correctAnswer" 
                                onClick={(event) => setAnswer(option)}/>
                        </div>
                    </div>
                    <input value={options[index]} className= "input-layout" onChange={(event) => {
                            let newOps = [...options];
                            newOps[index] = event.target.value;
                            setOptions(newOps);
                        }}>
                    </input>
                </label>
            </div>
        )
    }

    function addOptionFields() {
        setOptions([...options, ""]);
    }

    function removeOptionFields() {
        let temp = options;
        if (temp.length === 0) return;
        temp.pop();
        setOptions(temp);
    }

    async function confirm() {
        let question = {
            "representation": representation,
            "answer": answer,
            "Options": options,
            "imageURl": "https://techcrunch.com/wp-content/uploads/2015/04/codecode.jpg"
        };
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(question)
        };
        await fetch('api/questions/' + props.match.params.id, requestOptions);
        history.push("/CreateQuiz/" + props.match.params.event_id + "/" + props.match.params.quiz_id);
    }

    return (
        <div className="page-padding">
            <Container>
                <Row>
                    <Col>
                        <h2>Here you can create or edit a question </h2>
                        <br/>
                        <label>
                            <h5>Question Text</h5>
                            <input value={representation} className="input-layout input-question representation-text" onChange={(event) => setRepresentation(event.target.value)} />
                        </label>
                        <br />
                        <hr/>
                        <div> {options?.map((option, index) => renderOption(option, index))} </div>
                        <button className="btn btn-primary" type="button" onClick={() => addOptionFields()}>+</button>
                        <button className="btn btn-minus_question" type="button" onClick={() => removeOptionFields()}><Icon path={mdiTrashCan} size={1}/></button>
                    </Col>
                    <Col>
                        <h5 className="obj-top_padding">Select an image for the question</h5>
                        <br/>
                        {ImageUpload.Uploader(props.match.params.id, imageURl)}
                    </Col>
                </Row>
            </Container>
            <br /> <br />
            <button onClick={() => history.push("/CreateQuiz/" + props.match.params.event_id + "/" + props.match.params.quiz_id)} className="btn btn-secondary">Cancel</button>
            <button className="btn btn-primary btn-right" onClick={() => confirm()}>Save question</button>
        </div>
    );
}