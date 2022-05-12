import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";

export class Layout extends Component {
  static displayName = Layout.name;

  render () {
      return (
        <AuthenticatedTemplate>
            <div>
                <NavMenu />
                <Container>
                    {this.props.children}
                </Container>
            </div>
        </AuthenticatedTemplate>
    );
  }
}
