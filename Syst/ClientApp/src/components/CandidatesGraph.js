import React, { useEffect, useState, useRef } from "react";
import { Pie, Bar } from 'react-chartjs-2';
import { Chart as ChartJS, ArcElement, Tooltip, Legend,
    CategoryScale,
    LinearScale,
    BarElement,
    Title} from 'chart.js';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import {FetchOptions} from './FetchOptions';
import { InformationIcon } from "./InformationIcon";

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
            <div style={{width: 350, marginLeft: 200, marginRight: 100}}>
                <h4>Distribution of candidates
                    &nbsp; 
                        <InformationIcon>
                            {"Pie chart visualizing the distribution of all candidates for each University.<br>Click on a slice to see the distribution of answers of an individual University on the next chart."}
                        </InformationIcon>
                    </h4>
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
                <h4>Distribution of answers per University &nbsp; 
                    <InformationIcon>
                    {"Bar chart visualizing the distribution of candidates' answer rate in % for an indivudual University.<br>X-axis is answer rate and Y-axis is amount of candidates."}
                    </InformationIcon>
                </h4>
                
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