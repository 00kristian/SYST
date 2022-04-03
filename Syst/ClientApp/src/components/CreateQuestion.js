import React, { Component } from 'react';


export class CreateQuestion extends Component {
    static displayName = CreateQuestion.name;

    constructor(props) {
        super(props);
        this.state = { Name: "", Representation: "", Answer: ""};
    }

    render() {
        return (
            <div>
                <h2>Here you can create a question </h2>
                <br/>
                <form>
                <label>
                    <h5>Representation</h5>
                    <input className="input-field" onChange={(event) => this.state.Representation = event.target.value}></input>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5 id='a'>Q1: Answer A</h5>
                        <input className="input-field" onChange={(event) => this.state.Representation = event.target.value}></input>
                    </label>
                    <label>
                    <h5 id='b'>Q1: Answer B</h5>
                    <input className="input-field" onChange={(event) => this.state.Representation = event.target.value}></input>
                    </label>
                    <label>
                    <h5 id='c'>Q1: Answer C</h5>
                    <input className="input-field" onChange={(event) => this.state.Representation = event.target.value}></input>
                    </label>
                    <label>
                    <h5 id='d'>Q1: Answer D</h5>
                    <input className="input-field" onChange={(event) => this.state.Representation = event.target.value}></input>
                    </label>
                </form>
                <br />
                <button className="btn btn-primary leftbtn" onClick={this.rerouteToEvents}>Confirm </button>
                <button className="btn btn-primary rightbtn" onClick={this.rerouteToEvents}>Cancel</button>
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
