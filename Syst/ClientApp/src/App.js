import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Candidates } from './components/Candidates';
import { Events } from './components/Events';
import { Confirmation} from "./components/Confirmation";
import { EventDetail } from './components/EventDetail';
import './custom.css'
import { CreateEvent } from './components/CreateEvent';
import { CandidateQuiz } from './components/CandidateQuiz';
import { CandidateInformation } from './components/CandidateInformation';
import { ConfirmationCandidate } from './components/ConformationCandidate';
import { NavMenu } from './components/NavMenu';
import { Switch } from 'react-router-dom';
import { Container } from 'reactstrap';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <div>
        <Switch>
          
          <Route path='/CandidateQuiz' component={CandidateQuiz} />
          <Route path='/CandidateInformation' component={CandidateInformation} />
          <Route path='/ConformationCandidate' component={ConfirmationCandidate} />
          
          <div>
          <NavMenu />
          <Container>
            <Route exact path='/' component={Home} />
            <Route path='/candidates' component={Candidates} />
            <Route path='/fetch-data' component={FetchData} />
            <Route path='/CreateEvent' component={CreateEvent} />
            <Route path='/Events' component={Events} />
            <Route path='/Confirmation' component={Confirmation} />
            <Route path='/eventdetail/:id' component={EventDetail} />
          </Container>
          </div>
         
          </Switch>
      </div>
      /*<Layout>
        <Route exact path='/' component={Home} />
        <Route path='/candidates' component={Candidates} />
        <Route path='/fetch-data' component={FetchData} />
        <Route path='/CreateEvent' component={CreateEvent} />
        <Route path='/Events' component={Events} />
        <Route path='/Confirmation' component={Confirmation} />
        <Route path='/eventdetail/:id' component={EventDetail} />
      </Layout>*/
    );
  }
}
