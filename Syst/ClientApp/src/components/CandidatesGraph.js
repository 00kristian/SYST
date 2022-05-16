import React, { useEffect, useState } from "react";
import { Pie } from 'react-chartjs-2';
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import {FetchOptions} from './FetchOptions';

ChartJS.register(ArcElement, Tooltip, Legend);


export function CandidatesGraph(props) {
    const [universities, setUniversities] = useState(["Aalborg University",
                                                        "Aarhus University",
                                                        "Copenhagen Business School",
                                                        "IT-University of Copenhagen",
                                                        "Roskilde University",
                                                        "Technical University of Denmark",
                                                        "University of Copenhagen",
                                                        "University of Southern Denmark"]);
    const [graphData, setGraphData] = useState([]);
    const { instance, accounts } = useMsal();

    useEffect(async () => {
        const options = await FetchOptions.Options(instance, accounts, "PUT");
        options.body = universities
        options.headers ={ 
            ...options.headers,
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        };
        const data = await fetch('api/candidates/graphdata', options)
        .then(response => response.json())
        .catch(error => console.log(error));
        setGraphData(data);
    }, []);

    return (
        <Pie data={{
            labels: ['AAU', 'AU', 'CBS','ITU','RUC','DTU','KU','SDU','Other'],
            datasets: [{
                label: '# of Votes',
                data: graphData,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        }} />
    )
}