import React, { Component } from 'react';
import Icon from "@mdi/react";
import {mdiThumbUp, mdiThumbDown} from '@mdi/js';
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import ReactDropdown from 'react-dropdown';

export class Candidates extends Component {
    static displayName = Candidates.name;

    constructor(props) {
        super(props);
        this.state = { candidates: [], loading: true };

    }

    componentDidMount() {
        this.populateData();
    }

    static renderCandidatesTable(candidates, downvoteFun, upvoteFun) {
        
    
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
                        {candidate.isUpvoted ?(
                        <td>
                            <td><button className="btn btn-right btn-green" onClick={() => upvoteFun(candidate.id)} ><Icon path={mdiThumbUp} size={1}/></button></td>
                            <td>
                                <Popup trigger = {<button className="btn btn-primary btn-right"><Icon path={mdiThumbDown} size={1}/></button>} modal nested>
                                {close => (
                                    <div>
                                        <p>Are you sure you want to delete this candidate?</p>
                                        <button className="btn btn-primary btn-yes" onClick={()=>downvoteFun(candidate.id)}>Yes</button>
                                        <button className="btn btn-primary"onClick={() => {close();}}>No</button>
                                    </div>
                                )}
                                </Popup>
                            </td>
                        </td>
                        ) : (
                        <td>
                             <td><button className="btn btn-primary btn-right" onClick={() => upvoteFun(candidate.id)} ><Icon path={mdiThumbUp} size={1}/></button></td>
                            <td>
                                <Popup className="popup-overlay" trigger = {<button className="btn btn-primary btn-right"><Icon path={mdiThumbDown} size={1}/></button>} modal nested>
                                {close => (
                                    <div>
                                        <p className="txt-popup">Are you sure you want to delete this candidate?</p>
                                        <div className="div-center">
                                            <button className="btn btn-primary btn-yes btn-popup" onClick={()=>downvoteFun(candidate.id)}>Yes</button>
                                            <button className="btn btn-primary btn-popup"onClick={() => {close();}}>No</button>
                                        </div>
                                    </div>
                                )}
                                </Popup>
                            </td>
                        </td>
                        )} 
                    <br />
                    </tr>
                )}
                </tbody>
            </table>
        );

    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Candidates.renderCandidatesTable(this.state.candidates, this.clickToDownvoteCandidate, this.clickToUpvoteCandidate);

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

    clickToDownvoteCandidate = async (id) => {
        await fetch('api/candidates/' + id, {
            method: 'DELETE'
        });
        this.populateData();
    }

    getCandidateById = async (id) =>{
        await fetch('api/candidates/' + id, {
            method: 'GET'
        })
        this.populateData();    
    }
    
    clickToUpvoteCandidate =  async (id) => {
        
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
           
        };
        await fetch("api/candidates"+"/upvote/"+ id, requestOptions)
        
        this.populateData();
    }
       
} 
 