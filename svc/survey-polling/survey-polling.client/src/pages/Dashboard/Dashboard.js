import React, { useState, useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import logo from '../../assets/images/morum_logo.png';
import config from '../../config';
import MorumNavBar from '../../components/NavBar/NavBar';
import './Dashboard.scss';

export default function Dashboard(props) {
  const history = useHistory();
  const [polls, setPolls] = useState([{}]);

  useEffect(() => {
    document.title = props.title;
  }, [props.title]);

  const fetchPolls = async () => {
    const requestOptions = {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    };

    await fetch(
      config.apiUrl + 'poll/GetPollsByHostId/' + localStorage.getItem('userId'),
      requestOptions
    )
      .then(async (response) => {
        const data = await response.json();

        if (!response.ok) {
          const error = (data && data.message) || response.status;
          return Promise.reject(error);
        }

        let pollArray = [];
        data.forEach((poll, i) => {
          pollArray[i] = {
            id: poll.id,
            title: poll.title,
            questionCount: poll.questions.length,
          };
        });

        setPolls(pollArray);
      })
      .catch((error) => {
        console.error('There was an error!', error);
      });
  };

  useEffect(() => {
    fetchPolls();
  }, []);

  // ! This should happen in the ManagePoll page but it is here for testing.
  const goToManagePoll = (event) => {
    event.preventDefault();

    history.push('/dashboard/poll/');
    // const requestOptions = {
    //   method: 'POST',
    //   headers: { 'Content-Type': 'application/json' },
    //   body: JSON.stringify({
    //     hostId: parseInt(localStorage.getItem('userId')),
    //     title: 'A poll for testing the API',
    //   }),
    // };

    // fetch(config.apiUrl + 'poll', requestOptions)
    //   .then(async (response) => {
    //     const data = await response.json();

    //     if (!response.ok) {
    //       const error = (data && data.message) || response.status;
    //       return Promise.reject(error);
    //     }

    //     history.push('/dashboard/poll/' + data.id);
    //   })
    //   .catch((error) => {
    //     console.error('There was an error!', error);
    //   });
  };

  const startPoll = (pollId) => {
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        pollId: pollId,
      }),
    };

    fetch(config.apiUrl + 'lobby', requestOptions)
      .then(async (response) => {
        const data = await response.json();

        if (!response.ok) {
          const error = (data && data.message) || response.status;
          return Promise.reject(error);
        }

        history.push('/lobby/' + data.id, { lobby: { id: data.id } });
      })
      .catch((error) => {
        console.error('There was an error!', error);
      });
  };

  const editPoll = (pollId) => {};

  const deletePoll = (pollId, index) => {
    const requestOptions = {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
      },
    };

    fetch(config.apiUrl + 'poll/' + pollId, requestOptions)
      .then(async (response) => {
        if (!response.ok) {
          const error = response.status;
          return Promise.reject(error);
        }

        polls.splice(index, 1);
      })
      .catch((error) => {
        console.error('There was an error!', error);
      });
  };

  const PollsTable = () => {
    if (!polls) {
      return null;
    }

    if (polls.length > 0) {
      return (
        <>
          {polls.map((poll, i) => (
            <tr key={i}>
              <td>{i + 1}</td>
              <td>{poll.title}</td>
              <td>{poll.questionCount}</td>
              <td>N/A</td>
              <td>N/A</td>
              <td>
                <input
                  type='button'
                  value='Start'
                  className='btn btn--colour-blue'
                  onClick={() => startPoll(poll.id)}
                />
                <input
                  type='button'
                  value='Edit'
                  className='btn btn--colour-orange'
                  onClick={() => editPoll(poll.id)}
                />
                <input
                  type='button'
                  value='Delete'
                  className='btn btn--colour-red'
                  onClick={() => deletePoll(poll.id, i)}
                />
              </td>
            </tr>
          ))}
        </>
      );
    }
  };

  return (
    <div>
      <nav className='nav'>
        <div className='nav__items'>
          <img className='img__logo' src={logo} alt='morum logo'></img>
          <div className='nav__items_left'>
            <h2 className='title'>Morum OSS | Teacher Dashboard</h2>
          </div>
        </div>
      </nav>
      <div className='container manager'>
        <MorumNavBar />
        <br></br>
        <div className='container dashboard'>
          <div className='body__contrainer'>
            <section className='create__poll_sec'>
              <br></br>
              <div className='poll-container'>
                <h2 className='create_poll_title'>Create a new poll</h2>
                <br></br>
                <input
                  type='button'
                  value='New'
                  className='btn btn--colour-blue'
                  onClick={goToManagePoll}
                />
              </div>
            </section>
            <section className='table__sec'>
              <table className='dashboard__polls_table'>
                <thead>
                  <tr>
                    <th>#</th>
                    <th>Title</th>
                    <th>Questions</th>
                    <th>Date Created</th>
                    <th>Date Modified</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>{PollsTable()}</tbody>
              </table>
            </section>
          </div>
        </div>
      </div>
    </div>
  );
}
