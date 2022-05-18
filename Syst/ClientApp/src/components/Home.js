import React, { useEffect, useState } from "react";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useIsAuthenticated, useMsal } from "@azure/msal-react";
import { InteractiveTable } from './InteractiveTable';
import {FetchOptions} from './FetchOptions';
import { CandidatesGraph } from "./CandidatesGraph";
import { Transition } from 'react-transition-group';
import { LoginPage } from './LoginPage';

//The first page the admin sees. The page shows all the upcoming events 
export default Home

function Home(props) {
    const [events, setEvents] = useState([]);
    const { instance, accounts } = useMsal();
    const isAuthenticated = useIsAuthenticated();

    useEffect(async () => {
        if (isAuthenticated) {
            const options = await FetchOptions.Options(instance, accounts, "GET");
            const data = await fetch('api/eventsquery/upcoming', options)
            .then(response => response.json())
            .catch(error => console.log(error));
            setEvents(data);
        }
        }, [isAuthenticated]);

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

       let qId = await fetch('api/events', options)
       .then(response => response.json());

       const { history } = props;
       history.push("/CreateEvent/"+qId); 
      
    }
    let contents =  
            <InteractiveTable Columns={[["Id", "id"], ["Name", "name"], ["Date", "date"], ["Location", "location"], ["Rating", "rating"]]} Content={events}>
            {event =>
                <div className='div-right'>
                    <td><a href={'/eventdetail/' + event.id}> <button className="btn btn-secondary btn-right obj-right_margin">Details</button></a></td>
                    <td onClick={()=> window.open('/CandidateQuiz/' + event.id + '/' + event.quiz.id, "_blank", 'location=yes,height=800,width=1300,scrollbars=yes,status=yes')}><button className="btn btn-primary btn-right">HOST</button></td>
                </div>
            }
            </InteractiveTable>;
        
    return (
        <React.Fragment>
        <AuthenticatedTemplate>
            <div className="page-padding">
            <h1 id="tabelLabel">Upcoming Events
                <button className="btn btn-primary btn-right" onClick={() => rerouteToEventCreation()}>CREATE</button>
            </h1>
            {contents}
        </div>
        <hr></hr>
        <CandidatesGraph></CandidatesGraph>
        <hr></hr>

        </AuthenticatedTemplate>
            <UnauthenticatedTemplate>
                <div>
                <LoginPage></LoginPage>
            </div>
            </UnauthenticatedTemplate>
        </React.Fragment>
    );
}