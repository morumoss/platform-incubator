import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import config from '../../config';

import logo from '../../assets/images/morum_logo.png';

import './Home.scss';

export default function Home(props) {
  const history = useHistory();
  useEffect(() => {
    document.title = props.title;
  }, [props.title]);

  const goToDashboard = (event) => {
    event.preventDefault();

    const requestOptions = {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    };

    fetch(config.apiUrl + 'user/1', requestOptions)
      .then(async (response) => {
        const data = await response.json();

        if (!response.ok) {
          const error = (data && data.message) || response.status;
          return Promise.reject(error);
        }

        localStorage.setItem('userId', data.id);
        localStorage.setItem('userIsHost', data.isHost);
        history.push('/dashboard/');
      })
      .catch((error) => {
        console.error('There was an error!', error);
      });
  };

  const goToJoin = (event) => {
    event.preventDefault();

    history.push('/join/');
  };

  return (
    <div className='container home'>
      <img src={logo} alt='Morum OSS Logo' className='home__logo' />
      <h2 className='home__title'>SIGN IN AS</h2>
      <div className='home__btn_wrapper'>
        <input
          type='button'
          value='Teacher'
          className='btn btn--colour-orange'
          onClick={goToDashboard}
        />
        <input
          type='button'
          value='Student'
          className='btn btn--colour-blue'
          onClick={goToJoin}
        />
      </div>
      <p className='home__temp'>
        Note: This is temporary as the login process will happen before this
        widget starts. This widget is only a component of the final application.
      </p>
    </div>
  );
}
