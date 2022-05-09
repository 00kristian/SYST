import React, { Component } from 'react';
import logo from './Systematic_Logo.png';
import Systematic from './SystematicTower.png';
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
        
        const myStyle={
                backgroundImage: `url(${Systematic})`,
                height:'100vh',
                
                backgroundSize: 'cover',
                backgroundRepeat: 'no-repeat',
            };
        return (
            <div style={myStyle}>
            <div className='div-center'>
                <img style={{width: 600}} src={logo} alt="logo" />
                <br></br>
                <h1>Welcome to Event Tool</h1>
                {contents}
            </div>
            </div>
            
    );
    }

    async populateData() {
        this.setState({loading: false });
    }
}
