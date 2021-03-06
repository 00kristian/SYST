import React, { Component } from "react";
import { CSVLink } from "react-csv";

//Function that exports the candidates to a csv file
export default ExportCandidates

function ExportCandidates(props) {
    const csvData = props.Candidates.map(c => ({
        Name : c.name,
        Email : c.email,
        Degree : c.currentDegree, 
        Study_Program : c.studyProgram,
        University : c.university, 
        Graduation_date : c.graduationDate,
        Is_upvoted : c.isUpvoted,
        Answer_rate : c.percentageOfCorrectAnswers
    }))
    return <div className="div-right"><button className='btn btn-right btn-tertiary' > <CSVLink style={{textDecoration: "none", color: "#009BE6"}} filename={props.Name} data={csvData}>Export</CSVLink></button></div> ;
}

