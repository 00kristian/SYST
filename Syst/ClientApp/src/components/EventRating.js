import React, { Component } from 'react';
import Dropdown from 'react-dropdown';

export class EventRating extends Component {
    static displayName = EventRating.name;

    constructor(props) {
        super(props);
        this.state = {loading: true };
    }
    
    render() {
        const QuestionCandidates = [
            '1 - None',
            '2 - Fewer than expected',
            '3 - As many as expected',
            '4 - A few more than expected',
            '5 - Far more than expected'
        ];
        const QuestionApplications = [
            '1 - None',
            '2 - Fewer than expected',
            '3 - As many as expected',
            '4 - A few more than expected',
            '5 - Far more than expected'
        ];
        const QuestionCost = [
            '1 - Free or included in partnership',
            '2 - Less than 10.000 kr.',
            '3 - 11.000 to 25.000 kr.',
            '4 - 26.000 to 40.000 kr.',
            '5 - More than 41.000 kr.'
        ];
        const QuestionTime = [
            '1 - None',
            '2 - Fewer than expected',
            '3 - As many as expected',
            '4 - A few more than expected',
            '5 - Far more than expected'
        ];


        return (
            <div>
                <h2 className='div-center'>Event Rating</h2>
                <br/>
                <p>1. How many relevant candidates did we meet?</p>
                <Dropdown options={QuestionCandidates} value="Select Rating" />
                <br/>
                <p>2. How many applications do we expect to receive after the event?</p>
                <Dropdown options={QuestionApplications} value="Select Rating" />
                <br/>
                <p>3. What was the cost of the event?</p>
                <Dropdown options={QuestionCost} value="Select Rating" />
                <br/>
                <p>4. How much time did we invest in preparing for the event?</p>
                <Dropdown options={QuestionTime} value="Select Rating" />
                <br/>
                <button onClick={() => this.cancelRating()} className='btn btn-cancel'>Cancel</button>
                <button onClick={() => this.submitRating()} className='btn btn-primary btn-right'>Submit</button>
           
            </div>
        );
    }

    cancelRating = () => {
        const { history } = this.props;
        history.push("/Events");
    }

    submitRating = () => {
        const { history } = this.props;
        history.push("/Events");
    }
}