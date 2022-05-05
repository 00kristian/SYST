import React, { Component } from 'react';

export class Pager extends Component {
    
    static Pager(at, length, setFun, pageNumber) {
        let at_ = at;

        const hideButtons = () => {
            let back = document.getElementById('page-backBtn');
            if (at_ == 0) {
                back.style.display = "block";
            } else {
                back.style.display = "none";
            }
            let next = document.getElementById('page-nextBtn');
            if (at_ == length) {
                next.style.display = "block";
            } else {
                next.style.display = "none";
            }
        }

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
            <div className="horizontal-centered-div div-flex2">
                <button className='btn btn-primary' id="page-backBtn" onClick={back}>Back</button>
                {pageNumber != null && pageNumber ? <h3 className='progress-bar progress-text'>{at_ + 1} / {length + 1}</h3> : <progress className='progress-bar' id='page-progressBar' value={at_} max={length}> {at_} </progress>}
                
                <button className='btn btn-primary btn-right' id="page-nextBtn" onClick={next}>Next</button>
            </div>
        );  
    }
}