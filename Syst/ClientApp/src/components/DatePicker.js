import React, { Component } from 'react';

//The date picker that gets used when creating an event and when a a candidate sets in their graduation month
export class DatePicker extends Component {
    static Picker(value, changeFun) {
        return (
            <div>
                <input onInput={(event) => changeFun(event.target.value)} 
                value={value.toISOString().split('T')[0]}
                type="date" id="start" name="trip-start"
                min="2018-01-01" max="2030-12-31"></input>
            </div>
        );  
    }
}