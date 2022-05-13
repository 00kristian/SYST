import React, { Component } from "react";
import { CSVLink } from "react-csv";

export default ExportCandidates

function ExportCandidates(props) {
    const csvData = props.Candidates.map(c => ({
        Name : c.name,
        Email : c.email,
        Degree : c.currentDegree, 
        Study_Program : c.studyProgram,
        University : c.university, 
        Graduation_date : c.graduationDate,
        Is_upvoted : c.isUpvoted}))
    return <div className="div-right"><button className='btn btn-tertiary btn-right txt-small' > <CSVLink filename={props.Name} data={csvData}>Export</CSVLink></button></div> ;
}

