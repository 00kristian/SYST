import React, { Component } from 'react';
import Dropdown from 'react-dropdown';
import 'react-dropdown/style.css'


export class CreateQuestion extends Component {
    static displayName = CreateQuestion.name;

    constructor(props) {
        super(props);
        this.state = {
            Representation: "",
            Answer: "",
            inputValues: [{OptionName: ""}],
            ImageUrl: ""
        };
    }

    addOptionFields() {
        this.setState(({
            inputValues: [...this.state.inputValues, {OptionName:""}]
        }))
    }

    removeOptionFields(i) {
        let inputValues = this.state.inputValues;
        inputValues.splice(i, 1);
        this.setState({ inputValues });
    }

    render() {

        const letters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Æ", "Ø", "Å"];

        return (
            <div class="CreateQPage">
                <h2>Here you can create a question </h2>
                <br/>
                <form>
                    <label>
                        <h5>Question</h5>
                        <input className="input-field q-field" onChange={(event) => this.state.Representation = event.target.value} />
                    </label>
                    
                    <br />
                    <br />
                    {this.state.inputValues.map((answer, index) =>
                        <div key={index}>
                            <label>
                                <div className="flex-container">
                                    <h5 className="flex-child">Option {letters[index]}</h5>
                                    <div className="flex-child correctAns">
                                        <label className="cText">Correct answer?</label>
                                        <input type = "radio" name="correctAnswer" onChange={this.state.answer = letters[index]}/>
                                    </div>
                                </div>
                            <input className= "input-field" onChange={(event) => this.state.inputValues.OptionName = event.target.value } />
                                <button className="btn btn-primary" type="button" onClick={() => this.removeOptionFields()}>-</button>

                            </label>
                        </div>
                        )}
                        <button className="btn btn-primary" type="button" onClick={() => this.removeOptionFields()}>-</button>
                        <br />
                        <button className="btn btn-plus" type="button" onClick={() => this.addOptionFields()}>+</button>
                    <br />
                    <br />
                    <h5>Select an image for the question</h5>
                    <button className="btn btn-primary leftbtn" >Upload Image</button>
                </form>
                <br />
                <br />
                <br />
                <button className="btn btn-primary leftbtn" onClick={this.rerouteToQuiz}>Cancel</button>
                <button className="btn btn-primary rightbtn" onClick={this.rerouteToCreateEvent}>Create question</button>
            </div>
        );
    }

    rerouteToCreateEvent = () => {
        let event = {
            "representation": this.state.inputValues.Representation,
            "answer": this.state.inputValues.Answer,
            "Options": this.state.inputValues.Options,
            "imageUrl": this.state.inputValues.imageUrl
        };
        console.log(event);
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(event)
        };
        fetch('api/questions', requestOptions)
            .then(response => response.json())
        const { history } = this.props;
        history.push("/CreateQuiz/"+this.props.match.params.quiz_id+ "/"+ this.props.match.params.quiz_id);
    }
   

    rerouteToQuiz = () => {
        const { history } = this.props;
        history.push("/CreateQuiz/" + this.props.match.params.event_id + "/" + this.props.match.params.quiz_id);
    }
}
