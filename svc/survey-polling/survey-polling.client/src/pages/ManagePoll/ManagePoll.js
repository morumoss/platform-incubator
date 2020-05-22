import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Link } from 'react-router-dom';
import config from '../../config';

import './ManagePoll.scss';
import logo from '../../assets/images/morum_logo.png';

export default function ManagePoll(props) {
  const { id } = useParams();
  const [poll, setPoll] = useState({
    id: 0,
    hostId: 0,
    title: '',
    questions: [{ id: 0, pollId: 0, content: '', options: [{}] }],
  });

  const [questionFields, setQuestionFields] = useState([{ question: null }]);
  const [optionFields, setOptionFields] = useState([{ option: null }]);
  const [qError, setqError] = useState('');
  const [opError, setopError] = useState('');

  useEffect(() => {
    document.title = props.title;
  }, [props.title]);

  const fetchExistingPoll = async () => {
    const requestOptions = {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    };

    await fetch(
      `${config.apiUrl}poll/GetPollsByHostId/${localStorage.getItem(
        'userId'
      )}/${id}`,
      requestOptions
    )
      .then(async (response) => {
        const data = await response.json();

        if (!response.ok) {
          const error = (data && data.message) || response.status;
          return Promise.reject(error);
        }

        setPoll(data);
      })
      .catch((error) => {
        console.error('There was an error!', error);
      });
  };

  useEffect(() => {
    if (id != undefined) {
      fetchExistingPoll();
    }
  }, []);

  useEffect(() => {
    //
  }, [poll]);

  // Question Handling
  const addQuestion = function () {
    const qFields = [...questionFields];
    qFields.push({ question: null });
    setQuestionFields(qFields);
    setqError('');
  };

  const deleteQuestion = function (i) {
    const qFields = [...questionFields];

    if (qFields.length === 1) {
      setqError('You cannot delete this textbox');
    } else {
      setqError('');
      qFields.splice(i, 1);
      setQuestionFields(qFields);
    }
  };

  // Option Handling
  const addOptions = function () {
    const oFields = [...optionFields];
    oFields.push({ option: null });

    if (oFields.length === 5) {
      alert('You are only allowed to add 4 options');
    } else {
      setOptionFields(oFields);
      setopError('');
      console.log(oFields);
    }
  };

  const deleteOptions = function (i) {
    const oFields = [...optionFields];

    if (oFields.length === 1) {
      setopError('You cannot delete this option textbox');
    } else {
      setopError('');
      oFields.splice(i, 1);
      setOptionFields(oFields);
    }
  };

  // On change handling
  const handleQuestionChange = function (event) {
    const newPoll = poll;
    newPoll.title = event.target.value;
    setPoll(newPoll);
  };

  const handleOptionsChange = function (j, event) {
    const oFields = [...optionFields];
    oFields[j].value = event.target.value;
    setOptionFields(oFields);
  };

  // Saving a poll
  const savePoll = () => {
    // TODO: Save all inputs into the new/existing poll
  };

  const openQuestion = (index) => {
    // TODO: Display question in the right half of the page so it can be edited
  };

  return (
    <div className='poll'>
      <nav className='nav'>
        <div className='nav__items'>
          <img className='img__logo' src={logo} alt='morum logo'></img>
          <div className='nav__items_left'>
            <h2 className='title'>
              <Link to='/Dashboard' className='nav__dashboard'>
                Morum OSS | Manage Poll
              </Link>
            </h2>
          </div>
          <div className='nav__items_right'>
            <h2 className='poll__title'>Title</h2>
            <input
              type='text'
              id='pollTitle'
              name='pollTitle poll__input'
              placeholder='Enter poll title'
              value={poll.title}
            />

            <button id='btnExit' className='btn'>
              Exit
            </button>
            <button id='btnSave' className='btn btn--colour-blue'>
              Save
            </button>
          </div>
        </div>
      </nav>
      <div className='columns'>
        <div className='poll__questions'>
          <h2 className='poll__heading'>Questions</h2>
          <br></br>
          <p id='qError'>{qError}</p>
          <form onSubmit={addQuestion}>
            {questionFields.map((questionField, i) => {
              return (
                <div className='question__wrapper' key={`question-${i}`}>
                  <input
                    href='#'
                    className='questionInput'
                    id={`question-${i}`}
                    name={`question-${i}`}
                    data-idx={i}
                    type='button'
                    value={`${i + 1}. ${
                      questionField.value || 'Click to edit your question'
                    }`}
                    onClick={() => openQuestion(i)}
                  />

                  <button
                    id={`questionDelete-${i}`}
                    className='btn btn--colour-red questionDelete'
                    name={`questionDelete-${i}`}
                    type='button'
                    onClick={() => deleteQuestion(i)}
                  >
                    Delete
                  </button>
                  <br></br>
                  <br></br>
                </div>
              );
            })}
            <button
              id='btnAddQuestion'
              name='btnAddQuestion'
              type='button'
              className='btn btn--colour-blue'
              onClick={() => addQuestion()}
            >
              Add
            </button>
          </form>
        </div>

        <div className='poll__settings'>
          <h2 className='poll__heading'>Your Question</h2>
          <br></br>
          <form onSubmit={addOptions}>
            <input
              id='txtQuestionOptions'
              type='text'
              placeholder='Enter your question'
              className='poll__input'
              onChange={(event) => handleQuestionChange(event)}
            />
            <br></br>
            <br></br>
            <h2>Answers</h2>
            <br></br>
            <p id='opError'>{opError}</p>
            {optionFields.map((optionField, i) => {
              return (
                <div className='option__wrapper' key={`option-${i}`}>
                  <input
                    data-idx={i}
                    id={`option-${i}`}
                    name={`option-${i}`}
                    className='txtInputQuestion'
                    type='text'
                    placeholder={'Option ' + (i + 1)}
                    value={optionField.value || ''}
                    onChange={(event) => handleOptionsChange(i, event)}
                  />
                  <button
                    id={`optionDelete-${i}`}
                    className='btn btn--colour-red'
                    name={`optionDelete-${i}`}
                    type='button'
                    onClick={() => deleteOptions(i)}
                  >
                    Delete
                  </button>
                  <br></br>
                  <br></br>
                </div>
              );
            })}

            <button
              type='button'
              className='btn btn--colour-blue'
              id='btnOption'
              name='btnOption'
              onClick={() => addOptions()}
            >
              Add
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
