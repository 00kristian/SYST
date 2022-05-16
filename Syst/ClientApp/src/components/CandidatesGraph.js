import React, { useEffect, useState } from "react";
import { Pie, Bar } from 'react-chartjs-2';
import { Chart as ChartJS, ArcElement, Tooltip, Legend,
    CategoryScale,
    LinearScale,
    BarElement,
    Title} from 'chart.js';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import {FetchOptions} from './FetchOptions';

ChartJS.register(
    ArcElement, Tooltip, Legend,
    CategoryScale,
    LinearScale,
    BarElement,
    Title
);


export function CandidatesGraph(props) {
    const universities = ["Aalborg University",
        "Aarhus University",
        "Copenhagen Business School",
        "IT-University of Copenhagen",
        "Roskilde University",
        "Technical University of Denmark",
        "University of Copenhagen",
        "University of Southern Denmark"];
    const [graphData, setGraphData] = useState([]);
    const [selectedUni, setSelectedUni] = useState("Aalborg University");
    const { instance, accounts } = useMsal();

    useEffect(async () => {
        const options = await FetchOptions.Options(instance, accounts, "PUT");
        options.headers ={ 
            ...options.headers,
            'Content-Type': 'application/json',
            'Accept': 'text/plain'
        };
        options.body = JSON.stringify(universities);
        const data = await fetch('api/candidates/graphdata', options)
        .then(response => response.json())
        .catch(error => console.log(error));
        
        setGraphData(data);
    }, []);

    const labels = ['January', 'February', 'March', 'April', 'May', 'June', 'July'];

    const barData = {
    labels,
    datasets: [
        {
        label: 'Dataset 1',
        data: labels.map(() => (Math.random() * 100)),
        backgroundColor: 'rgba(255, 99, 132, 0.5)',
        }
    ],
    };

    return (
        <div className="div-flex horizontal-centered-div">

        <Pie data={{
            labels: ['AAU', 'AU', 'CBS','ITU','RUC','DTU','KU','SDU','Other'],
            datasets: [{
                label: '# of Votes',
                data: graphData,
                backgroundColor: [
                    'rgba(255, 105, 180, 0.2)',
                    'rgba(255, 0, 0, 0.2)',
                    'rgba(255, 102, 0, 0.2)',
                    'rgba(255, 218, 0, 0.2)',
                    'rgba(173, 255, 47, 0.2)',
                    'rgba(7, 218, 99, 0.2)',
                    'rgb(48, 213, 200, 0.2)',
                    'rgb(64, 34, 208, 0.2)',
                    'rgb(148, 0, 211, 0.2)'
                  ],
            
                borderColor: [
                    'rgba(255, 105, 180, 1)',
                    'rgba(255, 0, 0, 1)',
                    'rgba(255, 102, 0, 1)',
                    'rgba(255, 218, 0, 1)',
                    'rgba(173, 255, 47, 1)',
                    'rgba(7, 218, 99, 1)',
                    'rgb(48, 213, 200, 1)',
                    'rgb(64, 34, 208, 1)',
                    'rgb(148, 0, 211, 1)'
                ],
                borderWidth: 1
            }]
        }} onElementsClick={elems => {
            // if required to build the URL, you can 
            // get datasetIndex and value index from an `elem`:
            console.log(elems[0]._datasetIndex + ', ' + elems[0]._index);
            }}/>
        <Bar data={barData} />
        </div>
    )
}