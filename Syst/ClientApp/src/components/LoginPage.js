import React, { Component } from 'react';
import logo from './Systematic_Logo.png';
import { SignInButton } from "./SignInButton";

export class LoginPage extends Component {
    static displayName = LoginPage.name;

    constructor(props) {
        super(props);
        this.state = {loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : <SignInButton></SignInButton>

        return (
            <div className='div-center'>
                <img style={{width: 600}} src={logo} alt="logo" />
                <br></br>
                <h1>Welcome to Event Tool</h1>
                {contents}
            </div>
            
    );
    }

    async populateData() {
        this.setState({loading: false });
    }
}
