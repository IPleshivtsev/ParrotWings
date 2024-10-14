import React, { useState } from 'react'
import './Replenishment.less'
import { Form, Row, Col, Button } from 'react-bootstrap'
import InputMask from 'react-input-mask'
import { IReplenishmentFormElements, ITransactionData } from '../../auxiliary/Interfaces'
import {
  handleAmountOnKeyDown,
  handleStringOnKeyDown,
  validateAmount,
  validateCardCVV,
  validateCardDate,
  validateCardNumber,
} from '../../auxiliary/Validators'
import { HistoryBlock } from '../../blocks/HistoryBlock/HistoryBlock'
import { MessageBlock } from '../../blocks/MessageBlock/MessageBlock'
import { BannerBlock } from '../../blocks/BannerBlock/BannerBlock'
import { _CreateTransaction } from '../../../api/ApiService'
import { PWContext } from '../../../App'

export default function Replenishment() {
  const { userBalance, setUserBalance, isAuthorized, setIsLoading } = React.useContext(PWContext)
  const [isCardNumberInputClicked, setIsCardNumberInputClicked] = useState(false)
  const [validateCardNumberError, setValidateCardNumberError] = useState('')
  const [isCardDateInputClicked, setIsCardDateInputClicked] = useState(false)
  const [validateCardDateError, setValidateCardDateError] = useState('')
  const [isCardCVVInputClicked, setIsCardCVVInputClicked] = useState(false)
  const [validateCardCVVError, setValidateCardCVVError] = useState('')
  const [isAmountInputClicked, setIsAmountInputClicked] = useState(false)
  const [validateAmountError, setValidateAmountError] = useState('')
  const [serverError, setServerError] = useState('')
  const [isShowHistoryBlock, setIsShowHistoryBlock] = useState(false)
  const [isShowMessageBlock, setIsShowMessageBlock] = useState(false)
  const [MessageBlockTransactDateValue, setMessageBlockTransactDateValue] = useState('')
  const [MessageBlockCardNumberValue, setMessageBlockCardNumberValue] = useState('')
  const [MessageBlockAmountValue, setMessageBlockAmountValue] = useState('')

  const handleSubmitReplenishment = async (e: any) => {
    e.preventDefault()
    setIsLoading(true)
    const formElements = e.target.elements as IReplenishmentFormElements

    const transactionData: ITransactionData = {
      RecipientId: localStorage.getItem('userId'),
      RecipientName: localStorage.getItem('userName'),
      Amount: parseInt(formElements.formAmount.value),
      TransferCardNumber: formElements.formCardNumber.value,
    }

    const result = await _CreateTransaction(transactionData)

    if (typeof result !== 'string') {
      const newBalance = (parseInt(userBalance) + result.Amount).toString()
      localStorage.setItem('userBalance', newBalance)
      setUserBalance(newBalance)
      setMessageBlockTransactDateValue(new Date(result.CreatedDate).toLocaleString())
      setMessageBlockCardNumberValue(result.TransferCardNumber)
      setMessageBlockAmountValue(`${result.Amount.toString()} PW`)
      setIsShowMessageBlock(true)
    } else {
      setServerError(result)
    }
    setIsLoading(false)
  }

  const handleCloseMessageBlock = async (e: any) => {
    e.preventDefault()
    setIsShowMessageBlock(false)
  }

  const handleCardNumberOnInput = async (e: any) => {
    setIsCardNumberInputClicked(true)
    setValidateCardNumberError(validateCardNumber(e.currentTarget.value))
  }

  const handleCardDateOnInput = async (e: any) => {
    setIsCardDateInputClicked(true)
    setValidateCardDateError(validateCardDate(e.currentTarget.value))
  }

  const handleCardCVVOnInput = async (e: any) => {
    setIsCardCVVInputClicked(true)
    setValidateCardCVVError(validateCardCVV(e.currentTarget.value))
  }

  const handleAmountOnInput = async (e: any) => {
    setIsAmountInputClicked(true)
    setValidateAmountError(validateAmount(e.currentTarget.value))
  }

  return (
    <div className={`form-block ${!isAuthorized && 'banner-class'}`}>
      {isAuthorized && (
        <div>
          <div className={'switch-button-block'}>
            <Button
              className={`switch-button ${!isShowHistoryBlock ? 'active' : ''}`}
              onClick={() => {
                setIsCardNumberInputClicked(false)
                setIsCardDateInputClicked(false)
                setIsCardCVVInputClicked(false)
                setIsAmountInputClicked(false)
                
                if (isShowHistoryBlock === true) {
                  setIsShowHistoryBlock(false)
                } else {
                  setIsShowMessageBlock(false)
                }
              }}
            >
              Пополнение
            </Button>
            <Button
              className={`switch-button ${isShowHistoryBlock ? 'active' : ''}`}
              onClick={() => {
                setIsShowHistoryBlock(true)
                setIsShowMessageBlock(false)
              }}
            >
              История
            </Button>
          </div>

          {!isShowHistoryBlock && !isShowMessageBlock && (
            <Form className={'form-partial active-form'} noValidate onSubmit={handleSubmitReplenishment}>
              <Form.Group className={'form-card-number-group mb-3 form-group'} as={Row} controlId={'formCardNumber'}>
                <Form.Label className={'control-label'} column sm={'2'}>
                  Номер карты
                </Form.Label>
                <Col className={'form-input'} sm={'10'}>
                  <InputMask
                    id={'formCardNumber'}
                    className={'form-control'}
                    type={'text'}
                    mask={'9999 9999 9999 9999'}
                    placeholder={'0000 0000 0000 0000'}
                    onInput={handleCardNumberOnInput}
                    onKeyDown={(e) => handleStringOnKeyDown(e, /\s|_/g, 16)}
                    alwaysShowMask={false}
                  />
                </Col>
                <Form.Control.Feedback className={'input-error-message'}>
                  {validateCardNumberError}
                </Form.Control.Feedback>
              </Form.Group>
              <Form.Group className={'form-card-date-group mb-3 form-group'} as={Row} controlId={'formCardDate'}>
                <Form.Label className={'control-label'} column sm={'2'}>
                  Месяц и год
                </Form.Label>
                <Col className={'form-input'} sm={'10'}>
                  <InputMask
                    id={'formCardDate'}
                    className={'form-control'}
                    type={'text'}
                    mask={'99/99'}
                    placeholder={'00/00'}
                    onInput={handleCardDateOnInput}
                    onKeyDown={(e) => handleStringOnKeyDown(e, /\/|_/g, 4)}
                  />
                </Col>
                <Form.Control.Feedback className={'input-error-message'}>{validateCardDateError}</Form.Control.Feedback>
              </Form.Group>
              <Form.Group className={'form-card-CVV-group mb-3 form-group'} as={Row} controlId={'formCardCVV'}>
                <Form.Label className={'control-label'} column sm={'2'}>
                  CVV карты
                </Form.Label>
                <Col className={'form-input'} sm={'10'}>
                  <InputMask
                    id={'formCardCVV'}
                    className={'form-control'}
                    type={'text'}
                    mask={'999'}
                    placeholder={'000'}
                    onInput={handleCardCVVOnInput}
                    onKeyDown={(e) => handleStringOnKeyDown(e, /_/g, 3)}
                  />
                </Col>
                <Form.Control.Feedback className={'input-error-message'}>{validateCardCVVError}</Form.Control.Feedback>
              </Form.Group>
              <Form.Group className={'mb-3 form-group'} as={Row} controlId={'formAmount'}>
                <Form.Label className={'control-label'} column sm={'2'}>
                  Сумма пополнения
                </Form.Label>
                <Col className={'form-input'} sm={'10'}>
                  <Form.Control
                    className={'form-control'}
                    type={'text'}
                    placeholder={'Введите сумму перевода'}
                    onInput={handleAmountOnInput}
                    onKeyDown={handleAmountOnKeyDown}
                  />
                </Col>
                <Form.Control.Feedback className={'input-error-message'}>{validateAmountError}</Form.Control.Feedback>
              </Form.Group>
              <Button
                type={'submit'}
                disabled={
                  !isCardNumberInputClicked ||
                  validateCardNumberError !== '' ||
                  !isCardDateInputClicked ||
                  validateCardDateError !== '' ||
                  !isCardCVVInputClicked ||
                  validateCardCVVError !== '' ||
                  !isAmountInputClicked ||
                  validateAmountError !== ''
                }
              >
                Пополнить
              </Button>
              <Form.Control.Feedback className={'form-error-message'}>{serverError}</Form.Control.Feedback>
            </Form>
          )}

          {isShowMessageBlock && (
            <MessageBlock
              isTransfer={false}
              transactDateValue={MessageBlockTransactDateValue}
              senderValue={MessageBlockCardNumberValue}
              amountValue={MessageBlockAmountValue}
              handleCloseMessageBlock={handleCloseMessageBlock}
            />
          )}

          {isShowHistoryBlock && <HistoryBlock setIsShowHistoryBlock={null} />}
        </div>
      )}

      {!isAuthorized && <BannerBlock isTransfer={false} />}
    </div>
  )
}
