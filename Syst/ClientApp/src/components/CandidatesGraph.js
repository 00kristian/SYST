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

    const barOptions = {
        responsive: true,
        plugins: {
          legend: {
            position: 'top',
          },
          title: {
            display: true,
            text: 'Chart.js Bar Chart',
          },
        },
      };

    const barLabels = ['January', 'February', 'March', 'April', 'May', 'June', 'July'];

    const barData = {
    barLabels,
    datasets: [
        {
        label: 'Dataset 1',
        data: barLabels.map(() => (Math.random() * 100)),
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
        <Bar data={barData} />
        </div>
    )
}