import React, { Component } from 'react';
import Dropdown from 'react-dropdown';

export class EventRating extends Component {
    static displayName = EventRating.name;

    constructor(props) {
        super(props);
        this.state = {QCandidateRating: 0, QApplicationRating: 0, QCostRating: 0, QTimeRating: 0, OverAllRating: 0};
    }
    
    render() {
        const QuestionCandidates = [
            {value: 1, label: '1 - None'},
            {value: 2, label: '2 - Fewer than expected'},
            {value: 3, label: '3 - As many as expected'},
            {value: 4, label: '4 - A few more than expected'},
            {value: 5, label: '5 - Far more than expected'}
        ];
        const QuestionApplications = [
            {value: 1, label: '1 - None' },
            {value: 2, label: '2 - Fewer than expected' },
            {value: 3, label: '3 - As many as expected' },
            {value: 4, label: '4 - A few more than expected' },
            {value: 5, label: '5 - Far more than expected' }
        ];
        const QuestionCost = [
            {value: 1, label: '1 - Free or included in partnership'},
            {value: 2, label: '2 - Less than 10.000 kr.'},
            {value: 3, label: '3 - 11.000 to 25.000 kr.' },
            {value: 4, label: '4 - 26.000 to 40.000 kr.'},
            {value: 5, label: '5 - More than 41.000 kr.' }
        ];
        const QuestionTime = [
            {value: 1, label: '1 - None' },
            {value: 2, label: '2 - Fewer than expected' },
            {value: 3, label: '3 - As many as expected' },
            {value: 4, label: '4 - A few more than expected' },
            {value: 5, label: '5 - Far more than expected' }
        ];


        return (
            <div>
                <h2 className='div-center'>Event Rating</h2>
                <br/>
                <p>1. How many relevant candidates did we meet?</p>
                <Dropdown options={QuestionCandidates} onChange={(QuestionCandidates) => this.setState({QCandidateRating: QuestionCandidates.value})} value="Select Rating" />
                <br/>
                <p>2. How many applications do we expect to receive after the event?</p>
                <Dropdown options={QuestionApplications} onChange={(QuestionApplications) => this.setState({ QApplicationRating: QuestionApplications.value })} value="Select Rating" />
                <br/>
                <p>3. What was the cost of the event?</p>
                <Dropdown options={QuestionCost} onChange={(QuestionCost) => this.setState({ QCostRating: QuestionCost.value })} value="Select Rating" />
                <br/>
                <p>4. How much time did we invest in preparing for the event?</p>
                <Dropdown options={QuestionTime} onChange={(QuestionTime) => this.setState({ QTimeRating: QuestionTime.value })} value="Select Rating" />
                <br/>
                <button onClick={() => this.cancelRating()} className='btn btn-cancel'>Cancel</button>
                <button onClick={() => this.submitRating()} className='btn btn-primary btn-right'>Submit</button>

                <p>Rating: {this.state.OverAllRating}</p>
                <p>QCandidateRating: {this.state.QCandidateRating}</p>
                <p>QApplicationRating: {this.state.QApplicationRating}</p>
                <p>QCostRating: {this.state.QCostRating}</p>
                <p>QTimeRating: {this.state.QTimeRating}</p>
            </div>
        );
    }


    cancelRating = () => {
        const { history } = this.props;
        history.push("/Events");
    }


    submitRating = (QCandidateRating, QApplicationRating, QCostRating, QTimeRating ) => {
        this.setState({
            OverAllRating: this.state.QCandidateRating + this.state.QApplicationRating + this.state.QCostRating + this.state.QTimeRating
        });
        //const { history } = this.props;
        //history.push("/Events");
    }
}