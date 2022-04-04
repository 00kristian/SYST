import React, { Component } from 'react';


export class CreateQuiz extends Component {
    static displayName = CreateQuiz.name;

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

    async removeQuestion(i) {
        let Questions = this.state.Questions;
        await fetch('api/Questions/' + Questions[Questions.length - 1].Id, {method: 'DELETE'});

        console.log(Questions);
        Questions.pop();
        this.setState({ Questions : Questions});
    }

    componentDidMount() {
        
        this.populateData();
      }
    

    static renderQuiz(Quiz, Questions, eventId, quizId) { 
       return (
     <div>
                <h2>Here you can create a quiz</h2>
                <br/>
                <form>
                    <label>
                        <h5>Quiz name</h5>
                        <input placeholder={Quiz.name} className="input-field" onChange={(event) => Quiz.name = event.target.value}></input>
                    </label>
                    <br />
                    <br />
                    {Questions.map((answer, index) =>
                        <div key={index} className="flex">
                            <h5> <a href={"/CreateQuestion/"+eventId+"/" + quizId + "/" + Questions[index].Id}>Question {index + 1} </a></h5>
                            <label>{Questions[index].Representation}</label>    
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
      : CreateQuiz.renderQuiz(this.state.Quiz, this.state.Questions, this.props.match.params.event_id, this.props.match.params.id);
        return (
            <div>
                {contents}
                <button className="btn btn-primary" type="button" onClick={() => this.removeQuestion()}>-</button>
                <button className="btn btn-primary" type="button" onClick={() => this.addQuestion()}>+</button>         
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <button className="btn btn-primary rightbtn " onClick={this.rerouteToEvents}>Cancel</button>
                <button className="btn btn-primary rightbtn " onClick={this.rerouteToConfirmation}>Confirm</button>
            </div>
        );
    }

    rerouteToConfirmation = () => {
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
        console.log(quiz);
        fetch('api/quiz/' + this.props.match.params.id, requestOptions);

        const { history } = this.props;
        history.push("/CreateEvent/"+ this.props.match.params.event_id);
    }
    rerouteToEvents = () => {
        const { history } = this.props;
        history.push("/Events");
    }

    rerouteToQuestions = (id) => {
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

}
