import React, { Component, useEffect, useState } from 'react';
import { InteractiveTable } from './InteractiveTable';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { FetchOptions } from './FetchOptions';


//Page that shows alle the events
export default Events 

function Events (props) {

    const [upcomingEvents, setUpcomingEvents] = useState([]);
    const [recentEvents, setRecentEvents] = useState([]);
    const { instance, accounts } = useMsal();

    useEffect(async () => {
        const options = await FetchOptions.Options(instance, accounts, "GET");
        const dataUpcoming = await fetch('api/eventsquery/upcoming', options)
        .then(response => response.json())
        .catch(error => console.log(error));
        
        const dataRecent = await fetch('api/events', options)
        .then(response => response.json())
        .catch(error => console.log(error));

        console.log(dataUpcoming);
        console.log(dataRecent);

        setRecentEvents(dataRecent);
        setUpcomingEvents(dataUpcoming);
    }, []);




    
    const rerouteToEventCreation = async () => {
        let event = {
            "name": "new event",
            "date":  new Date().toISOString().split('T')[0],
            "location": "location"
        };

        let options = await FetchOptions.Options(instance, accounts, "POST");
        options.headers ={
            ...options.headers,
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        };
        options.body = JSON.stringify(event);


        const qId = await fetch('api/events', options)
        .then(response => response.json())
 
        const { history } = props;
        history.push("/CreateEvent/"+qId); 
    }

    
        let UpcomingContents =
             <InteractiveTable Columns={[["Id", "id"], ["Name", "name"], ["Date", "date"], ["Location", "location"], ["Rating", "rating"]]} Content={upcomingEvents}>
                {event =>
                    <div className='div-right'>
                        <td><a href={'/eventdetail/' + event.id}> <button className="btn btn-secondary btn-right obj-right_margin">Details</button></a></td>
                        <td onClick={()=> window.open('/CandidateQuiz/' + event.id + '/' + event.quiz.id, "_blank", 'location=yes,height=800,width=1300,scrollbars=yes,status=yes')}><button className="btn btn-primary btn-right">HOST</button></td>
                    </div>
                }
            </InteractiveTable>;
        let RecentContents = 
             <InteractiveTable SearchBar={true} PageSize={7} Columns={[["Id", "id"], ["Name", "name"], ["Date", "date"], ["Location", "location"], ["Rating", "rating"]]} Content={recentEvents}>
            {event =>
                <div className='div-right'>
                    <td><a href={'/eventdetail/' + event.id}> <button className="btn btn-secondary btn-right">Details</button></a></td>
                </div>
            }
        </InteractiveTable>;

        return (
            <AuthenticatedTemplate>
            <div className="page-padding">
                <h3 id="tabelLabel" >Upcoming Events
                    <button className="btn btn-primary btn-right" onClick= {()=>rerouteToEventCreation()}>CREATE</button>
                </h3>
                {UpcomingContents}
                <br/>
                <h3 className="obj-top_padding">All Events
                </h3>
                {RecentContents}
                </div>
            </AuthenticatedTemplate>
        );
    


    
}
