import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

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
            <tr>
              <td>{admin.id}</td>
              <td>{admin.name}</td>
              <td>{admin.email}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderAdminsTable(this.state.admins);

    return (
      <div>
        <h1 id="tabelLabel" >Fetch admin data</h1>
        {contents}
      </div>
    );
  }

  async populateData() {
    const response = await fetch('api/admins/1');
    const data = await response.json();
    this.setState({ admins: [data], loading: false });
  }
}
