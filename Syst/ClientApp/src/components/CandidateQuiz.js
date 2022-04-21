import React, { Component } from 'react';
import { Container, Row, Col } from 'react-grid';
import logo from './Systematic_Logo.png';

export class CandidateQuiz extends Component {
  static displayName = CandidateQuiz.name;

  constructor(props) {
    super(props);
    this.state = { quiz: Object, loading: true };
  }

  componentDidMount() {
    this.populateData();
  }

  centeredText = {
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        margin: 15
    };

  static renderCandidateQuestion(question) {
    const path = window.location.href.replace(window.location.pathname, "");
    const letters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Æ", "Ø", "Å"];

    let ops = [];
    let i = 0;
    question.options.forEach(op => {
        ops.push(
            <Col> 
                <div className="div-flex">           
                    <button className="btn-answer"> {letters[i++]} </button>
                    <div className='div-quiz_layout'>
                        <h5 className='question-text'> {op}</h5>
                    </div>
                </div>
            </Col>
        );
    });

    return (
        <div>
            <Container>
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
            </Container>
      </div>

    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : CandidateQuiz.renderCandidateQuestion(this.state.quiz.questions[0]);

    return (
      <div>
        <img style={{width: 600}} className='img-center' src={logo} alt="logo" />
        <h1 style={this.centeredText}>Can you solve this quiz?</h1>
        {contents}
        <div className='btn-next'>
        <a href={'/CandidateInformation'}><button className="btn btn-primary btn-right">NEXT</button></a>
        </div>
      </div>
    );
  }

  async populateData() {
    const response = await fetch('api/quiz/' + 1);
    const data = await response.json();
    this.setState({ quiz: data, loading: false });
  }
}