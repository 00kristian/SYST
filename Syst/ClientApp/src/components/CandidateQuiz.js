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
                            <br/>
                            <br/>
                            <Row>
                                <h3>This is a placeholder for the quiz representation</h3>
                            </Row>
                            <br/>
                            <Row className="ansRow">
                                <Col> 
                                    <div className="butNText">           
                                        <button className="answerButton"> A </button>
                                        <div className='vertical-center'>
                                            <h5>Option A</h5>
                                        </div>
                                    </div>
                                </Col>
                                <Col> 
                                    <div className="butNText">           
                                        <button className="answerButton"> B </button>
                                        <div className='vertical-center'>
                                            <h5>Option B</h5>
                                        </div>
                                    </div>
                                </Col>
                            </Row>
                            <br/>
                            <br/>
                            <br/>
                            <Row className="ansRow">
                                <Col> 
                                    <div className="butNText">           
                                        <button className="answerButton"> C </button>
                                        <div className='vertical-center'>
                                            <h5>Option C</h5>
                                        </div>
                                    </div>
                                </Col>
                                <Col> 
                                    <div className="butNText">           
                                        <button className="answerButton"> D </button>
                                        <div className='vertical-center'>
                                            <h5>Option D</h5>
                                        </div>
                                    </div>
                                </Col>
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
