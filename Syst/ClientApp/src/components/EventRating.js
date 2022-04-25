import React, { Component } from 'react';
import Dropdown from 'react-dropdown';

export class EventRating extends Component {
    static displayName = EventRating.name;

    constructor(props) {
        super(props);
        this.state = { event: { Rating: 0 }, QApplicationRating: 0, QCostRating: 0, QTimeRating: 0, QCandidateRating: 0};
    }
    
render() {
        const QuestionCandidates = [
            {value: 1.0, label: '1 - None'},
            {value: 2.0, label: '2 - Fewer than expected'},
            {value: 3.0, label: '3 - As expected'},
            {value: 4.0, label: '4 - More than expected'},
            {value: 5.0, label: '5 - Beyond our expectations'}
        ];
        const QuestionApplications = [
            {value: 1.0, label: '1 - None' },
            {value: 2.0, label: '2 - Fewer than expected' },
            {value: 3.0, label: '3 - As expected' },
            {value: 4.0, label: '4 - More than expected' },
            {value: 5.0, label: '5 - Beyond our expectations' }
        ];
        const QuestionCost = [
            {value: 1.0, label: '1 - More than 41.000 kr.'},
            {value: 2.0, label: '2 - 26.000 to 40.000 kr.'},
            {value: 3.0, label: '3 - 11.000 to 25.000 kr.' },
            {value: 4.0, label: '4 - Less than 10.000 kr.'},
            {value: 5.0, label: '5 - Free or included in partnership' }
        ];
        const QuestionTime = [
            {value: 1.0, label: '1 - Beyond our expectations' },
            {value: 2.0, label: '2 - More than expected' },
            {value: 3.0, label: '3 - As expected' },
            {value: 4.0, label: '4 - Fewer than expected' },
            {value: 5.0, label: '5 - None' }
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
            </div>
        );
    }


    cancelRating = () => {
        const { history } = this.props;
        history.push("/eventdetail/" + this.props.match.params.id);
    }


    submitRating = async (QCandidateRating, QApplicationRating, QCostRating, QTimeRating ) => {
        const finalRating = (this.state.QCandidateRating + this.state.QApplicationRating + this.state.QCostRating + this.state.QTimeRating) / 4.0
        this.setState({
            Rating: finalRating
        });
        await this.updateRating(finalRating);
        const { history } = this.props;
        history.push("/eventdetail/" + this.props.match.params.id);
    }

    updateRating = async (Rating) => {
        let event = {
            "rating": this.state.event.Rating
        };
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: (Rating * 1.0)
        };
        await fetch('api/events/rating/' + this.props.match.params.id, requestOptions)
    }

}