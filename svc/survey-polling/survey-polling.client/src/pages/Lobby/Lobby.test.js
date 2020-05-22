import React from 'react';
import ReactDOM from 'react-dom';
import Lobby from './Lobby';

test('renders without crashing', () => {
  const div = document.createElement('div');
  ReactDOM.render(<Lobby />, div);
});
