import React from 'react';
import ReactDOM from 'react-dom';
import PollOver from './PollOver';

test('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<PollOver />, div);
});
