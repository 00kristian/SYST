import React, { Component } from 'react';

export class DatePicker extends Component {
    static Picker(startValue, changeFun) {
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