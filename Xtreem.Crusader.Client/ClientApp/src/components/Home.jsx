import React, { Component } from 'react';
import { TVChartContainer } from './TVChartContainer/index';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = { baseCurrency: 'BTC', quoteCurrency: 'USD', status: 0 };
    this.handleErrors = this.handleErrors.bind(this);
    this.predict = this.predict.bind(this);
    this.updateBaseCurrency = this.updateBaseCurrency.bind(this);
    this.updateQuoteCurrency = this.updateQuoteCurrency.bind(this);
    this.tvChart = React.createRef();
  }

  handleErrors(response) {
    if (!response.ok) {
      if (response.status === 404) {
        this.setState({
          message: 'Prediction not possible.'
        });
      }
      if (response.status === 400) {
        this.setState({
          message: 'An error occurred. Please contact support for assistance.'
        });
      }
    }

    this.setState({
      status: response.status
    });

    return response;
  }

  predict() {
    if (this.state.baseCurrency !== '' && this.state.quoteCurrency !== '') {
      const now = new Date();
      const year = now.getFullYear();
      const months = now.getMonth() + 1;
      const date = now.getDate();
      const hours = now.getHours();
      const minutes = now.getMinutes();
      const seconds = now.getSeconds();
      const fromDate = year + '-' + months + '-' + (date - 1);
      const toDate = year + '-' + months + '-' + (hours < 23 ? date : date + 1);
      const from = fromDate + ' ' + hours + ':' + minutes + ':' + seconds;
      const to = toDate + ' ' + (hours < 23 ? hours + 1 : 0) + ':' + minutes + ':' + seconds;

      window.fetch(`crusader/predict`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({
            CurrencyPairBaseCurrency: this.state.baseCurrency,
            CurrencyPairQuoteCurrency: this.state.quoteCurrency,
            Resolution: this.tvChart.current.props.interval,
            From: from,
            To: to
          })
        })
        .then(this.handleErrors)
        .then(response => response.json())
        .then(this.tvChart.current.reload())
        .catch(error => console.log(error));
    }
    else {
      this.setState({
        orderStatus: 0,
        message: `Please enter valid values.`
      });
    }
  }

  updateBaseCurrency(e) {
    this.setState({ baseCurrency: e.target.value });
  }

  updateQuoteCurrency(e) {
    this.setState({ quoteCurrency: e.target.value });
  }

  render() {
    const predict = 'Predict';

    return (
      <div>
        <p>
          <span style={{ paddingRight: 10 }}><strong>Base Currency:</strong></span><input type="text" placeholder="BTC" value={this.state.baseCurrency} onChange={this.updateBaseCurrency} />
          <span style={{ paddingLeft: 10, paddingRight: 10 }}><strong>Quote Currency:</strong></span><input type="text" placeholder="USD" value={this.state.quoteCurrency} onChange={this.updateQuoteCurrency} />
        </p>
        <p><button className="btn btn-primary" onClick={this.predict}>{predict}</button></p>
        <TVChartContainer ref={this.tvChart} />
      </div>
    );
  }
}
