import React, { useState } from 'react'
import { Form, Row, Col, Button } from 'react-bootstrap'
import { NavLink, useNavigate } from 'react-router-dom'
import { ILoginFormElements, ILoginData, IUserData } from '../../auxiliary/Interfaces'
import { validateEmail, validatePassword } from '../../auxiliary/Validators'
import { _GetCurrentUserData, _Login } from '../../../api/ApiService'
import { PWContext } from '../../../App'

export default function LoginUser() {
  const { setIsAuthorized, setIsLoading } = React.useContext(PWContext)
  const [isEmailInputClicked, setIsEmailInputClicked] = useState(false)
  const [validateEmailError, setValidateEmailError] = useState('')
  const [isPasswordInputClicked, setIsPasswordInputClicked] = useState(false)
  const [validatePasswordError, setValidatePasswordError] = useState('')
  const [serverError, setServerError] = useState('')
  const navigate = useNavigate()

  const handleSubmitLogin = async (e: any) => {
    e.preventDefault()
    setIsLoading(true)
    const formElements = e.currentTarget.elements as ILoginFormElements

    const loginData: ILoginData = {
      Email: formElements.formEmail.value,
      Password: formElements.formPassword.value,
    }

    const result = await _Login(loginData)

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

  const handleEmailOnInput = (e: any) => {
    setIsEmailInputClicked(true)
    setValidateEmailError(validateEmail(e.currentTarget.value))
  }

  const handlePasswordOnInput = (e: any) => {
    setIsPasswordInputClicked(true)
    setValidatePasswordError(validatePassword(e.currentTarget.value))
  }

  return (
    <div className={'form-block'}>
      <Form noValidate onSubmit={handleSubmitLogin}>
        <h2>Вход в систему</h2>
        <hr />
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
              className={'form-control'}
              type={'password'}
              placeholder={'Введите пароль'}
              onInput={handlePasswordOnInput}
            />
          </Col>
          <Form.Control.Feedback className={'input-error-message'}>{validatePasswordError}</Form.Control.Feedback>
        </Form.Group>
        <Button
          type={'submit'}
          disabled={
            !isEmailInputClicked || validateEmailError !== '' || !isPasswordInputClicked || validatePasswordError !== ''
          }
        >
          Войти
        </Button>
        <Form.Control.Feedback className={'form-error-message'}>{serverError}</Form.Control.Feedback>
        <p>
          <NavLink className={'simple-link'} to={'/registerUser'}>
            Регистрация нового пользователя
          </NavLink>
        </p>
      </Form>
    </div>
  )
}
