import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Candidates } from './components/Candidates';
import { Events } from './components/Events';
import { Confirmation} from "./components/Confirmation";

import './custom.css'
import { CreateEvent } from './components/CreateEvent';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/candidates' component={Candidates} />
        <Route path='/fetch-data' component={FetchData} />
        <Route path='/CreateEvent' component={CreateEvent} />
        <Route path='/Events' component={Events} />
        <Route path='/Confirmation' component={Confirmation} />
      </Layout>
    );
  }
}
