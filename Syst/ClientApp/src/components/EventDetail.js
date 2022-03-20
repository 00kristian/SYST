import React, { Component } from 'react';

export class EventDetail extends Component {
  static displayName = EventDetail.name;

  constructor(props) {
    super(props);
    this.state = { event: Object, loading: true };
  }

  componentDidMount() {
    this.populateData();
  }

  static renderEvent(event) {
    return (
        <div>
            <h1>{event.name}</h1>
            <h2>{event.date}</h2>
            <br/>
            <h3>Participants</h3>
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>University</th>
                    <th>Degree</th>
                </tr>
                </thead>
                <tbody>
                {event.candidates.map(candidate =>
                    <tr key={candidate.id}>
                        <td>{candidate.id}</td>
                        <td>{candidate.name}</td>
                        <td>{candidate.email}</td>
                        <td>{candidate.university}</td>
                        <td>{candidate.studyProgram}</td>
                    </tr>
                )}
                </tbody>
            </table>
        </div>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : EventDetail.renderEvent(this.state.event);

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
