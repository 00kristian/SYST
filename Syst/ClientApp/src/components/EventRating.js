import React, { Component, useState } from 'react';
import Dropdown from 'react-dropdown';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import {FetchOptions} from './FetchOptions';
import { useHistory } from "react-router-dom";

//Page where the admins can rate a specific event 
export default EventRating

function EventRating(props) {

    const { instance, accounts } = useMsal();
    const [QCandidateRating, setQcandidaterating ] = useState(0);
    const [QApplicationRating, setQapplicationRating ] = useState(0);
    const [QCostRating, setQCostRating ] = useState(0);
    const [QTimeRating, setQTimeRating ] = useState(0);
    const [isFilledOut, setIsFilledOut ] = useState(false);
    const [finalRating, setFinalRating ] = useState(0);
    const history = useHistory();

    
    const cancelRating = () => {
        history.push("/eventdetail/" + props.match.params.id);
    }

    const updateRating = async (rating) => {
        const options = await FetchOptions.Options(instance, accounts, "PUT");

        options.headers = {
            ...options.headers,
            'Content-Type' : 'application/json',
        }
        options.body = JSON.stringify(rating*1.0);
        await fetch('api/events/rating/' + props.match.params.id, options)
    }
    const submitRating = async () => {

        if (QCandidateRating === 0.0 || QApplicationRating === 0.0 || QCostRating === 0.0 || QTimeRating === 0.0) {
            setIsFilledOut(false)
            
        } else {
            const rating = (QCandidateRating + QApplicationRating + QCostRating + QTimeRating) / 4.0
            setFinalRating(rating);
            await updateRating(rating);
            history.push("/eventdetail/" + props.match.params.id);
        }
    }
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
     <AuthenticatedTemplate>
            <div className="page-padding">
                <h2 className='div-center'>Event Rating</h2>
                <br/>
                <p>1. How many relevant candidates did we meet?</p>
                <Dropdown className='txt-small' options={QuestionCandidates} onChange={(QuestionCandidates) => setQcandidaterating(QuestionCandidates.value)} value="Select Rating" />

                <br/>
                <p>2. How many applications do we expect to receive after the event?</p>
                <Dropdown className='txt-small' options={QuestionApplications} onChange={(QuestionApplications) => setQapplicationRating(QuestionApplications.value)} value="Select Rating" />
                <br/>
                <p>3. What was the cost of the event?</p>
                <Dropdown className='txt-small' options={QuestionCost} onChange={(QuestionCost) => setQCostRating(QuestionCost.value)} value="Select Rating" />
                <br/>
                <p>4. How much time did we invest in preparing for the event?</p>
                <Dropdown className='txt-small' options={QuestionTime} onChange={(QuestionTime) => setQTimeRating(QuestionTime.value)} value="Select Rating" />
                <br/>
                <button onClick={() => cancelRating()} className='btn btn-secondary'>Cancel</button>
                <button onClick={() => submitRating()} className='btn btn-primary btn-right'>SUBMIT</button>
                {isFilledOut ? <p></p> : <p className='txt-red txt-right_padding'>Please select an option for each question.</p>}
            </div>
        </AuthenticatedTemplate>
        );
    


}