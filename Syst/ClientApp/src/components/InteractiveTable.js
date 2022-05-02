import React, { useEffect, useState } from "react";
import { Pager } from "./Pager";

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
    
    const filter = props.SearchBar ? (c => slc(c).filter(
        _c => {
                for (let i = 0; i < props.Columns.length; i++) {
                    const col = props.Columns[i];
                    if (_c[col[1]].toString().toLowerCase().includes(search.toLowerCase())) return true;
                }
                return false;
            })
        )
        :
        (c => slc(c));

    return (
        <div>
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                <tr>
                    {props.Columns.map(col =>
                        <th>
                            <button className="btn" onClick={() => sortContent(col[1])}>
                                <h5>
                                {col[0]} 
                                {sortedField == col[1] ? (
                                    descendingSort ? " ▲" : " ▼"
                                    ) : ""}
                                </h5> 
                            </button> 
                        </th>
                    )}
                    {props.SearchBar ?
                        <th>
                            <input value={search} placeholder="Search" onChange={(e) => setSearch(e.target.value)}></input>
                        </th>
                        :
                        <span></span>
                    }
                </tr>
                </thead>
                <tbody>
                {filter(_content).map(row =>
                    <tr key={row.id}>
                        {props.Columns.map(col =>
                            <td> {row[col[1]]} </td>
                        )}
                    {props.children != null ? props.children(row) : <span> </span>}
                    </tr>)}
                </tbody>
            </table>
            {props.PageSize != null ?
                Pager.Pager(pageAt, Math.ceil(_content.length / props.PageSize, 10) - 1, (page) => {setPageAt(page)}, true)
                : <span></span>
            }
        </div>
    );
}