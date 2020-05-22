import React, { useEffect } from 'react';
import { Link } from 'react-router-dom';
import logo from '../../assets/images/morum_logo.png';
import './PollOver.scss';

export default function PollOver(props) {
  useEffect(() => {
    document.title = props.title;
  }, [props.title]);

  return (
    <div className='pollover'>
      <div className='container pollover'>
        <section className='pollover__title'>
          <img
            src={logo}
            alt='Morum OSS Logo'
            className='pollover__join__logo'
          />
          <h1 className='pollover__over'>The poll is over</h1>
          <br></br>
          <p className='pollover__thanks'>
            Thanks for participating in the poll!
          </p>
          <br></br>
          <button className='btn btn--colour-blue'>
            <Link className='pollover__link' to='/join'>
              Return to the Join Page
            </Link>
          </button>
        </section>
      </div>
    </div>
  );
}
