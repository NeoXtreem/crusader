import React, { Component } from 'react';
import { TVChartContainer } from './TVChartContainer/index';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = { base: 'BTC', quote: 'USD', status: 0 };
    this.handleErrors = this.handleErrors.bind(this);
    this.predict = this.predict.bind(this);
    this.updateBase = this.updateBase.bind(this);
    this.updateQuote = this.updateQuote.bind(this);
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
      if (this.state.base !== '' && this.state.quote !== '') {
      window.fetch(`api/cryptoprediction/predict/${this.state.base}/${this.state.quote}`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          }
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

  updateBase(e) {
    this.setState({ base: e.target.value });
  }

  updateQuote(e) {
    this.setState({ quote: e.target.value });
  }

  render () {
    const predict = 'Predict';

    return (
      <div>
        <p>
          <span style={{ paddingRight: 10 }}><strong>Base:</strong></span><input type="text" placeholder="BTC" value={this.state.base} onChange={this.updateBase} />
          <span style={{ paddingLeft: 10, paddingRight: 10 }}><strong>Quote:</strong></span><input type="text" placeholder="USD" value={this.state.quote} onChange={this.updateQuote} />
        </p>
        <p><button className="btn btn-primary" onClick={this.predict}>{predict}</button></p>
        <TVChartContainer ref={this.tvChart} />
      </div>
    );
  }
}
