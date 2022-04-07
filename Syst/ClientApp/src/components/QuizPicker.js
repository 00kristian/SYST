import React, { Component } from 'react';

import Dropdown from 'react-dropdown';
import 'react-dropdown/style.css'

export class QuizPicker extends Component {
    
    static Picker(quizes, startvalue, changeFun) {
        let val = "Select quiz";
        const options = [];
        quizes.forEach(q => {
            options.push({value: q.id, label: q.name})
            if (q.id === startvalue) {
                val = q.name;
            }
        });

        return (
            <div>
                <Dropdown options={options} onChange={(opt) => changeFun(opt.value)} value={val}/>
            </div>
        );  
    }
}