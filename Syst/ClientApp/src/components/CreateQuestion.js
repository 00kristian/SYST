import React, { Component } from 'react';
import Dropdown from 'react-dropdown';
import 'react-dropdown/style.css'


export class CreateQuestion extends Component {
    static displayName = CreateQuestion.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            Question: {
                Representation: "",
                Answer: "",
                Options: [],
                imageUrl: ""
            },
            Options: []
        };
    }

    componentDidMount() {  
        this.populateData();
    }

    addOptionFields() {
        this.setState(({
            Options: [...this.state.Options, ""]
        }))
    }

    removeOptionFields(i) {
        let inputValues = this.state.Options;
        inputValues.splice(i, 1);
        this.setState({ Options });
    }

    static renderQuestion (ques, ops) {
        const options = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Æ", "Ø", "Å"];
        const defaultOption = options[0];

        return (
            <form>
                <label>
                    <h5>Question</h5>
                    <input placeholder={ques.Representation} className="input-field" onChange={(event) => ques.Representation = event.target.value} />
                </label>
                
                <br />
                <br />
                {ops.map((answer, index) =>
                    <div key={index}>
                        <label>
                            <h5>Option {options[index]}</h5>
                            <label>Correct answer?</label>
                            <input type = "radio" name="correctAnswer" onClick={(event) => ques.Answer = ops[index]}/>
                            <input placeholder={ops[index]} className= "input-field" onChange={(event) => ops[index] = event.target.value} />
                        </label>
                    </div>
                    )
                }
            </form>
        );
    }

    render() {
        let contents = this.state.loading
        ? <p><em>Loading...</em></p>
        : CreateQuestion.renderQuestion(this.state.Question, this.state.Options);

        return (
            <div className="CreateQPage">
                <h2>Here you can create a question </h2>
                <br/>
                {contents}
                <button className="btn btn-primary" type="button" onClick={() => this.removeOptionFields()}>-</button>
                    <button className="btn btn-primary" type="button" onClick={() => this.addOptionFields()}>+</button>
                <br />
                <h5>Select an image for the question</h5>
                <button className="btn btn-primary leftbtn" >Upload Image</button>
                <br />
                <br />
                <br />
                <button className="btn btn-primary leftbtn" onClick={this.rerouteToQuiz}>Cancel</button>
                <button className="btn btn-primary rightbtn" onClick={this.confirm}>Confirm</button>
            </div>
        );
    }

    confirm = () => {
        let question = {
            "representation": this.state.Question.Representation,
            "answer": this.state.Question.Answer,
            "Options": this.state.Options,
            "imageURL": "https://techcrunch.com/wp-content/uploads/2015/04/codecode.jpg"
        };
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(question)
        };
        fetch('api/questions/' + this.props.match.params.id, requestOptions);
        const { history } = this.props;
        history.push("/CreateQuiz/"+this.props.match.params.event_id+ "/"+ this.props.match.params.quiz_id);
    }
   

    rerouteToQuiz = () => {
        const { history } = this.props;
        history.push("/CreateQuiz/" + this.props.match.params.event_id + "/" + this.props.match.params.quiz_id);
    }

    async populateData() {
        const response = await fetch('api/questions/' + this.props.match.params.id);
        const data = await response.json();

        //TODO: load the correct answer radio button

        for (const q in data.options) {
            let x = data.options[q];
            this.setState(({
                Options: [...this.state.Options, x]
            }))
        }
        this.setState({ Question: {
            Representation: data.representation,
            Answer: data.answer,
            Options: data.options,
            imageUrl: data.imageUrl
        }, loading: false });
    }
}
