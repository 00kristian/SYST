import React, { Component } from 'react';
import { useParams } from 'react-router-dom';

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
            <h1>{event.date}</h1>
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
    const id = this.props.match.params;
    console.log(id);
    const response = await fetch('api/events/2');
    const data = await response.json();
    this.setState({ event: data, loading: false });
  }
}
