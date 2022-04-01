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

  static renderCandidateQuiz(quiz) {
    return (
        <div>
            <Container>
                <Row>
                    <Col>
                        <Container>
                        <Row>
                            <h3>This is a placeholder for the quiz representation</h3>
                        </Row>
                        <Row>
                            <Col> 
                                <div>
                                    <button className="answerButton"> A </button>
                                    <h5>Option A</h5>
                                </div>
                            </Col>
                            <Col> <button className="answerButton"> B </button></Col>
                        </Row>
                        <Row>
                            <Col> <button className="answerButton"> C </button></Col>
                            <Col> <button className="answerButton"> D </button></Col>
                        </Row>
                        </Container>
                    </Col>
                    <Col>
                        {//placeholder image
                        }
                        <img style={{height: 600}} src="https://images-ext-1.discordapp.net/external/HCEa_uu9JSzZOqkiNqbczN8PzGe6dkusshmtRzfBrIE/https/backoffice.systematic.com/media/hunhjd0w/screeningsp%25C3%25B8rgsm%25C3%25A5l4.png" alt="quizPic" />
                    </Col>
                </Row>
            </Container>
      </div>
        
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : CandidateQuiz.renderCandidateQuiz(this.state.quiz);

    return (
      <div >
        <img style={{width: 600}} className='centeredImage' src={logo} alt="logo" />
        <h1 style={this.centeredText}>Can you solve this quiz?</h1>
        {contents}
        <a href={'/CandidateInformation'}><button className="btn btn-primary rightbtn">NEXT</button></a>
      </div>
    );
  }

  async populateData() {
    const response = await fetch('api/events/' + this.props.match.params.id);
    const data = await response.json();
    this.setState({ event: data, loading: false });
  }
}
