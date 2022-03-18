import React, { Component } from 'react';

export class Candidates extends Component {
    static displayName = Candidates.name;

    constructor(props) {
        super(props);
        this.state = { candidates: [], loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    static renderCandidatesTable(candidates) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>University</th>
                    <th>Degree</th>
                </tr>
                </thead>
                <tbody>
                {candidates.map(candidate =>
                    <tr key={candidate.id}>
                        <td>{candidate.id}</td>
                        <td>{candidate.name}</td>
                        <td>{candidate.email}</td>
                        <td>{candidate.university}</td>
                        <td>{candidate.studyProgram}</td>
                    </tr>
                )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Candidates.renderCandidatesTable(this.state.candidates);

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
}
