import React from 'react';
import ReactDOM from 'react-dom';
import * as serviceWorker from './serviceWorker';

// Import base styles before component styles from routes
import './styles/main.scss';

import Routes from './pages/routes';

ReactDOM.render(<Routes />, document.getElementById('root'));
serviceWorker.unregister();
