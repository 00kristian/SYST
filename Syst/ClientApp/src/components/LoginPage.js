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
                width: "100%",
                height: "100vh",
                backgroundSize: 'cover',
                backgroundRepeat: 'no-repeat',
                backgroundPosition: 'center' 
            };
        return (
            <div style={myStyle}>
            <div className='div-center'>
                <img style={{width: 600, backgroundPosition: 'center'}} src={logo} alt="logo" />
                <br></br>
                <h1 className='txt-white'>Welcome to Event Tool</h1>
            </div>
            <div className='btn-center'>
             {contents}
            </div> 
            </div>
            
    );
    }

    async populateData() {
        this.setState({loading: false });
    }
}
