import React, { useState, useEffect, useRef } from 'react';
import {
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from '@microsoft/signalr';
import { useLocation, useHistory, useParams } from 'react-router-dom';
import config from '../../config';
import './Lobby.scss';

export default function Lobby(props) {
  const { id } = useParams();
  const location = useLocation();
  const history = useHistory();
  const didMountRef = useRef(false);
  const [hubConnection, setHubConnection] = useState({});
  const [userCount, setUserCount] = useState(0);
  const [lobby, setLobby] = useState({
    id: 0,
    pin: '',
    hasStarted: false,
    activeQuestionId: 0,
    poll: {
      title: '',
    },
  });
  const [isHost, setIsHost] = useState(false);

  useEffect(() => {
    document.title = props.title;
  }, [props.title]);

  const fetchLobby = async () => {
    const requestOptions = {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    };

    await fetch(config.apiUrl + 'lobby/' + id, requestOptions)
      .then(async (response) => {
        const data = await response.json();

        if (!response.ok) {
          const error = (data && data.message) || response.status;
          return Promise.reject(error);
        }

        setLobby(data);
      })
      .catch((error) => {
        console.error('There was an error!', error);
      });
  };

  useEffect(() => {
    if (location.state === undefined) {
      history.push('/join');
      return;
    }

    if (id != location.state.lobby.id) {
      history.push('/join');
      return;
    }

    const getUserIsHost = localStorage.getItem('userIsHost');
    setIsHost(getUserIsHost === 'true' ? true : false);
    fetchLobby();
  }, []);

  useEffect(() => {
    const createHubConnection = async () => {
      const connection = new HubConnectionBuilder()
        .withUrl(config.hubUrl)
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();

      const startHubConnection = async () => {
        try {
          await connection.start();
          console.assert(connection.state === HubConnectionState.Connected);
          console.log('Connection successful');

          setHubConnection(connection);
        } catch (err) {
          console.assert(connection.state === HubConnectionState.Disconnected);
          console.log('Error while establishing connection: ' + err);
          setTimeout(() => startHubConnection(), 5000);
        }
      };

      await startHubConnection();
    };

    if (didMountRef.current) {
      createHubConnection();
      if (lobby.hasStarted) {
        history.push(`/lobby/${id}/vote`, {
          lobby: lobby,
          hubConnection: hubConnection,
        });
      }
    }
  }, [lobby]);

  useEffect(() => {
    const updateUserConnectionId = async (connectionId) => {
      const requestOptions = {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          id: localStorage.getItem('userId'),
          connectionId: connectionId,
          isHost: isHost,
        }),
      };

      fetch(
        config.apiUrl + 'user/' + localStorage.getItem('userId'),
        requestOptions
      )
        .then(async (response) => {
          if (!response.ok) {
            const error = response.status;
            return Promise.reject(error);
          }

          hubConnection
            .invoke('JoinLobby', lobby.pin)
            .catch((err) => console.log(err));
        })
        .catch((error) => {
          console.error('There was an error!', error);
        });
    };

    if (didMountRef.current) {
      hubConnection.on('userConnected', (connectionId) => {
        updateUserConnectionId(connectionId);
      });

      hubConnection.on('userJoined', (count) => {
        setUserCount(count);
      });

      hubConnection.on('userLeft', (count) => {
        setUserCount(count);
      });

      hubConnection.on('pollStarted', () => {
        history.push(`/lobby/${id}/vote`, {
          lobby: lobby,
          hubConnection: hubConnection,
        });
      });
    } else {
      didMountRef.current = true;
    }
  }, [hubConnection]);

  const startPoll = () => {
    if (!isHost) {
      return;
    }

    if (lobby == null) {
      return;
    }

    hubConnection
      .invoke('StartPoll', lobby.pin)
      .catch((err) => console.log(err));
  };

  return (
    <div className='lobby'>
      {didMountRef.current === true ? (
        <div className='container lobby__wrapper'>
          <section className='lobby__info_section'>
            <h1 className='lobby__title'>{lobby.poll.title}</h1>
            <h2 className='lobby__pin'>
              The lobby pin is:
              <span className='lobby__pin_number'>{lobby.pin}</span>
            </h2>
          </section>
          <hr className='lobby__line'></hr>
          <section className='lobby__waiting_section'>
            <h2 className='lobby__waiting'>
              {isHost
                ? "Click start when you're ready to run the poll!"
                : 'Waiting for your host to start...'}
            </h2>
            <h2 className='lobby__count'>Users: {userCount}</h2>
            {isHost ? (
              <button
                className='btn btn--colour-blue lobby__btn_start'
                onClick={() => startPoll()}
              >
                Start
              </button>
            ) : (
              ''
            )}
          </section>
        </div>
      ) : (
        ''
      )}
    </div>
  );
}
