import React from 'react';
import ReactDOM from 'react-dom';
import Vote from './Vote';

test('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<Vote />, div);
});
