import React, { Component } from 'react';
import { AuthenticatedTemplate } from "@azure/msal-react";

//Makes the progress bar used when hosting an event
export class Pager extends Component {
    
    static Pager(at, length, noNext, setFun, pageNumber) {
        let at_ = at;


        const back = () => {
            if (at_ > 0) {
                at_--;
                if (pageNumber == null)
                {
                    let bar = document.getElementById('page-progressBar');
                    bar.value = at_;
                }
                setFun(at_);
            }
            //hideButtons();
        }
        const next = () => {
            if (at_ < length) {
                at_++;
                if (pageNumber == null)
                {
                    let bar = document.getElementById('page-progressBar');
                    bar.value = at_;
                }
                setFun(at_);
            }
            //hideButtons();
        }
        return (
            <AuthenticatedTemplate>
            <div className="horizontal-centered-div div-flex2">
                <button className='btn btn-secondary' id="page-backBtn" onClick={back}>Back</button>
                {pageNumber != null && pageNumber ? <h3 className='progress-bar progress-text'>{at_ + 1} / {length + 1}</h3> : <progress className='progress-bar' id='page-progressBar' value={at_} max={length}> {at_} </progress>}
                    {noNext ? <></> :
                        <button className='btn btn-secondary btn-right' id="page-nextBtn" onClick={next}>Next</button>
                    }
                    
                </div>
            </AuthenticatedTemplate>
        );  
    }
}