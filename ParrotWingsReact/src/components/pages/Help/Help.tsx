import React, { useState, useRef } from 'react'
import './Help.less'
import { Image } from 'react-bootstrap'
import { Form, Row, Col, Button } from 'react-bootstrap'
import { IAppealFormElements, IAppealData, IHelpBlockProps } from '../../auxiliary/Interfaces'
import { validateEmail, validateText } from '../../auxiliary/Validators'
import { _CreateAppeal } from '../../../api/ApiService'
import { PWContext } from '../../../App'

export default function Help() {
  const { setIsLoading } = React.useContext(PWContext)
  const [isTitleInputClicked, setIsTitleInputClicked] = useState(false)
  const [validateTitleError, setValidateTitleError] = useState('')
  const [isMessageInputClicked, setIsMessageInputClicked] = useState(false)
  const [validateMessageError, setValidateMessageError] = useState('')
  const [isEmailInputClicked, setIsEmailInputClicked] = useState(false)
  const [validateEmailError, setValidateEmailError] = useState('')
  const [serverError, setServerError] = useState('')
  const [isShowMessageAppealForm, setIsShowMessageAppealForm] = useState(false)
  const sendAppealFormRef = useRef(null)

  const handleSubmitSendAppeal = async (e: any) => {
    e.preventDefault()
    setIsLoading(true)
    const formElements = sendAppealFormRef.current.elements as IAppealFormElements

    const appealData: IAppealData = {
      Title: formElements.formTitle.value,
      Message: formElements.formMessage.value,
      Email: formElements.formEmail.value,
    }

    const result: boolean = await _CreateAppeal(appealData)

    typeof result !== 'string' ? setIsShowMessageAppealForm(true) : setServerError(result)

    setIsLoading(false)
  }

  const handleTitleOnInput = (e: any) => {
    setIsTitleInputClicked(true)
    setValidateTitleError(validateText(e.currentTarget.value))
  }

  const handleMessageOnInput = (e: any) => {
    setIsMessageInputClicked(true)
    setValidateMessageError(validateText(e.currentTarget.value))
  }

  const handleEmailOnInput = (e: any) => {
    setIsEmailInputClicked(true)
    setValidateEmailError(validateEmail(e.currentTarget.value))
  }

  const handleCloseMessageAppealForm = async (e: any) => {
    setIsTitleInputClicked(false)
    setIsMessageInputClicked(false)
    setIsEmailInputClicked(false)
    setIsShowMessageAppealForm(false)
  }

  function HelpBlock({ children, question }: IHelpBlockProps) {
    const [isShowAnswer, setIsShowAnswer] = useState(false)

    return (
      <div className={`help-block ${isShowAnswer ? 'show-answer' : ''}`} onClick={() => setIsShowAnswer(!isShowAnswer)}>
        <div className={'question-block'}>
          <div className={'question'}>{question}</div>
          {isShowAnswer && <Image src='/images/arrowMoreDown.svg' />}
          {!isShowAnswer && <Image src='/images/arrowMoreUp.svg' />}
        </div>
        <div className={'answer'}>
          <>{children}</>
        </div>
      </div>
    )
  }

  return (
    <div className={'form-block'}>
      <h2>Часто задаваемые вопросы</h2>
      <hr />

      <HelpBlock question={'Как начать пользоваться?'}>
        <div>
          Зарегистрироваться в ParrotWings очень просто. Достаточно ввести свое имя, электронную почту и придумать
          пароль.'
        </div>
      </HelpBlock>

      <HelpBlock question={'Как перевести деньги другому пользователю?'}>
        <div>
          <ol>
            <li>Необходимо зайти в раздел 'Переводы';</li>
            <li>В поле 'Получатель' ввести имя получателя, либо выбрать из выпадающего списка во время ввода;</li>
            <li>Указать сумму перевода;</li>
            <li>Нажать на кнопку 'Перевести'.</li>
          </ol>
          После удачной операции Вам будет выведено сообщение с результатом перевода.
        </div>
      </HelpBlock>

      <HelpBlock question={'Как пополнить свой счет?'}>
        <div>
          <ol>
            <li>Необходимо зайти в раздел 'Пополнение счета';</li>
            <li>Ввести данные карты;</li>
            <li>Указать сумму пополнения;</li>
            <li>Нажать на кнопку 'Пополнить';</li>
            <li>Далее необходимо подтвердить платеж на странице банка'.</li>
          </ol>
          После удачной операции Вам будет выведено сообщение с результатом пополнения.
        </div>
      </HelpBlock>

      <HelpBlock question={'Как быстро придут деньги на счет при переводе, либо пополнении счета?'}>
        <div>Деньги зачисляются от нескольких секунд до суток.</div>
      </HelpBlock>

      <HelpBlock question={'Где я могу просмотреть историю своих операций?'}>
        <div>
          Историю Ваших операций вы можете просмотреть на страницах 'Переводы' и 'Пополнение счета' в соответствующих
          разделах, а также в своем профиле.
        </div>
      </HelpBlock>

      <HelpBlock question={'Я отправил обращение в поддержку. Как долго мне ждать ответа?'}>
        <div>Большинство обращений решаются в тот же день, но иногда может понадобиться больше времени.</div>
      </HelpBlock>

      {!isShowMessageAppealForm && (
        <Form
          id={'SendAppealForm'}
          className={'form-partial active-form'}
          noValidate
          ref={sendAppealFormRef}
          onSubmit={handleSubmitSendAppeal}
        >
          <div style={{ fontSize: '18px', textAlign: 'center' }}>
            Не нашли ответ на свой вопрос? Задайте его нашей поддержке.
          </div>
          <span className={'text-dangerForm'} id={'formValidation'}></span>
          <Form.Group as={Row} className={'mb-3 form-group'} controlId={'formTitle'}>
            <Form.Label className={'control-label'} column sm={'2'}>
              Тема вопроса
            </Form.Label>
            <Col className={'form-input'} sm={'10'}>
              <Form.Control
                className={'form-control'}
                type={'text'}
                placeholder={'Введите тему вопроса'}
                onInput={handleTitleOnInput}
              />
            </Col>
            <Form.Control.Feedback className={'input-error-message'}>{validateTitleError}</Form.Control.Feedback>
          </Form.Group>
          <Form.Group as={Row} className={'mb-3 form-group'} controlId={'formMessage'}>
            <Form.Label className={'control-label'} column sm={'2'}>
              Описание
            </Form.Label>
            <Col className={'form-input'} sm={'10'}>
              <Form.Control
                className={'form-control'}
                type={'text'}
                placeholder={'Введите вопрос'}
                onInput={handleMessageOnInput}
              />
            </Col>
            <Form.Control.Feedback className={'input-error-message'}>{validateMessageError}</Form.Control.Feedback>
          </Form.Group>
          <Form.Group as={Row} className={'mb-3 form-group'} controlId={'formEmail'}>
            <Form.Label className={'control-label'} column sm={'2'}>
              Электронная почта
            </Form.Label>
            <Col className={'form-input'} sm={'10'}>
              <Form.Control
                className={'form-control'}
                type={'email'}
                placeholder={'Введите электронную почту'}
                onInput={handleEmailOnInput}
              />
            </Col>
            <Form.Control.Feedback className={'input-error-message'}>{validateEmailError}</Form.Control.Feedback>
          </Form.Group>
          <Button
            type={'submit'}
            disabled={
              !isTitleInputClicked ||
              validateTitleError !== '' ||
              !isMessageInputClicked ||
              validateMessageError !== '' ||
              !isEmailInputClicked ||
              validateEmailError !== ''
            }
          >
            Отправить
          </Button>
          <Form.Control.Feedback className={'form-error-message'}>{serverError}</Form.Control.Feedback>
        </Form>
      )}

      {isShowMessageAppealForm && (
        <Form id={'MessageAppealForm'} className={'form-partial active-form'}>
          <div>
            <Image src={'/images/success.svg'} />
            <label className={'first-label'}>Обращение успешно отправлено!</label>
            <label className={'second-label'}>Мы ответим Вам в течение 2-х дней.</label>
          </div>
          <Button type={'submit'} onClick={handleCloseMessageAppealForm}>
            Назад
          </Button>
        </Form>
      )}
    </div>
  )
}
