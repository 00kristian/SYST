import React, { Component } from 'react';
import Icon from "@mdi/react";
import {mdiThumbUp, mdiThumbDown} from '@mdi/js';

export class Candidates extends Component {
    static displayName = Candidates.name;

    constructor(props) {
        super(props);
        this.state = { candidates: [], loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    static renderCandidatesTable(candidates, delFun) {

    
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>University</th>
                    <th>Degree</th>
                    <th>Graduation date</th>
                </tr>
                </thead>
                <tbody>
                {candidates.map(candidate =>
                    <tr key={candidate.id}>
                        <td>{candidate.id}</td>
                        <td>{candidate.name}</td>
                        <td>{candidate.email}</td>
                        <td>{candidate.university}</td>
                        <td>{candidate.currentDegree} in {candidate.studyProgram}</td>
                        <td>{candidate.graduationDate}</td>
                        <td><button className="btn btn-primary btn-right"><Icon path={mdiThumbUp} size={1}/></button></td>
                        <td><button className="btn btn-primary btn-right" onClick={()=>delFun(candidate.id)}><Icon path={mdiThumbDown} size={1}/></button></td>
                    </tr>
                )}
                </tbody>
            </table>
        );

    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Candidates.renderCandidatesTable(this.state.candidates, this.clickToDeleteCandidate);

        return (
            <div>
                <h1 id="tabelLabel" >Candidates</h1>
                {contents}
            </div>
        );

    }

    async populateData() {
        const response = await fetch('api/candidates');
        const data = await response.json();
        this.setState({ candidates: data, loading: false });
    }

    clickToDeleteCandidate = async (id) => {
        await fetch('api/candidates/' + id, {
            method: 'DELETE'
        });
        this.populateData();
    }
       
} 
 