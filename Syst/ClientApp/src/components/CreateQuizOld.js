import React, { Component } from 'react';


export class CreateQuizOld extends Component {
    static displayName = CreateQuizOld.name;

    constructor(props) {
        super(props);
        this.state = { Quiz: Object, loading: true,
            Questions: []};
    }

    async addQuestion() {
        let question = {
            "representation": "New Question",
            "answer": "",
            "Options": [],
            "imageUrl": ""
        };
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(question)
        };
        let index = await fetch('api/QuizQuestion/' + this.props.match.params.id, requestOptions)
        .then(response => response.json());

        this.setState(({
            Questions: [...this.state.Questions, {Representation : question.representation, Id : index}]
        }))
    }

    async removeQuestion() {
        let Questions = this.state.Questions;
        if (Questions.length === 0) return;
        await fetch('api/Questions/' + Questions[Questions.length - 1].Id, {method: 'DELETE'});

        Questions.pop();
        this.setState({ Questions : Questions});
    }

    componentDidMount() {  
        this.populateData();
    }
    

    static renderQuiz(Quiz, Questions, onCli) { 
        return (
            <div>
                <h2>Here you can create a quiz</h2>
                <br/>
                <form>
                    <label>
                        <h5>Quiz name</h5>
                        <input placeholder={Quiz.name} className="input-layout" onChange={(event) => Quiz.name = event.target.value}>
                            
                        </input>
                    </label>
                    <br />
                    <hr/>
                    <br />
                    <h5>Questions:</h5>
                    {Questions.map((answer, index) =>
                        <div key={index}>
                            <h5> Question {index + 1}
                                <button className="btn btn-primary btn-modify" onClick={() => onCli(Questions[index].Id)}> Modify </button>
                            </h5>
                            <label className="obj-bottom_margin">{Questions[index].Representation}</label>
                        </div>
                        )
                    }
                    
                </form>
            </div>
        )
    }

    render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : CreateQuizOld.renderQuiz(this.state.Quiz, this.state.Questions, this.rerouteToQuestions);
        return (
            <div>
                {contents}
                <button className="btn btn-primary" type="button" onClick={() => this.addQuestion()}>+</button> 
                <button className="btn btn-minus_quiz" type="button" onClick={() => this.removeQuestion()}>-</button>        
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <button className="btn btn-primary btn-right" onClick={this.rerouteToConfirmation}>Confirm</button>
                <button className="btn btn-primary" onClick={this.deleteQuiz}>Back</button>
            </div>
        );
    }

    updateQuiz = async () => {
        let quiz = {
            "name": this.state.Quiz.name
        };
        const requestOptions = {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(quiz)
        };
        await fetch('api/quiz/' + this.props.match.params.id, requestOptions);
    }

    rerouteToConfirmation = async () => {
        await this.updateQuiz();

        const { history } = this.props;
        history.push("/CreateEvent/"+ this.props.match.params.event_id);
    }
    rerouteToEvents = () => {

        const { history } = this.props;
        history.push("/Events");
    }

    rerouteToQuestions = (id) => {
        this.updateQuiz();

        const { history } = this.props;
        history.push("/CreateQuestion/" + this.props.match.params.event_id + "/" + this.props.match.params.id + "/" + id);
    }

    async populateData() {
        const response = await fetch('api/quiz/' + this.props.match.params.id);
        const data = await response.json();
        this.setState({ Quiz: data, loading: false });

        for (const q in data.questions) {
            let x = data.questions[q];
            this.setState(({
                Questions: [...this.state.Questions, {Representation : x.representation, Id : x.id}]
            }))
        }
    }

    deleteQuiz = async () => {
        
        const requestOptions = {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(this.props.match.params.id)
        };
        await fetch('api/quiz'+"/"+this.props.match.params.id, requestOptions);

        const { history } = this.props;
        history.push("/CreateEvent/"+ this.props.match.params.event_id);
    }

}
