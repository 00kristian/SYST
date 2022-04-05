import React, { Component } from 'react';

import Picker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

export class DatePicker extends Component {
    static Picker(startValue, changeFun) {
        console.log(startValue);
        return (
            <div>
                <input onInput={() => {
                    var p = document.querySelector('input[type="date"]');
                    changeFun(p.value);
                }} 
                defaultValue={startValue}
                type="date" id="start" name="trip-start"
                min="2018-01-01" max="2030-12-31"></input>
            </div>
        );  
    }
}