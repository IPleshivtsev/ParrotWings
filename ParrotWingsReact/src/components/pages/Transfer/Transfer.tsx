import React, { useState, useRef, useEffect } from 'react'
import './Transfer.less'
import { Form, Row, Col, Button } from 'react-bootstrap'
import { ITransactionData, ITransferFormElements, IUnionProps } from '../../auxiliary/Interfaces'
import { handleAmountOnKeyDown, validateAmount, validateText } from '../../auxiliary/Validators'
import { HistoryBlock } from '../../blocks/HistoryBlock/HistoryBlock'
import { SearchUserElements } from '../../blocks/HistoryBlock/SearchUserElements'
import { MessageBlock } from '../../blocks/MessageBlock/MessageBlock'
import { BannerBlock } from '../../blocks/BannerBlock/BannerBlock'
import { _CreateTransaction } from '../../../api/ApiService'

export default function Transfer({ userBalance, setUserBalance, isAuthorized, setIsLoading }: IUnionProps) {
  const [isNameInputClicked, setIsNameInputClicked] = useState(false)
  const [validateNameError, setValidateNameError] = useState('')
  const [isAmountInputClicked, setIsAmountInputClicked] = useState(false)
  const [validateAmountError, setValidateAmountError] = useState('')
  const [serverError, setServerError] = useState('')
  const [isShowHistoryBlock, setIsShowHistoryBlock] = useState(false)
  const [isShowMessageBlock, setIsShowMessageBlock] = useState(false)
  const [MessageBlockTransactDateValue, setMessageBlockTransactDateValue] = useState('')
  const [MessageBlockSenderValue, setMessageBlockSenderValue] = useState('')
  const [MessageBlockAmountValue, setMessageBlockAmountValue] = useState('')
  const transferFormRef = useRef(null)

  const handleSubmitTransfer = async (e: any) => {
    e.preventDefault()
    setIsLoading(true)
    const formElements = transferFormRef.current.elements as ITransferFormElements

    const transactionData: ITransactionData = {
      RecipientId: formElements.formRecipientId.value,
      RecipientName: formElements.formRecipientName.value,
      Amount: parseInt(formElements.formAmount.value),
      SenderId: localStorage.getItem('userId'),
      SenderName: localStorage.getItem('userName'),
    }

    const result = await _CreateTransaction(transactionData)

    if (typeof result !== 'string') {
      const newBalance = (parseInt(userBalance) - result.Amount).toString()
      localStorage.setItem('userBalance', newBalance)
      setUserBalance(newBalance)
      setMessageBlockTransactDateValue(new Date(result.CreatedDate).toLocaleString())
      setMessageBlockSenderValue(result.RecipientName)
      setMessageBlockAmountValue(`${result.Amount.toString()} PW`)
      setIsShowMessageBlock(true)
    } else {
      setServerError(result)
    }
    setIsLoading(false)
  }

  const handleUpdateFormRecipientData = (recipientId: string, recipientName: string) => {
    const formElements = transferFormRef.current.elements as ITransferFormElements
    formElements.formRecipientId.value = recipientId
    formElements.formRecipientName.value = recipientName
  }

  const updateSearchUserElement = (value: string) => {
    localStorage.setItem('disableSearchUserElementCheckValue', 'true')
    let element = document.querySelector('.form-recipient-data .wrapper input') as HTMLInputElement
    var nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set
    nativeInputValueSetter.call(element, value)
    element.dispatchEvent(new Event('input', { bubbles: true }))
  }

  useEffect(() => {
    setIsNameInputClicked(false)
    setIsAmountInputClicked(false)
    if (localStorage.getItem('repeatRecipientId') !== null) {
      setIsNameInputClicked(true)
      setValidateNameError('')
      setIsAmountInputClicked(true)
      setValidateAmountError('')

      const formElements = transferFormRef.current.elements as ITransferFormElements
      formElements.formRecipientId.value = localStorage.getItem('repeatRecipientId')
      formElements.formRecipientName.value = localStorage.getItem('repeatRecipientName')
      formElements.formAmount.value = localStorage.getItem('repeatAmount')

      updateSearchUserElement(localStorage.getItem('repeatRecipientName'))

      localStorage.removeItem('repeatRecipientId')
      localStorage.removeItem('repeatRecipientName')
      localStorage.removeItem('repeatAmount')
    }
  }, [isShowHistoryBlock])

  const handleCloseMessageBlock = async (e: any) => {
    e.preventDefault()
    setIsNameInputClicked(false)
    setIsAmountInputClicked(false)
    setIsShowMessageBlock(false)
  }

  const handleNameOnInput = async () => {
    setIsNameInputClicked(true)
    setValidateNameError(validateText(transferFormRef.current.elements.formRecipientName.value))
  }

  const handleAmountOnInput = async (e: any) => {
    setIsAmountInputClicked(true)
    setValidateAmountError(validateAmount(e.currentTarget.value))
  }

  return (
    <div className={`form-block ${!isAuthorized ? 'banner-class' : ''}`}>
      {isAuthorized && (
        <div>
          <div className={'switch-button-block'}>
            <Button
              className={`switch-button ${!isShowHistoryBlock ? 'active' : ''}`}
              onClick={() => {
                setIsNameInputClicked(false)
                setIsAmountInputClicked(false)
                if (isShowHistoryBlock === true) {
                  setIsShowHistoryBlock(false)
                } else {
                  setIsShowMessageBlock(false)
                }
              }}
            >
              Перевод
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
            <Form
              className={'form-partial active-form'}
              ref={transferFormRef}
              noValidate
              onSubmit={handleSubmitTransfer}
            >
              <Form.Group as={Row} className={'mb-3 form-group hidden'} controlId={'formRecipientId'}>
                <Col className={'form-input'} sm={'10'}>
                  <Form.Control className={'form-control'} type={'text'} />
                </Col>
              </Form.Group>
              <Form.Group as={Row} className={'mb-3 form-group'} controlId={'formRecipientName'}>
                <Form.Label className={'control-label'} column sm={'2'}>
                  Имя получателя
                </Form.Label>
                <SearchUserElements
                  updateFormRecipientData={handleUpdateFormRecipientData}
                  className={'form-recipient-data'}
                  handleNameOnInput={handleNameOnInput}
                />
                <Col className={'form-input hidden'} sm={'10'}>
                  <Form.Control className={'form-control'} type={'text'} />
                </Col>
                <Form.Control.Feedback className={'input-error-message'}>{validateNameError}</Form.Control.Feedback>
              </Form.Group>
              <Form.Group as={Row} className={'mb-3 form-group'} controlId={'formAmount'}>
                <Form.Label className={'control-label'} column sm={'2'}>
                  Сумма перевода
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
                style={{ width: '220px' }}
                disabled={
                  !isNameInputClicked || validateNameError !== '' || !isAmountInputClicked || validateAmountError !== ''
                }
              >
                Перевести
              </Button>
              <Form.Control.Feedback className={'form-error-message'}>{serverError}</Form.Control.Feedback>
            </Form>
          )}

          {isShowMessageBlock && (
            <MessageBlock
              isTransfer={true}
              transactDateValue={MessageBlockTransactDateValue}
              senderValue={MessageBlockSenderValue}
              amountValue={MessageBlockAmountValue}
              handleCloseMessageBlock={handleCloseMessageBlock}
            />
          )}

          {isShowHistoryBlock && <HistoryBlock setIsShowHistoryBlock={setIsShowHistoryBlock} />}
        </div>
      )}

      {!isAuthorized && <BannerBlock isTransfer={true} />}
    </div>
  )
}
