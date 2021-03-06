import React, {useState } from 'react';
import { Collapse, Container, Navbar, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import logo from './Systematic_Logo.png';
import Icon from '@mdi/react'
import {mdiHome } from '@mdi/js'
import { mdiCalendar } from '@mdi/js';
import { mdiAccountGroup } from '@mdi/js';
import { useIsAuthenticated } from "@azure/msal-react";
import { SignInButton } from "./SignInButton";
import { AuthenticatedTemplate} from "@azure/msal-react";
import {SignOutButton} from "./SignOutButton";


//The navigation bar that makes the admin able to surf through the different pages
export function NavMenu(props) {
  //static displayName = NavMenu.name;
  const [collapsed, setCollapsed] = useState(true);
  const isAuthenticated = useIsAuthenticated();

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  }

    return (
        <AuthenticatedTemplate>
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <div style={{paddingLeft:"6%"}}>
              <a href={"/"}>
                <img src={logo} alt="Logo" width={230}/>
              </a>
            </div>
            <div>
              <h5
              style={{marginLeft: 9,
              paddingTop: 9}}
              >Event Tool</h5>
            </div>
            <NavbarToggler onClick={toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
              <ul className="navbar-nav flex-grow" style={{paddingRight:"5%"}}>
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
                { !isAuthenticated ? <SignInButton></SignInButton> : <SignOutButton></SignOutButton> }              
                  </NavItem>
              </ul>
            </Collapse>
          </Container>
        </Navbar>
       </header>
        </AuthenticatedTemplate>
    );
  
}
