import React, { useEffect, useState, useRef } from "react";
import ReactTooltip from 'react-tooltip';
import Icon from './infoIcon.svg';
export function InformationIcon(props) {

    return <>
            <img data-tip={props.children} width={20} src={Icon}></img>
            <ReactTooltip multiline={true}/>
        </>
}