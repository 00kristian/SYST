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
            <Col> 
                <div className="div-flex">           
                    <button onClick={answerFun} className="btn-answer"> {letters[i++]} </button>
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
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : 
       (<Container>         
            {this.state.currentQuestion >= this.state.quiz.questions.length ?
            <CandidateInformation/>
            :
            CandidateQuiz.renderCandidateQuestion(this.state.quiz.questions[this.state.currentQuestion], this.next)
            }
            <Row>
                {Pager.Pager(this.state.currentQuestion, this.state.quiz.questions, ((at) => this.setState({currentQuestion: at})))}
            </Row>
        </Container>);

    return (
      <div>
        <img style={{width: 600}} className='img-center' src={logo} alt="logo" />
        {contents}
      </div>
    );
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
    const response = await fetch('api/quiz/' + 1);
    const data = await response.json();
    this.setState({ quiz: data, loading: false});
  }
}