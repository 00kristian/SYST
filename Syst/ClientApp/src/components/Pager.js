import React, { Component } from 'react';

export class Pager extends Component {
    
    static Pager(at, questions, setFun, finishFun) {
        let at_ = at;
        const length = questions.length;
        const back = () => {
            if (at_ > 0) {
                let bar = document.getElementById('page-progressBar');
                bar.value = --at_ + 1;
                setFun(at_);
            }
        }
        const next = () => {
            if (at_ < length - 1) {
                let bar = document.getElementById('page-progressBar');
                bar.value = ++at_ + 1;
                setFun(at_);
            } else {
                finishFun();
            }
        }
        return (
            <div className="horizontal-centered-div">
                <button onClick={back}>Back</button>
                <progress id='page-progressBar' value={at_ + 1} max={length}> 10% </progress>
                <button onClick={next}>Next</button>
            </div>
        );  
    }
}