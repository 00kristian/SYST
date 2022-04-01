import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import logo from './Systematic_Logo.png';
import Icon from '@mdi/react'
import { mdiHome } from '@mdi/js'
import { mdiCalendar } from '@mdi/js';
import { mdiAccountGroup } from '@mdi/js';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render () {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <a href={"/"}>
              <img src={logo} alt="Logo" width={230}/>
            </a>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="navitem text-dark" to="/"><Icon path={mdiHome} size={1}/> Home</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink tag={Link} className="navitem text-dark" to="/Events"><Icon path={mdiCalendar} size={1}/> Events</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="navitem text-dark" to="/candidates"><Icon path={mdiAccountGroup} size={1}/> Candidates</NavLink>
                </NavItem>
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
  }
}
