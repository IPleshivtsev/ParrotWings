import React, { useRef, useState } from 'react'
import { Form, Row, Col, Button } from 'react-bootstrap'
import { NavLink, useNavigate } from 'react-router-dom'
import { IRegisterFormElements, IRegisterData, IUserData } from '../../auxiliary/Interfaces'
import { validateConfirmPassword, validateEmail, validateText, validatePassword } from '../../auxiliary/Validators'
import { _Register, _GetCurrentUserData } from '../../../api/ApiService'
import { PWContext } from '../../../App'

export default function RegisterUser() {
  const { setIsAuthorized, setIsLoading } = React.useContext(PWContext)
  const [isNameInputClicked, setIsNameInputClicked] = useState(false)
  const [validateNameError, setValidateNameError] = useState('')
  const [isEmailInputClicked, setIsEmailInputClicked] = useState(false)
  const [validateEmailError, setValidateEmailError] = useState('')
  const [isPasswordInputClicked, setIsPasswordInputClicked] = useState(false)
  const [validatePasswordError, setValidatePasswordError] = useState('')
  const [isConfirmPasswordInputClicked, setIsConfirmPasswordInputClicked] = useState(false)
  const [validateConfirmPasswordError, setValidateConfirmPasswordError] = useState('')
  const [serverError, setServerError] = useState('')
  const passwordInputRef = useRef(null)
  const confirmPasswordInputRef = useRef(null)
  const navigate = useNavigate()

  const handleSubmitRegister = async (e: any) => {
    e.preventDefault()
    setIsLoading(true)
    const formElements = e.currentTarget.elements as IRegisterFormElements

    const registerData: IRegisterData = {
      Name: formElements.formName.value,
      Email: formElements.formEmail.value,
      Password: formElements.formPassword.value,
    }

    const result = await _Register(registerData)

    if (result === true) {
      const userData: IUserData = await _GetCurrentUserData()
      if (typeof userData !== 'string') {
        localStorage.setItem('userId', userData.Id)
        localStorage.setItem('userName', userData.Name)
        localStorage.setItem('userEmail', userData.Email)
        localStorage.setItem('userBalance', userData.Balance.toString())
        setIsAuthorized(true)
        navigate('/transfer')
      } else {
        setServerError(result.toString())
      }
    } else {
      setServerError(result.toString())
    }
    setIsLoading(false)
  }

  const handleNameOnInput = (e: any) => {
    setIsNameInputClicked(true)
    setValidateNameError(validateText(e.currentTarget.value))
  }

  const handleEmailOnInput = (e: any) => {
    setIsEmailInputClicked(true)
    setValidateEmailError(validateEmail(e.currentTarget.value))
  }

  const handlePasswordOnInput = (e: any) => {
    setIsPasswordInputClicked(true)
    setValidatePasswordError(validatePassword(e.currentTarget.value))
    setValidateConfirmPasswordError(
      validateConfirmPassword(confirmPasswordInputRef.current.value, e.currentTarget.value),
    )
  }

  const handleConfirmPasswordOnInput = (e: any) => {
    setIsConfirmPasswordInputClicked(true)
    setValidateConfirmPasswordError(validateConfirmPassword(e.currentTarget.value, passwordInputRef.current.value))
  }

  return (
    <div className={'form-block'}>
      <Form noValidate onSubmit={handleSubmitRegister}>
        <h2>Регистрация</h2>
        <hr />
        <Form.Group as={Row} className={'mb-3 form-group'} controlId={'formName'}>
          <Form.Label className={'control-label'} column sm={'2'}>
            Имя
          </Form.Label>
          <Col className={'form-input'} sm={'10'}>
            <Form.Control
              className={'form-control'}
              type={'text'}
              placeholder={'Введите имя'}
              onInput={handleNameOnInput}
            />
          </Col>
          <Form.Control.Feedback className={'input-error-message'}>{validateNameError}</Form.Control.Feedback>
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
        <Form.Group as={Row} className={'mb-3 form-group'} controlId={'formPassword'}>
          <Form.Label className={'control-label'} column sm={'2'}>
            Пароль
          </Form.Label>
          <Col className={'form-input'} sm={'10'}>
            <Form.Control
              ref={passwordInputRef}
              className={'form-control'}
              type={'password'}
              placeholder={'Введите пароль'}
              onInput={handlePasswordOnInput}
            />
          </Col>
          <Form.Control.Feedback className={'input-error-message'}>{validatePasswordError}</Form.Control.Feedback>
        </Form.Group>
        <Form.Group as={Row} className={'mb-3 form-group'} controlId={'formConfirmPassword'}>
          <Form.Label className={'control-label'} column sm={'2'}>
            Подтверждение пароля
          </Form.Label>
          <Col className={'form-input'} sm={'10'}>
            <Form.Control
              ref={confirmPasswordInputRef}
              className={'form-control'}
              type={'password'}
              placeholder={'Подтвердите пароль'}
              onInput={handleConfirmPasswordOnInput}
            />
          </Col>
          <Form.Control.Feedback className={'input-error-message'}>
            {validateConfirmPasswordError}
          </Form.Control.Feedback>
        </Form.Group>
        <Button
          type={'submit'}
          style={{ width: '220px' }}
          disabled={
            !isNameInputClicked ||
            validateNameError !== '' ||
            !isEmailInputClicked ||
            validateEmailError !== '' ||
            !isPasswordInputClicked ||
            validatePasswordError !== '' ||
            !isConfirmPasswordInputClicked ||
            validateConfirmPasswordError !== ''
          }
        >
          Зарегистрироваться
        </Button>
        <Form.Control.Feedback className={'form-error-message'}>{serverError}</Form.Control.Feedback>
        <p>
          <NavLink className={'simple-link'} to={'/loginUser'}>
            Уже есть учетная запись? Войти
          </NavLink>
        </p>
      </Form>
    </div>
  )
}
