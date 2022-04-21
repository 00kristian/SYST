import React, { Component } from 'react';

export class EventRating extends Component {
    static displayName = EventRating.name;

    constructor(props) {
        super(props);
        this.state = {loading: true };
    }

    static renderRating(){
        return (
            <h1>Hello again!</h1>
        );
    }
    
    render(){
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : EventRating.renderRating()
        return (
        <div>
          <h1>Hello</h1>
          {contents}
        </div>
        );
    }

}