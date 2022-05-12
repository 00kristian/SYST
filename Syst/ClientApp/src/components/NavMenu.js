import React, { Component, useState } from 'react';
import { Collapse, Container, Navbar, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import logo from './Systematic_Logo.png';
import Icon from '@mdi/react'
import { mdiCloseOutline, mdiHome } from '@mdi/js'
import { mdiCalendar } from '@mdi/js';
import { mdiAccountGroup } from '@mdi/js';
import { useIsAuthenticated } from "@azure/msal-react";
import { SignInButton } from "./SignInButton";
import { AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";

export function NavMenu(props) {
  //static displayName = NavMenu.name;
  const [collapsed, setCollapsed] = useState(true);
  const isAuthenticated = useIsAuthenticated();

  console.log(isAuthenticated);

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  }

    return (
     // <AuthenticatedTemplate>
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <div>
              <a href={"/"}>
                <img src={logo} alt="Logo" width={230}/>
              </a>
            </div>
            <div >
              <h5>Event Tool</h5>
            </div>
            <NavbarToggler onClick={toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="navitem text-dark txt-navbar" to="/"><Icon path={mdiHome} size={1}/> HOME</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink tag={Link} className="navitem text-dark txt-navbar" to="/Events"><Icon path={mdiCalendar} size={1}/> EVENTS</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="navitem text-dark txt-navbar" to="/candidates"><Icon path={mdiAccountGroup} size={1}/> CANDIDATES</NavLink>
                </NavItem>
                <NavItem>
                { !isAuthenticated ? <SignInButton></SignInButton> : <NavLink tag={Link} className="navitem text-dark txt-navbar" to="/">SIGN OUT</NavLink>}              
                  </NavItem>
              </ul>
            </Collapse>
          </Container>
        </Navbar>
       </header>
    // </AuthenticatedTemplate>
    );
  
}
