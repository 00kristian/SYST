import React, { Component } from 'react';
import Dropdown from 'react-dropdown';


export class CreateQuestion extends Component {
    static displayName = CreateQuestion.name;

    constructor(props) {
        super(props);
        this.state = {
            inputValues: [{Representation: "", Answer: "", Options: "", imageUrl: ""}]
        };
    }

    addOptionFields() {
        this.setState(({
            inputValues: [...this.state.inputValues, {Answer: "", Options: ""}]
        }))
    }

    removeOptionFields(i) {
        let inputValues = this.state.inputValues;
        inputValues.splice(i, 1);
        this.setState({ inputValues });
    }

    render() {

        const options = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Æ", "Ø", "Å"];
        const defaultOption = options[0];

        return (
            <div class="CreateQPage">
                <h2>Here you can create a question </h2>
                <br/>
                <form>
                    <label>
                        <h5>Question</h5>
                        <input className="input-field" onChange={(event) => this.state.Representation = event.target.value} />
                    </label>
                    
                    <br />
                    <br />
                    {this.state.inputValues.map((answer, index) =>
                        <div key={index}>
                            <label>
                                <h5>Option {options[index]}</h5>
                                <label>Correct answer?</label>
                                <input type = "radio" name="correctAnswer" onClick={(event) => this.state.answer = event.target.value}/>
                            <input className= "input-field" onChange={(event) => this.state.Options[index] = event.target.value} />
                            </label>
                        </div>
                        )}
                        <button className="btn btn-primary" type="button" onClick={() => this.removeOptionFields()}>-</button>
                        <button className="btn btn-primary" type="button" onClick={() => this.addOptionFields()}>+</button>
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
            "representation": this.state.Representation,
            "answer": this.state.Answer,
            "Options": this.state.Options,
            "imageUrl": this.state.imageUrl
        };
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(event)
        };
        fetch('api/questions', requestOptions)
        .then(response => response.json())
        const { history } = this.props;
        history.push("/CreateQuiz");
    }
   

    rerouteToQuiz = () => {
        const { history } = this.props;
        history.push("/CreateQuiz");
    }
}
