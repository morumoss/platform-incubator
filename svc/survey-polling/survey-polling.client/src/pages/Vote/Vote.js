import React, { useState, useEffect, useRef } from 'react';
import { useLocation, useHistory, useParams } from 'react-router-dom';
import { Doughnut } from 'react-chartjs-2';

import './Vote.scss';

export default function Vote(props) {
  const { id } = useParams();
  const location = useLocation();
  const history = useHistory();
  const chartReference = useRef({});
  const [question, setQuestion] = useState({
    id: 0,
    pollId: 0,
    content: '',
    options: [{ id: 0, content: '', votes: 0 }],
  });

  const fetchData = () => {
    setQuestion(
      props.question ?? {
        id: 1,
        pollId: 1,
        content: 'Question title',
        options: [
          { id: 1, content: 'Option 1', votes: 4 },
          { id: 2, content: 'Option 2', votes: 7 },
          { id: 3, content: 'Option 3', votes: 2 },
          { id: 4, content: 'Option 4', votes: 2 },
        ],
      }
    );
  };

  const updateChart = (votes) => {
    const currentChart = chartReference.current.chartInstance;

    currentChart.data.datasets[0].data = votes;
    currentChart.update();
  };

  // const setupConnectionEvents = (connection) => {
  //   connection.on('userVoted', (votes) => {
  //     updateChart(question, votes);
  //   });
  // };

  useEffect(() => {
    // const connection = location.state.hubConnection;

    fetchData();
    // setupConnectionEvents(connection);
  }, []);

  const chooseOption = (barNumber) => {
    const currentChart = chartReference.current.chartInstance;

    currentChart.data.datasets[0].data[barNumber] += 1;
    currentChart.update();
  };

  const chartOptions = {
    legend: {
      display: false,
    },
  };

  const chartData = () => {
    const labelArray = [];
    const dataArray = [];

    for (let i = 0; i < question.options.length; i++) {
      labelArray[i] = question.options[i].content;
      dataArray[i] = question.options[i].votes;
    }

    return {
      labels: labelArray,
      datasets: [
        {
          data: dataArray,
          backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#55BE41'],
          hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#55BE41'],
        },
      ],
    };
  };

  return (
    <div className='container vote'>
      <h1 className='vote__poll_title'>{question.content}</h1>
      <div className='vote__diagram'>
        <Doughnut
          ref={chartReference}
          data={chartData}
          options={chartOptions}
        />
      </div>
      <div className='vote__btn_wrapper'>
        {question.options.map((option, index) => (
          <button
            className='vote__btn vote__btn--colour-dynamic'
            onClick={() => chooseOption(index)}
            key={index}
          >
            {option.content}
          </button>
        ))}
      </div>
    </div>
  );
}
