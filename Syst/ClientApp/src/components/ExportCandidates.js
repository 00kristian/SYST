import React, { Component } from "react";
import { CSVLink } from "react-csv";

export default ExportCandidates

function ExportCandidates(props) {
    const csvData = props.Candidates;
    return <CSVLink data={csvData}>Download me</CSVLink>;
}

