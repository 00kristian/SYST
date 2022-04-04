import React, { Component } from 'react';


export class CreateQuiz extends Component {
    static displayName = CreateQuiz.name;

    constructor(props) {
        super(props);
        this.state = { Quiz: Object, loading: true};
    }

    componentDidMount() {
        this.populateData();
      }
    

   static renderQuiz(Quiz) { 
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
                    
                </form>
            </div>

       )

   }
    render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : CreateQuiz.renderQuiz(this.state.Quiz);
        return (
            <div>
                {contents}
                {this.newQuestion()}              
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

    newQuestion = () => {
        const id = 1;
        return (
            <div>
                <br />
                <h5>Question {id}</h5>
                <a href={"/CreateQuestion/"+this.props.match.params.event_id+"/" + this.props.match.params.id + "/" + id}><button className="btn btn-primary rightbtn">Create Question</button> </a>
                <br />    
            </div>
        )
    }


    rerouteToConfirmation = () => {
        let event = {
            "name": this.state.Quiz.name
        };
        const requestOptions = {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(event)
        };
        console.log(event);
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
        console.log(data);
        this.setState({ Quiz: data, loading: false });
    }

}
