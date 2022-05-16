import React, { useEffect, useState } from "react";
import { Pager } from "./Pager";
import ExportCandidates from './ExportCandidates';
import { AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";

//Function that creates the interactive tables used under events and candidates 
export function InteractiveTable(props) {
    const _content = props.Content;
    const [sortedField, setSortedField] = useState(null);
    const [descendingSort, setDescendingSort] = useState(false);
    const [pageAt, setPageAt] = useState(0);
    const [search, setSearch] = useState("");

    const comparator = (a, b) => {
        if (a[sortedField] < b[sortedField]) {
          return -1;
        }
        if (a[sortedField] > b[sortedField]) {
          return 1;
        }
        return 0;
    };

    const sortContent = (field) => {
        if (field === sortedField) {
            setDescendingSort((ds) => !ds);
        } else {
            setDescendingSort(false);
            setSortedField(field);
        }
    }

    if (sortedField !== null) {
        if (descendingSort) {
            _content.sort(comparator);
            _content.reverse();
        } else {
            _content.sort(comparator);
        }
    }

    const slc = props.PageSize != null ? 
        (c => c.slice(pageAt * props.PageSize, pageAt * props.PageSize + props.PageSize))
        :
        (c => c);
    
    const filter = props.SearchBar ? (c => c.filter(
        _c => {
                for (let i = 0; i < props.Columns.length; i++) {
                    const col = props.Columns[i];
                    if (_c[col[1]].toString().toLowerCase().includes(search.toLowerCase())) return true;
                }
                return false;
            })
        )
        :
        (c => c);

    return (
        <AuthenticatedTemplate>
        <div>
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                <tr>
                    {props.Columns.map(col =>
                        <th>
                            <button className="btn" onClick={() => sortContent(col[1])}>
                                <p className="txt-secondary">
                                {col[0]} 
                                {sortedField == col[1] ? (
                                    descendingSort ? " ▲" : " ▼"
                                    ) : ""}
                                </p> 
                            </button> 
                        </th>
                    )}
                    {props.SearchBar ?
                        <div className="div-right">
                            <input value={search} placeholder="Search" onChange={(e) => setSearch(e.target.value)} className="txt-primary txt-small"></input>
                        </div>
                        :
                        <span></span>
                    }
                </tr>
                </thead>
                <tbody>
                {filter(slc(_content)).map(row =>
                    <tr key={row.id}>
                        {props.Columns.map(col =>
                            <td className="txt-small"> {row[col[1]]} </td>
                        )}
                    {props.children != null ? props.children(row) : <span> </span>}
                    </tr>)}
                </tbody>
            </table>
            {props.PageSize != null ?
                Pager.Pager(pageAt, Math.ceil(_content.length / props.PageSize, 10) - 1, false, (page) => {setPageAt(page)}, true)
                : <span></span>
            }

            {props.ExportName != null ? <ExportCandidates Name={props.ExportName} Candidates={filter(_content)}></ExportCandidates> : <span></span>}
            </div>
        </AuthenticatedTemplate>
    );
}