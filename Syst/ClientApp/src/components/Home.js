import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderAdminsTable(this.state.admins);

        return (
            <div>
                <h1>Welcome to Systematic Event Tool!</h1>
                <p>From this home page you'll be able to create, host and see and overview over events! Check it out!</p>

                <h1 id="tabelLabel" >Events</h1> <button className="btn btn-primary">Create</button>
                {contents}
            </div>
        );}

        constructor(props) {
          super(props);
          this.state = { admins: [], loading: true };
        }
      
        componentDidMount() {
          this.populateData();
        }

        static renderAdminsTable(admins) {
          return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
              <thead>
                <tr>
                  <th>Id</th>
                  <th>Name</th>
                  <th>Email</th>
                </tr>
              </thead>
              <tbody>
                {admins.map(admin =>
                  <tr key={admin.id}>
                    <td>{admin.id}</td>
                    <td>{admin.name}</td>
                    <td>{admin.email}</td>
                  </tr>
                )}
              </tbody>
            </table>
          );
        }

        async populateData() {
          const response = await fetch('api/admins/1');
          const data = await response.json();
          this.setState({ admins: [data], loading: false });
        }
}
