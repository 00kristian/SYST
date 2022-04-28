import React, { Component } from 'react';
import { Container, Row, Col } from 'react-grid';
import { Pager } from './Pager';
import { CandidateInformation } from './CandidateInformation';
import logo from './Systematic_Logo.png';

export class CandidateQuiz extends Component {
  static displayName = CandidateQuiz.name;

  constructor(props) {
    super(props);
    this.state = { quiz: Object, loading: true, currentQuestion: 0};
  }

  EVENTID = this.props.match.params.event_id;
  QUIZID = this.props.match.params.quiz_id;

  componentDidMount() {
    this.populateData();
  }

  static renderCandidateQuestion(question, answerFun) {
    const path = window.location.href.replace(window.location.pathname, "");
    const letters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Æ", "Ø", "Å"];

    let ops = [];
    let i = 0;
    question.options.forEach(op => {
        ops.push(
            <Col key={i}> 
                <div className="div-flex">           
                    <button onClick={() => answerFun(op)} className="btn-answer"> {letters[i++]} </button>
                    <div className='div-quiz_layout'>
                        <h5 className='question-text'> {op}</h5>
                    </div>
                </div>
            </Col>
        );
    });

    return (
        <div>
            <h3 style={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                margin: 15
            }}>{question.representation}</h3>
            <Row>
                <Col>
                    <div className='question-options'>
                        {ops}
                    </div>
                </Col>
                <Col>
                    <img style={{width: 400}} src={path + "/api/Image/" + question.imageURl} alt="quizPic" />
                </Col>        
            </Row>
      </div>

    );
  }

  render() {
    let contents = this.QUIZID > 0 ? this.state.loading
        ? <p><em>Loading...</em></p>
        : 
        (<Container>         
              {this.state.currentQuestion >= this.state.quiz.questions.length ?
              <CandidateInformation Answers={this.state.answers} QuizId={this.QUIZID} EventId={this.EVENTID}/>
              :
              CandidateQuiz.renderCandidateQuestion(this.state.quiz.questions[this.state.currentQuestion], this.answer)
              }
              {Pager.Pager(this.state.currentQuestion, this.state.quiz.questions, ((at) => this.setState({currentQuestion: at})))}  
          </Container>)
        : <h2 className="txt-center"> No quiz attached to this Event! </h2>;


    return (
      <div className='div-center'>
        <img style={{width: 600}} src={logo} alt="logo" />
        {contents}
      </div>
    );
  }

  answer = async (option) => {
    this.state.answers[this.state.currentQuestion] = option;
    this.next();
  }

  next = async () => {
    let cq = this.state.currentQuestion;
    const len = this.state.quiz.questions.length;
    cq++;
    if (cq > len) {
        //this.finish();
    } else {
        this.setState({currentQuestion : cq})
    }
  }

  finish = async () => {
    // const { history } = this.props;
    // history.push("/CandidateInformation");
  }

  async populateData() {
    const response = await fetch('api/quiz/' + this.QUIZID);
    const data = await response.json();
    let arLength = 0; 
    if (data.questions != null) {
      arLength = data.questions.length;
    }
    this.setState({ quiz: data, loading: false, answers: Array(arLength)});
  }
}