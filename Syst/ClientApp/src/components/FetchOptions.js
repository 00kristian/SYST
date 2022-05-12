import React, { Component } from 'react';

export function Options() {
    return (
        <div>
            <input onInput={(event) => changeFun(event.target.value)} 
            value={value.toISOString().split('T')[0]}
            type="date" id="start" name="trip-start"
            min="2018-01-01" max="2030-12-31"></input>
        </div>
    );
}