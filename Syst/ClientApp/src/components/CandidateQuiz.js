import React, { Component } from 'react';

export class CandidateQuiz extends Component {
  static displayName = CandidateQuiz.name;

  constructor(props) {
    super(props);
    this.state = { quiz: Object, loading: true };
  }

  componentDidMount() {
    this.populateData();
  }

  static renderCandidateQuiz(quiz) {
    return (
        <div>
            <h1>Can you solve this quiz?</h1>
            <a href={'/CandidateInformation'}><button className="btn btn-primary rightbtn">NEXT</button></a>
        </div>
        
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : CandidateQuiz.renderCandidateQuiz(this.state.quiz);

    return (
      <div>
        {contents}
      </div>
    );
  }

  async populateData() {
    const response = await fetch('api/events/' + this.props.match.params.id);
    const data = await response.json();
    this.setState({ event: data, loading: false });
  }
}
