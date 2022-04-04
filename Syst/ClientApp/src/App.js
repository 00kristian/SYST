import { Events } from './components/Events';
import { Home } from './components/Home';
import { Candidates } from './components/Candidates';
import { ConfirmationCandidate} from "./components/ConfirmationCandidate";
import { EventDetail } from './components/EventDetail';
import { CreateQuiz } from './components/CreateQuiz';
import { CreateQuestion } from './components/CreateQuestion';
import './custom.css'
import { CreateEvent } from './components/CreateEvent';
import { CandidateQuiz } from './components/CandidateQuiz';
import { CandidateInformation } from './components/CandidateInformation';
import { NavMenu } from './components/NavMenu';
import { Switch } from 'react-router-dom';
import { Container } from 'reactstrap';
import { Route } from 'react-router-dom';
import React, { Component } from 'react';

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
            <Route path='/CreateEvent' component={CreateEvent} />
            <Route path='/Events' component={Events} />
            <Route path='/Confirmation' component={ConfirmationCandidate} />
            <Route path='/CreateQuiz/:id' component={CreateQuiz} />
            <Route path='/CreateQuestion' component={CreateQuestion} />
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
        <Route path='/CreateQuiz' component={CreateQuiz} />
        <Route path='/CreateQuestion' component={CreateQuestion} />
        <Route path='/eventdetail/:id' component={EventDetail} 
        

        />
      </Layout>
        <Route path='/eventdetail/:id' component={EventDetail} />
      </Layout>*/
    );
  }
}