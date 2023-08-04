import React from 'react'
import './MessageBlock.less'
import { Button, Form } from 'react-bootstrap'
import { IMessageBlockProps } from '../../auxiliary/Interfaces'

export function MessageBlock({
  isTransfer,
  transactDateValue,
  senderValue,
  amountValue,
  handleCloseMessageBlock,
}: IMessageBlockProps) {
  return (
    <Form id={'MessageBlock'} className={'form-partial active-form'}>
      <Form.Label className={'control-label'} column sm={'2'}>
        {transactDateValue}
      </Form.Label>
      <Form.Label className={'control-label'} column sm={'2'}>
        {senderValue}
      </Form.Label>
      <Form.Label className={'control-label amount-value'} column sm={'2'}>
        {amountValue}
      </Form.Label>
      <Form.Label className='result-info' column sm={'2'}>
        {isTransfer ? 'Платеж успешно проведен!' : 'Пополнение успешно проведено!'}
      </Form.Label>
      <Button type={'submit'} onClick={handleCloseMessageBlock}>
        Назад
      </Button>
    </Form>
  )
}
