import React, { useEffect, useState, useRef } from "react";
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
    const colors = [
        'rgba(255, 105, 180, 0.5)',
        'rgba(255, 0, 0, 0.5)',
        'rgba(255, 102, 0, 0.5)',
        'rgba(255, 218, 0, 0.5)',
        'rgba(173, 255, 47, 0.5)',
        'rgba(7, 218, 99, 0.5)',
        'rgb(48, 213, 200, 0.5)',
        'rgb(64, 34, 208, 0.5)',
        'rgb(148, 0, 211, 0.5)'
    ];
    const borderColors = [
        'rgba(255, 105, 180, 1)',
        'rgba(255, 0, 0, 1)',
        'rgba(255, 102, 0, 1)',
        'rgba(255, 218, 0, 1)',
        'rgba(173, 255, 47, 1)',
        'rgba(7, 218, 99, 1)',
        'rgb(48, 213, 200, 1)',
        'rgb(64, 34, 208, 1)',
        'rgb(148, 0, 211, 1)' 
    ];
    const barColors = [
        'rgba(255, 105, 180, 0.7)',
        'rgba(255, 0, 0, 0.7)',
        'rgba(255, 102, 0, 0.7)',
        'rgba(255, 218, 0, 0.7)',
        'rgba(173, 255, 47, 0.7)',
        'rgba(7, 218, 99, 0.7)',
        'rgb(48, 213, 200, 0.7)',
        'rgb(64, 34, 208, 0.7)',
        'rgb(148, 0, 211, 0.7)'
    ];
    const [graphData, setGraphData] = useState([]);
    const [selectedUni, setSelectedUni] = useState("Aalborg University");
    const [barColor, setBarColor] = useState('rgba(255, 105, 180, 0.7)');
    const [answerRates, setAnswerRates] = useState([]);
    const [distribution, setDistribution] = useState([]);
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

    useEffect(async () => {
        const options = await FetchOptions.Options(instance, accounts, "GET");
        const data = await fetch('api/Candidates/AnswerDistribution/' + selectedUni, options)
        .then(response => response.json())
        .catch(error => console.log(error));
        setAnswerRates(data.answerRates.map(a => a + "%"));
        setDistribution(data.distribution);
    }, [selectedUni])

    const labels = answerRates;

    const pieOptions =  {
        'onClick' : function (evt, item) {
            if (item[0] == null) return;
            let id = item[0].index;
            setSelectedUni(universities[id]?? "Other");
            setBarColor(colors[id]);
        }
    }
    return (
        <div className="div-flex">
            <div style={{width: 350, marginLeft: 150, marginRight: 50}}>
                <h4>Distribution of candidates</h4>
                <Pie options={pieOptions} data={{
                    labels: ['AAU', 'AU', 'CBS','ITU','RUC','DTU','KU','SDU','Other'],
                    datasets: [{
                        label: '# of Votes',
                        data: graphData,
                        backgroundColor: colors,
                        borderColor: borderColors,
                        borderWidth: 1
                    }]
                }} />
            </div>
            <div style={{width: 600, marginTop: 0}}>
                <h4>Distribution of answers per University</h4>
                <Bar data={{
                    labels,
                    datasets: [
                        {
                            label: selectedUni,
                            data: distribution,
                            backgroundColor: barColor,
                        }
                    ],
                }} />
            </div>
        </div>
    )
}