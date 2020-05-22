import React, { useState, useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import config from '../../config';

import './Join.scss';
import logo from '../../assets/images/morum_logo.png';
export default function Join(props) {
  const history = useHistory();
  const [pin, setPin] = useState('');
  const [error, setError] = useState('');

  useEffect(() => {
    document.title = props.title;
  }, [props.title]);

  const createUser = async (callback) => {
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        isHost: false,
      }),
    };

    fetch(config.apiUrl + 'user', requestOptions)
      .then(async (response) => {
        const data = await response.json();

        if (!response.ok) {
          const error = (data && data.message) || response.status;
          return Promise.reject(error);
        }

        localStorage.setItem('userId', data.id);
        localStorage.setItem('userIsHost', data.isHost);
        callback();
      })
      .catch((error) => {
        console.error('There was an error!', error);
      });
  };

  const joinLobby = async (event) => {
    event.preventDefault();

    if (pin.length < 6) {
      setError('The pin must be 6 digits.');
      return;
    }

    const requestOptions = {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    };

    fetch(config.apiUrl + 'lobby/GetLobbyByPin/' + pin, requestOptions)
      .then(async (response) => {
        const data = await response.json();

        if (!response.ok) {
          const error = (data && data.message) || response.status;
          return Promise.reject(error);
        }

        try {
          if (data.id == null) {
            return;
          }

          createUser(() => {
            history.push('/lobby/' + data.id, {
              lobby: data,
            });
          });
        } catch (err) {
          console.log(err);
        }
      })
      .catch((error) => {
        console.error('There was an error!', error);
      });
  };

  return (
    <div className='join'>
      <div className='container join__wrapper'>
        <img src={logo} alt='Morum OSS Logo' className='join__logo' />
        <h1 className='join__title'>Please enter the pin:</h1>
        <form onSubmit={joinLobby}>
          <input
            className='join__pin'
            type='text'
            value={pin}
            onChange={(e) => setPin(e.target.value)}
            name='lobbyPin'
            id='lobbyPin'
            maxLength='6'
            placeholder='123456'
          />
          {error ? <p className='join__error'>{error}</p> : ''}
          <input
            type='submit'
            value='Join'
            className='btn btn--colour-blue join__submit'
            id='btnSubmit'
            onClick={joinLobby}
          />
        </form>
      </div>
    </div>
  );
}
