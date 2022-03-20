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
    const response = await fetch('api/events/' + this.props.match.params.id);
    const data = await response.json();
    this.setState({ event: data, loading: false });
  }
}
