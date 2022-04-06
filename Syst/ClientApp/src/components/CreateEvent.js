import React, { Component } from 'react';

import {DatePicker} from './DatePicker';

import "react-datepicker/dist/react-datepicker.css";
import { QuizPicker } from './QuizPicker';

export class CreateEvent extends Component {
    static displayName = CreateEvent.name;

    constructor(props) {
        super(props);
        this.state = { event : {name: "", date: new Date(), location: "", quiz: Object}, loading: true, quizes: [], quizid: 0};
    }

    componentDidMount() {  
        this.populateData();
    }

    static renderEvent(e, quizes, quizFun) {
        return (
            <div>
                <form>
                    <label>
                        <h5>Name</h5>
                        <input placeholder={e.name} className="input-field" onChange={(event) => e.name = event.target.value}></input>
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5>Date</h5>
                        {DatePicker.Picker(e.date.toISOString().split('T')[0], ((date) => e.date = new Date(date)))}
                    </label>
                    <br />
                    <br />
                    <label>
                        <h5>Location</h5>
                        <input placeholder={e.location} className="input-field" onChange={(event) => e.location = event.target.value}></input>
                    </label>
                </form>

                <br />
                <h5>Quiz</h5>
                {QuizPicker.Picker(quizes, e.quiz.id, (qid) => quizFun(qid))}
            </div>

        );
    }

    render() {
        let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : CreateEvent.renderEvent(this.state.event, this.state.quizes, (qid) => this.setState({quizid: qid}));
        return (
            <div>
                <h2>Here you can create an event</h2>
                <br/>
                {contents}
                {(this.state.event.quiz.id > 0) ? (
                    <div>
                        <button className="btn btn-primary" onClick={this.editQuiz}>Edit quiz</button>
                    </div>
                ) : (
                    <div> </div>
                )}
                <button className="btn btn-primary" onClick={this.rerouteToCreateQuiz}>Create quiz</button>
                <br />
                <br />
                <button className="btn btn-primary rightbtn" onClick={this.rerouteToConfirmation}>Save event</button>
                <br />
                <br />
                <button className="btn btn-primary rightbtn" onClick={this.deleteEvent}>Cancel</button>
            </div>
        );
    }

    rerouteToConfirmation = () => {
        this.updateEvent();
        if (this.state.quizid > 0) {
            this.updateQuizId(this.state.quizid);
        }
        const { history } = this.props;
        history.push("/eventdetail/" + this.props.match.params.id);
    }

    updateEvent = () => {
        let event = {
            "name": this.state.event.name,
            "date": this.state.event.date.toISOString(),
            "location": this.state.event.location
        };
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(event)
        };
        fetch('api/events/' + this.props.match.params.id, requestOptions);
    }

    rerouteToEvents = () => {
        const { history } = this.props;
        history.push("/Events");
    }

    editQuiz = async () => {
        const quizid = this.state.quizid;
        this.updateEvent();
        await this.updateQuizId(quizid);
        const { history } = this.props;
        history.push("/CreateQuiz/" + this.props.match.params.id +"/"+ quizid);
    }

    rerouteToCreateQuiz = async () => {
        let quiz = {
            "name": "New quiz"
        };
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(quiz)
        };
        let quizid = await fetch('api/quiz', requestOptions)
        .then(response => response.json())

        this.updateEvent();

        this.updateQuizId(quizid);

        const { history } = this.props;
        history.push("/CreateQuiz/" + this.props.match.params.id +"/"+ quizid);
    }

    async updateQuizId(id) {
        const requestOptions = {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        };

        fetch('api/events/' + this.props.match.params.id + '/' + id, requestOptions);
    }

    async populateData() {
        const response = await fetch('api/events/' + this.props.match.params.id);
        const data = await response.json();
        const response2 = await fetch('api/quiz');
        const data2 = await response2.json();
        this.setState({  event : {name: data.name, date: new Date(data.date.split('T')[0]), location: data.location, quiz: data.quiz}, loading: false, quizes: data2, quizid: data.quiz.id});
    }


    deleteEvent = async () => {
        
        const requestOptions = {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(this.props.match.params.id)
        };
        await fetch('api/events'+"/"+this.props.match.params.id, requestOptions);

        const { history } = this.props;
        history.push("/events");
    }



}
