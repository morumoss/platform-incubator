import React from 'react';
import ReactDOM from 'react-dom';
import ManagePoll from './ManagePoll';

test('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<ManagePoll />, div);
});
