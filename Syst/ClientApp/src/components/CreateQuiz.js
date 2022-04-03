import React, { Component } from 'react';


export class CreateQuiz extends Component {
    static displayName = CreateQuiz.name;

    constructor(props) {
        super(props);
        this.state = { Name: "", Representation: "", Answer: ""};
    }

    render() {
        return (
            <div>
                <h2>Here you can create a quiz</h2>
                <br/>
                <form>
                    <label>
                        <h5>Name</h5>
                        <input className="input-field" onChange={(event) => this.state.Name = event.target.value}></input>
                    </label>
                    <br />
                    <br />
                    
                </form>
                <br />
                <h5>Question 1</h5>
                <button className="btn btn-primary rightbtn" onClick={this.rerouteToQuestions}>Create Question</button>
                <br />                
                <br />
                <h5>Question 2</h5>
                <button className="btn btn-primary rightbtn" onClick={this.rerouteToQuestions}>Create Question</button>
                <br />
                <br />
                <h5>Question 3</h5>
                <button className="btn btn-primary rightbtn" onClick={this.rerouteToQuestions}>Create Question</button>
                <br />
                <br />
                <br />
                <br />
                <br />
                <button className="btn btn-primary" onClick={this.rerouteToQuestions}>Create quiz</button>

                <br />
                <button className="btn btn-primary rightbtn " onClick={this.rerouteToEvents}>Cancel</button>
            </div>
        );
    }

    rerouteToConfirmation = () => {
        let event = {
            "name": this.state.Name,
            "answer": this.state.Answer
        };
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(event)
        };
        fetch('api/questions', requestOptions)
        .then(response => response.json())
        const { history } = this.props;
        history.push("/Confirmation");
    }
    rerouteToEvents = () => {
        const { history } = this.props;
        history.push("/Events");
    }

    rerouteToQuestions = () => {
        const { history } = this.props;
        history.push("/CreateQuestion");
    }

    
}
