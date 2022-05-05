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
    return <button className='btn btn-cancel btn-right' > <CSVLink filename={props.Name} data={csvData}>Export</CSVLink></button> ;
}

