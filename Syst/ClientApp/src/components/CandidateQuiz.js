import React, { useEffect, useState } from "react";
import { Container, Row, Col } from 'react-grid';
import { Pager } from './Pager';
import { CandidateInformation } from './CandidateInformation';
import logo from './Systematic_Logo.png';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import {FetchOptions} from './FetchOptions';

//Page for the quiz the candidates participate in
export function CandidateQuiz(props) {
  const [quiz, setQuiz] = useState(null);
  const [currentQuestion, setCurrentQuestion] = useState(0);
  const [answers, setAnswers] = useState([]);

  const { instance, accounts } = useMsal();

  const EVENTID = props.match.params.event_id;
  const QUIZID = props.match.params.quiz_id;

  useEffect(async () => {
	  const options = await FetchOptions.Options(instance, accounts, "GET");
    const response = await fetch('api/quiz/' + QUIZID, options);
    const data = await response.json();
    let arLength = 0; 

    if (data.questions != null) {
      arLength = data.questions.length;
    }

	  setQuiz(data);
	  setAnswers(Array(arLength));
  }, []);

  //Methods
  const answer = async (option) => {
    answers[currentQuestion] = option;
    next();
  }

  const next = async () => {
    let cq = currentQuestion;
    const len = quiz.questions.length;
    cq++;
    if (cq > len) {
        //finish();
    } else {
        setCurrentQuestion(cq);
    }
  }

  const renderCandidateQuestion = (question, answerFun) => {
    const path = window.location.href.replace(window.location.pathname, "");
    const letters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Æ", "Ø", "Å"];

    let ops = [];
    let i = 0;
    question.options.forEach(op => {
        ops.push(
            <Col key={i}> 
                <div className="div-flex">           
                    <button onClick={() => answerFun(op)} className="btn-answer"> {letters[i++]} </button>
                    <div className='div-quiz_layout'>
                        <h5 className='question-text'> {op}</h5>
                    </div>
                </div>
            </Col>
        );
    });

    //User Interface
    return (
        <div>
            <h3 style={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                margin: 15
            }}>{question.representation}</h3>
            <Row>
                <Col>
                    <div className='question-options txt-small'>
                        {ops}
                    </div>
                </Col>
                <Col>
                    <img style={{width: 400}} src={path + "/api/Image/" + question.imageURl} alt="quizPic" />
                </Col>        
            </Row>
      </div>

    );
  }
    let contents = quiz == null ?
	<></>
	:
	 QUIZID > 0 ? 
        (<Container>         
              {currentQuestion >= quiz.questions.length ?
              <CandidateInformation Answers={answers} QuizId={QUIZID} EventId={EVENTID}/>
              :
              renderCandidateQuestion(quiz.questions[currentQuestion], answer)
              }
              {Pager.Pager(currentQuestion, quiz.questions.length, ((at) => setCurrentQuestion(at)))}  
          </Container>)
        : <h2 className="txt-center"> No quiz attached to this Event! </h2>;


    return (
      <div className='div-center'>
        <img style={{width: 600}} src={logo} alt="logo" />
        {contents}
      </div>
    );
}