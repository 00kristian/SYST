import React, { Component } from 'react';
import Dropdown from 'react-dropdown';


export class CreateQuestion extends Component {
    static displayName = CreateQuestion.name;

    constructor(props) {
        super(props);
        this.state = {  Representation: "", Answer: "", Options: ["", "", "", ""], imageUrl: ""};
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : CreateQuestion.createAnswers(options);
        
        const options = ["A", "B", "C", "D", "E", "F", "G"];
        const defaultOption = options[0];

        return (
            <div class="CreateQPage">
                <h2>Here you can create a question </h2>
                <br/>
                <form>
                    <label>
                        <h5>Question</h5>
                        <input className="input-field" onChange={(event) => this.state.Representation = event.target.value}></input>
                    </label>
                    
                    <br />
                    {contents}
                    <br />
                    <br />

                    <button className="btn btn-primary leftbtn" >Upload Image</button>
                    
                </form>
                <br />
                <br />
                <br />
                <h5>Correct Answer</h5>
                <Dropdown options={options} onChange={this.selectAnswer} value={defaultOption} placeholder="Choose Answer" />


                <button className="btn btn-primary leftbtn" onClick={this.rerouteToCreateEvent}>Confirm </button>
                <button className="btn btn-primary rightbtn" onClick={this.rerouteToQuiz}>Cancel</button>
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

    selectAnswer = (option) => {
        this.setState({Answer: option.value});        
    }
    
    static createAnswers(options) {
            const answerNumber = "Answer " + options[0]
            document.getElementById("answerNumber").innerHTML = "Answer " + options[0];
            return (
                 <form>                
                 <label>
                    <h5 id='a'>document.</h5>
                     <input className="input-field" onChange={(event) => this.state.Options[0] = event.target.value}></input>
                 </label>
                 <br />
                 <button onClick={this.createAnswers}>+</button>
                 </form>
            )
    }
    
    addAnswer() => {
        return (
            
        )
        
    }
    
}
